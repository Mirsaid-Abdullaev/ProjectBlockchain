Public Class Transaction
    Public ReadOnly Property Timestamp As String 'timestamp of transaction
    Public ReadOnly Property Sender As String 'sender address
    Public ReadOnly Property Recipient As String 'recipient address
    Public ReadOnly Property Quantity As UInteger 'quantity of transfer

    Public Sub New(iTimestamp As String, iSender As String, iRecipient As String, iQuantity As UInteger)
        Timestamp = iTimestamp
        Sender = iSender
        Recipient = iRecipient
        Quantity = iQuantity
    End Sub

    Public Function ReturnStringOfTransaction() As String
        Dim TransactData As String = Timestamp + Sender + Recipient + Quantity.ToString
        Return TransactData
    End Function
End Class
