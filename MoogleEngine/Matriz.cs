using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoogleEngine
{
    public class Matriz
    {
        public Matriz()
        {
            double [,] matriz = new double[3,3];
        }

        public static double[] MultMatrizXVector(double[,]Matriz, double[] vector)
        {
           double[] result = new double[Matriz.GetLength(0)];
            if (vector.Length == Matriz.GetLength(1))
            {

                for (int i = 0; i < Matriz.GetLength(0); i++)
                {
                    for (int j = 0; j < vector.Length; j++)

                        result[i] += Matriz[i,j] * vector[j];
                }

                return result;
            }


            else
                throw new ArgumentException("no se puede ejecutar");

        }

        public static double[,] SumaDeMatrices(double [,]Matriz1, double [,]Matriz2)
        {
            double[,] result = new double [Matriz1.GetLength(0),Matriz1.GetLength(1)];

            if (Matriz1.GetLength(0) == Matriz2.GetLength(0) && Matriz1.GetLength(1) == Matriz2.GetLength(1))
            {
                for (int i = 0; i < Matriz1.Rank; i++)
                    for (int j = 0; j < Matriz1.GetLength(0); j++)
                        result[i,j] = Matriz1[i,j] + Matriz2[i,j];

                return result;
            }
            else
                throw new ArgumentException("no se puede ejecutar");
        }

    }
}