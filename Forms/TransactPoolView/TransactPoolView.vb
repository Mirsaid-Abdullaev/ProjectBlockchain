﻿Imports System.Threading
Public Class TransactPoolView
    Private UpdateTransactPool As Thread
    Private FormExited As Boolean
    Private Sub BackBtn_Click(sender As Object, e As EventArgs) Handles BackBtn.Click
        FormExited = True
        Me.DialogResult = DialogResult.OK
        Me.Close()
        Me.Dispose()
        GC.Collect()
    End Sub

    Private Sub Disconnect_Click(sender As Object, e As EventArgs) Handles Disconnect.Click
        AppRunning = False
        DisconnectFromChain()
        Application.Exit()
    End Sub

    Private Sub TransactPoolView_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.StatusLbl.Text = SetSharedLblText()
        Me.FormExited = False
        DesignLoad(Me, TransactPoolColours)
        UpdateTransactPool = New Thread(AddressOf UpdateTxt) With {.IsBackground = True}
        CheckForIllegalCrossThreadCalls = False
        UpdateTransactPool.Start()
    End Sub

    Private Sub UpdateTxt() 'checks if the transaction pool changed in size since the last time, otherwise just continues
        Dim TransactPoolSize As Byte = 0
        While Not Me.FormExited
            If TransactPoolSize = WFTransactionPool.GetPoolSize() Then
                Continue While
            Else
                TransactPoolSize = WFTransactionPool.GetPoolSize
            End If

            Try
                Me.TransactTxt.Text = GetTextForTransactPool()
            Catch ex As Exception
            End Try
            Thread.Sleep(1000) 'checks every second
        End While
        CheckForIllegalCrossThreadCalls = True
        Exit Sub
    End Sub

    Private Sub TransactPoolView_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        FormExited = True
        GC.Collect()
    End Sub

    Private Function GetTextForTransactPool() As String
        Dim TransactionList As List(Of Transaction) = WFTransactionPool.GetTransactionList
        If TransactionList Is Nothing Then
            Return "Empty transaction pool"
        End If
        Dim Result As String = ""
        For I = 0 To TransactionList.Count - 1
            Dim Transact As Transaction = TransactionList(I)
            Result += $"{I + 1}: Node ""{Transact.Sender}"" is sending {Transact.Quantity} to node ""{Transact.Recipient}"", timestamp ""{Transact.Timestamp}"" and the miner fee is {Transact.Fee}.{vbCrLf & vbCrLf}"
        Next
        Return Result
    End Function

    Private Sub GetHelpManual_Click(sender As Object, e As EventArgs) Handles GetHelpManual.Click
        Return 'not implemented
    End Sub
End Class