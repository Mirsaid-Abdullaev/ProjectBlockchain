<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class BaseViewingForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(BaseViewingForm))
        Me.GetHelpManual = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Disconnect = New System.Windows.Forms.Button()
        Me.StatusLbl = New System.Windows.Forms.Label()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.BlockchainExpF = New System.Windows.Forms.Button()
        Me.NetworkStatusView = New System.Windows.Forms.Button()
        Me.BlockchainExpH = New System.Windows.Forms.Button()
        Me.BackBtn = New System.Windows.Forms.Button()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GetHelpManual
        '
        Me.GetHelpManual.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.GetHelpManual.Font = New System.Drawing.Font("Bahnschrift Light", 12.0!)
        Me.GetHelpManual.ForeColor = System.Drawing.Color.Goldenrod
        Me.GetHelpManual.Location = New System.Drawing.Point(10, 185)
        Me.GetHelpManual.Name = "GetHelpManual"
        Me.GetHelpManual.Size = New System.Drawing.Size(171, 51)
        Me.GetHelpManual.TabIndex = 3
        Me.GetHelpManual.Text = "ABOUT THE WAYFARER SYSTEM"
        Me.GetHelpManual.UseVisualStyleBackColor = False
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.Label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label1.Font = New System.Drawing.Font("Bahnschrift Light", 18.0!)
        Me.Label1.Location = New System.Drawing.Point(187, 10)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(418, 64)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "VIEWING FORMS OPTIONS"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Disconnect
        '
        Me.Disconnect.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(50, Byte), Integer))
        Me.Disconnect.Font = New System.Drawing.Font("Bahnschrift", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Disconnect.ForeColor = System.Drawing.Color.Goldenrod
        Me.Disconnect.Location = New System.Drawing.Point(189, 242)
        Me.Disconnect.Name = "Disconnect"
        Me.Disconnect.Size = New System.Drawing.Size(318, 51)
        Me.Disconnect.TabIndex = 17
        Me.Disconnect.Text = "DISCONNECT FROM WAYFARER SYSTEM"
        Me.Disconnect.UseVisualStyleBackColor = False
        '
        'StatusLbl
        '
        Me.StatusLbl.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.StatusLbl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.StatusLbl.Font = New System.Drawing.Font("Bahnschrift", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.StatusLbl.Location = New System.Drawing.Point(10, 242)
        Me.StatusLbl.Name = "StatusLbl"
        Me.StatusLbl.Size = New System.Drawing.Size(172, 51)
        Me.StatusLbl.TabIndex = 18
        Me.StatusLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = Global.ProjectBlockchain.My.Resources.Resources.Logo
        Me.PictureBox1.Location = New System.Drawing.Point(8, 10)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(173, 172)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 33
        Me.PictureBox1.TabStop = False
        '
        'BlockchainExpF
        '
        Me.BlockchainExpF.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.BlockchainExpF.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BlockchainExpF.ForeColor = System.Drawing.Color.Goldenrod
        Me.BlockchainExpF.Location = New System.Drawing.Point(188, 158)
        Me.BlockchainExpF.Name = "BlockchainExpF"
        Me.BlockchainExpF.Size = New System.Drawing.Size(418, 74)
        Me.BlockchainExpF.TabIndex = 37
        Me.BlockchainExpF.Text = "BLOCKCHAIN EXPLORER - FULL VIEW"
        Me.BlockchainExpF.UseVisualStyleBackColor = False
        '
        'NetworkStatusView
        '
        Me.NetworkStatusView.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.NetworkStatusView.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.NetworkStatusView.ForeColor = System.Drawing.Color.Goldenrod
        Me.NetworkStatusView.Location = New System.Drawing.Point(399, 77)
        Me.NetworkStatusView.Name = "NetworkStatusView"
        Me.NetworkStatusView.Size = New System.Drawing.Size(206, 75)
        Me.NetworkStatusView.TabIndex = 36
        Me.NetworkStatusView.Text = "NETWORK STATUS VIEW"
        Me.NetworkStatusView.UseVisualStyleBackColor = False
        '
        'BlockchainExpH
        '
        Me.BlockchainExpH.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.BlockchainExpH.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BlockchainExpH.ForeColor = System.Drawing.Color.Goldenrod
        Me.BlockchainExpH.Location = New System.Drawing.Point(188, 77)
        Me.BlockchainExpH.Name = "BlockchainExpH"
        Me.BlockchainExpH.Size = New System.Drawing.Size(205, 75)
        Me.BlockchainExpH.TabIndex = 34
        Me.BlockchainExpH.Text = "BLOCKCHAIN EXPLORER - HEADER VIEW"
        Me.BlockchainExpH.UseVisualStyleBackColor = False
        '
        'BackBtn
        '
        Me.BackBtn.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(50, Byte), Integer))
        Me.BackBtn.Font = New System.Drawing.Font("Bahnschrift", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BackBtn.ForeColor = System.Drawing.Color.Goldenrod
        Me.BackBtn.Location = New System.Drawing.Point(513, 242)
        Me.BackBtn.Name = "BackBtn"
        Me.BackBtn.Size = New System.Drawing.Size(93, 51)
        Me.BackBtn.TabIndex = 38
        Me.BackBtn.Text = "BACK TO MENU"
        Me.BackBtn.UseVisualStyleBackColor = False
        '
        'BaseViewingForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(60, Byte), Integer), CType(CType(60, Byte), Integer), CType(CType(70, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(613, 313)
        Me.Controls.Add(Me.BackBtn)
        Me.Controls.Add(Me.BlockchainExpF)
        Me.Controls.Add(Me.NetworkStatusView)
        Me.Controls.Add(Me.BlockchainExpH)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.StatusLbl)
        Me.Controls.Add(Me.Disconnect)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.GetHelpManual)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.HelpButton = True
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "BaseViewingForm"
        Me.Text = "BLOCKCHAIN VIEWER"
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GetHelpManual As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents Disconnect As Button
    Friend WithEvents StatusLbl As Label
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents BlockchainExpF As Button
    Friend WithEvents NetworkStatusView As Button
    Friend WithEvents BlockchainExpH As Button
    Friend WithEvents BackBtn As Button
End Class
