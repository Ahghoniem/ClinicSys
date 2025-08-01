using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.BLL.Exceptions
{
    public class UnAuthorizedExceptionv : ApplicationException
    {
        public UnAuthorizedExceptionv(string? message) : base(message) { }
    }
}
