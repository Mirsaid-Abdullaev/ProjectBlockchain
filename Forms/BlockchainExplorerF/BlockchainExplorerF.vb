Imports System.Threading
Public Class BlockchainExplorerF
    Private UpdateBlockchain As Thread
    Private FormExiting As Boolean
    Private Sub BackBtn_Click(sender As Object, e As EventArgs) Handles BackBtn.Click
        FormExiting = False
        Me.DialogResult = DialogResult.OK
        Me.Close()
        GC.Collect()
    End Sub

    Private Sub Disconnect_Click(sender As Object, e As EventArgs) Handles Disconnect.Click
        AppRunning = False
        DisconnectFromChain() 'appglobals sub to run through disconnect procedure
        Application.Exit()
    End Sub

    Private Sub BlockchainExplorer_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DesignLoad(Me, BlockchainExpColours)
        CheckForIllegalCrossThreadCalls = False
        Me.StatusLbl.Text = StatusLblText
        UpdateBlockchain = New Thread(AddressOf UpdateBlockchainTxt) With {.IsBackground = True}
        UpdateBlockchain.Start() 'checks every 2 seconds for any new blocks and displays them in the textbox
        GC.Collect()
    End Sub

    Private Sub UpdateBlockchainTxt() 'works
        Dim BlockchainSize As UInteger = 0
        Dim First As Boolean = True
        Me.BlockchainTxt.Text = ""
        While Not FormExiting
            If First Then
                Dim TempChain As List(Of Block) = WFBlockchain.GetChain()
                For Each CurrentBlock As Block In TempChain
                    Me.BlockchainTxt.Text &= CurrentBlock.GetFullData & vbCrLf & vbCrLf
                Next
                BlockchainSize = TempChain.Count - 1 'index of last block that was updated
                First = False
                Continue While
            End If

            Try
                If BlockchainSize = WFBlockchain.GetLastBlock.GetIndex Then
                    Continue While
                Else
                    BlockchainSize += 1
                End If
            Catch ex As Exception
                Thread.Sleep(2000)
                If BlockchainSize = WFBlockchain.GetLastBlock.GetIndex Then
                    Continue While
                Else
                    BlockchainSize += 1
                End If
            End Try

            Dim TempBlockchain As List(Of Block) = WFBlockchain.GetChain.GetRange(BlockchainSize, WFBlockchain.GetChain.Count - BlockchainSize)
            For Each CurrentBlock As Block In TempBlockchain
                Me.BlockchainTxt.Text += CurrentBlock.GetFullData() & vbCrLf & vbCrLf
            Next
            Thread.Sleep(2000)
        End While
        CheckForIllegalCrossThreadCalls = True
        GC.Collect()
        Exit Sub
    End Sub



    Private Sub BlockchainExplorerF_Closing(sender As Object, e As EventArgs) Handles Me.Closing
        FormExiting = True
        GC.Collect()
    End Sub
End Class