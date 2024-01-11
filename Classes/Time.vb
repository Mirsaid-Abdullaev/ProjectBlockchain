Module Time 'credit - https://stackoverflow.com/a/10534461/22325386 'this has been modified slightly to include a millisecond value too
    ''' <summary>
    ''' Calculates the Unix timestamp of the given date/time.
    ''' </summary>
    ''' <param name="CurrentDateTime">The current date/time as represented by the system clock - Date.Now works.</param>
    ''' <returns>A Unix timestamp with millisecond precision of the specified time as a string</returns>
    Public Function TimeToUnixMs(ByVal CurrentDateTime As Date) As String
        If CurrentDateTime.IsDaylightSavingTime = True Then
            CurrentDateTime = DateAdd(DateInterval.Hour, -1, CurrentDateTime)
        End If
        Return DateDiff(DateInterval.Second, #1/1/1970#, CurrentDateTime) * 1000 + CurrentDateTime.Millisecond
    End Function
End Module

