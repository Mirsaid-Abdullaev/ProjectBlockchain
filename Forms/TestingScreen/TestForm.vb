Imports ProjectBlockchain.Network.RequestsResponses
Imports Newtonsoft.Json

Public Class TestForm
    Public Sub UpdateStatusTxt(ByRef text As String)
        txtstatus.Text &= text & vbCrLf & vbCrLf
    End Sub


    Private Sub BeginTest_Click(sender As Object, e As EventArgs) Handles BeginTest.Click

        'Dim testblock As New Block(5, "xABCDEFGHIJx")
        'For i As Byte = 1 To 10
        '    Dim randsender As String = StrDup(i, "S")
        '    Dim randrecipient As String = StrDup(i, "R")
        '    Dim transact As New Transaction(randsender, randrecipient, i * 2.5)
        '    testblock.AddBlockTransaction(transact)
        'Next

        ''Dim genblock As New Block("Genesis")
        ''genblock.MineBlock(2)
        ''While GlobalData.IsMining
        ''    Continue While
        ''End While
        ''CreateFileFromBlock(genblock)

        'testblock.MineBlock(2)
        'While GlobalData.IsMining
        '    Continue While
        'End While
        'CreateFileFromBlock(testblock)
        ''works

        CreateFileFromBlock(GlobalData.GenesisBlock)
        Dim result As Block = GetBlockFromFile(GlobalData.DirectoryList(1) & "Block0" & GlobalData.ExtensionList(1))
        If result.GetFullData = GlobalData.GenesisBlock.GetFullData Then
            UpdateStatusTxt(True)
        End If
    End Sub

    Private Sub TestForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub


End Class