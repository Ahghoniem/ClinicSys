using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.BLL.Exceptions
{
    public class NotFoundExceptions : ApplicationException
    {

        public NotFoundExceptions(string name, object key) : base($"{name} with ({key}) is not found")
        {

        }
    }
}
