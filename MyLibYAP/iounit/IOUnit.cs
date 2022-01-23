using MyLibYAP.utils;
using System;
using System.IO;

namespace MyLibYAP.iounit
{
    public class File
    {
        private double[,] a; //Массив коэффициентов при неизвестных
        private double[] b; //Массив свободных коэффициентов
        private double[] x; //Массив корней системы
        private int n; //Количество неизвестных системы (количество уравнений)
        private bool error; //Поле ошибки

        public File()
        {
            error = false;
        }
        /// <summary>
        /// Конструктор
        /// </summary>
        public File(double[,] a, double[] b, double[] x, int n, bool error)
        {
            this.a = a;
            this.b = b;
            this.x = x;
            this.n = n;
            this.error = error;
        }

        /// <summary>
        /// Чтение матрицы из файла
        /// </summary>
        /// <param name="filename">Имя файла</param>
        public void ReadFile(string filename)
        {
            try
            {
                string[] lines = System.IO.File.ReadAllLines(filename);

                int rowCount = lines.Length; //Количество строк
                int colCount = lines[0].Split(' ').Length;//Количество столбцов

                if (colCount <= 21 && rowCount <= 20)
                {
                    n = rowCount;
                    a = new double[n, n];
                    b = new double[n];
                    double[,] a1 = new double[n, n + 1];
                    for (int i = 0; i < n; i++)
                    {
                        string[] line = lines[i].Split();
                        for (int j = 0; j < n + 1; j++)
                        {
                            a1[i, j] = int.Parse(line[j]);
                        }
                    }
                    for (int i = 0; i < n; i++)
                    {
                        for (int j = 0; j < n; j++)
                            a[i, j] = a1[i, j]; //Массив a(коэффициентов при неизвестных)
                        b[i] = a1[i, n]; //Массив b(массив свободных коэффициентов)
                    }
                    Array.Clear(a1, 0, n * (n + 1));
                }
                else error = true;
            }
            catch { error = true; }
        }

        /// <summary>
        /// Сохранение в файл
        /// </summary>
        /// <param name="filename">Имя файла</param>
        public void Save(string filename)
        {
            System.IO.StreamWriter sw = new System.IO.StreamWriter(filename, false,
                   System.Text.Encoding.GetEncoding(1251));
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    sw.Write(a[i, j] + " ");
                }
                sw.Write(b[i]);
                sw.WriteLine();
            }
            sw.Write("Решение системы: ");
            for (int i = 0; i < n; i++)
            {
                sw.Write("x[" + (i + 1) + "]=");
                sw.Write(x[i] + "; ");
            }
            Determinant det = new Determinant(a);
            sw.Write("\nОпределитель: " + det.Determinate().ToString());
            sw.Close();
        }

        public bool Error
        {
            get
            {
                return error;
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

