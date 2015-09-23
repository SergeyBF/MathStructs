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
    public class RealMatrix<T> : LinearSpace<T>, IDisposable where T : IConvertible, IEquatable<T>
    {
        /// <summary>
        /// Data members:
        /// </summary>
        /// <summary>
        /// Number of matrix rows.
        /// </summary>
        protected ulong mRows;
        /// <summary>
        /// Number of matrix columns.
        /// </summary>
        protected ulong mCols;
        /// <summary>
        /// An array for store elements of matrix.
        /// </summary>
        protected T[] mArray;
        /// <summary>
        /// Static infinity value. Changable only once.
        /// </summary>
        protected static T infLimit;
        /// <summary>
        /// Static infinity setting watcher. Ensure no modify more time the infinity value.
        /// </summary>
        protected static bool infSetted = false;
        private bool disposed = false;
        /// <summary>
        /// Unsafe constructor.
        /// <remarks>
        /// The constructor is private, for use is the Create method.
        /// </remarks>
        /// </summary>
        /// <param name="rows"> Number of matrix rows, not modifyable.</param>
        /// <param name="cols"> Number of matrix columns, not modifyable.</param>
        private RealMatrix(ulong rows, ulong cols)
        {
            /// <summary>
            /// Generic constructor get type from Create method.
            /// <remarks>
            /// Infinity value setting only for integer types. For other types maybe should to grant more criterias.
            /// </remarks>
            /// </summary>
            var typeT = typeof(T);
            try
            {
                mArray = new T[rows * cols];
                mRows = rows;
                mCols = cols;
                if (typeT == typeof(byte) || typeT == typeof(short) || typeT == typeof(int) || typeT == typeof(long) || typeT == typeof(sbyte) || typeT == typeof(ushort) || typeT == typeof(uint) || typeT == typeof(ulong))
                {
                    infLimit = (T)(object)0;
                }
            }
            catch (OutOfMemoryException e)
            {
                Console.WriteLine("Constructor Out of Memory exception: " + e.Message);
                throw;
            }
            /// <summary>
            /// <remarks>
            /// Ensure for no use unsuitable types.
            /// </remarks>
            /// </summary>
            if(typeT == typeof(String) || typeT == typeof(DateTime))
            {
                throw new ArgumentException(String.Format("The type {0} is not supported", typeT.FullName), "T");
            }
        }
        /// <summary>
        /// Safe instance creator. When constructor fail, it return null reference.
        /// </summary>
        /// <param name="rows">Number of matrix rows.</param>
        /// <param name="cols">Number of matrix columns.</param>
        public static RealMatrix<T> Create(ulong rows, ulong cols)
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
        /// <summary>
        /// IDisposable interface member.
        /// </summary>
        /// <param name="disposing"> Allow disposing.</param>
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
        /// This method usefull at particular operation.
        /// </summary>
        /// <returns> Return value is a string, which contain the matrix element by element with tabulator and new-line characters. </returns>
        public override string ToString()
        {//recommended use string-builder
            string s = "";
            if (this == null)
            {
                return null;
            }
            else
            {
                for (ulong i = 0; i < RowNumber; i++)
                {
                    for (ulong j = 0; j < ColNumber; j++)
                    {
                        s += this[i, j];
                        s += "\t";
                    }
                    s += "\n";
                }
            }
            return s;
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
        /// Warning: Using standard integer types, NO set it.
        /// <value>
        /// Set one infinity limit value for all distance. The setting is final.
        /// </value>
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
                infSetted = true;
            }
        }
        /// <summary>
        /// Get accessor to check if infinity limit is already setted.
        /// Warning: Using standard integer types, NO set infinity value.
        /// </summary>
        public bool infinitySetted
        {
            get
            {
                return infSetted;
            }
        }
        ///<summary>
        /// *********************************************************************************************************
        /// Linear Space interface criterias.
        /// </summary>
        /// <summary>
        /// Linear Space over T mathematical body contain an additional operation.
        /// This method is from interface but hidden, because there is '+' operator overload for the additional operation.
        /// </summary>
        /// <param name="Element3"> In method declaration seem like the parameter type is LinearSpace but it must be RealMatrix instance.</param>
        /// <param name="Element4"> In method declaration seem like the parameter type is LinearSpace but it must be RealMatrix instance.</param>
        LinearSpace<T> LinearSpace<T>.Operation(LinearSpace<T> Element3, LinearSpace<T> Element4)
        {
            RealMatrix<T> Element1 = (RealMatrix<T>)Element3;
            RealMatrix<T> Element2 = (RealMatrix<T>)Element4;
            if (Element1.RowNumber != Element2.RowNumber || Element1.ColNumber != Element2.ColNumber)
            {//Addition mathematical rules, handle only part of null matrix situation
                string message = "Operation fail: can not use '+' operator for matrix (" + Element1.RowNumber + " x " + Element1.ColNumber + ") and matrix ("
                    + Element2.RowNumber + " x " + Element2.ColNumber + ")";
                throw new System.Exception(message);
            }
            if (Element1 == null || Element2 == null)
            {//this protect when all of them is null
                return null;
            }
            RealMatrix<T> Element5 = new RealMatrix<T>(Element1.RowNumber, Element1.ColNumber);
            var type = typeof(T);
            T Value1;
            T Value2;
            if (type == typeof(String) || type == typeof(DateTime))
            {//I think this conditions are not necessary.
                throw new ArgumentException(String.Format("The type {0} is not supported", type.FullName), "T");
            }
            for (ulong i = 0; i < Element1.RowNumber; i++)
            {
                for (ulong j = 0; j < Element1.ColNumber; j++)
                {
                    Value1 = Element1[i, j];
                    Value2 = Element2[i, j];
                    Element5[i, j] = Sum(Value1, Value2);
                }
            }
            return Element5 as LinearSpace<T>;
        }
        /// <summary>
        /// The real public additional operation of Linear Space. The operation is commutative so not important the parameter's order.
        /// </summary>
        /// <param name="ELEMNET1">Get one of the real matrix instances.</param>
        /// <param name="Element2">Get other one of the real matrix instances.</param>
        /// <returns></returns>
        public static RealMatrix<T> operator +(RealMatrix<T> ELEMNET1, RealMatrix<T> Element2)
        {
            RealMatrix<T> Element3 = new RealMatrix<T>(ELEMNET1.RowNumber, ELEMNET1.ColNumber);

            Element3 = (RealMatrix<T>)(Element3 as LinearSpace<T>).Operation(ELEMNET1 as LinearSpace<T>, Element2 as LinearSpace<T>);
            return Element3;
        }
        /// <summary>
        /// This is a tool method for handle generic incidets. The base type values are from a mathematical body so not important the parameter's order.
        /// </summary>
        /// <param name="Value1"> The base type variable.</param>
        /// <param name="Value2"> The other base type valiable.</param>
        /// <returns></returns>
        private static T Sum(T Value1, T Value2)
        {
            return (dynamic)Value1 + (dynamic)Value2;
        }
        /// <summary>
        /// Linear Space over T mathematical body contain an inverse additional operation.
        /// This method is from interface but hidden, because there is '-' operator overload for the inverse additional operation.
        /// </summary>
        /// <param name="Element3"> In method declaration seem like the parameter type is LinearSpace but it must be RealMatrix instance.</param>
        /// <param name="Element4"> In method declaration seem like the parameter type is LinearSpace but it must be RealMatrix instance.</param>
        LinearSpace<T> LinearSpace<T>.OperationInverse(LinearSpace<T> Element3, LinearSpace<T> Element4)
        {
            RealMatrix<T> Element1 = (RealMatrix<T>)Element3;
            RealMatrix<T> Element2 = (RealMatrix<T>)Element4;
            if (Element1.RowNumber != Element2.RowNumber || Element1.ColNumber != Element2.ColNumber)
            {//Addition mathematical rules, handle only part of null matrix situation
                string message = "Operation fail: can not use '-' operator for matrix (" + Element1.RowNumber + " x " + Element1.ColNumber + ") and matrix ("
                    + Element2.RowNumber + " x " + Element2.ColNumber + ")";
                throw new System.Exception(message);
            }
            if (Element1 == null || Element2 == null)
            {//this protect when all of them is null
                return null;
            }
            RealMatrix<T> Element5 = new RealMatrix<T>(Element1.RowNumber, Element1.ColNumber);
            var type = typeof(T);
            T Value1;
            T Value2;
            if (type == typeof(String) || type == typeof(DateTime))
            {//I think this conditions are not necessary.
                throw new ArgumentException(String.Format("The type {0} is not supported", type.FullName), "T");
            }
            for (ulong i = 0; i < Element1.RowNumber; i++)
            {
                for (ulong j = 0; j < Element1.ColNumber; j++)
                {
                    Value1 = Element1[i, j];
                    Value2 = Element2[i, j];
                    Element5[i, j] = SumInverse(Value1, Value2);
                }
            }
            return Element5 as LinearSpace<T>;
        }
        /// <summary>
        /// The real public additional inverse operation of Linear Space. The operation is commutative so not important the parameter's order.
        /// </summary>
        /// <param name="ELEMNET1">Get one of the real matrix instances.</param>
        /// <param name="Element2">Get other one of the real matrix instances.</param>
        /// <returns></returns>
        public static RealMatrix<T> operator -(RealMatrix<T> ELEMNET1, RealMatrix<T> Element2)
        {
            RealMatrix<T> Element3 = new RealMatrix<T>(ELEMNET1.RowNumber, ELEMNET1.ColNumber);

            Element3 = (RealMatrix<T>)(Element3 as LinearSpace<T>).OperationInverse(ELEMNET1 as LinearSpace<T>, Element2 as LinearSpace<T>);
            return Element3;
        }
        /// <summary>
        /// This is a tool method for handle generic incidets. The base type values are from a mathematical body so not important the parameter's order.
        /// </summary>
        /// <param name="Value1"> The base type variable.</param>
        /// <param name="Value2"> The other base type valiable.</param>
        /// <returns></returns>
        private static T SumInverse(T Value1, T Value2)
        {
            return (dynamic)Value1 - (dynamic)Value2;
        }
        
        /// <summary>
        /// This method compare two matrixes elements by element.
        /// </summary>
        /// <param name="obj"> Parameter can be anything but real compare will with two RealMatrix instance.</param>
        /// <returns> Logical true, if instances are from RealMatrix class and all suitable pair of elements are equals.</returns>
        public override bool Equals(object obj)
        {//this is safe
            if (obj == null)
            {
                return false;
            }
            RealMatrix<T> Element1 = obj as RealMatrix<T>;
            if ((object)Element1 == null)
            {
                return false;
            }
            if (Element1.RowNumber != this.RowNumber || Element1.ColNumber != this.ColNumber)
            {
                return false;
            }
            if (infinitySetted)
            {
                for (ulong i = 0; i < Element1.RowNumber; i++)
                {
                    for (ulong j = 0; j < Element1.ColNumber; j++)
                    {
                        if ((dynamic)Element1[i, j] - (dynamic)this[i, j] > (dynamic)infinity || (dynamic)this[i, j] - (dynamic)Element1[i, j] > (dynamic)infinity)
                        {
                            return false;
                        }
                    }
                }
            }else
            {
                for (ulong i = 0; i < Element1.RowNumber; i++)
                {
                    for (ulong j = 0; j < Element1.ColNumber; j++)
                    {
                        if ((dynamic)Element1[i, j] != (dynamic)this[i, j])
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// Need for Equal method.
        /// </summary>
        /// <returns> A general hashcode. </returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        /// <summary>
        /// Overloaded operator for comforable equals operatuion. This work with only two RealMatrix instances.
        /// </summary>
        /// <param name="Element1"> An instance of RealMatrix class.</param>
        /// <param name="Element2"> An instance of RealMatrix class.</param>
        /// <returns>Logical true, if all suitable pair of elements are equals.</returns>
        public static bool operator ==(RealMatrix<T> Element1, RealMatrix<T> Element2)
        {
            return Element1.Equals(Element2);
        }
        /// <summary>
        /// Overloaded operator for comforable equals operatuion. This work with only two RealMatrix instances.
        /// </summary>
        /// <param name="Element1"> An instance of RealMatrix class.</param>
        /// <param name="Element2"> An instance of RealMatrix class.</param></param>
        /// <returns> Logical true, if all suitable pair of elements are equals.</returns>
        public static bool operator !=(RealMatrix<T> Element1, RealMatrix<T> Element2)
        {
            return !(Element1 == Element2);
        }
        /// <summary>
        /// Linear Space over T mathematical body contain a scalar-multiplication operation.
        /// This method is from interface but hidden, because there is '*' operator overload for the inverse additional operation.
        /// </summary>
        /// <param name="Scalar"> A constant value from T.</param>
        /// <param name="Element1"> A linear space element.</param>
        /// <returns> The solution matrix as linear space element.</returns>
        LinearSpace<T> LinearSpace<T>.MultiplyScalar(T Scalar, LinearSpace<T> Element1)
        {
            RealMatrix<T> Element2 = (RealMatrix<T>)Element1;
            RealMatrix<T> Element3 = new RealMatrix<T>(Element2.RowNumber, Element2.ColNumber);
            for (ulong i = 0; i < this.RowNumber; i++)
            {
                for (ulong j = 0; j < this.ColNumber; j++)
                {
                    Element3[i, j] = (dynamic)Scalar * (dynamic)Element2[i, j];
                    //this[i, j] = (T)(Object)(this[i, j].ToDouble(System.Globalization.NumberFormatInfo.CurrentInfo) * scalar.ToDouble(System.Globalization.NumberFormatInfo.CurrentInfo));
                }
            }
            return (Element3 as LinearSpace<T>);
        }
        /// <summary>
        /// The real public scalar-multiplication operation of Linear Space. The operation is commutative so need an other overload.
        /// </summary>
        /// <param name="Scalar">A constant value from T.</param>
        /// <param name="Element1"> A linear space element.</param>
        /// <returns> The solution matrix as linear space element.</returns>
        public static RealMatrix<T> operator *(T Scalar, RealMatrix<T> Element1)
        {
            RealMatrix<T> Element2 = new RealMatrix<T>(Element1.RowNumber, Element1.ColNumber);
            Element2 = (RealMatrix<T>)(Element2 as LinearSpace<T>).MultiplyScalar(Scalar, Element1 as LinearSpace<T>);
            return  Element2;
        }
        /// <summary>
        /// The real public scalar-multiplication operation of Linear Space. The operation is commutative so need an other overload.
        /// </summary>
        /// <param name="Scalar">A constant value from T.</param>
        /// <param name="Element1"> A linear space element.</param>
        /// <returns> The solution matrix as linear space element.</returns>
        public static RealMatrix<T> operator *(RealMatrix<T> Element1, T Scalar)
        {
            RealMatrix<T> Element2 = new RealMatrix<T>(Element1.RowNumber, Element1.ColNumber);
            Element2 = (RealMatrix<T>)(Element2 as LinearSpace<T>).MultiplyScalar(Scalar, Element1 as LinearSpace<T>);
            return Element2;
        }

    }
}
