Module Program
    Public Sub Main()
        CurrentWallet = Nothing
        InboundJSONBuffer = New DataBufferQueue(Of String)
        WFTransactionPool = New TransactionPool
        WFBlockchain = New BlockChain
        'initialising all dynamic objects at runtime to avoid nullrefexceptions :)

        Application.Run(SyncForm) 'runs the sync form on startup
        SyncForm.Close() 'if the sync process completes successfully, form is closed and form resources are released
        SyncForm.Dispose()

        Application.Run(MainMenu) 'go to running the main menu for the user now
        Application.Exit() 'once the main menu is closed, application quits
    End Sub
End Module