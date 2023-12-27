<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class TestForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(TestForm))
        Me.Button2 = New System.Windows.Forms.Button()
        Me.txtstatus = New System.Windows.Forms.RichTextBox()
        Me.BeginTest = New System.Windows.Forms.Button()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.RichTextBox2 = New System.Windows.Forms.RichTextBox()
        Me.RichTextBox1 = New System.Windows.Forms.RichTextBox()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Button2
        '
        Me.Button2.BackColor = System.Drawing.Color.Transparent
        Me.Button2.Font = New System.Drawing.Font("Bahnschrift Light", 12.0!)
        Me.Button2.ForeColor = System.Drawing.Color.Goldenrod
        Me.Button2.Location = New System.Drawing.Point(8, 188)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(173, 51)
        Me.Button2.TabIndex = 3
        Me.Button2.Text = "ABOUT THE WAYFARER SYSTEM"
        Me.Button2.UseVisualStyleBackColor = False
        '
        'txtstatus
        '
        Me.txtstatus.BackColor = System.Drawing.SystemColors.Control
        Me.txtstatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtstatus.Font = New System.Drawing.Font("Bahnschrift Light", 11.0!)
        Me.txtstatus.ForeColor = System.Drawing.Color.Black
        Me.txtstatus.Location = New System.Drawing.Point(187, 67)
        Me.txtstatus.Name = "txtstatus"
        Me.txtstatus.Size = New System.Drawing.Size(418, 295)
        Me.txtstatus.TabIndex = 23
        Me.txtstatus.Text = ""
        '
        'BeginTest
        '
        Me.BeginTest.BackColor = System.Drawing.Color.Transparent
        Me.BeginTest.Font = New System.Drawing.Font("Bahnschrift Light", 12.0!)
        Me.BeginTest.ForeColor = System.Drawing.Color.Goldenrod
        Me.BeginTest.Location = New System.Drawing.Point(187, 10)
        Me.BeginTest.Name = "BeginTest"
        Me.BeginTest.Size = New System.Drawing.Size(418, 51)
        Me.BeginTest.TabIndex = 28
        Me.BeginTest.Text = "START TEST"
        Me.BeginTest.UseVisualStyleBackColor = False
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = Global.ProjectBlockchain.My.Resources.Resources.Logo
        Me.PictureBox1.Location = New System.Drawing.Point(8, 12)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(173, 172)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 29
        Me.PictureBox1.TabStop = False
        '
        'RichTextBox2
        '
        Me.RichTextBox2.Font = New System.Drawing.Font("Bahnschrift Light", 9.0!)
        Me.RichTextBox2.ForeColor = System.Drawing.Color.Red
        Me.RichTextBox2.Location = New System.Drawing.Point(305, 101)
        Me.RichTextBox2.Name = "RichTextBox2"
        Me.RichTextBox2.Size = New System.Drawing.Size(207, 172)
        Me.RichTextBox2.TabIndex = 34
        Me.RichTextBox2.Text = resources.GetString("RichTextBox2.Text")
        '
        'RichTextBox1
        '
        Me.RichTextBox1.Font = New System.Drawing.Font("Bahnschrift Light", 9.0!)
        Me.RichTextBox1.ForeColor = System.Drawing.Color.Red
        Me.RichTextBox1.Location = New System.Drawing.Point(94, 101)
        Me.RichTextBox1.Name = "RichTextBox1"
        Me.RichTextBox1.Size = New System.Drawing.Size(207, 172)
        Me.RichTextBox1.TabIndex = 33
        Me.RichTextBox1.Text = "Header view:" & Global.Microsoft.VisualBasic.ChrW(10) & "Block #X:" & Global.Microsoft.VisualBasic.ChrW(10) & "[timestamp]" & Global.Microsoft.VisualBasic.ChrW(10) & "[hash of previous block]" & Global.Microsoft.VisualBasic.ChrW(10) & "[nonce]" & Global.Microsoft.VisualBasic.ChrW(10) & "[hash of tran" &
    "sactions]" & Global.Microsoft.VisualBasic.ChrW(10) & "Block #X-1:" & Global.Microsoft.VisualBasic.ChrW(10) & "etc..."
        '
        'TestForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Gray
        Me.ClientSize = New System.Drawing.Size(607, 374)
        Me.Controls.Add(Me.RichTextBox2)
        Me.Controls.Add(Me.RichTextBox1)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.BeginTest)
        Me.Controls.Add(Me.txtstatus)
        Me.Controls.Add(Me.Button2)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.HelpButton = True
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "TestForm"
        Me.Text = "Testing..."
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Button2 As Button
    Public WithEvents txtstatus As RichTextBox
    Friend WithEvents BeginTest As Button
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents RichTextBox2 As RichTextBox
    Friend WithEvents RichTextBox1 As RichTextBox
End Class
