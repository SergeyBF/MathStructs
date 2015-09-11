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
    /// This contain the criterias of mathematical axiomas for Linear Space.
    /// </remarks>
    /// </summary>
    /// <typeparam name="T"> This type param is only for real number typed variables. </typeparam>
    public interface LinearSpace<T> where T : IConvertible
    {
        LinearSpace<T> Operation(LinearSpace<T> Element1, LinearSpace<T> Element2);
        LinearSpace<T> Operation_inverse(LinearSpace<T> Element1, LinearSpace<T> Element2);
        LinearSpace<T> MultiplyScarar(T Scalar, LinearSpace<T> Element);
        LinearSpace<T> MultiplyScarar(LinearSpace<T> Element, T Scalar);
        bool Equals(LinearSpace<T> Element1, LinearSpace<T> Element2);
        int GetHashCode();
    }
}
