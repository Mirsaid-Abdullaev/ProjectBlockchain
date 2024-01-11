Imports System.Reflection
Imports System.Security.Policy
Public Class BlockChain
    Private ReadOnly GenesisData As String = String.Join(vbLf, {"Block: 0", "Previous hash: GenesisBlock", "Nonce: 0", "Timestamp: GenesisBlock", "Transactions: "})
    Public Sub AddBlock(ByVal NewBlock As Block)
        NewBlock.MineBlock(Difficulty)
        Chain.Add(NewBlock)
    End Sub

    Private Property Difficulty As Byte = 1 'Specifies limits for a hash - e.g. 6 trailing 0's in the hash (higher = more difficult to mine)
    Private Property Chain As List(Of Block)
    Public ReadOnly Property Blockchain As List(Of Block)
        Get
            Return Chain
        End Get
    End Property
    Private Function GetLastBlock() As Block
        Return Chain.Last()
    End Function

    Public Function IsValidChain() As Boolean
        If Chain.First.GetBlockDataForMining() <> GenesisData Then
            Return False
        Else
            For I As Integer = 1 To Chain.Count
                Dim CurrentBlock As Block = Chain.Item(I)
                Dim LastBlock As Block = Chain.Item(I - 1)
                If CurrentBlock.GetPrevHash <> LastBlock.GetHash() Or CurrentBlock.GetHash <> GetSHA256HashFromString(CurrentBlock.GetBlockDataForMining) Then
                    Return False
                End If
            Next
        End If
        Return True
    End Function

    Public Sub ReplaceNewChain(NewChain As BlockChain)
        If NewChain.Chain.Count < Chain.Count Then
            CustomMsgBox.ShowBox("Chain received is shorter than current chain!", "ERROR", False)
        ElseIf Not newchain.IsValidChain() Then
            CustomMsgBox.ShowBox("Chain received is not valid!", "ERROR", False)
        Else
            Chain = NewChain.Chain
        End If
    End Sub

    Public Sub New()
        'make a genesis block
        Dim GenesisBlock As New Block("Genesis")
        GenesisBlock.GetBlockDataForMining()
        Chain = New List(Of Block)
        AddBlock(GenesisBlock)
    End Sub
End Class
