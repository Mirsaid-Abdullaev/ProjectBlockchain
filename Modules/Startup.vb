Module Program
    Public Sub Main()
        CurrentWallet = Nothing
        InboundJSONBuffer = New DataBufferQueue(Of String)
        WFTransactionPool = New TransactionPool
        WFBlockchain = New BlockChain
        'initialising all dynamic objects at runtime to avoid nullrefexceptions :)

        Application.Run(New SyncForm) 'runs the sync form on startup
        Application.Run(New MainMenu) 'go to running the main menu for the user now
        Application.Exit() 'once the main menu is closed, application quits

        Application.Run(New WalletBaseView)
    End Sub
End Module