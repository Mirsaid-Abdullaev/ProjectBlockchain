Imports System.Net
Imports System.Net.Sockets
Imports System.Text
Imports System.Threading
Imports Newtonsoft.Json.Linq
Imports Newtonsoft.Json

Public Class RootHandler
    Private UDPSender As UdpClient 'udp client used to send data to client
    Private UDPResponder As UdpClient 'udp client used to send a syncresponse to client
    Private ReadOnly UDPReceiver As UdpClient 'udp client used to send all blockchain data to client
    Private ReadOnly RootEP As New IPEndPoint(IPAddress.Any, ROOT_CLIENT_PORT) 'endpoint to listen on - receipt of data
    Private Property KillHandler As Boolean = False 'property to stop the backgorund threads

    Private ReadOnly RootListenThread As Thread 'thread to listen for sync requests on
    Private ReadOnly ResponseManagementThread As Thread 'thread to send data to devices on
    Private NodeQueue As New DataBufferQueue(Of String) 'queue of devices to synchronise (shouldnt be required as the connection process is linear but just in case)
    Public Sub New() 'sets the threads and the receiving client
        UDPReceiver = New UdpClient(ROOT_SERVER_PORT) With {.EnableBroadcast = False}
        RootListenThread = New Thread(AddressOf ListenForSyncReq) With {.IsBackground = True} 'root taking requests
        ResponseManagementThread = New Thread(AddressOf CheckDeviceQueue) With {.IsBackground = True} 'root responses to the found devices
    End Sub

    Public Sub Start() 'initialises the root handler - starts the threads
        If Not IsRoot Then
            CustomMsgBox.ShowBox("Error initialising RootHandler: device is not a ROOT", "ERROR", False)
            Exit Sub
        End If
        RootListenThread.Start()
        ResponseManagementThread.Start()
    End Sub

    Private Sub CheckDeviceQueue() 'checks and sends data to any nodes currently in the nodequeue
        While AppRunning And Not KillHandler
            If Not NodeQueue.IsEmpty Then 'nodequeue stores a string of the form "{DeviceIP},{StartIndex}"
                Dim Node As String() = NodeQueue.Dequeue().Split(",") 'process this data
                Dim DeviceIP As String = Node(0)
                Dim StartIndex As UInteger = Node(1)
                SendChainData(DeviceIP, StartIndex) 'start sending the online sync data to the client
            End If
        End While
        Destruct() 'stop itself once complete and release unmanaged resources
    End Sub

    Private Sub SendChainData(DeviceIP As String, StartIndex As UInteger) 'this is called from the thread when a new device registers
        'send blocks here one by one in the specified range
        Dim NodeAddress As IPAddress = IPAddress.Parse(DeviceIP)

        Dim TempBlockchainList As List(Of Block) = WFBlockchain.Blockchain.GetRange(StartIndex, WFBlockchain.Blockchain.Count - StartIndex)
        'gets a static list of the current blocks on the network to send to the client
        Try
            If UDPSender IsNot Nothing Then
                UDPSender.Close()
                UDPSender.Dispose()
            End If
        Catch ex As Exception
            UDPSender = Nothing
        End Try
        'resetting the udpclient for sending to the new endpoint
        Dim EP As New IPEndPoint(NodeAddress, ROOT_SERVER_PORT)
        UDPSender = New UdpClient(ROOT_CLIENT_PORT)

        'sends the blocks on the network to the client one by one using the correct json schema
        For Each Item As Block In TempBlockchainList
            Dim BlockResponse As New BlockResponse(Item, True)
            Dim SndData As String = BlockResponse.GetJSONMessage
            Dim SndBytes As Byte() = Encoding.UTF8.GetBytes(SndData)
            UDPSender.Send(SndBytes, SndBytes.Length, EP)
        Next

        'sends the transaction pool to the client in the schema specified
        Dim TransactPoolResponse As New TransactionPoolResponse(True, WFTransactionPool)
        Dim Bytes As Byte() = Encoding.UTF8.GetBytes(TransactPoolResponse.GetJSONMessage)
        UDPSender.Send(Bytes, Bytes.Length, EP)
        UDPSender.Close() 'closes the client socket after transmission
        UDPSender.Dispose()
    End Sub

    Private Sub ListenForSyncReq() 'registers when a new device sends a syncrequest
        While AppRunning And Not KillHandler
            Dim RecvData As String
            Dim JsonObject As Object

            Try
                Dim RecvBytes As Byte() = UDPReceiver.Receive(RootEP) 'tries to see if any UDP datagrams have been sent
                RecvData = Encoding.UTF8.GetString(RecvBytes)
                JsonObject = JObject.Parse(RecvData) 'dynamic json object variable that parses data into a json style format for lookups efficiently
            Catch ex As Exception
                Continue While
            End Try

            'if data has gotten to this point, it is in some form of json
            Try
                Dim MessageType As String = JsonObject("MessageType").ToString
                Select Case MessageType 'check for a correct data type
                    Case "SyncRequest" 'add the device to the node queue for data sending
                        Dim SyncReq As SyncRequest = JsonConvert.DeserializeObject(Of SyncRequest)(RecvData)
                        NodeQueue.Enqueue(String.Join(",", {SyncReq.DeviceIP, SyncReq.StartBlock})) 'device IP and startindex added, split by commas
                        SendSyncResponse(SyncReq.DeviceIP, SyncReq.StartBlock)
                        'need to send this device all the blocks it needs for synchronisation
                    Case Else
                        Continue While 'something else was received here, ignore
                End Select

            Catch ex As Exception
                Continue While 'the underlying json message was not of the correct format
            End Try

        End While
    End Sub
    Private Sub SendSyncResponse(DeviceIP As String, StartBlock As UInteger)
        Dim NodeAddress As IPAddress = IPAddress.Parse(DeviceIP)
        Dim SyncResp As New SyncResponse(True, StartBlock)
        Dim SndBytes As Byte() = Encoding.UTF8.GetBytes(SyncResp.GetJSONMessage())

        Dim EP As New IPEndPoint(NodeAddress, ROOT_SERVER_PORT)
        Try
            If UDPResponder IsNot Nothing Then
                UDPResponder.Close()
                UDPResponder.Dispose()
            End If
        Catch ex As Exception
            UDPResponder = Nothing
        End Try

        UDPResponder = New UdpClient(ROOT_CLIENT_PORT)
        UDPResponder.Send(SndBytes, SndBytes.Length, EP) 'sends a syncresponse to the device
    End Sub

    Public Sub Destruct() 'disposes of the sockets used by UDP clients and kills the handler
        KillHandler = True
        Try
            If UDPReceiver IsNot Nothing Then
                UDPReceiver.Close()
                UDPReceiver.Dispose()
            End If
        Catch ex As Exception
        End Try

        Try
            If UDPSender IsNot Nothing Then
                UDPSender.Close()
                UDPSender.Dispose()
            End If
        Catch ex As Exception
            UDPSender = Nothing
        End Try
        Try
            If UDPResponder IsNot Nothing Then
                UDPResponder.Close()
                UDPResponder.Dispose()
            End If
        Catch ex As Exception
            UDPResponder = Nothing
        End Try
    End Sub
End Class
