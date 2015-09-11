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
    /// This class work with value types only.
    /// </remarks>
    /// </summary>
    /// <typeparam name="T"> This is a value type and it must to satisfy the IConcerible interface criterias.</typeparam>
    class RealMatrix<T> : LinearSpace<T>, IDisposable where T : IConvertible
    {
        /// <summary>
        /// Data members:
        /// Number of matrix rows.
        /// Number of matrix columns.
        /// An array for store elements of matrix.
        /// Neutral unit of base type. Default is 0.
        /// </summary>
        protected ulong mRows;
        protected ulong mCols;
        protected T[] mArray;
        protected T neutralUnit;
        protected static double infLimit;
        protected static bool infSetted = false;
        private bool disposed = false;
        /// <summary>
        /// Unsafe constructor.
        /// <remarks>
        /// The constructor is private, see Create method.
        /// </remarks>
        /// </summary>
        private RealMatrix(ulong rows, ulong cols)
        {
            try
            {
                mArray = new T[rows * cols];
                mRows = rows;
                mCols = cols;
            }
            catch (OutOfMemoryException e)
            {
                Console.WriteLine("Constructor Out of Memory exception: " + e.Message);
                throw;
            }
            var type = typeof(T);
            if(type == typeof(String) || type == typeof(DateTime))
            {
                throw new ArgumentException(String.Format("The type {0} is not supported", type.FullName), "T");
            }
            try
            {
                neutralUnit = (T)(Object)0;
            }
            catch ( Exception e)
            {
                throw new ApplicationException("The type is not supported with standard additional unit 0. It is possible this class if not suitable: " + e.Message);
            }
        }
        /// <summary>
        /// Safe instance creator. When constructor fail, it return null reference.
        /// </summary>
        /// <param name="rows">Number of matrix rows.</param>
        /// <param name="cols">Number of matrix columns.</param>
        /// <returns></returns>
        public RealMatrix<T> Create(ulong rows, ulong cols)
        {
            if(rows == 0 || cols == 0)
            {
                return null;
            }
            try
            {
                return new RealMatrix<T>(rows, cols);
            }
            catch (Exception e)
            {
                Console.WriteLine("Constructor fail. Instance has NULL reference: {0}", e.Message);
                return null;
            }
        }
        /// <summary>
        /// Disposable instances.
        /// </summary>
        public void Dispose()
        {
            Disposed(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose (bool disposing)
        {
            if(this.disposed)
            {
                if(disposing)
                {//Dispose resource.
                    Array.Clear(mArray, 0, mArray.Length);
                    mArray = null;
                }
                //Note for disposed
                disposed = true;
            }
        }
        
        public ulong RowNumber
        {
            get
            {
                if (this == null)
                    return 0;
                else
                    return mRows;
            }
        }
        public ulong ColNumber
        {
            get
            {
                if (this == null)
                    return 0;
                else
                    return mCols;
            }
        }

    }
}
