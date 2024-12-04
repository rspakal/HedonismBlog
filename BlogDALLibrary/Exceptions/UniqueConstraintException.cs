using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogDALLibrary.Exceptions
{
    public class UniqueConstraintException : Exception
    {
        public UniqueConstraintException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
