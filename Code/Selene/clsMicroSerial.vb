Imports System.IO.Ports
Imports System.Collections.Concurrent
Imports System.Threading
Imports System.Threading.Tasks

''' <summary>
''' Professional Asynchronous Serial Wrapper for NodeMCU/MicroPython
''' </summary>
Public Class clsMicroSerial
    ' The actual Serial Port
    Private WithEvents _Port As New SerialPort()

    ' Thread-Safe Buffer for incoming data
    Private _RxBuffer As New ConcurrentQueue(Of String)

    ' Cancellation token to stop background tasks safely
    Private _CancelToken As CancellationTokenSource

    ' Events to update the UI
    Public Event DataReceivedUI(data As String)
    Public Event ConnectionStatusChanged(isConnected As Boolean)
    Public Event PromptDetected() ' Fires when ">" is seen (Ready for next command)

    ' Properties
    Public ReadOnly Property IsOpen As Boolean
        Get
            Return _Port.IsOpen
        End Get
    End Property

    Public Sub New()
        ' Set defaults optimized for ESP8266/ESP32
        _Port.DtrEnable = True  ' Critical for some NodeMCU boards
        _Port.RtsEnable = True
        _Port.ReadTimeout = 500
        _Port.WriteTimeout = 500
    End Sub

    ''' <summary>
    ''' Connects to the specified COM port.
    ''' </summary>
    Public Function Connect(portName As String, baudRate As Integer) As Boolean
        Try
            If _Port.IsOpen Then _Port.Close()

            _Port.PortName = portName
            _Port.BaudRate = baudRate
            _Port.Open()

            ' Start the background listener
            _CancelToken = New CancellationTokenSource()
            Task.Run(AddressOf ProcessIncomingData, _CancelToken.Token)

            RaiseEvent ConnectionStatusChanged(True)
            Return True
        Catch ex As Exception
            ModGlobals.LogDebug("Connection Error: " & ex.Message)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Disconnects and cleans up resources.
    ''' </summary>
    Public Sub Disconnect()
        Try
            If _CancelToken IsNot Nothing Then _CancelToken.Cancel()
            If _Port.IsOpen Then _Port.Close()
        Catch ex As Exception
            ModGlobals.LogDebug("Disconnect Error: " & ex.Message)
        Finally
            RaiseEvent ConnectionStatusChanged(False)
        End Try
    End Sub


    Public Event DataSent(data As String)


    ''' <summary>
    ''' Sends a string to the device and notifies the UI.
    ''' </summary>
    Public Sub Send(text As String, Optional writeLine As Boolean = True)
        If Not _Port.IsOpen Then Exit Sub
        Try
            If writeLine Then
                _Port.WriteLine(text)
            Else
                _Port.Write(text)
            End If

            ' FIRE THE EVENT so the UI knows we sent something
            RaiseEvent DataSent(text)

        Catch ex As Exception
            ModGlobals.LogDebug("Send Error: " & ex.Message)
        End Try
    End Sub
    ''' <summary>
    ''' Sends a raw byte (useful for Ctrl+C / Ctrl+B signals).
    ''' </summary>
    Public Sub SendByte(data As Byte)
        If Not _Port.IsOpen Then Exit Sub
        Dim buffer As Byte() = {data}
        _Port.Write(buffer, 0, 1)
    End Sub

    ' ---------------------------------------------------------
    ' INTERNAL ENGINE LOGIC
    ' ---------------------------------------------------------

    ' Native Serial Event - Runs on a secondary thread
    Private Sub _Port_DataReceived(sender As Object, e As SerialDataReceivedEventArgs) Handles _Port.DataReceived
        Try
            Dim data As String = _Port.ReadExisting()
            If Not String.IsNullOrEmpty(data) Then
                _RxBuffer.Enqueue(data)
            End If
        Catch ex As Exception
            ' Handle disconnects gracefully
        End Try
    End Sub

    ' Background Task to process buffer and update UI safely
    Private Async Sub ProcessIncomingData()
        While Not _CancelToken.Token.IsCancellationRequested
            Dim dataChunk As String = ""

            ' Dequeue everything available to process in one batch
            While Not _RxBuffer.IsEmpty
                Dim temp As String = ""
                If _RxBuffer.TryDequeue(temp) Then
                    dataChunk &= temp
                End If
            End While

            If dataChunk.Length > 0 Then
                ' 1. Check for Lua Prompt (Critical for Handshake)
                If dataChunk.Contains(">") OrElse dataChunk.Contains(">>") Then
                    RaiseEvent PromptDetected()
                End If

                ' 2. Send to UI (Must be marshaled to UI thread by the Form later)
                RaiseEvent DataReceivedUI(dataChunk)
            End If

            ' Small delay to prevent CPU hogging
            Await Task.Delay(10)
        End While
    End Sub

    ''' <summary>
    ''' Returns a clean list of available COM ports.
    ''' </summary>
    Public Shared Function GetAvailablePorts() As String()
        Return SerialPort.GetPortNames()
    End Function

End Class