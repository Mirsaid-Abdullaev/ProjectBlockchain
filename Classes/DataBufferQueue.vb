Public Class DataBufferQueue(Of T)
    Private Property RearPointer As Integer = -1
    ' List(Of Tuple(Of Template, Integer)) to store both the element and its priority
    Private Property Queue As List(Of Tuple(Of T, Integer))
    Private Property MaxSize As Integer
    ' FrontPointer is not required - the first data item in the queue will always be in the 0th index
    Private Const FrontPointer As Integer = 0
    Public Property IsEmpty As Boolean
    Public Property IsFull As Boolean

    Public Sub New(Optional MaxSize As Integer = 200)
        Me.MaxSize = MaxSize
        IsEmpty = True
        IsFull = False
        Queue = New List(Of Tuple(Of T, Integer))
    End Sub

    Public Sub DeleteLastElement()
        If Queue.Count > 0 Then
            Select Case RearPointer
                Case 0
                    ' Remove the first item in the queue and update pointers
                    Queue.RemoveAt(FrontPointer)
                    RearPointer -= 1
                    IsEmpty = True
                Case Else
                    ' Remove the first item in the queue and update pointers
                    Queue.RemoveAt(FrontPointer)
                    RearPointer -= 1
            End Select
        End If
    End Sub

    Public Sub Enqueue(Data As T, Optional Priority As String = "")
        EnqueueWithPriority(Data, If(Priority.ToLower() = "high", 0, 1)) '0 is high priority, 1 is everything else, checks for priority string high, else default priority
    End Sub

    Private Sub EnqueueWithPriority(Data As T, Priority As Integer)
        If RearPointer < MaxSize - 1 Then
            RearPointer += 1
            ' Add the element and its priority to the queue
            Queue.Add(Tuple.Create(Data, Priority))
            Queue = Queue.OrderBy(Function(item) item.Item2).ToList() 'sorts the list by the priority key, ascending so 0 = high, 1 = low
            If RearPointer = MaxSize - 1 Then
                IsFull = True
            End If
        Else
            CustomMsgBox.ShowBox("Error: DataBufferQueue full. Item not added. Try again.", "ERROR", False)
        End If
        IsEmpty = False
    End Sub

    Public Function Dequeue() As T
        If Queue.Count > 0 Then
            ' Retrieve the element with the highest priority
            Dim Data As T = Queue(FrontPointer).Item1
            Queue.RemoveAt(FrontPointer)
            RearPointer -= 1
            Select Case RearPointer
                Case -1 ' last item in the queue
                    IsEmpty = True
            End Select
            Return Data
        Else
            Return Nothing 'empty queue
        End If
    End Function

    Public Function Peek() As T
        If Queue.Count > 0 Then
            Return Queue(FrontPointer).Item1
        Else
            Return Nothing
        End If
    End Function

    Public Function Contains(Element As T) As Boolean
        Return Queue.Contains(Tuple.Create(Element, 0)) OrElse Queue.Contains(Tuple.Create(Element, 1))
    End Function

    Public Function GetCurrentSize() As Integer
        Return RearPointer + 1
    End Function
End Class
