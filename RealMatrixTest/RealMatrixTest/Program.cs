using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//LinearSpace.dll is connected and it is working
using SergeyMS;

namespace RealMatrixTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //Creator is working (safe object-factory)
            RealMatrix<double> M = RealMatrix<double>.Create(4, 5);
            //indexer and row/col setters are working
            for(ulong i = 0; i < M.RowNumber; i++)
                for (ulong j = 0; j < M.ColNumber; j++)
                {
                    M[i, j] = i + j;
                }
            //ToString method is working
            Console.WriteLine(M);
            //scalar-multiplication and '*' operator overload are working
            Console.WriteLine(5 * M);
            RealMatrix<int> N = RealMatrix<int>.Create(4, 5);
            for (ulong i = 0; i < N.RowNumber; i++)
                for (ulong j = 0; j < N.ColNumber; j++)
                {
                    N[i, j] = (int)i - (int)j * (int)j;
                }
            Console.WriteLine(N);
            RealMatrix<double> O = RealMatrix<double>.Create(4, 5);
            for (ulong i = 0; i < O.RowNumber; i++)
                for (ulong j = 0; j < O.ColNumber; j++)
                {
                    O[i, j] = (int)i - (int)j * (int)j;
                }
            //operation results are suitable for mathematical rules
            Console.WriteLine(O + M);
            Console.WriteLine((O + M) * 0.25);
            RealMatrix<int> X = RealMatrix<int>.Create(2, 3);
            RealMatrix<int> Y = RealMatrix<int>.Create(2, 3);
            //Equals and '==' operator are working
            X[0, 1] = Y[0, 1] = 1;
            X[1, 0] = Y[1, 0] = X[1, 2] = Y[1, 2] = 137;
            Console.WriteLine(X + "\n **** \n" + Y);
            Console.WriteLine("X and Y equals: " + (X == Y) + "\n****\n");
            Console.WriteLine("X and Y not equals: " + (X != Y) + "\n****\n");
            RealMatrix<double> Z = RealMatrix<double>.Create(2, 3);
            Z[0, 1] = 1;
            Z[1, 0] = Z[1, 2] = 137;
            Console.WriteLine(Z);
            //The '-' operation is working too
            Console.WriteLine(X - Y);
            //This is not working, I don't wonder
            //Console.WriteLine("X and Z equals: " + (X == Z) + "\n****\n");

            //infinity setting is working well (modify only once)
            Console.WriteLine(M.infinity);
            M.infinity = 0.137;
            Console.WriteLine(M.infinity);
            M.infinity = 0.137137;
            Console.WriteLine(M.infinity);
            Console.WriteLine(O.infinity);
            //Allow to set other infinity value for integer as double
            Console.WriteLine(X.infinity);
            Y.infinity = 1;
            Console.WriteLine(X.infinity);
            X.infinity = 2;
            Console.WriteLine(X.infinity);
            M = null;
            Console.WriteLine(M);
        }
    }
}
