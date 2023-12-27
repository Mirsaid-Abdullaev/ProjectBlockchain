Imports System.IO

Namespace FileSystem
    Module WayfarerDirectory
        Public ReadOnly LocalLowPath As String = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) '\AppData\Local folder on any Windows device
        Public ReadOnly GlobalWFPath As String = LocalLowPath & "\Wayfarer"
        Public ReadOnly DirectoryList() As String = {GlobalWFPath & "\Wallets\", GlobalWFPath & "\Blocks\", GlobalWFPath & "\Network\"}
        Public ReadOnly ExtensionList() As String = {".wfwlt", ".wfbc", ".wfls"}
        Public Sub Initialise() 'folder system checked, 
            If Not Directory.Exists(GlobalWFPath) Then
                Directory.CreateDirectory(GlobalWFPath)
            End If
            '
            'add this once tested ok - hides folder = headache
            'Dim DI As New DirectoryInfo(GlobalWFPath) With {.Attributes = FileAttributes.Hidden}
            For i As Byte = 0 To 2 'generates the hierarchy of folders if they don't exist
                If Not Directory.Exists(DirectoryList(i)) Then
                    Directory.CreateDirectory(DirectoryList(i))
                End If
            Next
            'use the using/end using clause to remove background handle to the created file - can cause errors
            'Using fs As FileStream = File.Create(DirectoryList(1) & "TestFile" & ExtensionList(1))
            '    Using sw As New StreamWriter(fs)
            '        sw.WriteLine("This is some test block data.")
            '    End Using
            'End Using


            Exit Sub
        End Sub
    End Module

    Module BlockFileSystem
        Public Sub CreateBinaryBlockFile(ByRef Block As Block)

        End Sub



        Public Sub DeleteFile(ByRef FilePath As String)
            Try
                ' Check if the file exists before attempting to delete.
                If File.Exists(FilePath) Then
                    File.Delete(FilePath)
                End If
            Catch ex As Exception
                MsgBox("Error: " & ex.Message)
            End Try
        End Sub
    End Module


End Namespace