using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.BLL.DTOs
{
    public class GetAllBookAppointmentDTO
    {
        public int Id { get; set; }
        public int SId { get; set; }

        public String PId { get; set; }

        public String PName{ get; set; }

        public String DId { get; set; }

        public String DName { get; set; }

        public int Shift { get; set; }

        public String Day { get; set; }

        public DateTime Date { get; set; }

        public String Medicine { get; set; }

        public String Status { get; set; }
    }
}
