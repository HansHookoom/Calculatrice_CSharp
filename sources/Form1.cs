//using System;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Windows.Forms;
//using System.IO;
//using System.Collections.Generic;
//using System.Diagnostics;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using NCalc; // Pour la bibliothèque d'évaluation d'expressions

namespace Calculatrice
{
    public partial class Form1 : Form
    {
        private List<string> historique;
        private long? operand1 = null;
        private string currentOperator = "";
        private int currentBase = 10;

        public Form1()
        {
            InitializeComponent();
            InitializeModes();
            ComboBoxThemesSelectedIndexChanged(comboBoxThemes, EventArgs.Empty);

            Load += (s, e) =>
            {
                ResizeButtons();
            };
            historique = new List<string>();
            WindowState = FormWindowState.Maximized;
            Resize += new EventHandler(Form1_Resize);
        }

        private void InitializeModes()
        {
            panelStandard.Visible = true;
            panelScientifique.Visible = false;
            panelProgrammeur.Visible = false;
            panelStatistique.Visible = false;
        }


        private void Form1_Resize(object? sender, EventArgs e)
        {
            ResizeButtons();
        }

        private void ComboBoxChoixModeSelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxChoixMode.SelectedItem == null)
            {
                MessageBox.Show("Veuillez sélectionner un mode.",
                                "Erreur",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            panelStandard.Visible = false;
            panelScientifique.Visible = false;
            panelProgrammeur.Visible = false;
            panelStatistique.Visible = false;

            switch (comboBoxChoixMode.SelectedItem.ToString())
            {
                case "Standard":
                    panelStandard.Visible = true;
                    break;
                case "Scientifique":
                    panelScientifique.Visible = true;
                    break;
                case "Programmeur":
                    panelProgrammeur.Visible = true;
                    break;
                case "Statistique":
                    panelStatistique.Visible = true;
                    break;
            }

            ResizeButtons();
        }



        private void ButtonClick(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                string btnText = button.Text;

                // Si le champ contient uniquement "0", on le remplace par le texte du bouton
                if (txtDisplay.Text == "0")
                    txtDisplay.Text = "";

                switch (btnText)
                {
                    case "x²":
                        txtDisplay.Text += "^2";
                        break;

                    case "n!":
                        txtDisplay.Text += "factorial()";
                        break;

                    case "√x":
                        txtDisplay.Text += "√()";
                        break;

                    case "cos":
                    case "sin":
                    case "tan":
                    case "ln":
                    case "log":
                    case "e":
                        txtDisplay.Text += $"{btnText}()";
                        break;

                    default:
                        if (txtDisplay.Text == "0")
                            txtDisplay.Text = btnText;
                        else
                            txtDisplay.Text += btnText;
                        break;
                }
            }
        }


        private void ButtonEgaleClick(object sender, EventArgs e)
        {
            try
            {
                string expression = NormalizeExpression(txtDisplay.Text);
                double result = EvaluateExpression(expression);

                txtDisplay.Text = result.ToString();

                string date = DateTime.Now.ToString("[dd/MM/yyyy HH:mm:ss]");
                historique.Add($"{date} {expression} = {result}");
                SaveHistorique();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur de calcul. Veuillez vérifier votre saisie.\n\n" + ex.Message,
                                "Erreur",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                txtDisplay.Text = "0";
            }
        }

        private void ButtonClearAllClick(object sender, EventArgs e)
        {
            txtDisplay.Text = "0";
        }

        private void ButtonDeleteClick(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtDisplay.Text) && txtDisplay.Text.Length > 1)
                txtDisplay.Text = txtDisplay.Text.Substring(0, txtDisplay.Text.Length - 1);
            else
                txtDisplay.Text = "0"; // Réinitialiser à "0" si tout est supprimé
        }


        private string NormalizeExpression(string expression)
        {
            expression = expression
                .Replace("π", "PI")
                .Replace("e(", "exp(")
                .Replace("√(", "sqrt(")
                .Replace("x²", "^2")
                .Replace("log(", "log10(")
                .Replace("×", "*")
                .Replace("÷", "/")
                .Replace(",", ".")
                .Replace("mod", "%");

            expression = ReplacePowerOperator(expression);
            return expression;
        }

        private string ReplacePowerOperator(string expression)
        {
            var regex = new Regex(@"(\([^()]+\)|\d+(\.\d+)?|\w+)\^(\([^()]+\)|\d+(\.\d+)?|\w+)");
            while (regex.IsMatch(expression))
            {
                expression = regex.Replace(expression, match =>
                {
                    string basePart = match.Groups[1].Value;
                    string exponentPart = match.Groups[3].Value;
                    return $"pow({basePart},{exponentPart})";
                });
            }
            return expression;
        }

        private double EvaluateExpression(string expression) // utilisation de NCalc ici 
        {
            Expression e = new Expression(expression);
            e.Parameters["PI"] = Math.PI;
            e.Parameters["π"] = Math.PI;
            e.Parameters["e"] = Math.E;

            e.EvaluateFunction += (name, funcArgs) =>
            {
                if (name.Equals("pow", StringComparison.OrdinalIgnoreCase))
                {
                    double x = Convert.ToDouble(funcArgs.Parameters[0].Evaluate());
                    double y = Convert.ToDouble(funcArgs.Parameters[1].Evaluate());
                    funcArgs.Result = Math.Pow(x, y);
                }
                else if (name.Equals("exp", StringComparison.OrdinalIgnoreCase))
                {
                    double x = Convert.ToDouble(funcArgs.Parameters[0].Evaluate());
                    funcArgs.Result = Math.Exp(x);
                }
                else if (name.Equals("sin", StringComparison.OrdinalIgnoreCase))
                {
                    double x = Convert.ToDouble(funcArgs.Parameters[0].Evaluate());
                    funcArgs.Result = Math.Sin(x); // Sinus en radians
                }
                else if (name.Equals("cos", StringComparison.OrdinalIgnoreCase))
                {
                    double x = Convert.ToDouble(funcArgs.Parameters[0].Evaluate());
                    funcArgs.Result = Math.Cos(x); // Cosinus en radians
                }
                else if (name.Equals("tan", StringComparison.OrdinalIgnoreCase))
                {
                    double x = Convert.ToDouble(funcArgs.Parameters[0].Evaluate());
                    funcArgs.Result = Math.Tan(x); // Tangente en radians
                }
                else if (name.Equals("factorial", StringComparison.OrdinalIgnoreCase))
                {
                    int n = Convert.ToInt32(funcArgs.Parameters[0].Evaluate());
                    if (n < 0) throw new InvalidOperationException("Factorielle uniquement pour des entiers positifs.");
                    funcArgs.Result = Factorial(n);
                }
                else if (name.Equals("sqrt", StringComparison.OrdinalIgnoreCase))
                {
                    double x = Convert.ToDouble(funcArgs.Parameters[0].Evaluate());
                    if (x < 0) throw new InvalidOperationException("La racine carrée d'un nombre négatif est invalide.");
                    funcArgs.Result = Math.Sqrt(x);
                }
                else if (name.Equals("ln", StringComparison.OrdinalIgnoreCase))
                {
                    double x = Convert.ToDouble(funcArgs.Parameters[0].Evaluate());
                    if (x <= 0) throw new InvalidOperationException("Le logarithme népérien d'une valeur non positive est invalide.");
                    funcArgs.Result = Math.Log(x);
                }
                else if (name.Equals("log", StringComparison.OrdinalIgnoreCase) || name.Equals("log10", StringComparison.OrdinalIgnoreCase))
                {
                    double x = Convert.ToDouble(funcArgs.Parameters[0].Evaluate());
                    if (x <= 0) throw new InvalidOperationException("Logarithme décimal d'une valeur non positive invalide.");
                    funcArgs.Result = Math.Log10(x);
                }
            };

            object result = e.Evaluate();
            double doubleResult = Convert.ToDouble(result);

            if (double.IsInfinity(doubleResult))
                throw new DivideByZeroException("Division par zéro détectée !");
            else if (double.IsNaN(doubleResult))
                throw new InvalidOperationException("Expression non définie (NaN).");

            return doubleResult;
        }


        private int Factorial(int n)
        {
            int result = 1;
            for (int i = 2; i <= n; i++)
                result *= i;
            return result;
        }

        // *************************** Mode Programmeur *************************** //

        private void ProgrammeurButtonClick(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                string btnText = button.Text;

                try
                {
                    switch (btnText)
                    {
                        case "A":
                        case "B":
                        case "C":
                        case "D":
                        case "E":
                        case "F":
                            if (currentBase == 16)
                                txtDisplay.Text += btnText;
                            else
                                MessageBox.Show($"Le caractère '{btnText}' est uniquement valide en base HEX (16).",
                                                "Erreur",
                                                MessageBoxButtons.OK,
                                                MessageBoxIcon.Warning);
                            break;

                        case "AND":
                        case "OR":
                        case "XOR":
                        case "<<":
                        case ">>":
                            if (long.TryParse(txtDisplay.Text, out long value))
                            {
                                operand1 = ConvertToDecimal(txtDisplay.Text, currentBase);
                                currentOperator = btnText;
                                txtDisplay.Text = "0";
                            }
                            else
                            {
                                MessageBox.Show($"Entrée invalide pour la base {currentBase}.",
                                                "Erreur",
                                                MessageBoxButtons.OK,
                                                MessageBoxIcon.Warning);
                            }
                            break;

                        case "NOT":
                            long notValue = ConvertToDecimal(txtDisplay.Text, currentBase);
                            txtDisplay.Text = ConvertFromDecimal(~notValue, currentBase);
                            break;

                        case "DEC":
                            ConvertBase(10);
                            MessageBox.Show("Base Décimale (10) sélectionnée.",
                                            "Changement de Base",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Information);
                            break;

                        case "HEX":
                            ConvertBase(16);
                            MessageBox.Show("Base Hexadécimale (16) sélectionnée.",
                                            "Changement de Base",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Information);
                            break;

                        case "BIN":
                            ConvertBase(2);
                            MessageBox.Show("Base Binaire (2) sélectionnée.",
                                            "Changement de Base",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Information);
                            break;

                        case "OCT":
                            ConvertBase(8);
                            MessageBox.Show("Base Octale (8) sélectionnée.",
                                            "Changement de Base",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Information);
                            break;

                        case "=":
                            if (operand1.HasValue && !string.IsNullOrEmpty(currentOperator))
                            {
                                long operand2 = ConvertToDecimal(txtDisplay.Text, currentBase);
                                long result = EvaluateBinaryOperation(operand1.Value, operand2, currentOperator);
                                txtDisplay.Text = ConvertFromDecimal(result, currentBase);
                                operand1 = null;
                                currentOperator = "";
                            }
                            else
                            {
                                MessageBox.Show("Aucune opération valide détectée.",
                                                "Erreur",
                                                MessageBoxButtons.OK,
                                                MessageBoxIcon.Warning);
                            }
                            break;

                        case "CLR":
                            txtDisplay.Text = "0";
                            operand1 = null;
                            currentOperator = "";
                            break;

                        case "Del":
                            ButtonDeleteClick(sender, e);
                            break;

                        default:
                            if (IsValidInputForBase(btnText))
                            {
                                if (txtDisplay.Text == "0")
                                    txtDisplay.Text = btnText;
                                else
                                    txtDisplay.Text += btnText;
                            }
                            else
                            {
                                MessageBox.Show($"Le caractère '{btnText}' n'est pas valide pour la base {currentBase}.",
                                                "Erreur",
                                                MessageBoxButtons.OK,
                                                MessageBoxIcon.Warning);
                            }
                            break;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur lors de l'exécution : {ex.Message}",
                                    "Erreur",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                    txtDisplay.Text = "0";
                }
            }
        }
        private bool IsValidInputForBase(string input)
        {
            string validChars = currentBase switch
            {
                2 => "01",
                8 => "01234567",
                10 => "0123456789",
                16 => "0123456789ABCDEF",
                _ => throw new InvalidOperationException("Base non prise en charge.")
            };

            return input.All(c => validChars.Contains(c));
        }

        private long EvaluateBinaryOperation(long op1, long op2, string operation)
        {
            return operation switch
            {
                "AND" => op1 & op2,
                "OR" => op1 | op2,
                "XOR" => op1 ^ op2,
                "<<" => op1 << (int)op2,
                ">>" => op1 >> (int)op2,
                _ => throw new InvalidOperationException("Opération inconnue"),
            };
        }

        private long ConvertToDecimal(string value, int baseFrom)
        {
            return Convert.ToInt64(value, baseFrom);
        }

        private string ConvertFromDecimal(long value, int baseTo)
        {
            return Convert.ToString(value, baseTo).ToUpper();
        }

        private void ConvertBase(int newBase)
        {
            long value = ConvertToDecimal(txtDisplay.Text, currentBase);
            currentBase = newBase;
            txtDisplay.Text = ConvertFromDecimal(value, currentBase);
        }

        // *************************** Mode Statstique *************************** //

        private double CalculateMean(double[] numbers) => numbers.Average();

        private double CalculateVariance(double[] numbers, double mean)
            => numbers.Average(num => Math.Pow(num - mean, 2));

        private void StatistiqueButtonClick(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                if (button.Text == "Del")
                {
                    ButtonDeleteClick(sender, e);
                    return;
                }

                try
                {
                    string[] values = txtDisplay.Text.Split(',');
                    double[] numbers = Array.ConvertAll(values, double.Parse);

                    double result = button.Text switch
                    {
                        "Moyenne" => CalculateMean(numbers),
                        "Variance" => CalculateVariance(numbers, CalculateMean(numbers)),
                        "Écart-Type" => Math.Sqrt(CalculateVariance(numbers, CalculateMean(numbers))),
                        _ => throw new InvalidOperationException("Opération non reconnue.")
                    };

                    MessageBox.Show($"{button.Text} : {result}", button.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (FormatException)
                {
                    MessageBox.Show("Veuillez entrer une liste de nombres valides, séparés par des virgules.", "Erreur de Format", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Une erreur inattendue s'est produite : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // *************************** Méthode pour la sauvegarde *************************** //

        private void SaveHistorique()
        {
            string file_histo = "historique.json";

            if (File.Exists(file_histo))
            {
                var contenuExistant = File.ReadAllText(file_histo);
                var historiqueExistant = JsonConvert
                    .DeserializeObject<List<string>>(contenuExistant) ?? new List<string>();

                historiqueExistant.AddRange(historique);

                File.WriteAllText(file_histo,
                    JsonConvert.SerializeObject(historiqueExistant, Formatting.Indented));
            }
            else
            {
                File.WriteAllText(file_histo,
                    JsonConvert.SerializeObject(historique, Formatting.Indented));
            }
        }
    }
}
