using System;
using System.Windows.Forms;

namespace MyLibYAP.calculations
{
    public abstract class Matrix
    {
        public double[,] a { get; set; } //Массив коэффициентов при неизвестных

        public double[] b { get; set; } //Массив свободных коэффициентов
        public double[] x { get; set; } //Массив корней системы
        public int n { get; set; } //Количество неизвестных системы (количество уравнений)
        public bool error { get; set; } //Поле ошибки

        /// <summary>
        ///Конструктор по умолчанию
        /// </summary>
        public Matrix()
        {
            n = 2;
            error = false;
        }

        /// <summary>
        ///Конструктор с известным числом коэффициентов при неизвестных
        /// </summary>
        /// <param name="n">//кол-во коэффициентов при неизвестных</param>
        public Matrix(int n)
        {
            this.n = n;  //Количество неизвестных системы (количество уравнений)
            a = new double[n, n];
            b = new double[n];
            x = new double[n];
        }

        /// <summary>
        ///Конструктор со всеми известными коэффициентами
        /// </summary>
        /// <param name="a">Массив коэффициентов при неизвестных</param>
        /// <param name="b">Массив свободных коэффициентов</param>
        /// <param name="x">Массив корней системы</param>
        /// <param name="n">Количество неизвестных системы</param>
        /// <param name="error">Поле ошибки</param>
        public Matrix(double[,] a, double[] b, double[] x, int n, bool error)
        {
            this.a = a;
            this.b = b;
            this.x = x;
            this.n = n;
            this.error = error;
        }
        public abstract double[] calculate();

        /// <summary>
        /// Передача актуальных данных из главной формы
        /// </summary>
        /// <param name="dgv">dataGridView</param>
        /// <param name="n">//кол-во коэффициентов при неизвестных</param>
        public void Change(DataGridView dgv, int n)
        {
            this.n = n; //кол-во коэффициентов при неизвестных
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                    a[i, j] = Convert.ToDouble(dgv[j, i].Value); //массив a(коэффициентов при неизвестных)
                b[i] = Convert.ToDouble(dgv[n, i].Value); //массив b(массив свободных коэффициентов)
            }
        }
    }
}
