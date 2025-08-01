using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.BLL.DTOs
{
    public class AddScheduleDTO
    {
        public String DId { get; set; }
       
        public string[] day { get; set; }
        public int[] shift { get; set; }
        
    }
}
