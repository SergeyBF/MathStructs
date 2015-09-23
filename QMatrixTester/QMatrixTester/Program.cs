using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SergeyMS;

namespace QMatrixTester
{
    //Test QMatrix class
    class Program
    {
        static void Main(string[] args)
        {
            QMatrix<int> M = new QMatrix<int>(3);
            Console.WriteLine(M);
            Console.WriteLine("rows: " + M.RowNumber + "\ncols: " + M.ColNumber + "\ninf: " + M.infinity + "\ninfSet: " + M.infinitySetted);
            QMatrix<int> N = new QMatrix<int>(3);
            M[0, 0] = 1;
            M[2, 2] = 3;
            Console.WriteLine("M and N equals: " + (M == N) + "\nM and N not equals: " + (M != N));
            Console.WriteLine(5 * M);
            Console.WriteLine(M * 5);
            N[2, 2] = 10;
            N[1, 1] = -4;
            Console.WriteLine(M + N);
            Console.WriteLine(M - N);
        }
    }
}
