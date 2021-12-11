using System;
using System.Windows.Forms;

namespace MyLibYAP
{
    public class Matrix
    {
        private double[,] a; //Массив коэффициентов при неизвестных
        private double[] b; //Массив свободных коэффициентов
        private double[] x; //Массив корней системы
        private int n; //Количество неизвестных системы (количество уравнений)
        private bool error; //Поле ошибки

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
            error = false;
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

        /// <summary>
        /// Метод Гаусса
        /// </summary>
        /// <returns>Решения СЛАУ</returns>
        public double[] gauss()
        {
            double max;
            int k = 0, index;
            const double eps = 0.00000001; // точность 10^-8

            for (int i = 0; i < n; i++)
                x[i] = 0;

            double[,] a1 = new double[n, n];
            //временный массив для основной матрицы
            double[] b1 = new double[n];
            //временный массив для свободных коэффициентов
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                    a1[i, j] = a[i, j];
                b1[i] = b[i];
            }

            while (k < n)
            {
                // Поиск строки с максимальным a[i,k]
                max = Math.Abs(a1[k, k]);
                index = k;
                for (int i = k + 1; i < n; i++)
                {
                    if (Math.Abs(a1[i, k]) > max)
                    {
                        max = Math.Abs(a1[i, k]);
                        index = i;
                    }
                }
                // Перестановка строк
                if (max < eps)
                {
                    // нет ненулевых диагональных элементов
                    error = true;
                    return x;
                }
                for (int j = 0; j < n; j++)
                {
                    double temp1 = a1[k, j];
                    a1[k, j] = a1[index, j];
                    a1[index, j] = temp1;
                }
                double temp = b1[k];
                b1[k] = b1[index];
                b1[index] = temp;
                // Нормализация уравнений
                for (int i = k; i < n; i++)
                {
                    double temp1 = a1[i, k];
                    if (Math.Abs(temp1) < eps) continue; // для нулевого коэффициента пропустить
                    for (int j = 0; j < n; j++)
                        a1[i, j] = a1[i, j] / temp1;
                    b1[i] = b1[i] / temp1;
                    if (i == k) continue; // уравнение не вычитать само из себя
                    for (int j = 0; j < n; j++)
                        a1[i, j] = a1[i, j] - a1[k, j];
                    b1[i] = b1[i] - b1[k];
                }
                k++;
            }
            // обратная подстановка
            for (k = n - 1; k >= 0; k--)
            {
                x[k] = b1[k];
                for (int i = 0; i < k; i++)
                    b1[i] = b1[i] - a1[i, k] * x[k];
            }
            Array.Clear(a1, 0, n * n);//очистка массива
            Array.Clear(b1, 0, n);//очистка массива
            return x;
        }

        /// <summary>
        /// Матричный метод решения
        /// </summary>
        /// <returns>Решения СЛАУ</returns>
        public double[] inverse_method()
        {
            double temp;

            double[,] e = new double[n, n];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    if (i == j)
                        e[i, j] = 1.0;
                    else e[i, j] = 0.0;

            double[,] a1 = new double[n, n];
            for (int p = 0; p < n; p++)
                for (int q = 0; q < n; q++)
                    a1[p, q] = a[p, q];

            for (int k = 0; k < n; k++)
            {
                temp = a1[k, k];

                for (int j = 0; j < n; j++)
                {
                    a1[k, j] /= temp;
                    e[k, j] /= temp;
                }

                for (int i = k + 1; i < n; i++)
                {
                    temp = a1[i, k];

                    for (int j = 0; j < n; j++)
                    {
                        a1[i, j] -= a1[k, j] * temp;
                        e[i, j] -= e[k, j] * temp;
                    }
                }
            }

            for (int k = n - 1; k > 0; k--)
            {
                for (int i = k - 1; i >= 0; i--)
                {
                    temp = a1[i, k];

                    for (int j = 0; j < n; j++)
                    {
                        a1[i, j] -= a1[k, j] * temp;
                        e[i, j] -= e[k, j] * temp;
                    }
                }
            }

            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    a1[i, j] = e[i, j];

            for (int i = 0; i < n; i++)
            {
                temp = 0;
                for (int j = 0; j < n; j++)
                    temp += a1[i, j] * b[j];
                x[i] = temp;
            }
            Array.Clear(e, 0, n * n);
            Array.Clear(a1, 0, n * n);
            return x;
        }

        public bool Error
        {
            get
            {
                return error;
            }
            set
            {
                error = value;
            }
        }

        public double[,] A
        {
            get
            {
                return a;
            }
            set
            {
                a = value;
            }
        }
        public double[] B
        {
            get
            {
                return b;
            }
            set
            {
                b = value;
            }
        }
        public double[] X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
            }
        }
        public int N
        {
            get
            {
                return n;
            }
            set
            {
                n = value;
            }
        }

    }
}
