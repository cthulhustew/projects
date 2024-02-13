Sub KalenderExport()
    ' Deklaration der benötigten Variablen
    Dim myNameSpace As Outlook.NameSpace
    Dim startDate As String
    Dim endDate As String
    Dim myAppointments As Outlook.Items
    Dim currentAppointment As Outlook.AppointmentItem
    Dim xlApp As Object
    Dim xlWB As Object
    Dim xlSheet As Object
    Dim i As Integer
    Dim userInput As Variant
    Dim msgResponse As Integer
    Dim chartObj As Object
    Dim seriesObj As Object
    Dim lastRow As Long
    Dim printWks As Object
    Dim OutApp As Object
    Dim OutMail As Object

    ' Benutzereingabe abfragen
    Do
        userInput = InputBox("Bitte geben Sie den Monat und das Jahr im Format MM.JJJJ ein, z.B. 01.2024 für Januar 2024.")
        If userInput = "" Then
            msgResponse = MsgBox("Sie haben keine Daten eingegeben." & vbNewLine & "Wenn Sie OK drücken können Sie erneut Daten eingeben, wenn Sie auf Abbrechen drücken wird das Programm beendet", vbOKCancel + vbExclamation, "Keine Daten eingegeben")
            If msgResponse = vbCancel Then
                Exit Sub
            End If
        End If
    Loop While userInput = ""

    ' Konvertiere die Benutzereingabe in ein Datumsformat
    startDate = "01/" & Left(userInput, 2) & "/" & Right(userInput, 4) & " 12:00 AM"
    endDate = DateAdd("m", 1, startDate) - TimeValue("00:01:00")

    ' Outlook-Objekte initialisieren
    Set myNameSpace = Application.GetNamespace("MAPI")
    Set myAppointments = myNameSpace.GetDefaultFolder(olFolderCalendar).Items

    myAppointments.Sort "[Start]"
    myAppointments.IncludeRecurrences = True

    ' Erstes Terminobjekt im angegebenen Zeitraum finden
    Set currentAppointment = myAppointments.Find("[Start] >= '" & startDate & "' And [Start] < '" & endDate & "'")

    ' Excel-Anwendung und Arbeitsblatt erstellen
    Set xlApp = CreateObject("Excel.Application")
    Set xlWB = xlApp.Workbooks.Add
    Set xlSheet = xlWB.Sheets(1)

    ' Arbeitsblatt benennen
    xlSheet.Name = "Uebersicht_" & userInput

    ' Überschriften setzen
    xlSheet.Cells(1, 1).Value = "Tätigkeit"
    xlSheet.Cells(1, 2).Value = "Startzeit"
    xlSheet.Cells(1, 3).Value = "Endzeit"
    xlSheet.Cells(1, 4).Value = "Dauer (Stunden)"
    xlSheet.Cells(1, 5).Value = "Kategorien"
    xlSheet.Cells(1, 7).Value = "Kategorie"
    xlSheet.Cells(1, 8).Value = "Stunden"
    xlSheet.Cells(1, 9).Value = "Summe (H)"

    ' Überschriften fett drucken
    xlSheet.Range("A1:E1").Font.Bold = True
    xlSheet.Range("G1:I1").Font.Bold = True
    xlSheet.Range("G1:I1").Font.Underline = True
    i = 2

    ' Dictionary für die Summe der Stunden pro Kategorie erstellen
    Dim CategoryHours As Object
    Set CategoryHours = CreateObject("Scripting.Dictionary")

    ' Durchlaufe Termine und fülle das Excel-Arbeitsblatt
    While Not currentAppointment Is Nothing And currentAppointment.Start < CDate(endDate)
        ' Überprüfe, ob der Termin als "Free" markiert ist und ob es sich um ein ganztägiges Ereignis handelt
        
        ' und ob die Kategorie nicht "ignore" oder "Pause" ist
        If (currentAppointment.BusyStatus <> olFree Or currentAppointment.AllDayEvent) And currentAppointment.Categories <> "Berufsschule" And currentAppointment.Categories <> "Pause" And currentAppointment.Categories <> "Urlaub / Krank" Then
            ' Excel-Zellen mit Termininformationen füllen
            xlSheet.Cells(i, 1).Value = currentAppointment.Subject
            xlSheet.Cells(i, 2).Value = currentAppointment.Start
            xlSheet.Cells(i, 3).Value = currentAppointment.End
            ' Dauer des Termins in Stunden umrechnen
            Dim duration As Double
            duration = currentAppointment.duration / 60 ' Umrechnung von Minuten in Stunden
            ' Falls die Dauer größer als 24 ist, durch 3 teilen
            If duration >= 24 Then
                duration = duration / 3
            End If
            xlSheet.Cells(i, 4).Value = duration
            xlSheet.Cells(i, 5).Value = currentAppointment.Categories ' Kategorie zur Spalte E hinzufügen
            ' Wenn die Kategorie leer ist, Zelle rot einfärben
            If currentAppointment.Categories = "" Then
                xlSheet.Cells(i, 5).Interior.Color = RGB(255, 0, 0)
            End If
            ' Dauer zur Gesamtzeit für diese Kategorie hinzufügen
            If Not CategoryHours.Exists(currentAppointment.Categories) Then
                CategoryHours.Add currentAppointment.Categories, duration
            Else
                CategoryHours(currentAppointment.Categories) = CategoryHours(currentAppointment.Categories) + duration
            End If
            i = i + 1
        End If
        Set currentAppointment = myAppointments.FindNext
    Wend

    ' Kategorien und Gesamtzeiten zum Excel-Arbeitsblatt hinzufügen
    Dim j As Integer
    j = 2
    For Each key In CategoryHours.keys
        xlSheet.Cells(j, 7).Value = key
        xlSheet.Cells(j, 8).Value = CategoryHours(key)
        j = j + 1
    Next key

    ' Gesamtsumme der Stunden aus Spalte H berechnen
    Dim totalHours As Double
    For j = 2 To i - 1
        totalHours = totalHours + xlSheet.Cells(j, 8).Value
    Next j

    ' Formel für die Gesamtsumme in Zelle I2 setzen
    xlSheet.Range("I2").Formula = "=SUM(H2:H" & i - 1 & ")"

    ' Neues Diagramm auf demselben Blatt erstellen
    Set chartObj = xlSheet.Shapes.AddChart2(251, 5, xlSheet.Range("J2").Left, xlSheet.Range("J2").Top, 450, 450)

    ' Diagrammtyp auf Kreisdiagramm setzen
    chartObj.Chart.ChartType = -4102 'xlPie
    chartObj.Chart.ChartArea.Format.Fill.ForeColor.RGB = RGB(140, 142, 143) ' Setzt die Hintergrundfarbe auf Grau

    ' Datenquelle für das Diagramm festlegen
    Dim wsName As String
    wsName = "Uebersicht_" & userInput
    Set dataRange = xlSheet.Range(wsName & "!$H$2:$H$" & i)

    ' Kategoriebeschriftungen (Spalte G) setzen
    Dim lastRowG As Long
    lastRowG = xlSheet.Cells(xlSheet.Rows.count, "G").End(xlUp).row
    Dim rngXValues As Range
    Set rngXValues = xlSheet.Range(wsName & "!$G$2:$G$" & lastRowG)

    ' Kategoriebeschriftungen (Spalte G) nur für nicht leere Zellen setzen
    chartObj.Chart.SeriesCollection(1).XValues = rngXValues

    ' Datenwerte (Spalte H) setzen
    chartObj.Chart.SeriesCollection(1).Values = dataRange

    ' Diagrammtitel setzen
    chartObj.Chart.HasTitle = True
    chartObj.Chart.ChartTitle.Text = "Übersicht - " & myNameSpace.CurrentUser & " - " & userInput
    chartObj.Chart.ChartTitle.Format.TextFrame2.TextRange.Font.Fill.ForeColor.RGB = RGB(255, 255, 255) ' Setzt die Farbe auf Weiß
    
    ' Legende entfernen
    chartObj.Chart.HasLegend = False

    ' Datenbeschriftungen hinzufügen
    Set seriesObj = chartObj.Chart.SeriesCollection(1)
    seriesObj.HasDataLabels = True
    seriesObj.dataLabels.ShowPercentage = True
    seriesObj.dataLabels.ShowCategoryName = True
    seriesObj.dataLabels.Format.Fill.ForeColor.RGB = RGB(255, 255, 255) ' Setzt die Hintergrundfarbe auf Weiß
    For Each pt In seriesObj.Points
        pt.Explosion = 6 ' Setzt die Punktexplosion auf 6%
    Next pt
    ' Aufräumen
    Set dataRange = Nothing
    Set chartObj = Nothing

    ' Spaltenbreite anpassen
    xlSheet.Columns("A:I").AutoFit

    ' Spalten zentrieren
    xlSheet.Columns("A:I").HorizontalAlignment = xlCenter

    ' Neues Excel-Arbeitsblatt für den Druck erstellen
    Set printWks = xlWB.Worksheets.Add(After:=xlSheet)
    printWks.Name = "Print_Uebersicht_" & userInput

    ' Spalten G und H von xlSheet nach printWks kopieren
    xlSheet.Range("G:I").Copy Destination:=printWks.Range("A1")

    ' Neues Diagramm auf printWks erstellen
    Set chartObj = printWks.Shapes.AddChart2(251, 5, printWks.Range("E2").Left, printWks.Range("E2").Top, 460, 470)

    ' Diagrammtyp auf Kreisdiagramm setzen
    chartObj.Chart.ChartType = -4102 'xlPie
    chartObj.Chart.ChartArea.Format.Fill.ForeColor.RGB = RGB(140, 142, 143) ' Setzt die Hintergrundfarbe auf Grau
    
    ' Datenquelle für das Diagramm festlegen
    wsName = "Print_Uebersicht_" & userInput
    Set dataRange = printWks.Range(wsName & "!$B$2:$B$" & i)

    ' Kategoriebeschriftungen (Spalte A) nur für nicht leere Zellen setzen
    Set rngXValues = printWks.Range(wsName & "!$A$2:$A$" & lastRowG)

    ' Kategoriebeschriftungen (Spalte A) setzen
    chartObj.Chart.SeriesCollection(1).XValues = rngXValues

    ' Datenwerte (Spalte B) setzen
    chartObj.Chart.SeriesCollection(1).Values = dataRange

    ' Diagrammtitel setzen
    chartObj.Chart.HasTitle = True
    chartObj.Chart.ChartTitle.Text = "Übersicht - " & myNameSpace.CurrentUser & " - " & userInput
    chartObj.Chart.ChartTitle.Format.TextFrame2.TextRange.Font.Fill.ForeColor.RGB = RGB(255, 255, 255) ' Setzt die Farbe auf Weiß
    
    ' Legende entfernen
    chartObj.Chart.HasLegend = False

    ' Datenbeschriftungen hinzufügen
    Set seriesObj = chartObj.Chart.SeriesCollection(1)
    seriesObj.HasDataLabels = True
    seriesObj.dataLabels.ShowPercentage = True
    seriesObj.dataLabels.ShowCategoryName = True
    seriesObj.dataLabels.Format.Fill.ForeColor.RGB = RGB(255, 255, 255) ' Setzt die Hintergrundfarbe auf Weiß
    For Each pt In seriesObj.Points
        pt.Explosion = 6 ' Setzt die Punktexplosion auf 6%
    Next pt
    
    ' Aufräumen
    Set dataRange = Nothing
    Set chartObj = Nothing

    ' Seitenausrichtung für den Druck festlegen
    printWks.PageSetup.Orientation = xlLandscape

    ' Pfad zum Desktop ermitteln
    Dim desktopPath As String
    desktopPath = CreateObject("WScript.Shell").SpecialFolders("Desktop")

    ' Basisname für die PDF-Datei festlegen
    Dim baseFileName As String
    baseFileName = desktopPath & "\Print_Uebersicht_" & userInput

    ' Dateiname für die PDF-Datei festlegen
    Dim pdfFileName As String
    pdfFileName = baseFileName & ".pdf"

    ' Prüfen, ob eine Datei mit dem gleichen Namen bereits auf dem Desktop vorhanden ist
    Dim fso As Object
    Set fso = CreateObject("Scripting.FileSystemObject")
    Dim counter As Integer
    counter = 1
    While fso.FileExists(pdfFileName)
        ' Wenn ja, eine Zahl an den Dateinamen anhängen
        pdfFileName = baseFileName & "_" & Format(counter, "00") & ".pdf"
        counter = counter + 1
    Wend

    ' printWks als PDF auf dem Desktop speichern
    printWks.ExportAsFixedFormat Type:=xlTypePDF, fileName:=pdfFileName
    
    Set OutApp = CreateObject("Outlook.Application")
    Set OutMail = OutApp.CreateItem(0)

    ' Erstellung einer Email zum Versand an den Abteilungsleiter
    With OutMail
        .To = "johndoe@aol.com"
        .CC = ""
        .BCC = ""
        .Subject = "Monatsübersicht " & myNameSpace.CurrentUser & " " & userInput
        .Body = myNameSpace.CurrentUser & vbNewLine & vbNewLine & "Anbei die aktuelle Monatsübersicht für " & userInput & "."
        .Attachments.Add pdfFileName ' Hier wird die generierte PDF-Datei angehängt
        .Display ' Verwende .Send anstelle von .Display, um die E-Mail automatisch zu senden
    End With

    Set OutMail = Nothing
    Set OutApp = Nothing

    ' Excel sichtbar machen und in den Vordergrund bringen
    xlApp.Visible = True
    xlApp.UserControl = True
    xlApp.WindowState = -4137 'xlMaximized
End Sub