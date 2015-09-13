using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SergeyMS
{
    /// <summary>
    /// vvx
    /// Class of general Matrix
    /// <remarks>
    /// This class work with value types only.
    /// </remarks>
    /// </summary>
    /// <typeparam name="T"> This is a value type and it must to satisfy the IConcerible interface criterias.</typeparam>
    public class RealMatrix<T> : LinearSpace<T>, IDisposable where T : IConvertible
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
        protected static T infLimit;
        protected static bool infSetted = false;
        private bool disposed = false;
        protected T scalarNeutralUnit;
        /// <summary>
        /// Unsafe constructor.
        /// <remarks>
        /// The constructor is private, see Create method.
        /// </remarks>
        /// </summary>
        private RealMatrix(ulong rows, ulong cols)
        {
            var t = typeof(T);
            try
            {
                mArray = new T[rows * cols];
                mRows = rows;
                mCols = cols;
                if (t == typeof(byte) || t == typeof(short) || t == typeof(int) || t == typeof(long) || t == typeof(sbyte) || t == typeof(ushort) || t == typeof(uint) || t == typeof(ulong))
                {
                    infLimit = (T)(object)0;
                }
            }
            catch (OutOfMemoryException e)
            {
                Console.WriteLine("Constructor Out of Memory exception: " + e.Message);
                throw;
            }
            if(t == typeof(String) || t == typeof(DateTime))
            {
                throw new ArgumentException(String.Format("The type {0} is not supported", t.FullName), "T");
            }
            try
            {
                scalarNeutralUnit = (T)(object)1;
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
            Dispose(true);
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
        /// <summary>
        /// Properties for data members and Indexer to reach elements of matrix.
        /// </summary>
        /// <summary>
        /// Number of matrix rows get accerssor.
        /// </summary>
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
        /// <summary>
        /// Number of matrix columns get accerssor.
        /// </summary>
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
        /// <summary>
        /// Indexer to reach and to change matrix elements.
        /// <value>
        /// Must to be valid value of base type or from an other convertibile type.
        /// </value>
        /// </summary>
        public T this[ulong i, ulong j]
        {
            get
            {
                if(i >= mRows || j >= mCols)
                {
                    string message = "READ ELEMENT: Not exist element with index of (" + i + ", " + j + ")";
                    throw new System.ArgumentOutOfRangeException(message);
                }
                else
                {
                    return mArray[i * this.mCols + j];
                }
            }
            set
            {
                if (i >= mRows || j >= mCols)
                {
                    string message = "READ ELEMENT: Not exist element with index of (" + i + ", " + j + ")";
                    throw new System.ArgumentOutOfRangeException(message);
                }
                else
                {
                    mArray[i * this.mCols + j] = value;
                }
            }
        }
        /// <summary>
        /// Get-set accerssor for infinity value.
        /// The set accessor has one chance to modify and it will belong for all instance.
        /// </summary>
        public T infinity
        {
            get
            {
                return infLimit;
            }
            set
            {
                if (!infSetted)
                    infLimit = value;
            }
        }
        ///<summary>
        /// *********************************************************************************************************
        /// Linear Space interface criterias.
        /// </summary>
        LinearSpace<T> LinearSpace<T>.Operation(LinearSpace<T> ELEMENT3, LinearSpace<T> ELEMENT4)
        {
            if (ELEMENT3 is RealMatrix<T>)
            {
                RealMatrix<T> ELEMENT1 = (RealMatrix<T>)ELEMENT3;
            }
            else
                throw new Exception();
            RealMatrix<T> ELEMENT2 = (RealMatrix<T>)ELEMENT4;
            if (ELEMENT1.RowNumber != ELEMENT2.RowNumber || ELEMENT1.ColNumber != ELEMENT2.ColNumber)
            {//Addition mathematical rules, handle only part of null matrix situation
                string message = "Operation fail: can not use '+' operator for matrix (" + ELEMENT1.RowNumber + " x " + ELEMENT1.ColNumber + ") and matrix ("
                    + ELEMENT2.RowNumber + " x " + ELEMENT2.ColNumber + ")";
                throw new System.Exception(message);
            }
            if (ELEMENT1 == null || ELEMENT2 == null)
            {//this protect when all of them is null
                return null;
            }
            RealMatrix<T> ELEMENT5 = new RealMatrix<T>(ELEMENT1.RowNumber, ELEMENT1.ColNumber);
            var type = typeof(T);
            if (type == typeof(String) || type == typeof(DateTime))
            {
                throw new ArgumentException(String.Format("The type {0} is not supported", type.FullName), "T");
            }
            for (ulong i = 0; i < ELEMENT1.RowNumber; i++)
            {
                for (ulong j = 0; j < ELEMENT1.ColNumber; j++)
                {
                    //ELEMENT3[i, j] = ELEMENT1[i, j] + ELEMENT2[i, j];
                    try
                    {
                        ELEMENT5[i, j] = (T)(Object)(ELEMENT1[i, j].ToDouble(System.Globalization.NumberFormatInfo.CurrentInfo) + ELEMENT2[i, j].ToDouble(System.Globalization.NumberFormatInfo.CurrentInfo));
                    }
                    catch (Exception ex)
                    {
                        throw new ApplicationException("The operation failed.", ex);
                    }
                }
            }
            return ELEMENT5;
        }
        public static RealMatrix<T> operator +(RealMatrix<T> ELEMNET1, RealMatrix<T> ELEMENT2)
        {
            RealMatrix<T> ELEMENT3 = new RealMatrix<T>(ELEMNET1.RowNumber, ELEMNET1.ColNumber);
            return ELEMENT3 as (LinearSpace<T>.Operation((LinearSpace<T>)ELEMNET1, (LinearSpace<T>)ELEMENT2));
        }
        /*
        protected static RealMatrix<T> Operation_inverse(RealMatrix<T> ELEMENT3, RealMatrix<T> ELEMENT4)
        {
            RealMatrix<T> ELEMENT1 = (RealMatrix<T>)ELEMENT3;
            RealMatrix<T> ELEMENT2 = (RealMatrix<T>)ELEMENT4;
            if (ELEMENT1.RowNumber != ELEMENT2.RowNumber || ELEMENT1.ColNumber != ELEMENT2.ColNumber)
            {//Addition mathematical rules, handle only part of null matrix situation
                string message = "Operation fail: can not use '+' operator for matrix (" + ELEMENT1.RowNumber + " x " + ELEMENT1.ColNumber + ") and matrix ("
                    + ELEMENT2.RowNumber + " x " + ELEMENT2.ColNumber + ")";
                throw new System.Exception(message);
            }
            if (ELEMENT1 == null || ELEMENT2 == null)
            {//this protect when all of them is null
                return null;
            }
            RealMatrix<T> ELEMENT3 = new RealMatrix<T>(ELEMENT1.RowNumber, ELEMENT1.ColNumber);
            var type = typeof(T);
            if (type == typeof(String) || type == typeof(DateTime))
            {
                throw new ArgumentException(String.Format("The type {0} is not supported", type.FullName), "T");
            }
            for (ulong i = 0; i < ELEMENT1.RowNumber; i++)
            {
                for (ulong j = 0; j < ELEMENT1.ColNumber; j++)
                {
                    //ELEMENT3[i, j] = ELEMENT1[i, j] - ELEMENT2[i, j];
                    try
                    {
                        ELEMENT3[i, j] = (T)(Object)(ELEMENT1[i, j].ToDouble(System.Globalization.NumberFormatInfo.CurrentInfo) - ELEMENT2[i, j].ToDouble(System.Globalization.NumberFormatInfo.CurrentInfo));
                    }
                    catch (Exception ex)
                    {
                        throw new ApplicationException("The operation failed.", ex);
                    }
                }
            }
            return ELEMENT3;
        }
        public static RealMatrix<T> operator -(RealMatrix<T> ELEMNET1, RealMatrix<T> ELEMENT2)
        {
            return Operation_inverse(ELEMNET1, ELEMENT2);
        }
        public override bool Equals(System.Object obj)
        {//this is safe
            if (obj == null)
            {
                return false;
            }
            RealMatrix<T> ELEMENT1 = obj as RealMatrix<T>;
            if ((System.Object)ELEMENT1 == null)
            {
                return false;
            }
            if (ELEMENT1.RowNumber != this.RowNumber || ELEMENT1.ColNumber != this.ColNumber)
            {
                return false;
            }
            for (ulong i = 0; i < ELEMENT1.RowNumber; i++)
            {
                for (ulong j = 0; j < ELEMENT1.ColNumber; j++)
                {
                    var type = typeof(T);
                    if (type == typeof(String) || type == typeof(DateTime))
                    {
                        throw new ArgumentException(String.Format("The type {0} is not supported", type.FullName), "T");
                    }
                    try
                    {
                        if (ELEMENT1[i, j].ToDouble(System.Globalization.NumberFormatInfo.CurrentInfo) - this[i, j].ToDouble(System.Globalization.NumberFormatInfo.CurrentInfo) > (double)(object)infinity
                            || this[i, j].ToDouble(System.Globalization.NumberFormatInfo.CurrentInfo) - ELEMENT1[i, j].ToDouble(System.Globalization.NumberFormatInfo.CurrentInfo) > (double)(object)infinity)
                        {
                            return false;
                        }
                    }
                    catch (Exception ex)
                    {
                        return false;
                        throw new ApplicationException("RETURN FALSE Check the type for Equals." + ex.Message);
                    }
                }
            }
            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public static bool operator ==(RealMatrix<T> ELEMENT1, RealMatrix<T> ELEMENT2)
        {
            return ELEMENT1.Equals(ELEMENT2);
        }
        public static bool operator !=(RealMatrix<T> ELEMENT1, RealMatrix<T> ELEMENT2)
        {
            return !(ELEMENT1 == ELEMENT2);
        }
        public RealMatrix<T> MultiplyScalar(T scalar, RealMatrix<T> ELEMENT1)
        {
            for (ulong i = 0; i < this.RowNumber; i++)
            {
                for (ulong j = 0; j < this.ColNumber; j++)
                {
                    this[i, j] = (T)(Object)(this[i, j].ToDouble(System.Globalization.NumberFormatInfo.CurrentInfo) * scalar.ToDouble(System.Globalization.NumberFormatInfo.CurrentInfo));
                }
            }
        }
        */
    }
}
