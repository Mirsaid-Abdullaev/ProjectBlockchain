Imports System.IO

Namespace FileSystem
    Module WayfarerDirectory
        Public ReadOnly LocalLowPath As String = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) '\AppData\Local folder on any Windows device
        Public ReadOnly GlobalWFPath As String = LocalLowPath & "\Wayfarer"

        Public Sub Initialise() 'folder system checked
            If Not Directory.Exists(GlobalWFPath) Then 'creates the \Wayfarer directory if not existing already
                Directory.CreateDirectory(GlobalWFPath)
            End If

            Dim DI As New DirectoryInfo(GlobalWFPath) With {.Attributes = FileAttributes.Hidden}
            For i As Byte = 0 To 2 'generates the hierarchy of subdirectories if they don't exist already
                If Not Directory.Exists(GlobalData.DirectoryList(i)) Then
                    Directory.CreateDirectory(GlobalData.DirectoryList(i))
                End If
            Next
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