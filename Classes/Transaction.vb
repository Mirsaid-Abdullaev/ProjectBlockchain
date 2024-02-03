Public Class Transaction
    Public ReadOnly Property Timestamp As String 'timestamp of transaction
    Public ReadOnly Property Sender As String 'sender address
    Public ReadOnly Property Recipient As String 'recipient address
    Public ReadOnly Property Quantity As Double 'quantity of transfer

    Public Sub New(Sender As String, Recipient As String, Quantity As Double) 'auto transaction
        Timestamp = TimeToUnixMs(Date.Now)
        Me.Sender = Sender
        Me.Recipient = Recipient
        Me.Quantity = Quantity
    End Sub

    Public Sub New(Timestamp As String, Sender As String, Recipient As String, Quantity As Double)
        Me.Timestamp = Timestamp
        Me.Sender = Sender
        Me.Recipient = Recipient
        Me.Quantity = Quantity
    End Sub

    Public Overrides Function ToString() As String 'returns "[timestamp, sender, recipient, quantity]"
        Dim TransactData As String = "[" & String.Join(", ", {Timestamp, Sender, Recipient, Quantity.ToString}) & "]"
        Return TransactData
    End Function
End Class

Module TransactionOperations
    Public Function GetTransactionListFromString(TransactData As String) As List(Of Transaction)
        Dim CurrentPos As Integer = 0
        Dim TempTransaction As String = ""

        Dim TransactList As New List(Of Transaction)
        Try
            While CurrentPos <= TransactData.Length - 2
                Dim CurrentChar As String = TransactData(CurrentPos)
                If CurrentChar = "[" Then
                    CurrentPos += 1
                    CurrentChar = TransactData(CurrentPos)
                    While CurrentChar <> "]"
                        TempTransaction += CurrentChar
                        CurrentPos += 1
                        CurrentChar = TransactData(CurrentPos)
                    End While
                    TransactList.Add(GetTransactionFromString(TempTransaction))
                    TempTransaction = ""
                    While CurrentPos <= TransactData.Length - 2 AndAlso CurrentChar <> "["
                        CurrentPos += 1
                        CurrentChar = TransactData(CurrentPos)
                    End While
                End If
            End While
        Catch ex As Exception
            Return Nothing
        End Try
        Return TransactList
    End Function

    Public Function GetTransactionFromString(TempTransaction As String) As Transaction 'of the form Timestamp, Sender, Recipient, Quantity
        ' Remove brackets and split the string into individual components
        Dim Components As String() = TempTransaction.Split(", ")

        ' Extract values and create a Transaction object
        Dim Timestamp As String = Components(0).Replace(" ", "")
        Dim Sender As String = Components(1).Replace(" ", "")
        Dim Recipient As String = Components(2).Replace(" ", "")
        Dim Quantity As Double = Double.Parse(Components(3))

        ' Create and return the Transaction object
        Return New Transaction(Timestamp, Sender, Recipient, Quantity)
    End Function

    Public Function GetTransactionListAsString(TransactList As List(Of Transaction)) As String
        Try
            Return String.Join(",", TransactList)
        Catch ex As Exception
            Return ""
        End Try
    End Function

    Public Function GetTransactionListAsArray(TransactList As List(Of Transaction)) As String()
        If TransactList.Count = 0 Then
            Return Nothing
        End If
        Dim TransactArray(TransactList.Count - 1) As String
        Dim Counter As UInteger = 0
        For Each item In TransactList
            TransactArray(Counter) = item.ToString
            Counter += 1
        Next 'returns an array filled with string transacts
        Return TransactArray
    End Function

End Module