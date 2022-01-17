using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSC_36
{
    internal class MatrixCalc
    {
        public double [,] MatrixMult(double [,] matrixC, double[,] matrixW) {
            // Произведение матриц. https://habr.com/ru/post/359272/ , https://www.cyberforum.ru/csharp-beginners/thread72083.html

            double[,] matrixCresul = new double[1, 3];
            for (int m = 0; m < matrixCresul.GetLength(0); m++)
            {
                for (int n = 0; n < matrixCresul.GetLength(1); n++)
                {
                    for (int j = 0; j < matrixW.GetLength(1); j++)
                    {
                        matrixCresul[m, n] += matrixW[j, n] * matrixC[m, j];
                    }
                }
            }
            return matrixCresul;
        }

    }
}
