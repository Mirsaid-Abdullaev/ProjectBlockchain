Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports System.Net
Imports System.Net.Sockets
Imports System.Threading
Imports System.Text

Public Class ConnectionHandler
    Private ReadOnly RemoteEP As New IPEndPoint(IPAddress.Any, GLOBAL_SERVER_PORT) 'receiving endpoint
    Private ReadOnly BroadcastEP As New IPEndPoint(IPAddress.Broadcast, GLOBAL_CLIENT_PORT) 'broadcasting endpoint

    Private UDPSender As UdpClient 'sending client
    Private UDPReceiver As UdpClient 'receving client

    Private BroadcastThread As Thread 'thread to broadcast connection request to network devices
    Private ListenerThread As Thread 'thread that listens for incoming connection request broadcasts
    Private ConRespThread As Thread 'thread to respond to connection requests on
    Private Property KillHandler As Boolean = False 'flag for destructor
    Public Sub Start() 'initialises the threads based on program flags
        If IsLeaf AndAlso IsSynchronised Then 'sending connection requests (as you are the leaf node and synchronised to the chain now)
            UDPSender = New UdpClient(BroadcastEP) With {.EnableBroadcast = True}
            BroadcastThread = New Thread(AddressOf BroadcastConnectionRequest) With {.IsBackground = True}
            BroadcastThread.Start()

            'also listening for connection responses
            UDPReceiver = New UdpClient(RemoteEP) With {.EnableBroadcast = False}
            ConRespThread = New Thread(AddressOf ListenForResponse) With {.IsBackground = True}
            ConRespThread.Start()

        ElseIf Not IsSynchronised Then 'listening for an incoming connection request only, as you are not synchronised or the leaf node
            UDPReceiver = New UdpClient(RemoteEP) With {.EnableBroadcast = False}
            UDPSender = Nothing
            ListenerThread = New Thread(AddressOf ListenForBroadcasts) With {.IsBackground = True}
            ListenerThread.Start()

        End If
    End Sub


    Private Sub ListenForBroadcasts() 'this is for unsynchronised nodes to try and connect with the current leaf node
        While AppRunning AndAlso Not IsSynchronised And Not KillHandler
            Dim JsonObject As Object
            Dim RecvData As String
            Try
                Dim RecvBytes As Byte() = UDPReceiver.Receive(RemoteEP)
                RecvData = Encoding.UTF8.GetString(RecvBytes)
                JsonObject = JObject.Parse(RecvData) 'dynamic json object variable that tries to parses data into a json style format for lookups efficiently
            Catch ex As Exception
                Continue While 'data simply not in JSON format
            End Try

            'message processing begins here as the message is in JSON format
            Try
                Dim MessageType As String = JsonObject("MessageType").ToString
                Select Case MessageType
                    Case "ConnectionRequest"
                        Dim ConReq As ConnectionRequest = JsonConvert.DeserializeObject(Of ConnectionRequest)(RecvData)

                        If ConReq.AppIDMessage <> "WAYFARER_V1" Then 'checking field of appid to make sure the device is using the correct application
                            Continue While 'skips the current loop and keeps listening for broadcasts
                        End If

                        ROOT_IP = JsonObject("RootAddress") 'pulls down the root ip from the request to use later
                        SendConnectionResponse(ConReq.DeviceIP)
                        'sends a response for the node to allow them to begin a tcp/ip session with this node for bidirectional comms
                        Exit While 'closes the udp receiver and kills it.
                    Case Else
                        Continue While 'whatever arrived is not what i need, throw it out
                End Select
            Catch ex As Exception
                Continue While 'data was of an incorrect json format
            End Try
        End While
        Me.Destruct() 'once connection response sent and root ip saved, exit the sub and release unmanaged resources used i.e. sockets and threads
    End Sub

    Private Sub SendConnectionResponse(DeviceIP As String) 'sends the conn response to the leaf and initialises the tcp handler for prevptr
        Dim ConResp As New ConnectionResponse(True)
        Dim Bytes() As Byte = Encoding.UTF8.GetBytes(ConResp.GetJSONMessage) 'gets the byte array of the message (response) to send to the broadcaster
        Try
            Dim Address As IPAddress = IPAddress.Parse(DeviceIP)
            Dim EP As New IPEndPoint(Address, GLOBAL_CLIENT_PORT)
            Try
                UDPSender.Close()
                UDPSender.Dispose()
            Catch ex As Exception
            End Try
            'resetting the UDP sender just in case it is bound to another socket still

            UDPSender = New UdpClient(EP) With {.EnableBroadcast = True}
            UDPSender.Send(Bytes, Bytes.Length)
        Catch ex As Exception
            UDPSender.Send(Bytes, Bytes.Length) 'try one more time just in case
        End Try
        PrevTCP = New TCPHandler(IPAddress.Parse(DeviceIP))
        IsLeaf = True 'create the referential link between these nodes
    End Sub

    Private Sub BroadcastConnectionRequest() 'send con requests while connection response not received, i.e. if IsLeaf = true
        While AppRunning AndAlso IsLeaf AndAlso IsSynchronised And Not KillHandler 'sends 20 (+-) requests per second of size < 1kb (so using a max of 160kbits/s, insignificant)
            Try
                Dim ConReq As New ConnectionRequest()
                Dim Bytes() As Byte = Encoding.UTF8.GetBytes(ConReq.GetJSONMessage)
                UDPSender.Send(Bytes, Bytes.Length)
                Thread.Sleep(50)
            Catch ex As Exception
                Continue While
            End Try
        End While
        Me.Destruct()
    End Sub

    Private Sub ListenForResponse() 'synchronised leaf nodes listening simultaneously for responses to their broadcasts
        While AppRunning AndAlso IsLeaf AndAlso IsSynchronised AndAlso Not KillHandler
            Dim JsonObject As Object
            Dim RecvData As String
            Try
                Dim RecvBytes As Byte() = UDPReceiver.Receive(RemoteEP) 'receive a udp datagram if there is one to receive
                RecvData = Encoding.UTF8.GetString(RecvBytes) 'convert bytes to string
                JsonObject = JObject.Parse(RecvData) 'dynamic json object variable that parses data into a json style format for lookups efficiently
            Catch ex As Exception
                Continue While
            End Try

            'data is in JSON format if it passed the first try-catch
            Try
                Dim MessageType As String = JsonObject("MessageType").ToString
                Select Case MessageType 'check what kind of message arrived
                    Case "ConnectionResponse"
                        Dim DeviceIP As IPAddress = IPAddress.Parse(JsonObject("DeviceIP"))
                        IsLeaf = False
                        NextTCP = New TCPHandler(DeviceIP) 'create the referential link between devices
                    Case Else
                        Continue While
                End Select
            Catch ex As Exception
                Continue While 'invalid json format
            End Try
        End While
        Destruct()
    End Sub

    Public Sub Destruct() 'disposes of the sockets used by UDP clients and kills the handler
        KillHandler = True
        Try
            UDPReceiver.Close()
            UDPReceiver.Dispose()
        Catch ex As Exception
        End Try

        Try
            UDPSender.Close()
            UDPSender.Dispose()
        Catch ex As Exception
        End Try
    End Sub
End Class