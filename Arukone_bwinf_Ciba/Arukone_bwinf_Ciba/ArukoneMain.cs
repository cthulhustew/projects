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

        
        public formArukone()
        {
            // Initialisiert die Komponenten des Formulars, einschließlich Steuerelementen, Beschriftungen und Schaltflächen.
            // Diese Methode wird in der Regel automatisch generiert, wenn das Formular visuell mit dem Windows Forms Designer erstellt wird.
            InitializeComponent();
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
                        MessageBox.Show("Die Arukone wurde in " + filePath + " gespeichert", "Arukone gespeichert", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

            if (size % 2 == 1)
                numbers.Add(numbers[0]);
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