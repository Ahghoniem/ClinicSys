using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.BLL.ServicesContracts
{
    public interface IDoctorVistsReport
    {
        Task<(byte[] Content, string FileName)> GetExcel(string doctorId);
        public Task<byte[]> GenerateSamplePdf(String doctorId);


    }
}
