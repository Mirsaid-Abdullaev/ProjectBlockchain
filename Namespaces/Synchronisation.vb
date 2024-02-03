Imports System.IO

Module Synchronisation
    Public ReadOnly LocalLowPath As String = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) '\AppData\Local folder on any Windows device
    Public ReadOnly GlobalWFPath As String = LocalLowPath & "\Wayfarer"

    Public Sub Synchronise() 'folder system checked
        If Not Directory.Exists(GlobalWFPath) Then 'creates the \Wayfarer directory if not existing already
            Dim di As DirectoryInfo = Directory.CreateDirectory(GlobalWFPath)
            di.Attributes = FileAttributes.Hidden
        End If

        For i As Byte = 0 To 2 'generates the hierarchy of subdirectories if they don't exist already
            If Not Directory.Exists(DirectoryList(i)) Then
                Directory.CreateDirectory(DirectoryList(i))
            End If
        Next
        If File.Exists(DirectoryList(1) & "Block0" & ExtensionList(1)) Then
            DeleteFileOrDir(DirectoryList(1) & "Block0" & ExtensionList(1))
        End If
        CreateFileFromBlock(GenesisBlock) 'oveerrides any potential errors in genesis block by deleting and resaving a new copy
        ROOT = CheckRootStatus()
        If Not ROOT Then
            Dim RequestIndex As UInteger = LocalSync()
            'OnlineSync(RequestIndex)
            Exit Sub 'if not root, both a local and online need to be performed
        End If
        LocalSync() 'root performs local sync only and doesnt store result in a variable - this will be the most up-to-date version
        IsSynchronised = True 'as it is the root, it can be instantly synced
    End Sub

    Public Sub DeleteFileOrDir(Path As String)
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
    Public Sub OnlineSync(StartBlock As UInteger)
        Dim ConnectionHandler As New ConnectionHandler()
        ConnectionHandler.Start()
        While PrevTCP Is Nothing
            Continue While
        End While

        Dim BlkReq As New GetBlockchainDataRequest(StartBlock)

        'perform the online sync procedure
        'Dim BlkReq As New Network.GetBlockchainDataRequest(startblock)
        'send request to prevptr on the net thread and wait for responses to load in
    End Sub

    Public Function LocalSync() As UInteger 'returns index from which data needs to be requested
        Dim Dir As List(Of String) = Directory.GetFileSystemEntries(DirectoryList(1)).ToList
        For Each item As String In Dir 'clearing any non-block files in the first pass through the \blocks\ directory
            If Not Path.GetExtension(item) = ".wfbc" OrElse (Not IsValidBlockFilePath(Path.GetFileNameWithoutExtension(item)) And Path.GetFileNameWithoutExtension(item) <> "Block0") Then
                DeleteFileOrDir(item)
            End If
        Next
        'file directory now clear of any non-block files, we reset the list because we did not update in the for each loop to not mess up the iterator
        Dir = Directory.GetFiles(DirectoryList(1)).ToList
        Dir.Remove(DirectoryList(1) & "Block0" & ExtensionList(1)) 'removes the Genesis block as that has already been added as a file
        Dim NumOfBlocks As UInteger = Dir.Count 'a counter to use as our "expected index" for the sequential lookups to find if the specific block file exists
        Dim NeedToDelete As Boolean = False 'a flag for the program to exit the below for loop and delete all remaining files, because a block was faulty/corrupt/wrong format
        Dim RequestIndex As UInteger = 1 'the block index which will be used to send 
        While Dir.Count > 0
            If Not NeedToDelete Then
                For I As UInteger = 1 To NumOfBlocks

                    Dim ExpectedName As String = DirectoryList(1) & "Block" & I.ToString & ExtensionList(1)
                    If Dir.Contains(ExpectedName) Then
                        Dim TempBlock As Block = GetBlockFromFile(ExpectedName)
                        If TempBlock Is Nothing Then
                            GoTo ErrorConditional
                        End If
                        WFBlockchain.AddBlock(TempBlock)
                        If Not WFBlockchain.IsValidChain Then
                            WFBlockchain.DeleteLastBlock() 'if something breaks here, it can only be the last block, so we axe it
                            GoTo ErrorConditional
                        End If
                        Dir.Remove(ExpectedName)
                        RequestIndex += 1
                    Else
ErrorConditional:       NeedToDelete = True 'this terminates the for loop and all following block files are deleted, including the one that triggered the error
                        Exit For
                    End If
                Next
            End If
            Try
                Dim tempitem As String = Dir.Last
                DeleteFileOrDir(tempitem)
                Dir.RemoveAt(Dir.Count - 1) 'delete all the strings, and corresponding files one by one from the remaining list (all invalid block files)
            Catch ex As Exception
                Exit While
            End Try
        End While

        Return RequestIndex 'returns the index of the block from where the device needs to request a copy of the chain
    End Function

    Function UpdateBlockchain(StartIndex As UInteger, Optional EndIndex As UInteger = 0) As UInteger()
        Select Case EndIndex
            Case 0
            Case Else
                Dim ExpectedIndex As UInteger = StartIndex
                While ExpectedIndex < EndIndex
                    If ReceivedBlocks.Any(Function(tuple) tuple.Item1 = ExpectedIndex) Then 'checking if list contains expected index
                        Continue While
                    Else

                    End If
                End While
        End Select
        Return {9, 1}
    End Function


    Function CheckRootStatus() As Boolean
        Dim FilePath As String = DirectoryList(2) & "NetworkStatus" & ExtensionList(2)
        If File.Exists(FilePath) Then
            Dim NodeType As String = ""
            Using SR As New StreamReader(FilePath)
                Try
                    NodeType = SR.ReadLine
                Catch ex As Exception
                    GoTo ErrorCondition
                End Try
ErrorCondition: End Using
            If NodeType = GetSHA256FromString("ROOT") Then
                Return True 'root node, so we can skip the online sync and sync will be complete at the local stage only
            Else
                File.Delete(FilePath)
                File.Create(FilePath) 'reinitialise the file as an empty to clear any integrity issues
                Return False 'not a root
            End If
        Else 'file doesnt exist, create it and leave as empty
            File.Create(FilePath)
        End If
        Return True
    End Function
End Module