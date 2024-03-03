Module Program
    Public Sub Main()
        ' WFBlockchain = New BlockChain
        '    CurrentWallet = Nothing
        '    OutboundJSONBuffer = New DataBufferQueue(Of String)
        '    InboundJSONBuffer = New DataBufferQueue(Of String)
        WFTransactionPool = New TransactionPool
        CurrentBlockTransactionList = New List(Of Transaction)
        '    ReceivedBlocks = New List(Of Tuple(Of UInteger, Block))

        '    'initialising all dynamic objects at runtime

        '    Application.Run(SyncForm)
        '    SyncForm.Close()
        '    SyncForm.Dispose()
        '    Application.Run(MainMenu)
        Application.Run(WalletBaseView)
    End Sub
End Module



'above code is final code - below code is testing code

'WFBlockchain = New BlockChain
'For i As Byte = 1 To 10
'    Dim block As New Block()
'    block.AddTransaction(New Transaction(GetDeviceName, "TestRecipient" & i.ToString, New Random().NextDouble * 100, 2))
'    block.MineBlock(2)
'    While IsMining
'        Continue While
'    End While
'    WFBlockchain.AddBlock(block)
'Next
'LocalSync()
'Application.Run(MainMenu)