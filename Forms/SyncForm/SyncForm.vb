Public Class SyncForm
    Public CompletedPercent As Byte = 0
    Private ProgressBarUpdate As Threading.Thread
    Private SyncComplete As Boolean = False
    Private Sub SyncForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DesignLoad(Me, MainMenuColours)
        ProgressBarUpdate = New Threading.Thread(AddressOf UpdatePBValue) With {.IsBackground = True}
        CheckForIllegalCrossThreadCalls = False
        ProgressBarUpdate.Start()
    End Sub

    Private Sub ConnectAndSync_Click(sender As Object, e As EventArgs) Handles ConnectAndSync.Click
        'check if node wants to be a miner or not
        Dim Result As Object = CustomInputBox.ShowInputBox("Please input Yes or No for opting into being a mining node...", "MINER CHOICE", False)
        Try
            IsMiner = Result(0).ToString.ToLower = "yes" 'blame visual studio refactoring for this :) i didnt even know this was allowed
        Catch ex As Exception
            IsMiner = False
        End Try
        Synchronise(Me) 'performs the full sync procedure, and simultaneously updates the progress bar through bkg thread here
        SyncComplete = True
        ConnectAndSync.Enabled = False
        Me.Close()
    End Sub

    Private Sub UpdatePBValue()
        While Not SyncComplete
            ProgressBar1.Value = CompletedPercent
        End While
        ProgressBar1.Value = CompletedPercent
        CheckForIllegalCrossThreadCalls = True
    End Sub
End Class
