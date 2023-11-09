namespace Arukone_bwinf_Ciba
{
    partial class formArukone
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnCreateArukone = new Button();
            textBoxEingabe = new TextBox();
            lblEingabe = new Label();
            label2 = new Label();
            label1 = new Label();
            btnHelp = new Button();
            lblArukoneHeader = new Label();
            saveFileDialog = new SaveFileDialog();
            btnTestArukone = new Button();
            SuspendLayout();
            // 
            // btnCreateArukone
            // 
            btnCreateArukone.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnCreateArukone.Location = new Point(40, 144);
            btnCreateArukone.Name = "btnCreateArukone";
            btnCreateArukone.Size = new Size(300, 100);
            btnCreateArukone.TabIndex = 0;
            btnCreateArukone.Text = "Arukone erstellen";
            btnCreateArukone.UseVisualStyleBackColor = true;
            btnCreateArukone.Click += btnCreateArukone_Click;
            // 
            // textBoxEingabe
            // 
            textBoxEingabe.Location = new Point(40, 78);
            textBoxEingabe.Name = "textBoxEingabe";
            textBoxEingabe.Size = new Size(300, 23);
            textBoxEingabe.TabIndex = 5;
            // 
            // lblEingabe
            // 
            lblEingabe.Location = new Point(0, 0);
            lblEingabe.Name = "lblEingabe";
            lblEingabe.Size = new Size(100, 23);
            lblEingabe.TabIndex = 9;
            // 
            // label2
            // 
            label2.Location = new Point(0, 0);
            label2.Name = "label2";
            label2.Size = new Size(100, 23);
            label2.TabIndex = 8;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            label1.ForeColor = Color.Salmon;
            label1.Location = new Point(73, 104);
            label1.Name = "label1";
            label1.Size = new Size(215, 15);
            label1.TabIndex = 4;
            label1.Text = "Achtung, die Zahl muss größer 4 sein!";
            label1.TextAlign = ContentAlignment.TopCenter;
            // 
            // btnHelp
            // 
            btnHelp.BackColor = SystemColors.GradientActiveCaption;
            btnHelp.Location = new Point(126, 376);
            btnHelp.Name = "btnHelp";
            btnHelp.Size = new Size(128, 58);
            btnHelp.TabIndex = 6;
            btnHelp.Text = "Spielregeln";
            btnHelp.UseVisualStyleBackColor = false;
            btnHelp.Click += btnHelp_Click;
            // 
            // lblArukoneHeader
            // 
            lblArukoneHeader.AutoSize = true;
            lblArukoneHeader.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point);
            lblArukoneHeader.Location = new Point(73, 15);
            lblArukoneHeader.Name = "lblArukoneHeader";
            lblArukoneHeader.Size = new Size(234, 32);
            lblArukoneHeader.TabIndex = 7;
            lblArukoneHeader.Text = "Arukone Generator";
            // 
            // saveFileDialog
            // 
            saveFileDialog.FileName = "Arukone";
            // 
            // btnTestArukone
            // 
            btnTestArukone.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnTestArukone.Location = new Point(40, 250);
            btnTestArukone.Name = "btnTestArukone";
            btnTestArukone.Size = new Size(300, 100);
            btnTestArukone.TabIndex = 10;
            btnTestArukone.Text = "Arukone Testen";
            btnTestArukone.TextImageRelation = TextImageRelation.TextBeforeImage;
            btnTestArukone.UseVisualStyleBackColor = true;
            // 
            // formArukone
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(370, 451);
            Controls.Add(btnTestArukone);
            Controls.Add(lblArukoneHeader);
            Controls.Add(btnHelp);
            Controls.Add(label1);
            Controls.Add(label2);
            Controls.Add(lblEingabe);
            Controls.Add(textBoxEingabe);
            Controls.Add(btnCreateArukone);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "formArukone";
            Text = "アルコネ";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnCreateArukone;
        private TextBox textBoxEingabe;
        private Label lblEingabe;
        private Label label2;
        private Label label1;
        public HelpProvider helpArukone;
        private Button btnHelp;
        private Label lblArukoneHeader;
        private SaveFileDialog saveFileDialog;
        private Button btnTestArukone;
    }
}