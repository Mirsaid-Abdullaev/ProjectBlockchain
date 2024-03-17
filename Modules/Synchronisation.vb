Imports System.IO

Module Synchronisation
    Public ReadOnly LocalPath As String = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) '\AppData\Local folder on any Windows device
    Public ReadOnly GlobalWFPath As String = LocalPath & "\Wayfarer"

    Public Sub SynchroniseNode(ParFrm As SyncForm) 'combines the full sync procedure in one subroutine
        ParFrm.CompletedPercent = 0
        ParFrm.UpdateStatus("STATUS OF SYNCHRONISATION: CHECKING THROUGH DIRECTORIES AND SUBDIRECTORIES. INITIALISING NECESSARY FILES")

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
        CreateFileFromBlock(GENESIS_BLOCK) 'oveerrides any potential errors in genesis block by deleting and resaving a new copy
        ParFrm.UpdateStatus("STATUS OF SYNCHRONISATION: FINISHED DOING A LOCAL DIRECTORY CHECK, ALL SUBDIRECTORIES CREATED/EXIST")
        ParFrm.CompletedPercent = 8


        ParFrm.UpdateStatus("STATUS OF SYNCHRONISATION: CHECKING IF NODE IS ROOT DEVICE")
        IsRoot = CheckRootStatus() 'sets the flag for device being a root
        ParFrm.CompletedPercent = 12
        ParFrm.UpdateStatus($"STATUS OF SYNCHRONISATION: ROOT CHECK COMPLETE, DEVICE {If(IsRoot, "IS", "ISN'T")} A ROOT.")


        If Not IsRoot Then 'if device not root, must perform the online sync process after the local sync
            ParFrm.UpdateStatus("STATUS OF SYNCHRONISATION: BEGINNING LOCAL SYNCHRONISATION FROM STORAGE.")
            Dim RequestIndex As UInteger = LocalSync(ParFrm) 'performs the local synchronisation process
            ParFrm.UpdateStatus($"STATUS OF SYNCHRONISATION: LOCAL SYNCHRONISATION FROM STORAGE COMPLETE. {RequestIndex - 1} BLOCKS LOADED.")
            ParFrm.UpdateStatus("STATUS OF SYNCHRONISATION: BEGINNING ONLINE SYNCHRONISATION ON THE WAYFARER NETWORK. LOOKING FOR NODES...")
            OnlineSync(RequestIndex) 'performs the online sync process
            ParFrm.CompletedPercent = 100
            ParFrm.UpdateStatus($"STATUS OF SYNCHRONISATION: COMPLETED ONLINE SYNCHRONISATION ON THE WAYFARER NETWORK. DEVICE IS NOW SYNCED. CONNECTED TO DEVICE: {PrevTCP}")
            Exit Sub 'if not root, both a local and online need to be performed
        End If

        ParFrm.CompletedPercent = 70
        ParFrm.UpdateStatus("STATUS OF SYNCHRONISATION: BEGINNING LOCAL SYNCHRONISATION FROM STORAGE.")
        LocalSync(ParFrm) 'root performs local sync only and doesnt store result in a variable - this will be the most up-to-date version
        IsSynchronised = True 'as it is the root, it can be instantly synced
        IsLeaf = True 'first live device on the network
        ParFrm.UpdateStatus("STATUS OF SYNCHRONISATION: COMPLETED LOCAL SYNCHRONISATION ON THE WAYFARER NETWORK. ROOT NODE IS NOW SYNCED.")


        WFRootHandler = New RootHandler() 'initialising the root handler as it is a root device, the only way to cut it off is to exit app, root device cannot become non-root and stay on chain
        WFRootHandler.Start()
        WFConnectionHandler = New ConnectionHandler() 'sets up the broadcasting of connection requests
        WFConnectionHandler.Start()
        ParFrm.CompletedPercent = 100
    End Sub


    Public Sub OnlineSync(StartBlock As UInteger) 'performs the online sync process
        WFConnectionHandler = New ConnectionHandler()
        WFConnectionHandler.Start() 'begins the connection handler - listening for incoming connection broadcasts

        'waiting until we link with a leaf node
        Dim TimeOutSW As New Stopwatch()
        TimeOutSW.Start() 'stopwatch used to keep track of elapsed time, and manage timed out requests
        While PrevTCP Is Nothing
            If TimeOutSW.ElapsedMilliseconds > 30000 Then
                TimeOutSW.Reset()
                '30 seconds have passed, notify user that this has happened, and give choice to continue looking for nodes, or exit.
                If CustomMsgBox.ShowBox("No devices active or live on the network. Unable to connect to the blockchain. You can keep trying, or exit the application. Click OK to continue, CANCEL to exit the application.", "NO LIVE NODES FOUND", True) Then
                    TimeOutSW.Restart() 'user clicked OK, to wait more
                Else 'user clicked cancel, so exit the application
                    AppRunning = False
                    DisconnectFromChain()
                    WFConnectionHandler.Destruct()
                    Application.Exit()
                End If
            End If
            Continue While 'this will keep the program flow here while this node is not connected to an existing leaf
        End While
        'now we have connected to a previous device and are the leaf node - time to communicate with the root


        WFRootHandlerClient = New RootHandlerClient(StartBlock) 'starts the communications with the root device and receives blockchain data
        While Not WFRootHandlerClient.HasFinished
            If Not WFRootHandlerClient.HasTimedOut Then
                Continue While 'checking for root client handler timeout through its bool property
            Else
                CustomMsgBox.ShowBox("Root node timeout error - synchronisation process failed. Try again later.", "ERROR", False)
                AppRunning = False
                IsSynchronised = False
                DisconnectFromChain()
                Application.Exit()
                Exit Sub 'kicks the user out of the app
            End If
        End While


        'now we should have all the received blocks
        Dim ReceivedBlocks As List(Of Block) = WFRootHandlerClient.GetReceivedBlocks
        If ReceivedBlocks.First().GetIndex > StartBlock Then
            'error occurred, must redo the sync process - this will kick the user out of the app
            CustomMsgBox.ShowBox("Error: the root device sent back incomplete/corrupted block data. Online sync process failed, try again later.", "ERROR", False)
            AppRunning = False
            IsSynchronised = False
            DisconnectFromChain()
            Application.Exit()
            Exit Sub
        End If

        WFBlockchain.UpdateBlockchain(ReceivedBlocks) 'gets all the received blocks stored onto the chain and storage
        IsSynchronised = True
        IsLeaf = True

        WFRootHandlerClient.Destruct() 'kills the root handler client
        WFConnectionHandler.Destruct() 'resets the connection handler
        WFConnectionHandler = New ConnectionHandler() ' resets the connection handler to send out broadcasts now
        WFConnectionHandler.Start()
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
            Using File.Create(FilePath)
            End Using
            Return False
        End If
        Return True
    End Function

    Sub RemoveRootStatus()
        Dim FilePath As String = DirectoryList(2) & "NetworkStatus" & ExtensionList(2) 'gets the network status file's filepath
        If File.Exists(FilePath) Then
            File.Delete(FilePath)
            File.Create(FilePath) 'reinitialise the file as an empty to clear any integrity issues
        Else 'file doesnt exist, create it and leave as empty
            File.Create(FilePath)
        End If
    End Sub

    Sub SetRootStatus()
        Dim FilePath As String = DirectoryList(2) & "NetworkStatus" & ExtensionList(2) 'gets the network status file's filepath
        Dim Text As String = GetSHA256FromString("ROOT" & "WAYFARER_V1")
        If File.Exists(FilePath) Then
            File.Delete(FilePath)
        End If
        Using SW As New StreamWriter(FilePath)
            SW.Write(Text)
        End Using 'reinitialise the file
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
End Module