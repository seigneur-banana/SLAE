using System;

namespace MyLibYAP.calculations
{
    public class GaussMethod : Matrix
    {
        /// <summary>
        ///Конструктор с известным числом коэффициентов при неизвестных
        /// </summary>
        /// <param name="n">//кол-во коэффициентов при неизвестных</param>
        public GaussMethod(int n)
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
        public GaussMethod(double[,] a, double[] b, double[] x, int n, bool error)
            : base(a, b, x, n, error)
        {

        }

        /// <summary>
        /// Решение методос Гаусса
        /// </summary>
        /// <returns>Решения СЛАУ</returns>
        public override double[] calculate()
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

    }
}
