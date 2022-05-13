using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

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
        public static string letters = "qwertyuiop[]asdgfhjlk;'xzcvbnm/`|\\_\"=+~./<>(){}:?йцукенгшщзхъфывапролджэячсмитьбюё";
        //Метод проверки вводимых значение в Textbox
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
            //Объявление матрицы
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
                            //Запись значений Textbox'ов в матрицу
                            try
                            {
                                matrix[i, j] = Convert.ToDouble(textBox.Text);
                                goto mark;
                            }
                            catch (Exception)
                            {
                                MessageBox.Show("Введенны некорректные значения");
                                goto end;
                            }

                        }

                        else
                            continue;

                    }
                }

            mark:
                Console.WriteLine();

            }
            //Проверка на значение целевой функции
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
                            //Определение строки оценков
                            marks[counter1] = (double)matrix[i, j];
                            counter1++;
                        }
                        if ((i == 0 && j == 4) || (i == 1 && j == 4))
                        {
                            //Определение столбца свободных элементов
                            freeEl[counter2] = (double)matrix[i, j];
                            counter2++;
                        }
                    }

                for (int i = 0; i < marks.Length; i++)
                {
                    //Определение разрешающего столбца
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
                            //Определение разрещающей строки
                            else if (matrix[i, 4] >= 0 && matrix[i, j] > 0)
                            {
                                minRatio = ((double)((double)matrix[i, 4] / matrix[i, j] < minRatio ? matrix[i, 4] / matrix[i, j] : minRatio));
                                resolveRow = i + 1;
                            }

                        }
                    }
                //Находим разрешающий элемент и выводим значения переменных в TextBox
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
                            //Определение количества свободжных переменных
                            freeperem++;
                            TextBox root = rots.ToArray()[j];
                            //Вывод корней в TextBox
                            root.Text = matrix[freeperem - 1, 4].ToString();
                        }
                foreach (var item in Roots.Children.OfType<TextBox>())
                    if (item.Text == "")
                        item.Text = "Нет";
                //Вывод количества свободных и базисных переменных
                FreePerem.Text = freeperem.ToString();
                BasisPerem.Text = (matrix.GetLength(1) - freeperem - 1).ToString();

            }
            else
                MessageBox.Show("Значение целевой функции не равно 0");
            end:
            Console.WriteLine();
        }
        //Метод определяющий является ли столбец единичным
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
        //Очищение значений всех TextBox'ов
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            foreach (TextBox textBox in grid.Children.OfType<TextBox>())
                textBox.Clear();
            foreach (TextBox textBox in Info.Children.OfType<TextBox>())
                textBox.Clear();
            foreach (TextBox textBox in Roots.Children.OfType<TextBox>())
                textBox.Clear();
        }
        //Заполнение TextBox'ов рандомными значениями
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

        //Проверка вводимых значений TextBox'ов
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
        //Вывод постановки задачи
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Постановка задачи: Нахождение разрешающего элемента в симплекс-таблице, определение количества свободных и базисных переменных, а также значения всех переменных X1=? Х2=? Х3=? Х4=?");
        }
    }
}