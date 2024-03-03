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
    End Sub
    Public Function FlushPool(NumOfElements As Integer) As List(Of Transaction)
        Dim TempList As List(Of Transaction) = TransactionList.GetQueueAsList
        If NumOfElements = -1 Then 'all of the elements
            TransactionList = New DataBufferQueue(Of Transaction)
            Return TempList
        End If
        TempList = TempList.GetRange(0, NumOfElements) 'this will usually be the MAX_TRANSACTPOOL_SIZE
        For i As Integer = 1 To NumOfElements
            TransactionList.DeleteLastElement() 'removes the transactions from the pool
        Next
        Return TempList
    End Function

    Public Function GetPoolSize() As Integer
        Return TransactionList.GetCurrentSize
    End Function
End Class
