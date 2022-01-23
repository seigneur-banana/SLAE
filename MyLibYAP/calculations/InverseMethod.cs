using System;

namespace MyLibYAP.calculations
{
    public class InverseMethod : Matrix
    {
        /// <summary>
        ///Конструктор с известным числом коэффициентов при неизвестных
        /// </summary>
        /// <param name="n">//кол-во коэффициентов при неизвестных</param>
        public InverseMethod(int n)
            : base(n)
        {
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
        public InverseMethod(double[,] a, double[] b, double[] x, int n, bool error)
            : base(a, b, x, n, error)
        {

        }

        /// <summary>
        /// Матричный метод решения
        /// </summary>
        /// <returns>Решения СЛАУ</returns>
        public override double[] calculate()
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

    }
}
