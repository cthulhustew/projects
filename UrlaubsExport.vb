Sub TermineExportieren()
    Dim olApp As Object
    Dim olNamespace As Object
    Dim olFolder As Object
    Dim olItem As Object
    Dim xlApp As Object
    Dim xlWorkbook As Object
    Dim xlWorksheet As Object
    Dim i As Long
    Dim outputPath As String
    Dim fileNumber As Long
    Dim fileName As String
    
    ' Outlook-Verbindung herstellen
    Set olApp = CreateObject("Outlook.Application")
    Set olNamespace = olApp.GetNamespace("MAPI")
    Set olFolder = olNamespace.GetDefaultFolder(9) ' 9 entspricht dem Kalenderordner
    
    ' Excel-Verbindung herstellen
    Set xlApp = CreateObject("Excel.Application")
    Set xlWorkbook = xlApp.Workbooks.Add
    Set xlWorksheet = xlWorkbook.Sheets(1)
    
    ' Pfad und Dateiname für die Excel-Tabelle
    outputPath = "C:\Temp\"
    fileName = "Uebersicht_2024.xlsx"
    
    ' Überprüfen, ob die Datei bereits existiert
    fileNumber = 1
    Do While Dir(outputPath & fileName) <> ""
        fileName = "Uebersicht_2024_" & Format(fileNumber, "00") & ".xlsx"
        fileNumber = fileNumber + 1
    Loop
    
    ' Spaltenüberschriften in Excel setzen
    xlWorksheet.Cells(1, 1).Value = "Termin"
    xlWorksheet.Cells(1, 2).Value = "Anfangsdatum"
    xlWorksheet.Cells(1, 3).Value = "Enddatum"
    
    ' Alle Termine durchgehen
    i = 2
    For Each olItem In olFolder.Items
        If olItem.Class = 26 And olItem.Categories = "Urlaub / Krank" Then ' 26 entspricht einem Termin
            xlWorksheet.Cells(i, 1).Value = olItem.Subject
            xlWorksheet.Cells(i, 2).Value = Format(olItem.Start, "dd.mm.yyyy") ' Nur das Anfangsdatum übernehmen
            xlWorksheet.Cells(i, 3).Value = Format(olItem.GetRecurrencePattern.PatternEndDate, "dd.mm.yyyy") ' Nur das Enddatum des Serientermins übernehmen
            i = i + 1
        End If
    Next olItem
    
    ' Spaltenbreite anpassen
    xlWorksheet.Columns("A:C").AutoFit
    
    ' Fett darstellen
    xlWorksheet.Range("A1:C1").Font.Bold = True
    
    ' Excel-Tabelle speichern und schließen
    xlWorkbook.SaveAs outputPath & fileName
    xlWorkbook.Close SaveChanges:=False
    xlApp.Quit
    
    ' Aufräumen
    Set olApp = Nothing
    Set olNamespace = Nothing
    Set olFolder = Nothing
    Set olItem = Nothing
    Set xlApp = Nothing
    Set xlWorkbook = Nothing
    Set xlWorksheet = Nothing
    
    MsgBox "Die Termine wurden erfolgreich in die Excel-Tabelle " & fileName & " exportiert!", vbInformation
End Sub
