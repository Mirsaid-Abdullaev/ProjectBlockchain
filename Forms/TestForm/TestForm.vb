Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Public Class TestForm
    Public Sub UpdateStatusTxt(ByRef text As String)
        txtstatus.Text &= text & vbCrLf & vbCrLf
    End Sub


    Private Sub BeginTest_Click(sender As Object, e As EventArgs) Handles BeginTest.Click

        'Dim testblock As New Block(0, GenesisBlock.GetHash)
        'testblock.AddBlockTransaction(New Transaction("abc", testwal.GetPublicAddress, 500))
        'Blockchain.AddBlock(testblock)
        'CreateFileFromBlock(testblock)
        'For i As Byte = 1 To 9
        '    Dim sendorreceive As Byte = rng.Next(0, 2)
        '    Dim block As New Block(Blockchain.GetLastBlock.GetIndex, Blockchain.GetLastBlock.GetHash)
        '    If sendorreceive = 0 Then 'sending
        '        Dim transact As New Transaction(testwal.GetPublicAddress, "abc", 20)
        '        block.AddBlockTransaction(transact)
        '    Else 'receiving
        '        Dim transact As New Transaction("abc", testwal.GetPublicAddress, 100)
        '        block.AddBlockTransaction(transact)
        '    End If
        '    Blockchain.AddBlock(block)
        '    CreateFileFromBlock(block)
        'Next

        'Dim rng As New Random
        'Dim testwal As New Wallet("Testing Wallet Balance", "1127876B2FE4C0AD686FF86E8DB8D4CDFD34ECCEEAE9FE04E33E09EAC5077BDE", "C:\Users\abdul\AppData\Local\Wayfarer\Wallets\Testing Wallet Balance.wfwlt")
        'Dim reqind As UInteger = FileSystem.Synchronise
        'CurrentWallet = testwal

        'CustomMsgBox.ShowBox("Balance of testwallet: " & CurrentWallet.GetWalletBalance.ToString & vbCrLf & "Balance of abc: " & ScanBlockBalanceUpdate("abc").ToString, "BALANCE TESTING", False)


    End Sub


End Class