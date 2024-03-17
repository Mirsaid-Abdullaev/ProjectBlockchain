Public Class SyncForm
    Public CompletedPercent As Byte = 0
    Private ProgressBarUpdate As Threading.Thread
    Private RunSyncThread As Threading.Thread
    Private SyncComplete As Boolean = False
    Private Sub SyncForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DesignLoad(Me, MainMenuColours)
        ProgressBarUpdate = New Threading.Thread(AddressOf UpdatePBValue) With {.IsBackground = True}
        CheckForIllegalCrossThreadCalls = False
        ProgressBarUpdate.Start()
        RunSyncThread = New Threading.Thread(AddressOf RunSync) With {.IsBackground = True}
    End Sub

    Private Sub ConnectAndSync_Click(sender As Object, e As EventArgs) Handles ConnectAndSync.Click
        'check if node wants to be a miner or not
        Dim Result As Object = CustomInputBox.ShowInputBox("Please input Yes or No for opting into being a mining node...", "MINER CHOICE", False)
        Try
            IsMiner = Result(0).ToString.ToLower = "yes" 'blame visual studio refactoring for this :) i didnt even know this was allowed
        Catch ex As Exception
            IsMiner = False
        End Try
        ConnectAndSync.Enabled = False
        RunSyncThread.Start() 'performs the full sync procedure, and simultaneously updates the progress bar through bkg thread here
    End Sub
    Private Sub RunSync()
        SynchroniseNode(Me)
        SyncComplete = True
        Me.Close()
    End Sub
    Public Sub UpdateStatus(NewTxt As String)
        Me.StatusTxt.Text = NewTxt
        StatusTxt.Invalidate()
        StatusTxt.Update()
    End Sub
    Private Sub UpdatePBValue()
        Dim Prev As Byte = CompletedPercent
        While Not SyncComplete
            If Prev = CompletedPercent Then
                Continue While
            End If
            SyncProgBar.Value = CompletedPercent
            Prev = CompletedPercent
            SyncProgBar.Invalidate()
            SyncProgBar.Update()
        End While
        SyncProgBar.Value = CompletedPercent
        CheckForIllegalCrossThreadCalls = True
    End Sub

    Private Sub SyncForm_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        If Not SyncComplete Then
            AppRunning = False
            DisconnectFromChain()
            Application.Exit()
        End If
    End Sub
End Class
