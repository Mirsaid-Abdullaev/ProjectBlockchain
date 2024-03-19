Imports System.Net
Imports System.Net.Sockets
Imports System.Threading
Imports System.Text
Imports Newtonsoft.Json.Linq
Imports Newtonsoft.Json

Public Class RootHandlerClient
    Private ReadOnly UDPSender As UdpClient
    Private ReadOnly UDPReceiver As UdpClient
    Private ReadOnly ReceivingEP As New IPEndPoint(RootAddress, ROOT_CLIENT_PORT)
    Private ReadOnly RootEP As New IPEndPoint(RootAddress, ROOT_SERVER_PORT) 'endpoint for root comms

    Private ReadOnly RootAddress As IPAddress 'address of the root node
    Private Property KillHandler As Boolean = False 'flag to stop thread 
    Private ReadOnly ListenForDataThread As New Thread(AddressOf ListenForData) With {.IsBackground = True} 'thread for listening to responses from root

    Private ReceivedBlocks As List(Of String) 'list for storing inbound blocks
    Public ReadOnly Property GetReceivedBlocks As List(Of Block) 'property to convert each of the blockresponse jsons into block instances
        Get
            Dim Blocks As List(Of String) = ReceivedBlocks

            Dim BlockList As New List(Of Block)
            For Each Item As String In Blocks
                Dim BlockResp As BlockResponse = JsonConvert.DeserializeObject(Of BlockResponse)(Item)
                Dim Block As New Block(BlockResp.Hash,
                                       BlockResp.PrevHash,
                                       BlockResp.Index,
                                       BlockResp.Nonce,
                                       BlockResp.Timestamp,
                                       GetTransactionListFromString(BlockResp.Transactions),
                                       BlockResp.Miner)
                BlockList.Add(Block)
            Next
            Return BlockList
        End Get
    End Property
    Private ExpectedDataItems As UInteger
    Public HasFinished As Boolean = False
    Public HasTimedOut As Boolean = False
    Public Sub New(StartBlock As UInteger)
        If IsRoot OrElse IsSynchronised Then
            CustomMsgBox.ShowBox("Unable to start RootHandlerClient as device is not in the correct state for this.", "ERROR", False)
            Exit Sub
        End If

        If Not IPAddress.TryParse(ROOT_IP, RootAddress) Then 'failed to parse root ip
            CustomMsgBox.ShowBox("Unable to start RootHandlerClient: ROOT_IP was not valid.", "ERROR", False)
            Exit Sub
        End If
        'if the conversion succeeded, rootaddress now holds the root ip

        UDPReceiver = New UdpClient(ROOT_SERVER_PORT)
        UDPSender = New UdpClient(ROOT_CLIENT_PORT)

        Dim SyncReq As New SyncRequest(StartBlock)
        Dim SndBytes As Byte() = Encoding.UTF8.GetBytes(SyncReq.GetJSONMessage)
        UDPSender.Send(SndBytes, SndBytes.Length, RootEP) 'sends a sync request to the root

        ExpectedDataItems = 1000000 'to begin with, set to 1 million to allow the listen loop to run without problems, but this will change once a sync response is received
        ReceivedBlocks = New List(Of String) 'init list
        ListenForDataThread.Start() 'starts the listening thread
    End Sub

    Private Sub ListenForData() 'monitors incoming data from the root
        Dim TimeOutSW As New Stopwatch()
        TimeOutSW.Start() 'timeout mechanism of 2 minutes started for receiving all the data
        While ReceivedBlocks.Count < ExpectedDataItems AndAlso Not KillHandler
            If TimeOutSW.ElapsedMilliseconds > 120000 Then '120 seconds have passed
                HasTimedOut = True 'sets the timeout flag to true - this is checked in the onlinesync procedure
            End If

            Dim RecvData As String
            Dim JsonObject As Object

            Try
                Dim RecvBytes As Byte() = UDPReceiver.Receive(ReceivingEP)
                RecvData = Encoding.UTF8.GetString(RecvBytes)
                JsonObject = JObject.Parse(RecvData)
            Catch ex As Exception
                Continue While
            End Try

            'data is in json format, now need to see if it is in the correct json schema format
            Try
                Dim MessageType As String = JsonObject("MessageType")
                Select Case MessageType
                    Case "SyncResponse" 'received sync response from root - need to record and set the expected blocks property to keep track of received data
                        Dim ExpectedBlocks As UInteger = JsonObject("ExpectedBlocks")
                        ExpectedDataItems = ExpectedBlocks 'saving this for other block data - to keep track of how many received vs expecting
                    Case "TransactionPoolResponse"
                        Dim TransPoolResp As TransactionPoolResponse = JsonConvert.DeserializeObject(Of TransactionPoolResponse)(RecvData)
                        Dim TransPool As List(Of Transaction) = GetTransactionListFromString(TransPoolResp.Transactions)
                        WFTransactionPool.FlushPool()
                        'flush/reset the transact pool, and add all the transactions one by one into the empty pool
                        For Each Transact As Transaction In TransPool
                            WFTransactionPool.AddTransaction(Transact)
                        Next
                        'finished processing the transact pool
                    Case "BlockResponse"
                        ReceivedBlocks.Add(RecvData) 'just add the entire json data to the queue of blocks
                    Case Else
                        Continue While 'erroneous data
                End Select
            Catch ex As Exception
                Continue While 'erroneous data
            End Try
        End While
        HasFinished = True 'sets the property to true
        Destruct() 'releases unmanaged resources/sockets
    End Sub

    Public Sub Destruct() 'same as in other communication handlers, makes sure all sockets are unbound if any still bound, and disposes of unmanaged resources
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
        End Try
    End Sub
End Class
