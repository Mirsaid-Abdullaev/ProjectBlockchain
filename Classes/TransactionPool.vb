Imports System.Threading
Public Class TransactionPool
    Private ReadOnly CheckMiningProgressThread As Thread
    Public Sub New()
        TransactionList = New DataBufferQueue(Of Transaction)
        CheckMiningProgressThread = New Thread(AddressOf TrackMining) With {.IsBackground = True}
    End Sub

    Private TransactionList As DataBufferQueue(Of Transaction)
    Public ReadOnly Property GetTransactionList As List(Of Transaction)
        Get
            Return TransactionList.GetQueueAsList
        End Get
    End Property

    Public Sub AddTransaction(Transact As Transaction)
        TransactionList.Enqueue(Transact)
        If TransactionList.GetCurrentSize = MAX_TRANSACT_SIZE Then 'max transact size is the constant which says how many transactions allowed in a block
            'starts mining from here automatically
            ReachedMiningSeq = True

            If IsMiner Then
                CurrentBlock = New Block()
                For Each Transact In TransactionList.GetQueueAsList
                    CurrentBlock.AddTransaction(Transact)
                Next
                'block has all transacts now - can start mining
                CurrentBlock.MineBlock(WFBlockchain.Difficulty)
                CheckMiningProgressThread.Start()
            End If
        End If
    End Sub
    Private Sub TrackMining() 'run in the background to verify and keep track of when the block gets mined
        While IsMining And AppRunning And Not StopMining
            Continue While
        End While
        If Not IsValidNextBlock(CurrentBlock, WFBlockchain.LastBlock) Or StopMining Then
            Exit Sub 'something would have needed to be messed up to reach this code - this is just a precaution
        End If
        Dim VNMBRq As New ValidateNewMinedBlockRequest(CurrentBlock) 'initialises a request to send to its peers
        If IsLeaf Or IsRoot Then
            ReceivedBlockConfirmations = 1 'as you are an end-device, you only need one confirmation to validate the block - accepted limitation
        Else
            ReceivedBlockConfirmations = 0
        End If
        SendToBothNodes(VNMBRq.GetJSONMessage) 'send the request to up to both connected nodes (depending if they exist)
    End Sub
    Public Function FlushPool() As List(Of Transaction) 'gets all the elements contained in the pool and resets the underlying queue
        Dim TempList As List(Of Transaction) = TransactionList.GetQueueAsList
        TransactionList = New DataBufferQueue(Of Transaction)
        Return TempList
    End Function

    Public Function GetPoolSize() As Integer
        Return TransactionList.GetCurrentSize 'returns current pool size
    End Function
End Class
