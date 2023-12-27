Module Program
    Public Sub Main()
        Application.Run(SyncForm)
        SyncForm.Close()
        SyncForm.Dispose()
        Application.Run(MainMenu)
    End Sub
End Module