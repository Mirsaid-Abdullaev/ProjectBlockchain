Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports System.Net
Imports System.Net.Sockets
Imports System.Threading
Imports System.Text

Public Class ConnectionHandler
    Private UDPSender As UdpClient
    Private UDPReceiver As UdpClient

    Private BroadcastThread As Thread
    Private ListenerThread As Thread
    Private ConRespThread As Thread

    Public Sub Start()
        If IsLeaf AndAlso IsSynchronised Then 'sending connection requests (as you are the leaf node and synchronised to the chain now)
            UDPSender = New UdpClient(BroadcastEP) With {.EnableBroadcast = True}
            BroadcastThread = New Thread(AddressOf BroadcastConnectionRequest) With {.IsBackground = True}
            BroadcastThread.Start()
            'also listening for connection responses
            UDPReceiver = New UdpClient(RemoteEP) With {.EnableBroadcast = False}
            ConRespThread = New Thread(AddressOf ListenForResponse) With {.IsBackground = True}
            ConRespThread.Start()
        Else 'listening for an incoming connection request only, as you are not synchronised or the leaf node
            UDPReceiver = New UdpClient(RemoteEP) With {.EnableBroadcast = False}
            UDPSender = Nothing
            ListenerThread = New Thread(AddressOf ListenForBroadcasts) With {.IsBackground = True}
            ListenerThread.Start()
        End If
    End Sub

    Public Sub Disconnect() 'for a device leaving the application to notify its peers that it doesnt exist anymore
        Try
            Dim DCRequest As New DisconnectRequest()
            If PrevTCP IsNot Nothing Then
                PrevTCP.SendData(DCRequest.GetJSONMessage)
            End If
            If NextTCP IsNot Nothing Then
                NextTCP.SendData(DCRequest.GetJSONMessage)
            End If
        Catch ex As Exception
            CustomMsgBox.ShowBox($"Error in UDPHandler.Disconnect sub, error: {ex.Message}")
            Disconnect() 'i dont know why this is a good idea but it should work
        End Try
    End Sub
    Private Sub ListenForBroadcasts() 'awesome ostrich algorithm use-case
        While AppRunning AndAlso Not IsSynchronised
            UDPReceiver = New UdpClient(RemoteEP) With {.EnableBroadcast = False}
            UDPReceiver.Client.ReceiveTimeout = 2000
            Try
                Dim RecvBytes As Byte() = UDPReceiver.Receive(RemoteEP)
                Dim RecvData As String = Encoding.UTF8.GetString(RecvBytes)
                Dim JsonObject As Object = JObject.Parse(RecvData) 'dynamic json object variable that parses data into a json style format for lookups efficiently
                Dim MessageType As String = JsonObject("MessageType").ToString
                Select Case MessageType
                    Case "ConnectionRequest"
                        Dim ConReq As ConnectionRequest = JsonConvert.DeserializeObject(Of ConnectionRequest)(RecvData) 'deserialize i.e. construct a new instance of conreq class
                        If ConReq.AppIDMessage <> "WAYFARER_V1" Then 'checking field of appid to make sure the device is using the correct application
                            Throw New Exception 'breaks the current loop and keeps listening for broadcasts
                        End If
                        PreviousPointer = ConReq.DeviceIP
                        ROOT_IP = JsonObject("RootAddress")
                        SendConnectionResponse(PreviousPointer)
                        'sends a response for the node to allow them to begin a tcp/ip session with this node for bidirectional comms

                        Dim SyncReq As New SyncRequest() 'sending to root node to sync with it
                        Dim TempTCPHandler As New TCPHandler(IPAddress.Parse(ROOT_IP))
                        TempTCPHandler.SendData(SyncReq.GetJSONMessage)
                        Exit Sub
                    Case Else
                        Throw New Exception 'whatever arrived is not what i need, throw it out
                End Select
            Catch ex As Exception
                CustomMsgBox.ShowBox($"Error in UDPHandler.Listen thread, error: {ex.Message}")
                Continue While
            End Try
        End While
    End Sub

    Private Function SendConnectionResponse(DeviceIP As String) As Boolean 'sends the conn response to the leaf and initialises the tcp handler for prevptr
        Dim ConResp As New ConnectionResponse("Accepted")
        Dim Bytes() As Byte = Encoding.UTF8.GetBytes(ConResp.GetJSONMessage)
        Try
            Dim Address As IPAddress = IPAddress.Parse(DeviceIP)
            Dim EP As New IPEndPoint(Address, ClientPort)
            UDPSender = New UdpClient(EP) With {.EnableBroadcast = True}
            UDPSender.Send(Bytes, Bytes.Length)
        Catch ex As Exception
            CustomMsgBox.ShowBox($"Error in UDPHandler.SendconResp function, error: {ex.Message}")
            Return False
        End Try
        PrevTCP = New TCPHandler(IPAddress.Parse(DeviceIP))
        IsLeaf = True
        Return True
    End Function

    Private Sub BroadcastConnectionRequest() 'send con requests while connection response not received, i.e. if IsLeaf = true
        While AppRunning AndAlso IsLeaf AndAlso IsSynchronised 'sends 20 (+-) requests per second of size < 1kb (so using a max of 160kbits/s, insignificant)
            Try
                Dim ConReq As New ConnectionRequest()
                Dim Bytes() As Byte = Encoding.UTF8.GetBytes(ConReq.GetJSONMessage)
                UDPSender.Send(Bytes, Bytes.Length)
                Thread.Sleep(50)
            Catch ex As Exception
                CustomMsgBox.ShowBox($"Error in UDPHandler.Broadcast thread, error: {ex.Message}")
                Continue While
            End Try
        End While
    End Sub

    Private Sub ListenForResponse()
        While AppRunning AndAlso IsLeaf AndAlso IsSynchronised
            Try
                Dim RecvBytes As Byte() = UDPReceiver.Receive(RemoteEP)
                Dim RecvData As String = Encoding.UTF8.GetString(RecvBytes)
                Dim JsonObject As Object = JObject.Parse(RecvData) 'dynamic json object variable that parses data into a json style format for lookups efficiently
                Dim MessageType As String = JsonObject("MessageType").ToString
                Select Case MessageType
                    Case "ConnectionResponse"
                        Dim DeviceIP As IPAddress = IPAddress.Parse(JsonObject("DeviceIP"))
                        NextPointer = DeviceIP.ToString
                        IsLeaf = False
                        NextTCP = New TCPHandler(DeviceIP)
                    Case Else
                        Throw New Exception
                End Select
            Catch ex As Exception

            End Try
        End While
    End Sub
End Class