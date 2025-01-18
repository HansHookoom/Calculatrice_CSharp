namespace Calculatrice
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.TextBox txtDisplay;
        private System.Windows.Forms.ComboBox comboBoxChoixMode;
        private System.Windows.Forms.ComboBox comboBoxThemes;
        private System.Windows.Forms.Panel panelComboBoxes;
        private System.Windows.Forms.Panel panelStandard;
        private System.Windows.Forms.Panel panelScientifique;
        private System.Windows.Forms.Panel panelProgrammeur;
        private System.Windows.Forms.Panel panelStatistique;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }


        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            // ************************ Création des composants ************************ //

            this.txtDisplay = new System.Windows.Forms.TextBox();
            this.comboBoxChoixMode = new System.Windows.Forms.ComboBox();
            this.comboBoxThemes = new System.Windows.Forms.ComboBox();
            this.panelComboBoxes = new System.Windows.Forms.Panel();
            this.panelStandard = new System.Windows.Forms.Panel();
            this.panelScientifique = new System.Windows.Forms.Panel();
            this.panelProgrammeur = new System.Windows.Forms.Panel();
            this.panelStatistique = new System.Windows.Forms.Panel();

            // ************************ Configuration de txtDisplay ************************ //

            this.txtDisplay.Font = new System.Drawing.Font("Microsoft Sans Serif", 28F);
            this.txtDisplay.Text = "0";
            this.txtDisplay.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtDisplay.Dock = System.Windows.Forms.DockStyle.Top;

            // ************************ Panel pour les ComboBox ************************ //

            this.panelComboBoxes.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelComboBoxes.Height = 60; // Augmentée pour éviter le découpage

            // TableLayoutPanel pour organiser les ComboBox
            var tableLayout = new System.Windows.Forms.TableLayoutPanel();
            tableLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayout.ColumnCount = 2;
            tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F)); // 50% pour chaque ComboBox
            tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayout.RowCount = 1;
            tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F)); // Une seule ligne

            // ************************ comboBoxChoixMode ************************ //

            this.comboBoxChoixMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxChoixMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F);
            this.comboBoxChoixMode.Items.AddRange(new object[] {
                "Standard", "Scientifique", "Programmeur", "Statistique"
            });
            this.comboBoxChoixMode.Dock = System.Windows.Forms.DockStyle.Fill; // Remplit sa cellule
            this.comboBoxChoixMode.SelectedIndex = 0;
            this.comboBoxChoixMode.SelectedIndexChanged += new System.EventHandler(this.ComboBoxChoixModeSelectedIndexChanged);

            // ************************ comboBoxThemes ************************ //

            this.comboBoxThemes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxThemes.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F);
            this.comboBoxThemes.Items.AddRange(new object[] {"Sombre", "Clair", "Saphir", "Rubis","Emeraude"
            });
            this.comboBoxThemes.Dock = System.Windows.Forms.DockStyle.Fill; // Remplit sa cellule
            this.comboBoxThemes.SelectedIndex = 0;
            this.comboBoxThemes.SelectedIndexChanged += new System.EventHandler(this.ComboBoxThemesSelectedIndexChanged);

            // Ajout des ComboBox au TableLayoutPanel
            tableLayout.Controls.Add(this.comboBoxChoixMode, 0, 0);
            tableLayout.Controls.Add(this.comboBoxThemes, 1, 0);

            // Ajout du TableLayoutPanel au panelComboBoxes
            this.panelComboBoxes.Controls.Add(tableLayout);

            // ************************ Panels pour les boutons ************************ //

            this.panelStandard.Dock = System.Windows.Forms.DockStyle.Fill;
            AddStandardButtons();

            this.panelScientifique.Dock = System.Windows.Forms.DockStyle.Fill;
            AddStandardAndScientificButtons();

            this.panelProgrammeur.Dock = System.Windows.Forms.DockStyle.Fill;
            AddProgrammeurButtons();

            this.panelStatistique.Dock = System.Windows.Forms.DockStyle.Fill;
            AddStatistiqueButtons();

            // ************************ Configuration globale ************************ //

            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;

            this.ClientSize = new System.Drawing.Size(1904, 1041);

            this.Controls.Add(this.panelStandard);
            this.Controls.Add(this.panelScientifique);
            this.Controls.Add(this.panelProgrammeur);
            this.Controls.Add(this.panelStatistique);

            this.Controls.Add(this.panelComboBoxes);
            this.Controls.Add(this.txtDisplay);

            this.Name = "Form1";
            this.Text = "Calculatrice Avancée";

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        // ************************ Configuration changement de thèmes ************************ //

        private void ComboBoxThemesSelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxThemes.SelectedItem == null)
                return;

            string selectedTheme = comboBoxThemes.SelectedItem.ToString();

            switch (selectedTheme)
            {
                case "Sombre":
                    ApplyCustomTheme(Color.FromArgb(30, 30, 30), Color.White, Color.FromArgb(50, 50, 50), Color.White);
                    break;
                case "Clair":
                    ApplyCustomTheme(Color.White, Color.Black, Color.LightGray, Color.Black);
                    break;
                case "Saphir":
                    ApplyCustomTheme(Color.FromArgb(10, 10, 50), Color.White, Color.FromArgb(20, 20, 100), Color.White);
                    break;
                case "Rubis":
                    ApplyCustomTheme(Color.FromArgb(50, 10, 10), Color.White, Color.FromArgb(100, 20, 20), Color.White);
                    break;
                case "Emeraude":
                    ApplyCustomTheme(Color.FromArgb(10, 50, 10), Color.White, Color.FromArgb(20, 100, 20), Color.White);
                    break;
            }
        }

        private void ApplyCustomTheme(Color formBackColor, Color formForeColor, Color buttonBackColor, Color buttonForeColor)
        {
            BackColor = formBackColor;
            ForeColor = formForeColor;
            txtDisplay.BackColor = formBackColor;
            txtDisplay.ForeColor = formForeColor;

            foreach (Control panel in this.Controls)
            {
                if (panel is Panel p)
                {
                    foreach (Control control in p.Controls)
                    {
                        if (control is Button btn)
                        {
                            btn.BackColor = buttonBackColor;
                            btn.ForeColor = buttonForeColor;
                            btn.Font = new Font("Consolas", 18F, FontStyle.Bold, GraphicsUnit.Point);
                        }
                    }
                }
            }

            comboBoxThemes.BackColor = buttonBackColor;
            comboBoxThemes.ForeColor = buttonForeColor;
            comboBoxChoixMode.BackColor = buttonBackColor;
            comboBoxChoixMode.ForeColor = buttonForeColor;
        }

        // ************************ Méthodes d'ajout de boutons ************************ //
        // ************************   en fonction des panels   ************************ //

        private void AddStandardButtons()
        {
            string[] buttonTexts = {
                "7", "8", "9", "C",
                "4", "5", "6", "/",
                "1", "2", "3", "*",
                "0", ".", "=", "+", "-", "Del"
            };
            AddButtonsToPanel(panelStandard, buttonTexts, 5, 4);
        }

        private void AddStandardAndScientificButtons()
        {
            string[] sciTexts = {
                        "7", "8", "9", "/", "C",
                        "4", "5", "6", "*", "x²",
                        "1", "2", "3", "-", "√x",
                        "0", ".", "=", "+", "1/x",
                        "(", ")", "mod", "n!", "x^y",
                        "sin", "cos", "tan", "log", "ln",
                        "π","e", "Del"
                    };

            AddButtonsToPanel(panelScientifique, sciTexts, 7, 5);
        }

        private void AddProgrammeurButtons()
        {
            string[] progTexts = {
                        "A", "B", "C", "D",
                        "E", "F", "AND", "OR",
                        "XOR", "<<", ">>", "NOT",
                        "DEC", "HEX", "BIN", "OCT",
                        "CLR", "Del"
                    };

            AddButtonsToPanel(panelProgrammeur, progTexts, 4, 5);
        }

        private void AddStatistiqueButtons()
        {
            string[] statTexts = { "Moyenne", "Variance", "Écart-Type", "Del" };
            AddButtonsToPanel(panelStatistique, statTexts, 2, 2);
        }

        private void AddButtonsToPanel(System.Windows.Forms.Panel panel, string[] texts, int rows, int cols)
        {
            int buttonWidth = panel.Width / cols - 10;
            int buttonHeight = panel.Height / rows - 10;

            for (int i = 0; i < texts.Length; i++)
            {
                Button btn = new Button();
                btn.Text = texts[i];
                btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);

                btn.Size = new System.Drawing.Size(buttonWidth, buttonHeight);
                btn.Location = new System.Drawing.Point(
                    (i % cols) * (buttonWidth + 10),
                    (i / cols) * (buttonHeight + 10)
                );

                if (panel == panelProgrammeur)
                {
                    btn.Click += new System.EventHandler(this.ProgrammeurButtonClick);
                }
                else if (panel == panelStatistique)
                {
                    btn.Click += new System.EventHandler(this.StatistiqueButtonClick);
                }
                else
                {
                    switch (btn.Text)
                    {
                        case "=":
                            btn.Click += new System.EventHandler(this.ButtonEgaleClick);
                            break;
                        case "C":
                            btn.Click += new System.EventHandler(this.ButtonClearAllClick);
                            break;
                        case "Del":
                            btn.Click += new System.EventHandler(this.ButtonDeleteClick);
                            break;
                        default:
                            btn.Click += new System.EventHandler(this.ButtonClick);
                            break;
                    }
                }

                panel.Controls.Add(btn);
            }
        }

        // ************************ Changements de tailles des boutons ************************ //

        private void ResizeButtons()
        {
            foreach (Control ctrl in Controls)
            {
                if (ctrl is Panel panel && panel.Visible)
                {
                    if (panel.Controls.Count == 0) continue;

                    int rows = panel == panelScientifique ? 7 : (panel == panelProgrammeur ? 4 : 5);
                    int cols = panel == panelScientifique || panel == panelProgrammeur ? 5 : 4;

                    int padding = 5;
                    int buttonWidth = (panel.Width - padding * (cols + 1)) / cols;
                    int buttonHeight = (panel.Height - padding * (rows + 1)) / rows;

                    int index = 0;
                    foreach (Control c in panel.Controls)
                    {
                        if (c is Button btn)
                        {
                            btn.Size = new Size(buttonWidth, buttonHeight);
                            btn.Location = new Point(
                                padding + (index % cols) * (buttonWidth + padding),
                                padding + (index / cols) * (buttonHeight + padding)
                            );
                            index++;
                        }
                    }
                }
            }
        }
    }
}
