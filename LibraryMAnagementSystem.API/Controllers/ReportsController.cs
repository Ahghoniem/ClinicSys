using LibraryManagementSystem.BLL.ServicesContracts;
using LibraryMAnagementSystem.API.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace LibraryMAnagementSystem.API.Controllers
{
    public class ReportsController :  BaseAPIController   
    {
        private readonly IDoctorSchedueReportServcies doctorSchedueReportServcies;
        private readonly IDoctorVistsReport doctorVistsReport;

        public ReportsController(IDoctorSchedueReportServcies doctorSchedueReportServcies,IDoctorVistsReport doctorVistsReport)
        {
            this.doctorSchedueReportServcies = doctorSchedueReportServcies;
            this.doctorVistsReport = doctorVistsReport;
        }

        [HttpGet("DoctorSceduduleReport")]
        public async Task<IActionResult> DoctorSchedule([FromQuery] string docId)
        {
            try
            {
                var (content, fileName) = await doctorSchedueReportServcies.GetExcel(docId);

                return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);

                //return StatusCode(200, new
                //{
                //    Message = "تم تصدير كشف المراقية",
                //    StatusCode = 200,
                //    File = File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName)
                //});
            }
            catch (FormatException ex)
            {
                return StatusCode(400, new
                {
                    Message = "Invalid format.",
                    Error = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An error occurred while generating the report.",
                    Error = ex.Message
                });
            }
        }

        [HttpGet("DoctorVisitReports")]
        public async Task<IActionResult> DoctorVists([FromQuery] string docId)
        {
            try
            {
                var (content, fileName) = await doctorVistsReport.GetExcel(docId);

                return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);

                //return StatusCode(200, new
                //{
                //    Message = "تم تصدير كشف المراقية",
                //    StatusCode = 200,
                //    File = File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName)
                //});
            }
            catch (FormatException ex)
            {
                return StatusCode(400, new
                {
                    Message = "Invalid format.",
                    Error = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An error occurred while generating the report.",
                    Error = ex.Message
                });
            }
        }

        [HttpGet("generate{Id}")]
        public async Task<IActionResult> GeneratePdf([FromRoute] string Id)
        {
            var pdfBytes = await doctorVistsReport.GenerateSamplePdf(Id);
            return File(pdfBytes, "application/pdf", "sample.pdf");
        }
    }
}
