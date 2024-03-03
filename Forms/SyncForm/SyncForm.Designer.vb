<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class SyncForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SyncForm))
        Me.Title = New System.Windows.Forms.Label()
        Me.ConnectAndSync = New System.Windows.Forms.Button()
        Me.GetHelpManual = New System.Windows.Forms.Button()
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar()
        Me.StatusTxt = New System.Windows.Forms.Label()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Title
        '
        Me.Title.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Title.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Title.Font = New System.Drawing.Font("Bahnschrift Light", 24.0!)
        Me.Title.Location = New System.Drawing.Point(206, 36)
        Me.Title.Name = "Title"
        Me.Title.Size = New System.Drawing.Size(454, 162)
        Me.Title.TabIndex = 0
        Me.Title.Text = "WELCOME TO THE WAYFARER SYSTEM. CONNECT TO THE NETWORK BY PRESSING 'CONNECT' BELO" &
    "W"
        Me.Title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ConnectAndSync
        '
        Me.ConnectAndSync.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.ConnectAndSync.Font = New System.Drawing.Font("Bahnschrift Light", 16.0!)
        Me.ConnectAndSync.ForeColor = System.Drawing.Color.Goldenrod
        Me.ConnectAndSync.Location = New System.Drawing.Point(206, 204)
        Me.ConnectAndSync.Name = "ConnectAndSync"
        Me.ConnectAndSync.Size = New System.Drawing.Size(454, 53)
        Me.ConnectAndSync.TabIndex = 2
        Me.ConnectAndSync.Text = "CONNECT AND SYNCHRONISE"
        Me.ConnectAndSync.UseVisualStyleBackColor = False
        '
        'GetHelpManual
        '
        Me.GetHelpManual.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.GetHelpManual.Font = New System.Drawing.Font("Bahnschrift Light", 12.0!)
        Me.GetHelpManual.ForeColor = System.Drawing.Color.Goldenrod
        Me.GetHelpManual.Location = New System.Drawing.Point(29, 204)
        Me.GetHelpManual.Name = "GetHelpManual"
        Me.GetHelpManual.Size = New System.Drawing.Size(171, 53)
        Me.GetHelpManual.TabIndex = 5
        Me.GetHelpManual.Text = "ABOUT THE WAYFARER SYSTEM"
        Me.GetHelpManual.UseVisualStyleBackColor = False
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Location = New System.Drawing.Point(29, 366)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(631, 39)
        Me.ProgressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous
        Me.ProgressBar1.TabIndex = 6
        '
        'StatusTxt
        '
        Me.StatusTxt.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.StatusTxt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.StatusTxt.Font = New System.Drawing.Font("Bahnschrift Light", 12.0!)
        Me.StatusTxt.Location = New System.Drawing.Point(29, 260)
        Me.StatusTxt.Name = "StatusTxt"
        Me.StatusTxt.Size = New System.Drawing.Size(631, 92)
        Me.StatusTxt.TabIndex = 7
        Me.StatusTxt.Text = "STATUS OF SYNCHRONISATION: [insert status messages here]" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(Progress bar below wil" &
    "l load at the same rate as it takes to connect to, synchronise and update a node" &
    ")" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        Me.StatusTxt.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = Global.ProjectBlockchain.My.Resources.Resources.Logo
        Me.PictureBox1.Location = New System.Drawing.Point(31, 36)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(169, 160)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 21
        Me.PictureBox1.TabStop = False
        '
        'SyncForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(60, Byte), Integer), CType(CType(60, Byte), Integer), CType(CType(70, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(694, 422)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.StatusTxt)
        Me.Controls.Add(Me.ProgressBar1)
        Me.Controls.Add(Me.GetHelpManual)
        Me.Controls.Add(Me.ConnectAndSync)
        Me.Controls.Add(Me.Title)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.HelpButton = True
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "SyncForm"
        Me.Text = "SYNCHRONISATION"
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Title As Label
    Friend WithEvents ConnectAndSync As Button
    Friend WithEvents GetHelpManual As Button
    Friend WithEvents ProgressBar1 As ProgressBar
    Friend WithEvents StatusTxt As Label
    Friend WithEvents PictureBox1 As PictureBox
End Class
