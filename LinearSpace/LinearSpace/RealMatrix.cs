using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SergeyMS
{
    /// <summary>
    /// Class of general Matrix
    /// <remarks>
    /// 
    /// </remarks>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class RealMatrix<T> : LinearSpace<T> where T : IConvertible
    {
        /// <summary>
        /// Data members:
        /// <param name="rows"> Number of matrix rows.</param>
        /// <param name="cols"> Number of matrix coloums.</param>
        /// <param name="array"> An array for store elements of matrix.</param>
        /// </summary>
        protected ulong row;
        protected ulong cols;
        protected T[] array;

    }
}
