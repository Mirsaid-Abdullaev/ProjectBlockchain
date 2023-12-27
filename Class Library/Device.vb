Imports System.Net
Public Class Device
    Public Property Address As IPAddress
    Public Property Name As String
    Public Property NodeType As String
    Public Property PrevPtr As String
    Public Property NextPtr As String

    Public Sub New(Address As IPAddress, Name As String, NodeType As String, PrevPtr As String, NextPtr As String)
        Me.Address = Address
        Me.Name = Name
        Me.PrevPtr = PrevPtr
        Me.NextPtr = NextPtr
        Me.NodeType = NodeType
    End Sub
End Class
