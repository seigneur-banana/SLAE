using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MyLibYAP.iounit;
using MyLibYAP.calculations;
using MyLibYAP.utils;

namespace ProjectYAP
{
    public partial class Form1 : Form
    {
        Matrix matrix;
        OpenFileDialog openFileDialog1 = new OpenFileDialog();
        string filename;
        public Form1()
        {
            InitializeComponent();
            matrix = new GaussMethod(2);
            openFileDialog1.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
            saveFileDialog1.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
        }

        /// <summary>
        /// Решение системы линейных алгебраических уравнений
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                label1.Text = "Результат:";
                int n = Convert.ToInt32(numericUpDown1.Text);
                matrix = new GaussMethod(n);
                matrix.Change(dgv, n);

                Rank rank = new Rank(matrix.a, matrix.b, matrix.n);
                int rank_a2 = rank.Rank_a2(),
                    rank_a = rank.Rank_a();
   
                double[] x = new double[n];

                if (rank_a != rank_a2)
                {
                    MessageBox.Show("Система решений не имеет");
                }
                else
                {
                    if (rank_a != n)
                    {
                        MessageBox.Show("Система имеет множество решений");
                    }
                    else
                    {
                        x = matrix.calculate(); //Вычисление корней(решений) СЛАУ методом Гаусса
                        Determinant det = new Determinant(matrix.a);
                        if (matrix.error == true && det.Determinate() != 0) //вычисление матричным методом в случае ошибки
                        {
                            Matrix matrix1 = new InverseMethod(n);
                            matrix1.Change(dgv, n);
                            x = matrix1.calculate();
                        }
                        for (int i = 0; i < n; i++)
                            label1.Text += "\nx[" + (i + 1) + "] = " + x[i].ToString("F2");
                    }
                }
            }
            catch
            {
                MessageBox.Show("Ошибка(but1)\nВычислить невозможно.");
            }
        }

        /// <summary>
        /// Вычисление определителя
        /// </summary>
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                matrix.Change(dgv, Convert.ToInt32(numericUpDown1.Text));
                Determinant det = new Determinant(matrix.a);
                label2.Text = "Определитель:  " + det.Determinate().ToString("F2");
            }
            catch
            {
                MessageBox.Show("Ошибка(Det)\nВычислить невозможно.");
            }
        }

        /// <summary>
        /// Изменение количества неизвестных
        /// </summary>
        private void numericUpDown1_Click(object sender, EventArgs e)
        {
            Print print = new Print();
           print.clear_dgv(dgv, Convert.ToInt32(numericUpDown1.Text));
            label1.Text = "Результат:";
            label2.Text = "Определитель:";
        }

        private void dgv_CellValueChanged(object sender, DataGridViewCellEventArgs e)
            //Событие срабатывает при изменении любой ячейки в dataGridView
        {
            matrix = new GaussMethod(Convert.ToInt32(numericUpDown1.Text));
        }

        /// <summary>
        /// Чтение коэффициентов из файла
        /// </summary>
        private void чтениеИзФайлаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK && openFileDialog1.FileName.Length > 0)
                try
                {
                    File file = new File();
                    file.ReadFile(openFileDialog1.FileName);
                    Print print = new Print(file.A, file.B, file.X, file.N, file.Error);
                    matrix = new GaussMethod(file.A, file.B, file.X, file.N, file.Error);
                    print.PrintDgv(dgv, numericUpDown1);
                    filename = openFileDialog1.FileName;
                }
                catch
                {
                    MessageBox.Show("Ошибка чтения!\nНеверный формат данных.");
                    return;
                }
        }
        /// <summary>
        /// Сохранение результатов
        /// </summary>
        private void сохранитьКакToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        button1_Click(sender, e);
                        button2_Click(sender, e);
                        File file = new File(matrix.a, matrix.b, matrix.x, matrix.n, matrix.error);
                        file.Save(saveFileDialog1.FileName);
                    }
                    catch
                    {
                        MessageBox.Show("Ошибка!\n Сохранить не удалось..");
                        return;
                    }
                }
            }
            catch
            {
                MessageBox.Show("Ошибка!\n Сохранить не удалось..");
                return;
            }
        }
        /// <summary>
        /// Вывод печати на принтер
        /// </summary>
        private void печатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fileName= "Name";
            button1_Click(sender, e);
            button2_Click(sender, e);
            File file = new File(matrix.a, matrix.b, matrix.x, matrix.n, matrix.error);
            Print print = new Print(matrix.a, matrix.b, matrix.x, matrix.n, matrix.error);
            file.Save(fileName);
            try
            {
                if (printDialog1.ShowDialog() == DialogResult.OK)
                {
                    print.PrintResult(Font, fileName);
                }
            }
            catch
            {
                MessageBox.Show("Ошибка !");
                return;
            }
        }

        /// <summary>
        /// KeyPress для контроля вводимых данных
        /// </summary>
        private void dgv_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
                TextBox tb = (TextBox)e.Control;
                tb.KeyPress += new KeyPressEventHandler(tb_KeyPress);
        }
        void tb_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsDigit(e.KeyChar)) && 
                (e.KeyChar != ',') && 
                (e.KeyChar != '-') && 
                (e.KeyChar != '.'))
            {
                if (e.KeyChar != (char)Keys.Back)
                { e.Handled = true; }
            }
            if (e.KeyChar == '.') e.KeyChar = ',';//замена точки на запятую
            if (e.KeyChar == ',')
            {
                if ((sender as TextBox).Text.IndexOf(',') != -1)//не печатать вторую запятую
                    e.Handled = true;
                return;
            }
            if ((sender as TextBox).Text == "-" && e.KeyChar == '-') 
                //минус только в начале и не больше одного
                if (e.KeyChar != (char)Keys.Back)
                { e.Handled = true; }
            if (e.KeyChar == '-')
            {
                if ((sender as TextBox).Text.IndexOf('-') != -1) 
                    e.Handled = true;
                return;
            }
        }
    }
}
