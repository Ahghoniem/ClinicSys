using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.BLL.ServicesContracts
{
    public interface IDoctorSchedueReportServcies
    {
        Task<(byte[] Content, string FileName)> GetExcel(string? doctorId);

    }
}
