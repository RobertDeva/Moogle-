using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoogleEngine
{
    public class Vector
    {
        public Vector()
        {
            double [] vector = new double[4];
        }
        static double [] VectroXEscalar(double [] vector, double escalar)
        {
            double [] result = new double [vector.Length];
            for (int i = 0; i < vector.Length; i++)
                result[i] = vector [i] * escalar;
            return result;
        }
       /// <summary>
       /// Normalizar el vector con la norma 
       /// </summary>
       /// <param name="vector">es el vector q se le va aplicar la norma</param>
       /// <returns>double result</returns>
        public static double AplicarL2Norm(double[] vector)
        { 
            double result;
            double sumSquared = 0;  

            foreach (var value in vector)
            {
               sumSquared += Math.Pow(value, 2);
            }
            result = sumSquared;

            return result;
        }
    }
}
