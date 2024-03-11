Imports System.IO

Public Class BlockChain
    Public Sub AddBlock(NewBlock As Block) 'adds a block to the underlying list of blocks and attempts local copy save
        Chain.Add(NewBlock)
        Try
            CreateFileFromBlock(NewBlock)
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub

    Public ReadOnly Property Difficulty As Byte = 2 'Specifies limits for a hash - e.g. 6 trailing 0's in the hash (higher = more difficult to mine)
    'will pull this out to be a global const to allow all processes to use it by reference and not hardcoding it in all code manually
    Private Property Chain As List(Of Block) 'underlying list of blocks in the blockchain
    Public ReadOnly Property GetChain As List(Of Block)
        Get
            Return Chain
        End Get
    End Property 'getter for the chain
    Public ReadOnly Property GetLastBlock As Block
        Get
            Return Chain.Last() 'returns the last block on the chain
        End Get
    End Property

    Public Function IsValidChain() As Boolean 'validates the chain to check if all blocks and their relations are valid by common rules and definitions
        For I As Integer = 1 To Chain.Count - 1 'starts at 1 as block 0 is always correct and can take for granted
            Dim CurrentBlock As Block = Chain.Item(I)
            Dim LastBlock As Block = Chain.Item(I - 1)
            If CurrentBlock.GetPrevHash <> LastBlock.GetHash() OrElse CurrentBlock.GetHash <> GetSHA256FromString(CurrentBlock.GetBlockDataForMining) OrElse CULng(CurrentBlock.GetTimestamp) < CULng(LastBlock.GetTimestamp) OrElse CurrentBlock.GetIndex <> LastBlock.GetIndex + 1 Then
                Return False
            End If
        Next
        Return True
    End Function

    Public Sub ReplaceNewChain(NewChain As BlockChain) 'possibly redundant at the moment
        If NewChain.Chain.Count < Chain.Count Then
            CustomMsgBox.ShowBox("Chain received is shorter than current chain!", "ERROR", False)
        ElseIf Not NewChain.IsValidChain() Then
            CustomMsgBox.ShowBox("Chain received is not valid!", "ERROR", False)
        Else
            Chain = NewChain.Chain
        End If
    End Sub

    Public Sub DeleteLastBlock() 'literally deletes the last block from the list but also from the file system
        If Chain.Count > 1 Then
            Me.Chain.RemoveAt(Chain.Count - 1)
            Dim FilePath As String = DirectoryList(1) & "Block" & Chain.Count.ToString
            If File.Exists(FilePath) Then
                File.Delete(FilePath)
            End If
        Else
            CustomMsgBox.ShowBox("Error: Blockchain is empty - no blocks to delete. Not altered the chain", "ERROR", False)
        End If
    End Sub

    Public Sub New()
        Chain = New List(Of Block)
        AddBlock(GenesisBlock) 'adds the genesis block by default
    End Sub
End Class
