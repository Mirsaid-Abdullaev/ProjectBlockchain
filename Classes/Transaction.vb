Public Class Transaction
    Public ReadOnly Property Timestamp As String 'timestamp of transaction
    Public ReadOnly Property Sender As String 'sender address
    Public ReadOnly Property Recipient As String 'recipient address
    Public ReadOnly Property Quantity As Double 'quantity of transfer
    Public ReadOnly Property Fee As Double 'quantity for the miner of the block - new addition to the structure

    Public Sub New(Sender As String, Recipient As String, Quantity As Double, Fee As Double) 'auto transaction used for generating user-generated transacts
        Timestamp = TimeToUnixMs(Date.Now)
        Me.Sender = Sender
        Me.Recipient = Recipient
        Me.Quantity = Quantity
        Me.Fee = Fee
    End Sub

    Public Sub New(Sender As String, Recipient As String, Quantity As Double, Fee As Double, Timestamp As String) 'used for loading transacts
        Me.Timestamp = Timestamp
        Me.Sender = Sender
        Me.Recipient = Recipient
        Me.Quantity = Quantity
        Me.Fee = Fee
    End Sub

    Public Overrides Function ToString() As String 'returns "[timestamp, sender, recipient, quantity, fee]"
        Dim TransactData As String = "[" & String.Join(", ", {Timestamp, Sender, Recipient, Quantity.ToString, Fee.ToString}) & "]"
        Return TransactData 'custom overriden method to return formatted string of object, like JSON
    End Function
End Class

Module TransactionOperations
    Public Function GetTransactionListFromString(TransactData As String) As List(Of Transaction)
        Dim CurrentPos As Integer = 0 'current character index
        Dim TempTransaction As String = "" 'transaction string holder variable from data

        Dim TransactList As New List(Of Transaction) 'final result list
        Try
            While CurrentPos <= TransactData.Length - 2 'the end counter is the index of the last char before the final "]" close
                Dim CurrentChar As String = TransactData(CurrentPos) 'set the current char
                If CurrentChar = "[" Then 'skip opening bracket and start collecting transaction string
                    CurrentPos += 1 'push forward the counter
                    CurrentChar = TransactData(CurrentPos) 'reset the character
                    While CurrentChar <> "]" 'loop to create the transaction contained within the [] chars
                        TempTransaction += CurrentChar 'keep adding current character until ] reached
                        CurrentPos += 1
                        CurrentChar = TransactData(CurrentPos)
                    End While
                    'call the helper method to convert the resulting temp transaction and add to resulting list
                    TransactList.Add(GetTransactionFromString(TempTransaction))
                    TempTransaction = "" 'reset the temp transaction store
                    While CurrentPos <= TransactData.Length - 2 AndAlso CurrentChar <> "[" 'loop ahead to the next opening brackets
                        CurrentPos += 1
                        CurrentChar = TransactData(CurrentPos)
                    End While
                End If
            End While
        Catch ex As Exception
            Return Nothing 'any errors return Nothing, as the transaction list had to have been invalid anyway
        End Try
        Return TransactList
    End Function

    Public Function GetTransactionFromString(TempTransaction As String) As Transaction
        'split the string into individual components - currently in form "timestamp, sender, recipient, quantity, fee" without the square brackets
        Dim Components As String() = TempTransaction.Split(", ") 'gets each component by itself

        ' Extract values and create a Transaction object
        Dim Timestamp As String = Components(0).Replace(" ", "") 'remove any leading/trailing spaces
        Dim Sender As String = Components(1).Replace(" ", "") 'same as above
        Dim Recipient As String = Components(2).Replace(" ", "") 'same as above
        Dim Quantity As Double = Double.Parse(Components(3)) 'direct parsing to double from string
        Dim Fee As Double = Double.Parse(Components(4)) 'same as above

        ' Create and return the Transaction object
        Return New Transaction(Sender, Recipient, Quantity, Fee, Timestamp)
    End Function

    Public Function GetTransactionListAsString(TransactList As List(Of Transaction)) As String
        Try
            Return String.Join(",", TransactList) 'gets a big string of transact strings separated by commas, "[Transact1], [Transact2], ... "
        Catch ex As Exception
            Return "" 'empty list = empty string
        End Try
    End Function

    Public Function GetTransactionListAsArray(TransactList As List(Of Transaction)) As String()
        If TransactList.Count = 0 Then
            Return Nothing
        End If 'nothing is empty list
        Dim TransactArray(TransactList.Count - 1) As String 'array initialisation
        Dim Counter As UInteger = 0 'to set the array index
        For Each item In TransactList 'loop through list
            TransactArray(Counter) = item.ToString 'save each transact as string to index
            Counter += 1
        Next 'returns an array filled with string transacts
        Return TransactArray
    End Function

End Module