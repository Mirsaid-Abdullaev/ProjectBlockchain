Module AppGlobals
    Class GlobalData 'these will be used throughout all of the program to store and save data
        Public Shared CurrentWallet As Wallet
        Public Shared StatusLblText As String = SetSharedLblText("Offline", "No wallet logged in", "No wallet logged in") 'label text across all forms
        Public Shared TransactionPool As TransactionPool 'main transaction pool
        Public Shared MiningRewardBuffer As TransactionPool 'for saving and sending mining rewards to the miner of that block
        Public Shared Blockchain As BlockChain
        Public Shared Online As Boolean = False
    End Class

    Public Function SetSharedLblText(NetStatus As String, WalletName As String, Balance As String) As String
        Return "STATUS:" & NetStatus & vbCrLf & "WALLET: " & WalletName & vbCrLf & "BALANCE: " & Balance
    End Function
End Module