using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Helpers.Exceptions
{
    public class InvalidCommentException : Exception
    {
        public InvalidCommentException(string message) : base(message)
        {
        }
    }
}
