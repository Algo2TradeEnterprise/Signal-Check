Namespace Indicator
    Public Module FractalBands
        Public Sub CalculateFractal(ByVal inputPayload As Dictionary(Of Date, Payload), ByRef outputHighPayload As Dictionary(Of Date, Decimal), ByRef outputLowPayload As Dictionary(Of Date, Decimal))
            If inputPayload IsNot Nothing AndAlso inputPayload.Count > 0 Then
                Dim highFractal As Decimal = 0
                Dim lowFractal As Decimal = 0
                For Each runningPayload In inputPayload.Keys
                    If inputPayload(runningPayload).PreviousCandlePayload IsNot Nothing AndAlso
                        inputPayload(runningPayload).PreviousCandlePayload.PreviousCandlePayload IsNot Nothing AndAlso
                        inputPayload(runningPayload).PreviousCandlePayload.PreviousCandlePayload.PreviousCandlePayload IsNot Nothing AndAlso
                        inputPayload(runningPayload).PreviousCandlePayload.PreviousCandlePayload.PreviousCandlePayload.PreviousCandlePayload IsNot Nothing Then
                        If inputPayload(runningPayload).PreviousCandlePayload.PreviousCandlePayload.PreviousCandlePayload.PreviousCandlePayload.High < inputPayload(runningPayload).PreviousCandlePayload.PreviousCandlePayload.High AndAlso
                            inputPayload(runningPayload).PreviousCandlePayload.PreviousCandlePayload.PreviousCandlePayload.High <= inputPayload(runningPayload).PreviousCandlePayload.PreviousCandlePayload.High AndAlso
                            inputPayload(runningPayload).PreviousCandlePayload.High < inputPayload(runningPayload).PreviousCandlePayload.PreviousCandlePayload.High AndAlso
                            inputPayload(runningPayload).High < inputPayload(runningPayload).PreviousCandlePayload.PreviousCandlePayload.High Then
                            highFractal = inputPayload(runningPayload).PreviousCandlePayload.PreviousCandlePayload.High
                        End If
                        If inputPayload(runningPayload).PreviousCandlePayload.PreviousCandlePayload.PreviousCandlePayload.PreviousCandlePayload.Low > inputPayload(runningPayload).PreviousCandlePayload.PreviousCandlePayload.Low AndAlso
                            inputPayload(runningPayload).PreviousCandlePayload.PreviousCandlePayload.PreviousCandlePayload.Low > inputPayload(runningPayload).PreviousCandlePayload.PreviousCandlePayload.Low AndAlso
                            inputPayload(runningPayload).PreviousCandlePayload.Low > inputPayload(runningPayload).PreviousCandlePayload.PreviousCandlePayload.Low AndAlso
                            inputPayload(runningPayload).Low > inputPayload(runningPayload).PreviousCandlePayload.PreviousCandlePayload.Low Then
                            lowFractal = inputPayload(runningPayload).PreviousCandlePayload.PreviousCandlePayload.Low
                        End If
                    End If
                    If outputHighPayload Is Nothing Then outputHighPayload = New Dictionary(Of Date, Decimal)
                    outputHighPayload.Add(runningPayload, highFractal)
                    If outputLowPayload Is Nothing Then outputLowPayload = New Dictionary(Of Date, Decimal)
                    outputLowPayload.Add(runningPayload, lowFractal)
                Next
            End If
        End Sub
    End Module
End Namespace
