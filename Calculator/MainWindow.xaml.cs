using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Calculator
{
    
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private double mainValue = 0;
        private string lastOperation = "+";
        private bool isOperation = true;
        private bool isTotal = false;
        private double memoryDigit = 0;

        private bool err = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Clear()
        {
            mainValue = 0;
            lastOperation = "+";
            isOperation = true;
            isTotal = false;
            err = false;
        }

        private void UpdateText(string text)
        {
            if (text == "∞")
            {
                textBox.FontSize = 14;
                textBox.Text = "Деление на ноль невозможно";
                err = true;
            }
            else
                textBox.Text = text;
        }

        private void UpdateMemory(double digit)
        {
            if (digit != 0)
            {
                TextMemory.Text = "M";
            }
            else
                TextMemory.Text = "";
        }

        private double Sum(double x, double y, string sign)
        {
            if (sign == "+")
                return x + y;
            if (sign == "-")
                return x - y;
            if (sign == "*")
                return x * y;
            else
                return x / y;
        }

        private void HistoryOp(string sign)
        {
            if (isOperation)
            {
                if (isTotal)
                {
                    mainValue = 0;
                    lastOperation = "+";
                    isTotal = false;
                }
                double tempValue = Convert.ToDouble(textBox.Text);
                mainValue = Sum(mainValue, tempValue, lastOperation);
                isOperation = false;
                UpdateText(mainValue.ToString());
                lastOperation = sign;

                int length = textHistory.Text.Length + Convert.ToString(tempValue).Length + 3;

                if (length > 40)
                {
                    textHistory.Text = textHistory.Text.Remove(0, length - 40);
                    textHistory.Text = textHistory.Text.Insert(0, "«");
                }

                textHistory.Text = textHistory.Text + tempValue + ' ' + lastOperation + ' ';
            }
            else
            {
                if (textHistory.Text.Length > 2)
                {
                    textHistory.Text = textHistory.Text.Remove(textHistory.Text.Length - 2, 1);
                    textHistory.Text = textHistory.Text.Insert(textHistory.Text.Length - 1, sign);
                }

                lastOperation = sign;
            }
        }

        
        private void digit(object sender, RoutedEventArgs e)
        {
            if (!err)
            {
                Button number = (Button)sender;
                if (textBox.Text == "0" || !isOperation)
                {
                    UpdateText(number.Content.ToString());
                    isOperation = true;
                }
                else
                    UpdateText(textBox.Text + number.Content.ToString());
            }
        }

        private void del(object sender, RoutedEventArgs e)
        {
            if (!(textBox.Text == "0") && !err)
            {
                UpdateText(textBox.Text.Remove(textBox.Text.Length - 1));

                if (textBox.Text.Length == 0)
                    UpdateText("0");
            }
        }

        private void plus(object sender, RoutedEventArgs e)
        {
            if (!err) HistoryOp("+");
        }

        private void minus(object sender, RoutedEventArgs e)
        {
            if (!err) HistoryOp("-");
        }

        private void multiplication(object sender, RoutedEventArgs e)
        {
            if (!err) HistoryOp("*");
        }

        private void division(object sender, RoutedEventArgs e)
        {
            if (!err) HistoryOp("/");
        }

        private void percent(object sender, RoutedEventArgs e)
        {
            if (!err)
            {
                string temp = Convert.ToString((mainValue * Convert.ToDouble(textBox.Text)) / 100);
                UpdateText(temp);
                isOperation = true;
            }
            
        }

        private void comma(object sender, RoutedEventArgs e)
        {
            if(!textBox.Text.Contains(",") && !err)
            {
                UpdateText(textBox.Text + ",");
            }
        }

        private void reciproc(object sender, RoutedEventArgs e)
        {
            if (!err)
            {
                double reciproc = 1 / Convert.ToDouble(textBox.Text);
                
                UpdateText(reciproc.ToString());
                isOperation = true;
            }

        }

        private void Negative(object sender, RoutedEventArgs e)
        {
            if (!err) UpdateText((-Convert.ToDouble(textBox.Text)).ToString());
        }

        private void Sqrt(object sender, RoutedEventArgs e)
        {
            if (!err)
            {
                double sqrt = Convert.ToDouble(textBox.Text);
                if (sqrt >= 0)
                {
                    UpdateText(Math.Sqrt(sqrt).ToString());
                    isOperation = true;
                }
                else
                {
                    UpdateText("Недопустимый ввод");
                    err = true;
                }
            }
        }

        private void CE(object sender, RoutedEventArgs e)
        {
            if (!err)
                UpdateText("0");
            else 
            {
                textHistory.Text = "";
                UpdateText("0");
                Clear();
            }
        }

        private void C(object sender, RoutedEventArgs e)
        {
            textHistory.Text = "";
            UpdateText("0");
            Clear();
        }

        private void Total(object sender, RoutedEventArgs e)
        {
            double tempValue = Convert.ToDouble(textBox.Text);

            if (!isTotal)
            {
                UpdateText(Sum(mainValue, tempValue, lastOperation).ToString());
                mainValue = tempValue;
                isTotal = true;
                
            }
            else
                UpdateText(Sum(tempValue, mainValue, lastOperation).ToString());

            isOperation = true;
            textHistory.Text = "";
        }

        private void MemorySave(object sender, RoutedEventArgs e)
        {
            memoryDigit = Convert.ToDouble(textBox.Text);
            UpdateMemory(memoryDigit);
        }

        private void MemoryRead(object sender, RoutedEventArgs e)
        {
            UpdateText(memoryDigit.ToString());
        }

        private void MemoryClear(object sender, RoutedEventArgs e)
        {
            memoryDigit = 0;
            UpdateMemory(memoryDigit);
        }

        private void MemoryPlus(object sender, RoutedEventArgs e)
        {
            memoryDigit += Convert.ToDouble(textBox.Text);
            UpdateMemory(memoryDigit);
        }

        private void MemoryMinus(object sender, RoutedEventArgs e)
        {
            memoryDigit -= Convert.ToDouble(textBox.Text);
            UpdateMemory(memoryDigit);
        }
    }
}
