/* 
Marcel Ciba
ITM7
Arukone | bwinf Runde 1
*/

// Import der benötigten Bibliothken
using System;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Arukone_bwinf_Ciba
{
    public partial class formArukone : Form
    {
        private int puzzleSize = 4;  // Die Mindestgröße des Puzzles ist auf 4 gesetzt.
        private int[,] importedArukone;  // Added variable to store the imported Arukone.

        public formArukone()
        {
            // Initialisiert die Komponenten des Formulars, einschließlich Steuerelementen, Beschriftungen und Schaltflächen.
            // Diese Methode wird in der Regel automatisch generiert, wenn das Formular visuell mit dem Windows Forms Designer erstellt wird.
            InitializeComponent();
            btnTestArukone.Click += btnTestArukone_Click; // Register the click event handler for btnTestArukone.
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Diese Methode wird aufgerufen, wenn das Formular geladen wird, aber sie ist aktuell leer.
        }

        private void textBoxEingabe_TextChanged(object sender, EventArgs e)
        {
            // Diese Methode wird aufgerufen, wenn sich der Text im Eingabefeld ändert.

            if (int.TryParse(textBoxEingabe.Text, out int newSize) && newSize >= 4)
            {
                // Versuche, den Text im Eingabefeld in eine ganze Zahl umzuwandeln.
                // Wenn dies erfolgreich ist und die Zahl größer oder gleich 4 ist, ändere die Puzzelgröße.
                puzzleSize = newSize;
            }
            else
            {
                // Zeige eine Fehlermeldung, wenn die Eingabe ungültig ist.
                MessageBox.Show("Der Arukone Generator generiert erst ab 4!", "(╯°□°）╯︵ ┻━┻", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                // Setze den Text im Eingabefeld auf 4 und die Puzzelgröße auf 4 zurück.
                textBoxEingabe.Text = "4";
                puzzleSize = 4;
            }
        }
        private void btnTestArukone_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Bitte wählen Sie die Datei aus, die Sie importieren möchten.", "Arukone importieren", MessageBoxButtons.OK, MessageBoxIcon.Information);

            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Textdateien|*.txt",  // Filter for text files.
                Title = "Arukone importieren",  // Dialog title.
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)  // Default directory.
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;

                try
                {
                    string importedContent = System.IO.File.ReadAllText(filePath);
                    importedArukone = ParseArukoneContent(importedContent);  // Call the new function to parse and store the imported Arukone.
                    DrawLinesAndCheckSolvability(importedArukone);  // Call the new function to draw lines and check solvability.
                    MessageBox.Show("Arukone-Inhalt aus " + filePath + " importiert:\n\n" + importedContent, "Arukone importiert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    // Show an error message if the import fails.
                    MessageBox.Show("Fehler beim Importieren des Arukone-Inhalts: " + ex.Message, "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void DrawLinesAndCheckSolvability(int[,] arukone)
        {
            int size = arukone.GetLength(0);

            List<Point> linePoints = new List<Point>();

            for (int number = 1; number <= size / 2; number++)
            {
                // Find positions of the current number in the arukone grid.
                List<Point> numberPositions = FindNumberPositions(arukone, number);

                // Draw lines between the pairs of matching numbers.
                for (int i = 0; i < numberPositions.Count; i += 2)
                {
                    Point start = numberPositions[i];
                    Point end = numberPositions[i + 1];

                    DrawLine(start, end, linePoints);

                    // Check for intersections after drawing each line.
                    if (CheckForIntersections(linePoints))
                    {
                        MessageBox.Show("Das Puzzle ist nicht lösbar.", "Nicht lösbar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
            }

            MessageBox.Show("Das Puzzle ist lösbar.", "Lösbar", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private List<Point> FindNumberPositions(int[,] arukone, int number)
        {
            List<Point> positions = new List<Point>();
            int size = arukone.GetLength(0);

            for (int row = 0; row < size; row++)
            {
                for (int col = 0; col < size; col++)
                {
                    if (arukone[row, col] == number)
                    {
                        positions.Add(new Point(row, col));
                    }
                }
            }

            return positions;
        }

        private void DrawLine(Point start, Point end, List<Point> linePoints)
        {
            // Draw a line on the Arukone grid between the start and end points.
            // You can customize this method based on how you want to represent the lines in your application.
            // For simplicity, I'm just adding the points to the linePoints list.

            linePoints.Add(start);
            linePoints.Add(end);
        }

        private bool CheckForIntersections(List<Point> linePoints)
        {
            for (int i = 0; i < linePoints.Count; i += 2)
            {
                Point line1Start = linePoints[i];
                Point line1End = linePoints[i + 1];

                for (int j = i + 2; j < linePoints.Count; j += 2)
                {
                    Point line2Start = linePoints[j];
                    Point line2End = linePoints[j + 1];

                    if (DoIntersect(line1Start, line1End, line2Start, line2End))
                    {
                        // If there is an intersection between line1 and line2, return true.
                        return true;
                    }
                }
            }

            return false;
        }

        private bool DoIntersect(Point p1, Point q1, Point p2, Point q2)
        {
            // This function checks if the line segment (p1, q1) intersects with (p2, q2).
            // It uses orientation to determine if the two segments intersect.

            int o1 = Orientation(p1, q1, p2);
            int o2 = Orientation(p1, q1, q2);
            int o3 = Orientation(p2, q2, p1);
            int o4 = Orientation(p2, q2, q1);

            // General case
            if (o1 != o2 && o3 != o4)
                return true;

            // Special Cases

            // p1 , q1 and p2 are collinear and p2 lies on segment p1q1
            if (o1 == 0 && OnSegment(p1, p2, q1)) return true;

            // p1 , q1 and q2 are collinear and q2 lies on segment p1q1
            if (o2 == 0 && OnSegment(p1, q2, q1)) return true;

            // p2 , q2 and p1 are collinear and p1 lies on segment p2q2
            if (o3 == 0 && OnSegment(p2, p1, q2)) return true;

            // p2 , q2 and q1 are collinear and q1 lies on segment p2q2
            if (o4 == 0 && OnSegment(p2, q1, q2)) return true;

            return false; // Doesn't fall in any of the above cases
        }

        private int Orientation(Point p, Point q, Point r)
        {
            // This function finds the orientation of triplet (p, q, r).
            // The function returns the following values:
            // 0 : Collinear points
            // 1 : Clockwise points
            // 2 : Counterclockwise

            int val = (q.Y - p.Y) * (r.X - q.X) - (q.X - p.X) * (r.Y - q.Y);

            if (val == 0) return 0; // Collinear
            return (val > 0) ? 1 : 2; // Clockwise or Counterclockwise
        }

        private bool OnSegment(Point p, Point q, Point r)
        {
            // Given three collinear points p, q, r, the function checks if point q lies on line segment 'pr'.
            return q.X <= Math.Max(p.X, r.X) && q.X >= Math.Min(p.X, r.X) && q.Y <= Math.Max(p.Y, r.Y) && q.Y >= Math.Min(p.Y, r.Y);
        }


        private int[,] ParseArukoneContent(string content)
        {
            string[] lines = content.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            int size = int.Parse(lines[0].Trim());
            int[,] arukone = new int[size, size];

            int numberOfNumbers = int.Parse(lines[1].Trim());

            for (int i = 0; i < size; i++)
            {
                string[] values = lines[i + 2].Trim().Split(' ');
                for (int j = 0; j < size; j++)
                {
                    arukone[i, j] = int.Parse(values[j]);
                }
            }

            return arukone;
        }
        private void btnCreateArukone_Click(object sender, EventArgs e)
        {
            // Diese Methode wird aufgerufen, wenn der "Arukone erstellen" Button geklickt wird.

            textBoxEingabe_TextChanged(sender, e);  // Rufe die Methode textBoxEingabe_TextChanged auf, um die Puzzelgröße zu aktualisieren.
            ArukoneGenerator arukone = new ArukoneGenerator(puzzleSize);  // Erstelle ein ArukoneGenerator-Objekt mit der aktuellen Puzzelgröße.
            string puzzleContent = arukone.ToString();  // Erhalte den Inhalt des generierten Arukone-Puzzels.

            DialogResult result = MessageBox.Show(puzzleContent, "Arukone", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

            if (result == DialogResult.OK)
            {
                // Wenn der Benutzer "OK" auswählt, öffne einen Dialog zum Speichern des Arukones.

                SaveFileDialog saveFileDialog1 = new SaveFileDialog
                {
                    Filter = "Textdateien|*.txt",  // Filter für Textdateien festlegen.
                    Title = "Arukone speichern",  // Titel des Speicherdialogs.
                    FileName = "ArukonePuzzle.txt",  // Standarddateinamen festlegen.
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)  // Standardverzeichnis festlegen.
                };

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string filePath = saveFileDialog1.FileName;

                    try
                    {
                        System.IO.File.WriteAllText(filePath, puzzleContent);  // Schreibe den Puzzelinhalt in die ausgewählte Datei.
                        MessageBox.Show("Arukone-Inhalt wurde in " + filePath + " gespeichert", "Arukone gespeichert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        // Zeige eine Fehlermeldung, wenn das Speichern fehlschlägt.
                        MessageBox.Show("Fehler beim Speichern des Arukone-Inhalts: " + ex.Message, "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            // Diese Methode wird aufgerufen, wenn der "Hilfe" Button geklickt wird.

            string arukoneInstructions = "Arukone sind japanische Logikrätsel.\n" +
                "\n" +
                "Man erhält ein Gitter, in dem jedes Feld entweder eine Zahl enthält oder leer ist.\n" +
                "\n" +
                "Jede Zahl im Gitter kommt genau zwei Mal vor.\n" +
                "\n" +
                "Bei der Lösung eines Arukone-Rätsels werden für jedes Paar gleicher Zahlen die entsprechenden Felder mit einem Linienzug verbunden.\n" +
                "\n" +
                "---------------------------------------------------------------------\n" +
                "\n" +
                "Dabei gelten folgende Regeln:\n" +
                "\n" +
                "-> Jeder Linienzug besteht nur aus horizontalen und vertikalen Abschnitten (nicht schräg).\n" +
                "\n" +
                "-> In jedem Feld mit einer Zahl beginnt oder endet genau ein Linienzug.\n" +
                "\n" +
                "-> Jedes Feld ohne Zahl wird von genau einem Linienzug durchlaufen oder ist leer. ";

            MessageBox.Show(arukoneInstructions, "Arukone Spielregeln", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    public class ArukoneGenerator
    {
        // Definition der Variable Size und Arukone für den Generator
        private int size;
        private int[,] arukone;

        public ArukoneGenerator(int size)
        {
            // Konstruktor für den Arukone-Generator, der die Größe des Puzzles festlegt und das Arukone generiert.
            this.size = size;
            this.arukone = new int[size, size];
            GenerateArukone();  // Methode zum Generieren des Puzzles aufrufen.
        }

        private void GenerateArukone()
        {
            // Diese Methode dient zum Generieren des Arukone-Puzzles.

            Random random = new Random();
            // Ein Zufallszahlengenerator (Random-Objekt) wird erstellt, um zufällige Anordnungen zu erzeugen.

            List<int> numbers = Enumerable.Range(1, size / 2).SelectMany(i => new[] { i, i }).ToList();
            // Eine Liste von Zahlen wird erstellt, die jeweils zweimal im Arukone vorkommen. Die Zahlen werden von 1 bis (size/2) erzeugt.

            // if (size % 2 == 1)
            //    numbers.Add(numbers[0]);

            // Wenn die Puzzelgröße ungerade ist, wird eine zusätzliche Kopie der ersten Zahl hinzugefügt, um sicherzustellen, dass jede Zahl zweimal vorkommt.

            for (int i = numbers.Count - 1; i > 0; i--)
            {
                int j = random.Next(0, i + 1);
                int temp = numbers[i];
                numbers[i] = numbers[j];
                numbers[j] = temp;
            }
            // Die Liste der Zahlen wird zufällig gemischt, indem Zahlen miteinander getauscht werden.

            List<Tuple<int, int>> cells = Enumerable.Range(0, size).SelectMany(row => Enumerable.Range(0, size).Select(col => Tuple.Create(row, col))).OrderBy(x => random.Next()).ToList();
            // Es wird eine Liste von Tupeln erstellt, die alle möglichen Zellpositionen im Arukone repräsentieren. Die Reihenfolge der Zellen wird zufällig sortiert.

            int currentIndex = 0;
            foreach (var cell in cells)
            {
                int row = cell.Item1;
                int col = cell.Item2;
                if (currentIndex < numbers.Count)
                {
                    // Wenn es noch nicht alle Zahlen im Arukone platziert wurden, wird die aktuelle Zahl in das Arukone an der aktuellen Position eingefügt.
                    arukone[row, col] = numbers[currentIndex];
                    currentIndex++;
                }
            }
            // Schließlich werden die Zahlen aus der gemischten Liste in das Arukone an zufälligen Positionen eingefügt.
        }
        public override string ToString()
        {
            // Diese Methode wird überschrieben, um eine benutzerdefinierte Zeichenkette darzustellen, die den Puzzelinhalt repräsentiert.

            string puzzleString = $"{size}{Environment.NewLine}{size / 2}{Environment.NewLine}";
            // Erstelle eine Zeichenkette (puzzleString), die die Puzzelgröße, die Anzahl der Zahlen (size/2) und Zeilenumbrüche enthält.

            for (int row = 0; row < size; row++)
            {
                // Schleife durch die Zeilen des Puzzles.
                for (int col = 0; col < size; col++)
                {
                    // Schleife durch die Spalten des Puzzles.

                    puzzleString += arukone[row, col] == 0 ? "0 " : arukone[row, col] + " ";
                    // Wenn das Puzzel-Element an der aktuellen Position (row, col) gleich 0 ist, füge "0 " zur Zeichenkette hinzu, sonst füge die Puzzelzahl und ein Leerzeichen hinzu.
                }

                puzzleString += Environment.NewLine;
                // Füge einen Zeilenumbruch hinzu, um zur nächsten Zeile des Puzzles zu wechseln.
            }

            return puzzleString;
            // Gebe die erstellte Zeichenkette (puzzleString) als das Ergebnis der ToString-Methode zurück.
        }
    }
}