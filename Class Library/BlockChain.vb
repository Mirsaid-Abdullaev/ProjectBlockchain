Public Class BlockChain
    Const GenesisData As String = "GenesisBlock*GenesisBlock*0*0*GenesisBlock*"
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
        If Chain.First.BlockData <> GenesisData Then
            Return False
        Else
            For I As Integer = 1 To Chain.Count
                Dim TempBlock As Block = Chain.Item(I)
                Dim LastBlock As Block = Chain.Item(I - 1)
                If TempBlock.PrevHash <> LastBlock.Hash Or TempBlock.Hash <> GetSHA256HashFromString(TempBlock.BlockData) Then
                    Return False
                End If
            Next
        End If
        Return True
    End Function

    Public Sub ReplaceNewChain(NewChain As BlockChain)
        If NewChain.Chain.Count < Chain.Count Then
            MsgBox("Chain received is shorter than current chain!")
        ElseIf Not newchain.IsValidChain() Then
            MsgBox("Chain received is not valid!")
        Else
            MsgBox("Replacing chain with new chain: ")
            Chain = NewChain.Chain
        End If
    End Sub

    Public Sub New()
        'make a genesis block
        Dim GenesisBlock As New Block("Genesis")
        GenesisBlock.GetBlockDataAsString()
        Chain = New List(Of Block)
        AddBlock(GenesisBlock)
    End Sub
End Class
