Imports System.Threading
Public Class BlockchainExplorerH
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
        Me.StatusLbl.Text = SetSharedLblText()
        UpdateBlockchain = New Thread(AddressOf UpdateBlockchainTxt) With {.IsBackground = True}
        UpdateBlockchain.Start() 'checks every 2 seconds for any new blocks and displays them in the textbox
        GC.Collect()
    End Sub

    Private Sub UpdateBlockchainTxt() 'the sub run on the background thread - updates the blockchain view every 2 seconds
        Dim LastBlockchainSize As UInteger = 0 'stores the last recorded size of the chain - useful to check whether any new blocks have been added since the last while loop
        Dim First As Boolean = True 'this is the flag used for the first iteration and update of the viewer in the while loop only
        Me.BlockchainTxt.Text = ""
        While Not FormExiting
            If First Then 'first time displaying
                Dim TempChain As List(Of Block) = WFBlockchain.Blockchain 'gets the entire blockchain
                For Each CurrentBlock As Block In TempChain
                    Me.BlockchainTxt.Text &= CurrentBlock.HeaderData & vbCrLf & vbCrLf
                Next 'displays each block one by one into the main textbox
                LastBlockchainSize = TempChain.Count - 1 'index of last block that was updated
                First = False 'this code block is not touched any more
                Continue While
            End If

            Try
                If LastBlockchainSize = WFBlockchain.LastBlock.GetIndex Then
                    Continue While 'if the last size is the same as current size, nothing changed, so just continue
                Else
                    LastBlockchainSize += 1
                End If
            Catch ex As Exception 'this is used in the rare case that the thread catches a block halfway through being added to the underlying blockchain class's list, and throws an index exception
                Thread.Sleep(2000) 'the thread allows a 2 second wait for the block to be added successfully
                If LastBlockchainSize = WFBlockchain.LastBlock.GetIndex Then 'try again
                    Continue While
                Else
                    LastBlockchainSize += 1
                End If
            End Try

            Dim TempBlockchain As List(Of Block) = WFBlockchain.Blockchain.GetRange(LastBlockchainSize, WFBlockchain.Blockchain.Count - LastBlockchainSize) 'gets the new blocks that were added since the last time viewer was updated
            For Each CurrentBlock As Block In TempBlockchain 'for each one of these blocks, add their string versions to the explorer textbox
                Me.BlockchainTxt.Text += CurrentBlock.FullData() & vbCrLf & vbCrLf
            Next
            Thread.Sleep(2000) 'allow a 2 second gap before checking again to ease CPU time utilisation
        End While
        CheckForIllegalCrossThreadCalls = True
        GC.Collect()
        Exit Sub
    End Sub



    Private Sub BlockchainExplorerF_Closing(sender As Object, e As EventArgs) Handles Me.Closing
        FormExiting = True ' flag switched on to kill the background thread
        GC.Collect()
    End Sub
End Class