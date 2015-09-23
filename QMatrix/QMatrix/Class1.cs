using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SergeyMS;

namespace SergeyMS
{
    //To demonstrate power of inheritance
    public class QMatrix<T> : RealMatrix<T> where T : IConvertible, IEquatable<T>
    {
        private ulong size;
        public QMatrix(ulong size) : base(size, size)
        {
            //
            this.size = size;
        }
        /*
        Create(ulong size)
        {

        }*/
    }
}
