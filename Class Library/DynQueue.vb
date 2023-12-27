Public Class DynQueue(Of Template)

    Private Property RearPointer As Integer = -1
    Private Property Queue As Template()
    Private Property MaxSize As Integer = 100
    Public Function GetMaxSize() As Integer
        Return MaxSize
    End Function
    'FrontPointer is not required - first data item in the queue will always be in the 0th index
    Private Const FrontPointer As Integer = 0

    Public Sub New()
        ReDim Queue(0)
    End Sub

    Public Sub RemoveElement()
        If Queue(RearPointer) IsNot Nothing Then
            Select Case RearPointer
                Case 0
                    Queue(RearPointer) = Nothing
                    RearPointer -= 1
                Case Else
                    RearPointer -= 1
                    ReDim Preserve Queue(RearPointer)
            End Select
        End If
    End Sub
    Public Sub AddElement(Data As Template)
        If RearPointer = -1 Then
            RearPointer += 1
            Queue(RearPointer) = Data
        ElseIf RearPointer < MaxSize - 1 Then
            RearPointer += 1
            ReDim Preserve Queue(RearPointer)
            Queue(RearPointer) = Data
        Else
            MsgBox("Queue full - no more elements allowed.")
        End If
    End Sub

    Public Function Pop() As Object
        If Queue(FrontPointer) Is Nothing Then
            MessageBox.Show("Empty queue, returned Nothing.", "ERROR MESSAGE")
            Return Nothing
        Else
            Dim Data As Template = Queue(FrontPointer)

            Select Case RearPointer
                Case 0 'last item in the queue
                    Queue(FrontPointer) = Nothing
                    RearPointer -= 1
                Case Else
                    For I As Integer = 1 To RearPointer
                        Queue(I - 1) = Queue(I)
                    Next
                    RearPointer -= 1
                    ReDim Preserve Queue(RearPointer)
            End Select
            Return Data
        End If
    End Function

    Public Function Peek() As Object
        Return Queue(FrontPointer)
    End Function

    Public Function Contains(Element As Template) As Boolean
        If Queue.Contains(Element) Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Function GetCurrentSize() As Integer
        Return RearPointer + 1
    End Function
End Class
