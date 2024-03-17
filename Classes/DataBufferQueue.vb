Public Class DataBufferQueue(Of T)
    Private Property RearPointer As Integer = -1
    ' List(Of Tuple(Of Template, Integer)) to store both the element and its priority
    Private Property Queue As List(Of Tuple(Of T, Integer))
    Private Property MaxSize As Integer
    ' FrontPointer is not required to change - the first data item in the queue will always be in the 0th index
    Private Const FrontPointer As Integer = 0
    Private Property QueueEmpty As Boolean
    Public ReadOnly Property IsEmpty As Boolean 'property to get whether queue is empty
        Get
            Return QueueEmpty
        End Get
    End Property

    Private Property QueueFull As Boolean
    Public ReadOnly Property IsFull As Boolean 'property to get if queue is full
        Get
            Return QueueFull
        End Get
    End Property
    Public Sub New(Optional MaxElements As Integer = 200)
        Me.MaxSize = MaxElements
        QueueEmpty = True
        QueueFull = False
        Queue = New List(Of Tuple(Of T, Integer))
    End Sub

    Public Sub DeleteLastElement() 'deletes the last added element to the queue
        If Queue.Count > 0 Then
            Select Case RearPointer
                Case 0 'front pointer = rear pointer meaning that it is the last item in the queue
                    ' Remove the first item in the queue and update pointers
                    Queue.RemoveAt(FrontPointer)
                    RearPointer -= 1
                    QueueEmpty = True
                Case Else
                    ' Remove the first item in the queue and update pointers
                    Queue.RemoveAt(FrontPointer)
                    RearPointer -= 1
            End Select
        End If
    End Sub

    Public Sub Enqueue(Data As T, Optional Priority As String = "") 'adds an item to the queue with a specified or default (low) priority
        Dim PriorityInt As Integer = If(Priority.ToLower() = "high", 0, 1) '0 is high priority, 1 is everything else, checks for priority string high, else default priority
        If RearPointer < MaxSize - 1 Then
            RearPointer += 1
            ' Add the element and its priority to the queue
            Queue.Add(Tuple.Create(Data, PriorityInt))
            Queue = Queue.OrderBy(Function(item) item.Item2).ToList() 'sorts the list by the priority key, ascending so 0 = high, 1 = low
            If RearPointer = MaxSize - 1 Then
                QueueFull = True
            End If
        Else
            CustomMsgBox.ShowBox("Error: DataBufferQueue full. Item not added.", "ERROR", False)
        End If
        QueueEmpty = False
    End Sub

    Public Function Dequeue() As T 'takes off the first data item in the queue and returns it, updating the pointers as well
        If Queue.Count > 0 Then
            ' Retrieve the element with the highest priority
            Dim Data As T = Queue(FrontPointer).Item1
            Queue.RemoveAt(FrontPointer)
            RearPointer -= 1
            Select Case RearPointer
                Case -1 ' last item in the queue
                    QueueEmpty = True
            End Select
            Return Data
        Else
            Return Nothing 'empty queue
        End If
    End Function

    Public Function Peek() As T 'peeks at the first item in the queue
        If Queue.Count > 0 Then
            Return Queue(FrontPointer).Item1
        Else
            Return Nothing
        End If
    End Function

    Public Function Contains(Element As T) As Boolean 'checks whether an element is in the queue - either high or low priority
        Return Queue.Contains(Tuple.Create(Element, 0)) OrElse Queue.Contains(Tuple.Create(Element, 1))
    End Function

    Public ReadOnly Property GetCurrentSize() As Integer 'returns current size of the queue
        Get
            Return RearPointer + 1
        End Get
    End Property

    Public ReadOnly Property GetQueueAsList() As List(Of T) 'returns the underlying tuple-list as a list of the data type without priorities
        Get
            Return Queue.Select(Function(item) item.Item1).ToList()
        End Get
    End Property
End Class
