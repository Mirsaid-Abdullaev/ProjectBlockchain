Public Class TransactionPool

    Private Property TransactionList As List(Of Transaction)
    Public Function GetTransactions() As String
        Try
            Return String.Join(",", TransactionList)
        Catch ex As Exception
            Return "" 'empty pool
        End Try
    End Function

    Friend Function GetContents() As String
        Return ""
        'when implemented, should return a string of transactions in the current transactionpool
    End Function
End Class
