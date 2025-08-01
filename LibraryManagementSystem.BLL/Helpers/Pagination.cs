using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.BLL.Helpers
{
    public class Pagination<T>
    {


        public int PageIndex { get; set; }
        public int Count { get; set; }
        public int PageSize { get; set; }

        public IEnumerable<T> Data { get; set; }

        public Pagination(int pageIndex, int pageSize, IEnumerable<T> data, int count)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            Data = data;
            Count = count;
        }
    }
}
