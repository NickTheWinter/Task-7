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

namespace Task7
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        public static string letters = "qwertyuiop[]asdgfhjlk;'xzcvbnm,/`|\\_\"=+~./<>(){}:?йцукенгшщзхъфывапролджэячсмитьбюё";
        public static string CorrectInput(string text)
        {
            foreach (char item in letters.ToCharArray())
            {
                if (text.ToLower().Contains(item))
                    return "";
                continue;
            }
            return text;
        }
        private void Calculate_Click(object sender, RoutedEventArgs e)
        {
            double?[,] matrix = new double?[3, 5];
            foreach (TextBox textBox in grid.Children.OfType<TextBox>())
            {
                if (textBox.Text == "")
                {
                    MessageBox.Show("Введите все значения в таблицу");
                    goto end;
                }
                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    for (int j = 0; j < matrix.GetLength(1); j++)
                    {
                        if (matrix[i, j] == null)
                        {
                            matrix[i, j] = Convert.ToDouble(textBox.Text);
                            goto mark;
                        }

                        else
                            continue;

                    }
                }
                
            mark:
                Console.WriteLine();

            }
            if (matrix[2, 4] == 0)
            {
                double[] marks = new double[4];
                double[] freeEl = new double[2];
                int counter1 = 0;
                int counter2 = 0;
                double maxMark = -9999;
                int resolveColumn = 0;
                int resolveRow = 0;
                double minRatio = 9999;
                for (int i = 0; i < matrix.GetLength(0); i++)
                    for (int j = 0; j < matrix.GetLength(1); j++)
                    {
                        if (i == 2 && j < 4)
                        {
                            marks[counter1] = (double)matrix[i, j];
                            counter1++;
                        }
                        if ((i == 0 && j == 4) || (i == 1 && j == 4))
                        {
                            freeEl[counter2] = (double)matrix[i, j];
                            counter2++;
                        }
                    }

                for (int i = 0; i < marks.Length; i++)
                {
                    if (marks[i] > maxMark)
                    {
                        maxMark = marks[i];
                        resolveColumn = i + 1;
                    }
                }

                for (int i = 0; i < matrix.GetLength(0); i++)
                    for (int j = 0; j < matrix.GetLength(1); j++)
                    {
                        if (j == resolveColumn - 1 && i != 2)
                        {
                            if (matrix[i, j] == 0 || matrix[i, 4] < 0)
                                continue;
                            else if (matrix[i, 4] >= 0 && matrix[i, j] > 0)
                            {
                                minRatio = ((double)((double)matrix[i, 4] / matrix[i, j] < minRatio ? matrix[i, 4] / matrix[i, j] : minRatio));
                                resolveRow = i + 1;
                            }

                        }
                    }

                if (resolveColumn > 0 && resolveRow > 0)
                {
                    double resolveEl = (double)matrix[resolveRow - 1, resolveColumn - 1];
                    ResolveColumn.Text = resolveColumn.ToString();
                    ResolveEl.Text = resolveEl.ToString();
                    ResolveRow.Text = resolveRow.ToString();
                }
                else
                {
                    ResolveColumn.Text = "нет";
                    ResolveEl.Text = "нет";
                    ResolveRow.Text = "нет";

                }


                int freeperem = 0;
                IEnumerable<TextBox> rots = Roots.Children.OfType<TextBox>();
                for (int i = 0; i < matrix.GetLength(0); i++)
                    for (int j = 0; j < matrix.GetLength(1); j++)
                        if (IsSingleColumn(matrix, i, j))
                        {
                            freeperem++;
                            TextBox root = rots.ToArray()[j];
                            root.Text = matrix[freeperem - 1, 4].ToString();
                        }
                foreach (var item in Roots.Children.OfType<TextBox>())
                    if (item.Text == "")
                        item.Text = "Нет";

                FreePerem.Text = freeperem.ToString();
                BasisPerem.Text = (matrix.GetLength(1) - freeperem - 1).ToString();
            
            }
            else
                MessageBox.Show("Значение целевой функции не равно 0");
            end:
            Console.WriteLine();
        }

        public bool IsSingleColumn(double?[,] matrix, int i, int j)
        {
            try
            {
                if ((matrix[i, j] == 1 && matrix[i + 1, j] == 0 && matrix[i + 2, j] == 0) || (matrix[i, j] == 0 && matrix[i + 1, j] == 1 && matrix[i + 2, j] == 0) || (matrix[i, j] == 0 && matrix[i + 1, j] == 0 && matrix[i + 2, j] == 1))
                    return true;
                else
                    return false;
            }
            catch (IndexOutOfRangeException) { }
            return false;
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            foreach (TextBox textBox in grid.Children.OfType<TextBox>())
                textBox.Clear();
            foreach (TextBox textBox in Info.Children.OfType<TextBox>())
                textBox.Clear();
            foreach (TextBox textBox in Roots.Children.OfType<TextBox>())
                textBox.Clear();
        }

        private void FillWithRand_Click(object sender, RoutedEventArgs e)
        {
            Random rand = new Random();
            foreach (TextBox textBox in grid.Children.OfType<TextBox>())
                textBox.Text = rand.Next(-10, 10).ToString();

            ThreeFive.Text = "0";
            foreach (TextBox textBox in Info.Children.OfType<TextBox>())
                textBox.Clear();
            foreach (TextBox textBox in Roots.Children.OfType<TextBox>())
                textBox.Clear();
        }


        private void OneOne_TextChanged(object sender, TextChangedEventArgs e)
        {
            OneOne.Text = CorrectInput(OneOne.Text);
        }

        private void OneTwo_TextChanged(object sender, TextChangedEventArgs e)
        {
            OneTwo.Text = CorrectInput(OneTwo.Text);
        }

        private void OneThree_TextChanged(object sender, TextChangedEventArgs e)
        {
            OneThree.Text = CorrectInput(OneThree.Text);
        }

        private void OneFour_TextChanged(object sender, TextChangedEventArgs e)
        {
            OneFour.Text = CorrectInput(OneFour.Text);
        }

        private void OneFive_TextChanged(object sender, TextChangedEventArgs e)
        {
            OneFive.Text = CorrectInput(OneFive.Text);
        }

        private void TwoOne_TextChanged(object sender, TextChangedEventArgs e)
        {
            TwoOne.Text = CorrectInput(TwoOne.Text);
        }

        private void TwoTwo_TextChanged(object sender, TextChangedEventArgs e)
        {
            TwoTwo.Text = CorrectInput(TwoTwo.Text);
        }

        private void TwoThree_TextChanged(object sender, TextChangedEventArgs e)
        {
            TwoThree.Text = CorrectInput(TwoThree.Text);
        }

        private void TwoFour_TextChanged(object sender, TextChangedEventArgs e)
        {
            TwoFour.Text = CorrectInput(TwoFour.Text);
        }

        private void TwoFive_TextChanged(object sender, TextChangedEventArgs e)
        {
            TwoFive.Text = CorrectInput(TwoFive.Text);
        }

        private void ThreeOne_TextChanged(object sender, TextChangedEventArgs e)
        {
            ThreeOne.Text = CorrectInput(ThreeOne.Text);
        }

        private void ThreeTwo_TextChanged(object sender, TextChangedEventArgs e)
        {
            ThreeTwo.Text = CorrectInput(ThreeTwo.Text);
        }

        private void ThreeThree_TextChanged(object sender, TextChangedEventArgs e)
        {
            ThreeThree.Text = CorrectInput(ThreeThree.Text);
        }

        private void ThreeFour_TextChanged(object sender, TextChangedEventArgs e)
        {
            ThreeFour.Text = CorrectInput(ThreeFour.Text);
        }

        private void ThreeFive_TextChanged(object sender, TextChangedEventArgs e)
        {
            ThreeFive.Text = CorrectInput(ThreeFive.Text);
        }


    }
}
