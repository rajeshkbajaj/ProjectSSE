; PB980 SW update using VTS and AutoIT scripting
;
; This script uses the VTS to do a 980 vent software update.
; It assumes that the VTS package is at C:\PB980-REV-AUTOBUILD.pkg

Local $vtsWindowName = "980 Ventilator Test Software"
Local $swUpload = "Software Upload"
Local $swUploadSuccess = "Software Download Succeeded"
Local $fileSelectWindow = "Open"
Local $returnCode = 0

; Run the VTS
ConsoleWrite ( "Starting VTS" & @CRLF )
Run("C:\Program Files (x86)\Covidien\VTS\VTS.exe")

; Register a periodic to check for popups
AdlibRegister("PopupReviewException")

ConsoleWrite ( "Wait for the VTS to become active" & @CRLF )
WinWait($vtsWindowName)

ConsoleWrite ( "Wait for connect button to appear" & @CRLF )
While 1
    If ControlCommand($vtsWindowName, "", "[TEXT:Connect]", "IsEnabled", "") Then ExitLoop
    Sleep(100) ; Important or you eat all the CPU
WEnd

ConsoleWrite ("Click the Connect button" & @CRLF )
ControlClick( $vtsWindowName, "", "[TEXT:Connect]")

ConsoleWrite( "Wait for connection" & @CRLF )
While 1
	Local $sText = ControlGetText($vtsWindowName, "", "[NAME:mConnectionStatusLabel]")
    If $sText == "Connected" Then ExitLoop
    Sleep(100) ; Important or you eat all the CPU
WEnd

ConsoleWrite("Select Software Update tab" & @CRLF)
ControlCommand($vtsWindowName, "", "[NAME:mMainTabControl]", "TabRight", "" )

Sleep(1000)

ConsoleWrite( "Enter user name and password and LOGIN" & @CRLF )
ControlSetText($vtsWindowName, "", "[NAME:mUserNameTextBox]", "anon.ymous@covidien.com")
ControlSetText($vtsWindowName, "", "[NAME:mPasswordTextBox]", "Anon11235*")

ConsoleWrite ("Wait for the Login button to appear" & @CRLF)
While 1
    If ControlCommand($vtsWindowName, "", "[TEXT:Login]", "IsEnabled", "") Then ExitLoop
    Sleep(100) ; Important or you eat all the CPU
WEnd

ConsoleWrite("Click on Login button" & @CRLF)
ControlClick( $vtsWindowName, "", "[TEXT:Login]")

ConsoleWrite("Wait for the Update Device Software button to appear" & @CRLF)
While 1
    If ControlCommand($vtsWindowName, "", "[TEXT:Update Device Software]", "IsEnabled", "") Then ExitLoop
    Sleep(100) ; Important or you eat all the CPU
WEnd

ConsoleWrite("Bring up the Open dialog and specify the file to open" & @CRLF)
ControlClick( $vtsWindowName, "", "[TEXT:Update Device Software]")
WinWait($fileSelectWindow)
ControlSetText($fileSelectWindow, "", "Edit1", "C:\PB980-REV-AUTOBUILD.pkg")
ControlClick( $fileSelectWindow, "", "[TEXT:&Open]")

ConsoleWrite("Wait for software upload dialog" & @CRLF)
WinWait($swUpload)
ConsoleWrite("Click Start" & @CRLF)
ControlClick( $swUpload, "", "[TEXT:Start]")

ConsoleWrite( "Wait for download to finish" & @CRLF)
While 1
	If WinExists($swUploadSuccess) Then ExitLoop
    Sleep(100) ; Important or you eat all the CPU
WEnd

ConsoleWrite("Click OK" & @CRLF)
ControlClick( $swUploadSuccess, "", "[TEXT:OK]")

ConsoleWrite("Download complete. Exiting" & @CRLF)

; Now quit by sending a "close" request to the VTS
WinClose($vtsWindowName)

; Now wait for VTS to close before continuing
WinWaitClose($vtsWindowName)

Exit $returnCode
;;;;; Finished! ;;;;;

;;;; Subroutines ;;;;
Func PopupReviewException()
	Local $connectionFailed =  "Connection Failed"
	Local $closeConfirmation =  "Close Confirmation"
	If WinExists($connectionFailed) Then
		ControlClick($connectionFailed,  "", "[CLASS:Button; TEXT:OK]", "left")
		WinWaitClose($connectionFailed)
		ConsoleWriteError ( "Connection Failed" & @CRLF )
		$returnCode = 1
		WinClose($vtsWindowName)
	 EndIf
	If WinExists($closeConfirmation) Then
		ControlClick($closeConfirmation, "", "[CLASS:Button; TEXT:&Yes]")
		WinWaitClose($closeConfirmation)
		ConsoleWrite("Close confirmed. Exiting" & @CRLF)
		Exit $returnCode
	 EndIf
EndFunc
