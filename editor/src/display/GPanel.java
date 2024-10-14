/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */
 
package display;
 
import java.awt.Color;
import java.awt.Dimension;
import java.awt.Graphics;
import javax.swing.JPanel;
 
 
/**
 *
 * @author t.tschorn
 */
public class GPanel extends JPanel {
    
    public GPanel() { //Konstruktur ist notwendig für GUI-Editor
        this(40, 30, Color.BLUE);
    }
    
    public GPanel(int xSize, int ySize, Color bgColor) {
     
        this.setBackground(bgColor);
        this.setPreferredSize(new Dimension(xSize, ySize));
    }
    
    @Override
    public void paintComponent(Graphics g) {
        super.paintComponent(g); // Aufruf der Super-Klasse für korrektes Zeichnen

        int w = this.getWidth();
        int h = this.getHeight();

        // Hintergrund löschen
        g.setColor(this.getBackground());
        g.fillRect(0, 0, w, h);

        // Torii zeichnen
        g.setColor(Color.RED); // Farbe für das Torii

        // Obere Querbalken
        g.fillRect(w / 2 - 60, h / 2 - 50, 120, 20); // oberer Balken
        g.fillRect(w / 2 - 80, h / 2 - 30, 20, 10); // linke Stütze
        g.fillRect(w / 2 + 60, h / 2 - 30, 20, 10); // rechte Stütze

        // Senkrechte Balken
        g.fillRect(w / 2 - 40, h / 2 - 30, 10, 80); // linke senkrechte Stütze
        g.fillRect(w / 2 + 30, h / 2 - 30, 10, 80); // rechte senkrechte Stütze

        // Untere Querbalken
        g.fillRect(w / 2 - 60, h / 2 + 10, 120, 20); // unterer Balken
        g.fillRect(w / 2 - 80, h / 2 + 30, 20, 10); // linke Stütze
        g.fillRect(w / 2 + 60, h / 2 + 30, 20, 10); // rechte Stütze

        g.dispose();
    }
}
 