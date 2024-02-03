Imports System.IO

Public Class BlockChain
    Public Sub AddBlock(ByVal NewBlock As Block, Optional NeedsToMine As Boolean = True)
        If Not NeedsToMine Then
            GoTo SkipAndAddBlock
        End If
        NewBlock.MineBlock(Difficulty)
        While IsMining
            Continue While
        End While
SkipAndAddBlock: Chain.Add(NewBlock)
        Try
            CreateFileFromBlock(NewBlock)
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub

    Private Property Difficulty As Byte = 2 'Specifies limits for a hash - e.g. 6 trailing 0's in the hash (higher = more difficult to mine)
    Private Property Chain As List(Of Block)
    Public ReadOnly Property GetChain As List(Of Block)
        Get
            Return Chain
        End Get
    End Property
    Public Function GetLastBlock() As Block
        Return Chain.Last()
    End Function

    Public Function IsValidChain() As Boolean
        For I As Integer = 1 To Chain.Count - 1
            Dim CurrentBlock As Block = Chain.Item(I)
            Dim LastBlock As Block = Chain.Item(I - 1)
            If CurrentBlock.GetPrevHash <> LastBlock.GetHash() OrElse CurrentBlock.GetHash <> GetSHA256FromString(CurrentBlock.GetBlockDataForMining) OrElse CULng(CurrentBlock.GetTimestamp) < CULng(LastBlock.GetTimestamp) Then
                Return False
            End If
        Next
        Return True
    End Function

    Public Sub ReplaceNewChain(NewChain As BlockChain)
        If NewChain.Chain.Count < Chain.Count Then
            CustomMsgBox.ShowBox("Chain received is shorter than current chain!", "ERROR", False)
        ElseIf Not NewChain.IsValidChain() Then
            CustomMsgBox.ShowBox("Chain received is not valid!", "ERROR", False)
        Else
            Chain = NewChain.Chain
        End If
    End Sub

    Public Sub DeleteLastBlock()
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
        AddBlock(GenesisBlock)
    End Sub
End Class
