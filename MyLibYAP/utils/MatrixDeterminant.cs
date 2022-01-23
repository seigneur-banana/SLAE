using System;

namespace MyLibYAP.utils
{
    public class Determinant
    {
        private double[,] a; //Массив коэффициентов при неизвестных

        /// <summary>
        ///Конструктор с известным числом коэффициентов при неизвестных
        /// </summary>
        /// <param name="a">//Массив коэффициентов при неизвестных</param>
        public Determinant(double[,] a)
        {
            this.a = a;
        }

        /// <summary>
        /// Подсчет определителя матрицы.
        /// </summary>
        /// <returns>Определитель</returns>
        public double Determinate()
        {
            double det = 0;
            int Rank = a.GetLength(0); //измерение массива
            if (Rank == 1) det = a[0, 0];
            if (Rank == 2) det = a[0, 0] * a[1, 1] - a[0, 1] * a[1, 0];
            if (Rank > 2)
            {
                for (int j = 0; j < a.GetLength(1); j++)
                {
                    det += Math.Pow(-1, 0 + j) * a[0, j] * Determinate(GetMinor(a, 0, j));
                }
            }
            return det;
        }
        private double Determinate(double[,] a)
        {
            double det = 0;
            int Rank = a.GetLength(0);
            if (Rank == 1) det = a[0, 0];
            if (Rank == 2) det = a[0, 0] * a[1, 1] - a[0, 1] * a[1, 0];
            if (Rank > 2)
            {
                for (int j = 0; j < a.GetLength(1); j++)
                {
                    det += Math.Pow(-1, 0 + j) * a[0, j] * Determinate(GetMinor(a, 0, j));
                }
            }
            return det;
        }
        /// <summary>
        /// Получение минора
        /// </summary>
        /// <returns>Минор</returns>
        private static double[,] GetMinor(double[,] matrix, int row, int column)
        {
            double[,] buf = new double[matrix.GetLength(0) - 1, matrix.GetLength(0) - 1];
            for (int i = 0; i < matrix.GetLength(0); i++)
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if ((i != row) || (j != column))
                    {
                        if (i > row && j < column) buf[i - 1, j] = matrix[i, j];
                        if (i < row && j > column) buf[i, j - 1] = matrix[i, j];
                        if (i > row && j > column) buf[i - 1, j - 1] = matrix[i, j];
                        if (i < row && j < column) buf[i, j] = matrix[i, j];
                    }
                }
            return buf;
        }
    }
}
