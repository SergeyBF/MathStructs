using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SergeyMS
{
    /// <summary>
    /// Linear Space interface
    /// <remarks>
    /// This contain the criterias of mathematical axiomas for Linear Space. The structure will work only mathematical types.
    /// </remarks>
    /// </summary>
    /// <typeparam name="T"> This type param is only for real number typed variables. </typeparam>
    public interface LinearSpace<T> where T : IConvertible, IEquatable<T>
    {
        LinearSpace<T> Operation(LinearSpace<T> Element1, LinearSpace<T> Element2);
        LinearSpace<T> OperationInverse(LinearSpace<T> Element1, LinearSpace<T> Element2);
        LinearSpace<T> MultiplyScalar(T Scalar, LinearSpace<T> Element);
        bool Equals(object obj);
        int GetHashCode();
    }
}
