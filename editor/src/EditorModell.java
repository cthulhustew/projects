import javax.swing.JTextArea;
import java.io.File;
import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Paths;
import java.awt.*;

/**
 * Fachlogik fuer einen TexttextInput. Unabhaengig von GUI und Controller.
 * Nicht ganz sauber, da von JTextArea abgeleitet. Das stoert aber nicht, 
 * da Swing in Java SE enthalten ist.
 * 
 * @author M.Ciba
 */
public class EditorModell extends JTextArea {
    private String message = "";      // Nachricht, z.B. für Suchergebnisse
    private int findPosition = -1;    // Position des gesuchten Texts

    // Getter für die Nachricht
    public String getMessage() {
        return message;
    }

    // Getter für die Position des gefundenen Texts
    public int getFindPosition() {
        return findPosition;
    }

    /**
     * Sucht nach einem String in der Textarea.
     * @param su Der zu suchende String.
     * @return True, wenn der String gefunden wurde, sonst false.
     */
    public boolean find(String su, boolean caseInsensitive, boolean vonEndeNeu) {
            String text = this.getText();
            int start = this.getCaretPosition();
                    
            if (caseInsensitive) {
                su = su.toLowerCase();
                text = text.toLowerCase();
            }
 
            findPosition = text.indexOf(su, start);
            if (findPosition < 0) {
                if (vonEndeNeu) {
                    findPosition = text.indexOf(su, 0);
                } else {
                    message = "'" + su + "' nicht gefunden.";
                    return false;
                }
            }
            message = "'" + su + "' an Position " + findPosition + " gefunden.";
            this.setSelectionStart(findPosition);
            this.setSelectionEnd(findPosition + su.length());    
            return true;
        }
    
    public int[] berechneSpalte(int position, int spalte) {
            String text = this.getText();
            
            // Berechne die Position des Zeilenanfangs der aktuellen Zeile
            int zeilenAnfang = text.lastIndexOf("\n", position - 1);
            int zeilenEnde = text.indexOf("\n", zeilenAnfang + 1); 
            
            if (zeilenEnde == -1) {
                zeilenEnde = text.length();
            }
            
            if (zeilenAnfang + spalte > zeilenEnde) {
                return new int[] {(zeilenAnfang+spalte), (zeilenEnde-zeilenAnfang), 0};
            }
            return new int[] {(zeilenAnfang+spalte), (zeilenEnde-zeilenAnfang), 1};
        }
    
    public int berechneZeile(int zeile) {
            String text = this.getText();
            String[] zeilen = text.split("\n",-1);
            int position = 0;
            
            if (zeile > zeilen.length) {
                return (zeilen.length) * -1;
            }
            
            for (int i=0; i < zeile-1; i++) {
                position += zeilen[i].length()+1;
            }
            
            return position;
        }
    
    public boolean ersetzen(String su, String se,boolean caseInsensitive, boolean vonEndeNeu) {

                   
            String s = this.getText();
            int start = this.getCaretPosition();
            String text = this.getText();
            // Überprüfen, ob die Eingabeparameter gültig sind
            if (s == null || su == null || se == null || start < 0 || start > s.length() || su.isEmpty()) {
                return false;
            }
            
            if (caseInsensitive) {
                su = su.toLowerCase();
                text = s.toLowerCase();
            } else {
                text = s;
            }
 
            // Ab der Startposition nach dem ersten Vorkommen der Zeichenkette su suchen
            int index = text.indexOf(su, start);
 
            // Wenn su nicht gefunden wird, die Ausgangszeichenkette zurückgeben
            if (index == -1) {
                if (vonEndeNeu) {
                    index = text.indexOf(su, 0);
                } else {
                    message = "'" + su + "' nicht gefunden.";
                    return false;
                }
            }
 
            // Zeichenkette su durch se ersetzen
            this.setText(s.substring(0, index) + se + s.substring(index + su.length()));
            message = "'" + su + "' an Position." + findPosition + "gefunden.";
            return true;
}
    
    
    public  int spaltenNummer() {
        
    String s = this.getText();
    int p = this.getCaretPosition();
    // Basisfälle abdecken
    if (s == null || p < 0) {
        return 0;
    }
 
    // Wenn p außerhalb der String-Länge liegt, auf das Ende des Strings setzen
    if (p > s.length()) {
        p = s.length()-1;
    }
 
        if (s.equals("")) {
            return 1;
        }
    // Letzten Zeilenumbruch vor der Position p finden
    int letzteNeueZeile = s.lastIndexOf('\n', p - 1);
 
    // Wenn kein Zeilenumbruch gefunden wurde, beginnt die Zeile am Anfang des Strings
    if (letzteNeueZeile == -1) {
        return p + 1; // Spaltennummer ist p+1, da String null-basiert ist
    }
 
    // Spaltennummer ist die Anzahl der Zeichen seit dem letzten Zeilenumbruch
    return p - letzteNeueZeile;
}
    
    public int zeilenNummer() {
        
            String s = this.getText();
            int p = this.getCaretPosition();
            
           // Überprüfen, ob die Zeichenkette null ist oder p nicht positiv ist
           if (s == null || p < 0) {
               return 0;
           }

           // Wenn p = 0 ist, geben wir Zeile 1 zurück
           if (p == 0) {
               return 1;
           }

           // Anpassung, da p 1-basiert ist
           p = p - 1;

           // Überprüfen, ob p außerhalb der Zeichenkette liegt
           if (p >= s.length()) {
               p = s.length() - 1; // Setze p auf die letzte Position
           }

           int zeilenNummer = 1; // Startwert, da die erste Zeile mit 1 beginnt

           // Durchlaufe die Zeichenkette bis zur Position p
           for (int i = 0; i <= p; i++) {
               // Wenn ein Zeilenumbruch gefunden wird, erhöhe die Zeilennummer
               if (s.charAt(i) == '\n') {
                   zeilenNummer++;
               }
           }

           return zeilenNummer;
       }

    /**
     * Öffnet eine Datei und setzt den Inhalt in der Textarea.
     * @param selectedFile Die zu öffnende Datei.
     * @return True, wenn die Datei erfolgreich geladen wurde, sonst false.
     */
    public boolean open(File selectedFile) {
        try {
            // Lese den Dateiinhalt als String und setze ihn in der Textarea
            String content = Files.readString(Paths.get(selectedFile.getPath()));
            this.setText(content);
            return true;
        } catch (IOException e) {
            return false;  // Falls ein Fehler auftritt, gib false zurück
        }
    }
   

    /**
     * Überschreibt die paintComponent-Methode, um eine vertikale Linie an der Position 
     * zu zeichnen, an der das Limit von 60 Zeichen pro Zeile überschritten wird.
     */
    @Override
    public void paintComponent(Graphics g) {
        super.paintComponent(g);  // Rufe die Standard-Mal-Funktion von JTextArea auf

        // Höhe und Breite der Komponente
        int h = this.getHeight();
        int w = this.getWidth();

        // Schriftmetriken für die Berechnung der Zeichenbreite
        FontMetrics fm = g.getFontMetrics(getFont());

        // Breite eines Zeichens ('_' als Referenz)
        int charWidth = fm.charWidth('_');

        // Berechnung der x-Position, wo die 60-Zeichen-Linie gezeichnet werden soll
        int x = (charWidth * 60) + 6;

        // Zeichne die vertikale Linie bei Überschreitung der 60-Zeichen-Grenze
        g.setColor(Color.GRAY);  // Setze die Farbe für die Linie
        g.drawLine(x, 0, x, h);  // Zeichne die Linie von oben nach unten

        // Freigabe der Ressourcen des Graphics-Objekts (optional, wird oft automatisch gehandhabt)
        g.dispose();
    }
}
