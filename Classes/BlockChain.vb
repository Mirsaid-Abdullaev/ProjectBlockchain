Imports System.IO

Public Class BlockChain
    Public ReadOnly Property Difficulty As Byte = 2 'Specifies limits for a hash - e.g. 6 trailing 0's in the hash (higher = more difficult to mine)
    Private Chain As List(Of Block) 'underlying list of blocks in the blockchain
    Public ReadOnly Property Blockchain As List(Of Block)
        Get
            Return Chain
        End Get
    End Property 'getter for the underlying chain
    Public ReadOnly Property LastBlock As Block
        Get
            Return Chain.Last() 'returns the last block on the chain
        End Get
    End Property

    Public Function IsValidChain() As Boolean 'validates the chain to check if all blocks and their relations are valid by common rules and definitions
        For I As Integer = 1 To Chain.Count - 1 'starts at 1 as block 0 is always correct and can take for granted
            Dim CurrentBlock As Block = Chain.Item(I)
            Dim LastBlock As Block = Chain.Item(I - 1)
            If Not IsValidNextBlock(CurrentBlock, LastBlock) Then
                Return False
            End If
        Next
        Return True
    End Function

    Public Sub DeleteLastBlock() 'literally deletes the last block from the list and also from the file system
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
        AddBlock(GENESIS_BLOCK) 'adds the genesis block by default
    End Sub

    Public Sub AddBlock(NewBlock As Block) 'adds a block to the underlying list of blocks and attempts local copy save
        Chain.Add(NewBlock)
        Try
            CreateFileFromBlock(NewBlock)
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub
    Public Sub UpdateBlockchain(BlockList As List(Of Block)) 'takes in a list of blocks received from root handler client, and adds them to the chain
        Dim LocalList As List(Of Block) = BlockList 'makes a copy of the list
        LocalList.Sort(New BlockIndexComparer()) 'sorts the list according to their indexes, in ascending order
        For I = 0 To LocalList.Count - 1 'iterating through the blocks, they are in index-sorted ascending order now
            Dim Item As Block = LocalList(I)

            If Item.GetIndex <= Chain.Last.GetIndex Then 'this means data is not necessary, we need to skip through
                Continue For
            Else
                AddBlock(Item)
            End If
        Next

        If Not Me.IsValidChain() Then 'either node data is messed up, or received data is messed up - try delete all invalid blocks
            While Not Me.IsValidChain()
                Me.DeleteLastBlock() 'keep deleting the last block and checking if chain is valid
            End While
            CustomMsgBox.ShowBox("Error: Root device sent incomplete/corrupt data. Application will exit now.")
            IsSynchronised = False
            DisconnectFromChain()
            Application.Exit() 'exit the application, as synchronisation failed.
        End If
    End Sub
End Class
