Imports System.IO

Module Synchronisation
    Public ReadOnly LocalPath As String = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) '\AppData\Local folder on any Windows device
    Public ReadOnly GlobalWFPath As String = LocalPath & "\Wayfarer"

    Public Sub Synchronise(ParFrm As SyncForm) 'folder system checked
        ParFrm.CompletedPercent = 0
        ParFrm.StatusTxt.Text = "STATUS OF SYNCHRONISATION: CHECKING THROUGH DIRECTORIES AND SUBDIRECTORIES. INITIALISING NECESSARY FILES"
        If Not Directory.Exists(GlobalWFPath) Then 'creates the \Wayfarer directory if not existing already
        Dim DI As DirectoryInfo = Directory.CreateDirectory(GlobalWFPath)
            DI.Attributes = FileAttributes.Hidden 'makes the \Wayfarer directory hidden
        End If
        ParFrm.CompletedPercent = 2 '2% completion on the syncform from where it is called.
        For I As Byte = 0 To 2 'generates the hierarchy of subdirectories if they don't exist already
            If Not Directory.Exists(DirectoryList(I)) Then
                Directory.CreateDirectory(DirectoryList(I)) '\Blocks, \Wallets, \Network
            End If
        Next
        ParFrm.CompletedPercent = 5 '5% sync process complete at this point
        If File.Exists(DirectoryList(1) & "Block0" & ExtensionList(1)) Then 'delete existing genesis block file
            DeleteFileOrDir(DirectoryList(1) & "Block0" & ExtensionList(1))
        End If
        'if the genesis block is corrupt, the entire chain is corrupt, so the genesis block has to be valid guaranteed
        'only way to do that is by deleting it and recreating the block and blockfile again
        ParFrm.StatusTxt.Text = "STATUS OF SYNCHRONISATION: FINISHED DOING A LOCAL DIRECTORY CHECK, ALL SUBDIRECTORIES CREATED/EXIST"
        SyncForm.CompletedPercent = 8
        CreateFileFromBlock(GenesisBlock) 'oveerrides any potential errors in genesis block by deleting and resaving a new copy
        ParFrm.StatusTxt.Text = "STATUS OF SYNCHRONISATION: CHECKING IF NODE IS ROOT DEVICE"

        IS_ROOT = CheckRootStatus() 'sets the flag for device being a root
        ParFrm.CompletedPercent = 12
        ParFrm.StatusTxt.Text = $"STATUS OF SYNCHRONISATION: ROOT CHECK COMPLETE, DEVICE {If(IS_ROOT, "IS", "ISN'T")} A ROOT."

        If Not IS_ROOT Then 'if device not root, must perform the online sync process after the local sync
            ParFrm.StatusTxt.Text = "STATUS OF SYNCHRONISATION: BEGINNING LOCAL SYNCHRONISATION FROM STORAGE."
            Dim RequestIndex As UInteger = LocalSync(ParFrm) 'performs the local synchronisation process
            ParFrm.StatusTxt.Text = $"STATUS OF SYNCHRONISATION: LOCAL SYNCHRONISATION FROM STORAGE COMPLETE. {RequestIndex - 1} BLOCKS LOADED."

            ParFrm.StatusTxt.Text = "STATUS OF SYNCHRONISATION: BEGINNING ONLINE SYNCHRONISATION ON THE WAYFARER NETWORK."

            OnlineSync(RequestIndex) 'performs the online sync process
            ParFrm.StatusTxt.Text = "STATUS OF SYNCHRONISATION: COMPLETED ONLINE SYNCHRONISATION ON THE WAYFARER NETWORK. DEVICE IS NOW SYNCED."
            Exit Sub 'if not root, both a local and online need to be performed
        End If
        ParFrm.StatusTxt.Text = "STATUS OF SYNCHRONISATION: BEGINNING LOCAL SYNCHRONISATION FROM STORAGE."
        LocalSync(ParFrm) 'root performs local sync only and doesnt store result in a variable - this will be the most up-to-date version
        IsSynchronised = True 'as it is the root, it can be instantly synced
        ParFrm.StatusTxt.Text = "STATUS OF SYNCHRONISATION: COMPLETED LOCAL SYNCHRONISATION ON THE WAYFARER NETWORK. ROOT NODE IS NOW SYNCED."
        WFRootHandler = New RootHandler() 'initialising the root handler as it is a root device, the only way to cut it off is to exit app, root device cannot become non-root and stay on chain
    End Sub

    Public Sub DeleteFileOrDir(Path As String) 'generic utility to delete a file or folder
        Try
            ' Check if the file/dir exists before attempting to delete
            If Directory.Exists(Path) Then
                Directory.Delete(Path)
            End If
            If File.Exists(Path) Then
                File.Delete(Path)
            End If
        Catch ex As Exception
            CustomMsgBox.ShowBox("Error: " & ex.Message, "ERROR", False)
        End Try
    End Sub
    Public Sub OnlineSync(StartBlock As UInteger) 'unfinished implementation but flowchart for algorithm is ready
        Dim ConnectionHandler As New ConnectionHandler()
        ConnectionHandler.Start()
        While PrevTCP Is Nothing
            Continue While
        End While

        Dim BlkReq As New SyncRequest(StartBlock)

        'perform the online sync procedure
        'send request to root on the net thread and wait for responses to load in 
    End Sub

    Public Function LocalSync(Optional ParFrm As SyncForm = Nothing) As UInteger 'returns index from which data needs to be requested
        Dim Dir As List(Of String) = Directory.GetFileSystemEntries(DirectoryList(1)).ToList
        For Each Item As String In Dir 'clearing any non-block files in the first pass through the \blocks\ directory
            If Not Path.GetExtension(Item) = ".wfbc" OrElse (Not IsValidBlockFilePath(Path.GetFileNameWithoutExtension(Item)) And Path.GetFileNameWithoutExtension(Item) <> "Block0") Then
                DeleteFileOrDir(Item)
            End If
        Next
        If ParFrm IsNot Nothing Then
            ParFrm.CompletedPercent = 15
        End If
        'file directory now clear of any non-block files, we reset the list because we did not update in the for each loop to not mess up the iterator
        Dir = Directory.GetFiles(DirectoryList(1)).ToList
        Dir.Remove(DirectoryList(1) & "Block0" & ExtensionList(1)) 'removes the Genesis block as that has already been added as a file
        Dim NumOfBlocks As UInteger = Dir.Count 'a counter to use as our "expected index" for the sequential lookups to find if the specific block file exists
        Dim NeedToDelete As Boolean = False 'a flag for the program to exit the below for loop and delete all remaining files, because a block was faulty/corrupt/wrong format
        Dim RequestIndex As UInteger = 1 'the block index which will be used to send 
        If ParFrm IsNot Nothing Then
            ParFrm.CompletedPercent = 20 'updates the syncform's progress bar variable to update the animations
        End If
        While Dir.Count > 0 'main loop to iterate through every file
            If Not NeedToDelete Then 'erroneous block hasn't been reached yet
                For I As UInteger = 1 To NumOfBlocks 'loop through expected number of blocks
                    If ParFrm IsNot Nothing Then
                        ParFrm.CompletedPercent += If(ParFrm.CompletedPercent < 60, 5, 0) 'add 5% per block to the progressbar
                    End If
                    Dim ExpectedName As String = DirectoryList(1) & "Block" & I.ToString & ExtensionList(1) 'get expected block name
                    If Dir.Contains(ExpectedName) Then 'checking for the item in the directory list
                        Dim TempBlock As Block = GetBlockFromFile(ExpectedName) 'generating a block from the file specified
                        If TempBlock Is Nothing Then
                            GoTo ErrorConditional 'if getblockfromfile returns nothing, thats an error, so delete all blocks including this one
                        End If
                        WFBlockchain.AddBlock(TempBlock) 'otherwise, add the block to the chain without needing to mine it, hence the False
                        'checking for validity of chain after this addition, as block 0 is valid, any invalid chains would be caused by the last block
                        If Not WFBlockchain.IsValidChain Then
                            WFBlockchain.DeleteLastBlock() 'if something breaks here, it can only be the last block, so we axe it
                            GoTo ErrorConditional 'delete all remaining block files including this one
                        End If
                        Dir.Remove(ExpectedName) 'otherwise, remove the file path from the list and continue
                        RequestIndex += 1 'increment the request index to the index of the next block
                    Else
ErrorConditional:       NeedToDelete = True 'this terminates the for loop and all following block files are deleted, including the one that triggered the error
                        Exit For
                    End If
                Next
            End If
            Try
                Dim TempItem As String = Dir.Last
                DeleteFileOrDir(TempItem)
                Dir.RemoveAt(Dir.Count - 1) 'delete all the strings, and corresponding files one by one from the remaining list (all invalid block files)
            Catch ex As Exception
                Exit While 'happens when list is empty, which is ok
            End Try
        End While
        If ParFrm IsNot Nothing Then
            ParFrm.CompletedPercent = 65 'after local sync 65% of the sync progress is done, update progress bar
        End If
        Return RequestIndex 'returns the index of the block from where the device needs to request a copy of the chain
    End Function

    Function UpdateBlockchain(StartIndex As UInteger) 'incomplete
        'will use the received blocks and the current blockchain instance to load all the blocks in sequentially in order
        Return Nothing
    End Function


    Function CheckRootStatus() As Boolean 'returns whether a device is the root or not
        Dim FilePath As String = DirectoryList(2) & "NetworkStatus" & ExtensionList(2) 'gets the network status file's filepath
        If File.Exists(FilePath) Then
            Dim NodeType As String = ""
            Using SR As New StreamReader(FilePath)
                Try
                    NodeType = SR.ReadLine 'get the first (and only) line of the file into the sub
                Catch ex As Exception
                    NodeType = ""
                End Try
            End Using
            If NodeType = GetSHA256FromString("ROOT" & "WAYFARER_V1") Then 'root status is encoded as the hash of that string
                Return True 'root node, so we can skip the online sync and sync will be complete at the local stage only
            Else
                File.Delete(FilePath) 'whatever is in that file was not the root digest, so we recreate the file as an empty
                File.Create(FilePath) 'reinitialise the file as an empty to clear any integrity issues
                Return False 'not a root
            End If
        Else 'file doesnt exist, create it and leave as empty
            File.Create(FilePath)
        End If
        Return True
    End Function
End Module