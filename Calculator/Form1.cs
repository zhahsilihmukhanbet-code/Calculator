
using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace Calculator
{
    public partial class Form1 : Form
    {
        private TextBox display;
        private double firstNumber = 0;
        private string operation = "";
        private bool startNew = true;
        private double memory = 0;

        public Form1()
        {
            Text = "Calculator";
            ClientSize = new Size(320, 300);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = SystemColors.Control;

            display = new TextBox
            {
                Location = new Point(10, 10),
                Size = new Size(300, 25),
                Text = "0.",
                TextAlign = HorizontalAlignment.Right,
                ReadOnly = true,
                Font = new Font("Arial", 11)
            };
            Controls.Add(display);

            string[][] keys =
            {
                new[] { "Backspace", "CE", "C" },
                new[] { "MC", "7", "8", "9", "/", "sqrt" },
                new[] { "MR", "4", "5", "6", "*", "%" },
                new[] { "MS", "1", "2", "3", "-", "1/x" },
                new[] { "M+", "0", "+/-", ".", "+", "=" }
            };

            for (int r = 0; r < keys.Length; r++)
            {
                for (int c = 0; c < keys[r].Length; c++)
                {
                    string key = keys[r][c];
                    Button b = new Button
                    {
                        Text = key,
                        Size = new Size(
                            key == "Backspace" ? 97 : 46, 34),
                        Location = new Point(
                            key == "Backspace" ? 61 : 10 + c * 51,
                            45 + r * 43),
                        Font = new Font("Arial", 9)
                    };

                    b.Click += Button_Click;
                    Controls.Add(b);
                }
            }
        }

        private double GetValue()
        {
            double.TryParse(
                display.Text,
                NumberStyles.Float,
                CultureInfo.InvariantCulture,
                out double value);

            return value;
        }

        private void ShowValue(double value)
        {
            display.Text = value.ToString(
                "G12", CultureInfo.InvariantCulture);
        }

        private void Button_Click(object sender, EventArgs e)
        {
            string key = ((Button)sender).Text;
            double value = GetValue();

            if (key.Length == 1 && char.IsDigit(key[0]))
            {
                if (startNew || display.Text == "0.")
                    display.Text = key;
                else
                    display.Text += key;

                startNew = false;
                return;
            }

            switch (key)
            {
                case ".":
                    if (startNew)
                        display.Text = "0.";

                    if (!display.Text.Contains("."))
                        display.Text += ".";

                    startNew = false;
                    break;

                case "C":
                    display.Text = "0.";
                    firstNumber = 0;
                    operation = "";
                    startNew = true;
                    break;

                case "CE":
                    display.Text = "0.";
                    startNew = true;
                    break;

                case "Backspace":
                    if (display.Text.Length > 1)
                        display.Text =
                            display.Text.Substring(
                                0, display.Text.Length - 1);
                    else
                        display.Text = "0";

                    break;

                case "+/-":
                    ShowValue(-value);
                    break;

                case "sqrt":
                    if (value < 0)
                        display.Text = "Error";
                    else
                        ShowValue(Math.Sqrt(value));

                    startNew = true;
                    break;

                case "1/x":
                    if (value == 0)
                        display.Text = "Error";
                    else
                        ShowValue(1 / value);

                    startNew = true;
                    break;

                case "%":
                    ShowValue(value / 100);
                    startNew = true;
                    break;

                case "MC":
                    memory = 0;
                    break;

                case "MR":
                    ShowValue(memory);
                    startNew = true;
                    break;

                case "MS":
                    memory = value;
                    break;

                case "M+":
                    memory += value;
                    break;

                case "+":
                case "-":
                case "*":
                case "/":
                    firstNumber = value;
                    operation = key;
                    startNew = true;
                    break;

                case "=":
                    double result = value;

                    switch (operation)
                    {
                        case "+":
                            result = firstNumber + value;
                            break;

                        case "-":
                            result = firstNumber - value;
                            break;

                        case "*":
                            result = firstNumber * value;
                            break;

                        case "/":
                            if (value == 0)
                            {
                                display.Text = "Error";
                                startNew = true;
                                return;
                            }

                            result = firstNumber / value;
                            break;
                    }

                    ShowValue(result);
                    operation = "";
                    startNew = true;
                    break;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
