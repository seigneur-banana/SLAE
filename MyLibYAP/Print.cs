using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Windows.Forms;

namespace MyLibYAP
{
    public class Print
    {
        private double[,] a; //Массив коэффициентов при неизвестных
        private double[] b; //Массив свободных коэффициентов
        private double[] x; //Массив корней системы
        private int n; //Количество неизвестных системы (количество уравнений)
        private bool error; //Поле ошибки

        public Print()
        {
            error = false;
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public Print(double[,] a, double[] b, double[] x, int n, bool error)
        {
            this.a = a;
            this.b = b;
            this.x = x;
            this.n = n;
            this.error = error;
        }

        /// <summary>
        /// Геттер на поле Error
        /// </summary>
        public bool Error
        {
            get
            {
                return error;
            }
        }

        /// <summary>
        /// Прорисовка dataGridView
        /// </summary>
        /// <param name="dgv">dataGridView</param>
        /// <param name="n">//кол-во коэффициентов при неизвестных</param>
        public void clear_dgv(DataGridView dgv, int n)
        {
            dgv.RowCount = n;
            dgv.ColumnCount = n + 1;
            for (int i = 0; i < dgv.Columns.Count; i++)
            {
                if (i == dgv.Columns.Count - 1)
                    dgv.Columns[i].HeaderText = "b";
                else
                    dgv.Columns[i].HeaderText = "x" + (i + 1);
            }

            for (int i = 0; i < n; i++) //rows
                for (int j = 0; j < n + 1; j++) // columns
                    dgv[j, i].Value = 0;
        }

        /// <summary>
        /// Вывод матрицы в таблицу
        /// </summary>
        /// <param name="dgv">dataGridView</param>
        /// <param name="NUD">numericUpDown</param>
        public void PrintDgv(DataGridView dgv, NumericUpDown NUD)
        {
            dgv.RowCount = n;
            dgv.ColumnCount = n + 1;
            NUD.Value = n;
            for (int i = 0; i < dgv.Columns.Count; i++)
            {
                if (i == dgv.Columns.Count - 1)
                    dgv.Columns[i].HeaderText = "b";
                else
                    dgv.Columns[i].HeaderText = "x" + (i + 1);
            }

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                    dgv[j, i].Value = a[i, j];
                dgv[n, i].Value = b[i];
            }
        }

        StreamReader streamToPrint; //Поток для принтера
        Font printFont;
        /// <summary>
        /// Печать
        /// </summary>
        /// <param name="pF"></param>
        /// <param name="filename"></param>
        public void PrintResult(Font pF, string filename)
        {
            try
            {
                streamToPrint = new System.IO.StreamReader(filename,
                    System.Text.Encoding.GetEncoding(1251));
                try
                {
                    printFont = pF;
                    PrintDocument pd = new PrintDocument();
                    pd.PrintPage += new PrintPageEventHandler
                       (pd_PrintPage);
                    pd.Print();
                    error = true;
                }
                finally
                {
                    streamToPrint.Close();
                }
            }
            catch
            {
                error = true;
            }
        }

        /// <summary>
        /// Событие PrintPage вызывается для каждой страницы, которая будет напечатана
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ev"></param>
        private void pd_PrintPage(object sender, PrintPageEventArgs ev)
        {
            float linesPerPage = 0;
            float yPos = 0;
            int count = 0;
            float leftMargin = ev.MarginBounds.Left;
            float topMargin = ev.MarginBounds.Top;
            string line = null;

            // Чтобы вычислить количество строк на странице
            linesPerPage = ev.MarginBounds.Height / printFont.GetHeight(ev.Graphics);

            // Печатаем каждую строку файла
            while (count < linesPerPage && ((line = streamToPrint.ReadLine()) != null))
            {
                yPos = topMargin + (count * printFont.GetHeight(ev.Graphics));
                ev.Graphics.DrawString(line, printFont, Brushes.Black,
                leftMargin, yPos, new StringFormat());
                count++;
            }

            // если строки не закончились, распечатаем еще одну страницу
            if (line != null)
                ev.HasMorePages = true;
            else
                ev.HasMorePages = false;
        }
    }
}

