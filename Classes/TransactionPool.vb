Public Class TransactionPool
    Public Sub New()
        TransactionList = New DataBufferQueue(Of Transaction)
    End Sub

    Private Property TransactionList As DataBufferQueue(Of Transaction)
    Public Function GetTransactionList() As List(Of Transaction)
        Return TransactionList.GetQueueAsList
    End Function

    Public Sub AddTransaction(Transact As Transaction)
        TransactionList.Enqueue(Transact)
        If TransactionList.GetCurrentSize = MAX_TRANSACT_SIZE Then
            'start mining from here automatically
            ReachedMiningSeq = True

            If IsMiner Then
                CurrentBlock = New Block()
                For Each Transact In TransactionList.GetQueueAsList
                    CurrentBlock.AddTransaction(Transact)
                Next
                'block has all transacts now - can start mining
                CurrentBlock.MineBlock(WFBlockchain.Difficulty)
            Else

            End If
        End If
    End Sub
    Public Function FlushPool() As List(Of Transaction) 'gets all the elements contained in the pool and resets the underlying queue
        Dim TempList As List(Of Transaction) = TransactionList.GetQueueAsList
        'If NumOfElements = -1 Then 'all of the elements
        '    TransactionList = New DataBufferQueue(Of Transaction)
        '    Return TempList
        'End If
        'TempList = TempList.GetRange(0, NumOfElements) 'this will usually be the MAX_TRANSACTPOOL_SIZE
        'For i As Integer = 1 To NumOfElements
        '    TransactionList.DeleteLastElement() 'removes the transactions from the pool
        'Next
        'Return TempList
        TransactionList = New DataBufferQueue(Of Transaction)
        Return TempList
    End Function

    Public Function GetPoolSize() As Integer
        Return TransactionList.GetCurrentSize
    End Function
End Class
