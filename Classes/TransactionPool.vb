Public Class TransactionPool
    Public Sub New()
        TransactionList = New List(Of Transaction)
    End Sub

    Private Property TransactionList As List(Of Transaction)
    Public Function GetTransactionListAsString() As String
        Try
            Dim TransactionString As String() = GetTransactionListAsArray(TransactionList)

            Return String.Join(vbCrLf, TransactionString)

        Catch ex As Exception
            Return "" 'empty pool
        End Try
    End Function

    Public Sub AddTransaction(Transact As Transaction)
        TransactionList.Add(Transact)
    End Sub
    Public Function FlushPool() As List(Of Transaction)
        Dim TempList As List(Of Transaction) = TransactionList
        TransactionList = New List(Of Transaction)
        Return TempList
    End Function
End Class
