﻿Namespace Indicator
    Public Module SwingHighLowWithVWAP
        Public Sub CalculateSwingHighLowWithVWAP(ByVal inputPayload As Dictionary(Of Date, Payload), ByVal strict As Boolean, ByRef outputSwingHighPayload As Dictionary(Of Date, Decimal), ByRef outputSwingLowPayload As Dictionary(Of Date, Decimal), ByRef outputVWAPTouchSwingHighPayload As Dictionary(Of Date, Decimal), ByRef outputVWAPTouchSwingLowPayload As Dictionary(Of Date, Decimal))
            If inputPayload IsNot Nothing AndAlso inputPayload.Count > 0 Then
                Dim VWAPPayload As Dictionary(Of Date, Decimal) = Nothing
                Indicator.VWAP.CalculateVWAP(inputPayload, VWAPPayload)
                For Each runningPayload In inputPayload.Keys
                    Dim swingHigh As Decimal = 0
                    Dim swingLow As Decimal = 0
                    Dim vwapTouchSwingHigh As Decimal = 0
                    Dim vwapTouchSwingLow As Decimal = 0
                    If strict Then
                        If inputPayload(runningPayload).PreviousCandlePayload Is Nothing Then
                            swingHigh = inputPayload(runningPayload).High
                            swingLow = inputPayload(runningPayload).Low
                            If inputPayload(runningPayload).High > VWAPPayload(runningPayload) AndAlso
                                inputPayload(runningPayload).Low < VWAPPayload(runningPayload) Then
                                vwapTouchSwingHigh = swingHigh
                                vwapTouchSwingLow = swingLow
                            End If
                        ElseIf inputPayload(runningPayload).PreviousCandlePayload IsNot Nothing AndAlso inputPayload(runningPayload).PreviousCandlePayload.PreviousCandlePayload Is Nothing Then
                            If inputPayload(runningPayload).PreviousCandlePayload.High > inputPayload(runningPayload).High AndAlso
                                inputPayload(runningPayload).PreviousCandlePayload.Volume <> 0 Then
                                swingHigh = inputPayload(runningPayload).PreviousCandlePayload.High
                                If (inputPayload(runningPayload).High > VWAPPayload(runningPayload) AndAlso
                                    inputPayload(runningPayload).Low < VWAPPayload(runningPayload)) OrElse
                                    (inputPayload(runningPayload).PreviousCandlePayload.High > VWAPPayload(inputPayload(runningPayload).PreviousCandlePayload.PayloadDate) AndAlso
                                    inputPayload(runningPayload).PreviousCandlePayload.Low < VWAPPayload(inputPayload(runningPayload).PreviousCandlePayload.PayloadDate)) Then
                                    vwapTouchSwingHigh = swingHigh
                                Else
                                    vwapTouchSwingHigh = outputVWAPTouchSwingHighPayload(inputPayload(runningPayload).PreviousCandlePayload.PayloadDate)
                                End If
                            Else
                                swingHigh = outputSwingHighPayload(inputPayload(runningPayload).PreviousCandlePayload.PayloadDate)
                                vwapTouchSwingHigh = outputVWAPTouchSwingHighPayload(inputPayload(runningPayload).PreviousCandlePayload.PayloadDate)
                            End If
                            If inputPayload(runningPayload).PreviousCandlePayload.Low < inputPayload(runningPayload).Low AndAlso
                                inputPayload(runningPayload).PreviousCandlePayload.Volume <> 0 Then
                                swingLow = inputPayload(runningPayload).PreviousCandlePayload.Low
                                If (inputPayload(runningPayload).High > VWAPPayload(runningPayload) AndAlso
                                    inputPayload(runningPayload).Low < VWAPPayload(runningPayload)) OrElse
                                    (inputPayload(runningPayload).PreviousCandlePayload.High > VWAPPayload(inputPayload(runningPayload).PreviousCandlePayload.PayloadDate) AndAlso
                                    inputPayload(runningPayload).PreviousCandlePayload.Low < VWAPPayload(inputPayload(runningPayload).PreviousCandlePayload.PayloadDate)) Then
                                    vwapTouchSwingLow = swingLow
                                Else
                                    vwapTouchSwingLow = outputVWAPTouchSwingLowPayload(inputPayload(runningPayload).PreviousCandlePayload.PayloadDate)
                                End If
                            Else
                                swingLow = outputSwingLowPayload(inputPayload(runningPayload).PreviousCandlePayload.PayloadDate)
                                vwapTouchSwingLow = outputVWAPTouchSwingLowPayload(inputPayload(runningPayload).PreviousCandlePayload.PayloadDate)
                            End If
                        Else
                            If inputPayload(runningPayload).PreviousCandlePayload.High > inputPayload(runningPayload).High AndAlso
                                inputPayload(runningPayload).PreviousCandlePayload.High > inputPayload(runningPayload).PreviousCandlePayload.PreviousCandlePayload.High AndAlso
                                inputPayload(runningPayload).PreviousCandlePayload.Volume <> 0 Then
                                swingHigh = inputPayload(runningPayload).PreviousCandlePayload.High
                                If (inputPayload(runningPayload).High > VWAPPayload(runningPayload) AndAlso
                                    inputPayload(runningPayload).Low < VWAPPayload(runningPayload)) OrElse
                                    (inputPayload(runningPayload).PreviousCandlePayload.High > VWAPPayload(inputPayload(runningPayload).PreviousCandlePayload.PayloadDate) AndAlso
                                    inputPayload(runningPayload).PreviousCandlePayload.Low < VWAPPayload(inputPayload(runningPayload).PreviousCandlePayload.PayloadDate)) OrElse
                                    (inputPayload(runningPayload).PreviousCandlePayload.PreviousCandlePayload.High > VWAPPayload(inputPayload(runningPayload).PreviousCandlePayload.PreviousCandlePayload.PayloadDate) AndAlso
                                    inputPayload(runningPayload).PreviousCandlePayload.PreviousCandlePayload.Low < VWAPPayload(inputPayload(runningPayload).PreviousCandlePayload.PreviousCandlePayload.PayloadDate)) Then
                                    vwapTouchSwingHigh = swingHigh
                                Else
                                    vwapTouchSwingHigh = outputVWAPTouchSwingHighPayload(inputPayload(runningPayload).PreviousCandlePayload.PayloadDate)
                                End If
                            Else
                                swingHigh = outputSwingHighPayload(inputPayload(runningPayload).PreviousCandlePayload.PayloadDate)
                                vwapTouchSwingHigh = outputVWAPTouchSwingHighPayload(inputPayload(runningPayload).PreviousCandlePayload.PayloadDate)
                            End If
                            If inputPayload(runningPayload).PreviousCandlePayload.Low < inputPayload(runningPayload).Low AndAlso
                                inputPayload(runningPayload).PreviousCandlePayload.Low < inputPayload(runningPayload).PreviousCandlePayload.PreviousCandlePayload.Low AndAlso
                                inputPayload(runningPayload).PreviousCandlePayload.Volume <> 0 Then
                                swingLow = inputPayload(runningPayload).PreviousCandlePayload.Low
                                If (inputPayload(runningPayload).High > VWAPPayload(runningPayload) AndAlso
                                    inputPayload(runningPayload).Low < VWAPPayload(runningPayload)) OrElse
                                    (inputPayload(runningPayload).PreviousCandlePayload.High > VWAPPayload(inputPayload(runningPayload).PreviousCandlePayload.PayloadDate) AndAlso
                                    inputPayload(runningPayload).PreviousCandlePayload.Low < VWAPPayload(inputPayload(runningPayload).PreviousCandlePayload.PayloadDate)) OrElse
                                    (inputPayload(runningPayload).PreviousCandlePayload.PreviousCandlePayload.High > VWAPPayload(inputPayload(runningPayload).PreviousCandlePayload.PreviousCandlePayload.PayloadDate) AndAlso
                                    inputPayload(runningPayload).PreviousCandlePayload.PreviousCandlePayload.Low < VWAPPayload(inputPayload(runningPayload).PreviousCandlePayload.PreviousCandlePayload.PayloadDate)) Then
                                    vwapTouchSwingLow = swingLow
                                Else
                                    vwapTouchSwingLow = outputVWAPTouchSwingLowPayload(inputPayload(runningPayload).PreviousCandlePayload.PayloadDate)
                                End If
                            Else
                                swingLow = outputSwingLowPayload(inputPayload(runningPayload).PreviousCandlePayload.PayloadDate)
                                vwapTouchSwingLow = outputVWAPTouchSwingLowPayload(inputPayload(runningPayload).PreviousCandlePayload.PayloadDate)
                            End If
                        End If
                    Else
                        If inputPayload(runningPayload).PreviousCandlePayload Is Nothing Then
                            swingHigh = inputPayload(runningPayload).High
                            swingLow = inputPayload(runningPayload).Low
                            If inputPayload(runningPayload).High > VWAPPayload(runningPayload) AndAlso
                                inputPayload(runningPayload).Low < VWAPPayload(runningPayload) Then
                                vwapTouchSwingHigh = swingHigh
                                vwapTouchSwingLow = swingLow
                            End If
                        ElseIf inputPayload(runningPayload).PreviousCandlePayload IsNot Nothing AndAlso inputPayload(runningPayload).PreviousCandlePayload.PreviousCandlePayload Is Nothing Then
                            If inputPayload(runningPayload).PreviousCandlePayload.High >= inputPayload(runningPayload).High AndAlso
                                inputPayload(runningPayload).PreviousCandlePayload.Volume <> 0 Then
                                swingHigh = inputPayload(runningPayload).PreviousCandlePayload.High
                                If (inputPayload(runningPayload).High >= VWAPPayload(runningPayload) AndAlso
                                    inputPayload(runningPayload).Low <= VWAPPayload(runningPayload)) OrElse
                                    (inputPayload(runningPayload).PreviousCandlePayload.High >= VWAPPayload(inputPayload(runningPayload).PreviousCandlePayload.PayloadDate) AndAlso
                                    inputPayload(runningPayload).PreviousCandlePayload.Low <= VWAPPayload(inputPayload(runningPayload).PreviousCandlePayload.PayloadDate)) Then
                                    vwapTouchSwingHigh = swingHigh
                                Else
                                    vwapTouchSwingHigh = outputVWAPTouchSwingHighPayload(inputPayload(runningPayload).PreviousCandlePayload.PayloadDate)
                                End If
                            Else
                                swingHigh = outputSwingHighPayload(inputPayload(runningPayload).PreviousCandlePayload.PayloadDate)
                                vwapTouchSwingHigh = outputVWAPTouchSwingHighPayload(inputPayload(runningPayload).PreviousCandlePayload.PayloadDate)
                            End If
                            If inputPayload(runningPayload).PreviousCandlePayload.Low <= inputPayload(runningPayload).Low AndAlso
                                inputPayload(runningPayload).PreviousCandlePayload.Volume <> 0 Then
                                swingLow = inputPayload(runningPayload).PreviousCandlePayload.Low
                                If (inputPayload(runningPayload).High >= VWAPPayload(runningPayload) AndAlso
                                    inputPayload(runningPayload).Low <= VWAPPayload(runningPayload)) OrElse
                                    (inputPayload(runningPayload).PreviousCandlePayload.High >= VWAPPayload(inputPayload(runningPayload).PreviousCandlePayload.PayloadDate) AndAlso
                                    inputPayload(runningPayload).PreviousCandlePayload.Low <= VWAPPayload(inputPayload(runningPayload).PreviousCandlePayload.PayloadDate)) Then
                                    vwapTouchSwingLow = swingLow
                                Else
                                    vwapTouchSwingLow = outputVWAPTouchSwingLowPayload(inputPayload(runningPayload).PreviousCandlePayload.PayloadDate)
                                End If
                            Else
                                swingLow = outputSwingLowPayload(inputPayload(runningPayload).PreviousCandlePayload.PayloadDate)
                                vwapTouchSwingLow = outputVWAPTouchSwingLowPayload(inputPayload(runningPayload).PreviousCandlePayload.PayloadDate)
                            End If
                        Else
                            If inputPayload(runningPayload).PreviousCandlePayload.High >= inputPayload(runningPayload).High AndAlso
                                inputPayload(runningPayload).PreviousCandlePayload.High >= inputPayload(runningPayload).PreviousCandlePayload.PreviousCandlePayload.High AndAlso
                                inputPayload(runningPayload).PreviousCandlePayload.Volume <> 0 Then
                                swingHigh = inputPayload(runningPayload).PreviousCandlePayload.High
                                If (inputPayload(runningPayload).High >= VWAPPayload(runningPayload) AndAlso
                                   inputPayload(runningPayload).Low <= VWAPPayload(runningPayload)) OrElse
                                   (inputPayload(runningPayload).PreviousCandlePayload.High >= VWAPPayload(inputPayload(runningPayload).PreviousCandlePayload.PayloadDate) AndAlso
                                   inputPayload(runningPayload).PreviousCandlePayload.Low <= VWAPPayload(inputPayload(runningPayload).PreviousCandlePayload.PayloadDate)) OrElse
                                   (inputPayload(runningPayload).PreviousCandlePayload.PreviousCandlePayload.High >= VWAPPayload(inputPayload(runningPayload).PreviousCandlePayload.PreviousCandlePayload.PayloadDate) AndAlso
                                   inputPayload(runningPayload).PreviousCandlePayload.PreviousCandlePayload.Low <= VWAPPayload(inputPayload(runningPayload).PreviousCandlePayload.PreviousCandlePayload.PayloadDate)) Then
                                    vwapTouchSwingHigh = swingHigh
                                Else
                                    vwapTouchSwingHigh = outputVWAPTouchSwingHighPayload(inputPayload(runningPayload).PreviousCandlePayload.PayloadDate)
                                End If
                            Else
                                swingHigh = outputSwingHighPayload(inputPayload(runningPayload).PreviousCandlePayload.PayloadDate)
                                vwapTouchSwingHigh = outputVWAPTouchSwingHighPayload(inputPayload(runningPayload).PreviousCandlePayload.PayloadDate)
                            End If
                            If inputPayload(runningPayload).PreviousCandlePayload.Low <= inputPayload(runningPayload).Low AndAlso
                                inputPayload(runningPayload).PreviousCandlePayload.Low <= inputPayload(runningPayload).PreviousCandlePayload.PreviousCandlePayload.Low AndAlso
                                inputPayload(runningPayload).PreviousCandlePayload.Volume <> 0 Then
                                swingLow = inputPayload(runningPayload).PreviousCandlePayload.Low
                                If (inputPayload(runningPayload).High >= VWAPPayload(runningPayload) AndAlso
                                    inputPayload(runningPayload).Low <= VWAPPayload(runningPayload)) OrElse
                                    (inputPayload(runningPayload).PreviousCandlePayload.High >= VWAPPayload(inputPayload(runningPayload).PreviousCandlePayload.PayloadDate) AndAlso
                                    inputPayload(runningPayload).PreviousCandlePayload.Low <= VWAPPayload(inputPayload(runningPayload).PreviousCandlePayload.PayloadDate)) OrElse
                                    (inputPayload(runningPayload).PreviousCandlePayload.PreviousCandlePayload.High >= VWAPPayload(inputPayload(runningPayload).PreviousCandlePayload.PreviousCandlePayload.PayloadDate) AndAlso
                                    inputPayload(runningPayload).PreviousCandlePayload.PreviousCandlePayload.Low <= VWAPPayload(inputPayload(runningPayload).PreviousCandlePayload.PreviousCandlePayload.PayloadDate)) Then
                                    vwapTouchSwingLow = swingLow
                                Else
                                    vwapTouchSwingLow = outputVWAPTouchSwingLowPayload(inputPayload(runningPayload).PreviousCandlePayload.PayloadDate)
                                End If
                            Else
                                swingLow = outputSwingLowPayload(inputPayload(runningPayload).PreviousCandlePayload.PayloadDate)
                                vwapTouchSwingLow = outputVWAPTouchSwingLowPayload(inputPayload(runningPayload).PreviousCandlePayload.PayloadDate)
                            End If
                        End If
                    End If

                    If outputSwingHighPayload Is Nothing Then outputSwingHighPayload = New Dictionary(Of Date, Decimal)
                    outputSwingHighPayload.Add(runningPayload, swingHigh)
                    If outputSwingLowPayload Is Nothing Then outputSwingLowPayload = New Dictionary(Of Date, Decimal)
                    outputSwingLowPayload.Add(runningPayload, swingLow)
                    If outputVWAPTouchSwingHighPayload Is Nothing Then outputVWAPTouchSwingHighPayload = New Dictionary(Of Date, Decimal)
                    outputVWAPTouchSwingHighPayload.Add(runningPayload, vwapTouchSwingHigh)
                    If outputVWAPTouchSwingLowPayload Is Nothing Then outputVWAPTouchSwingLowPayload = New Dictionary(Of Date, Decimal)
                    outputVWAPTouchSwingLowPayload.Add(runningPayload, vwapTouchSwingLow)
                Next
            End If
        End Sub
    End Module
End Namespace