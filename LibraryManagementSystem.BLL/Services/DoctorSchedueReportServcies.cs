//using LibraryManagementSystem.BLL.ServicesContracts;
//using LibraryManagementSystem.DAL.DTO;
//using LibraryManagementSystem.DAL.Entities;
//using LibraryManagementSystem.DAL.Repositories;
//using LibraryManagementSystem.DAL.RepositoriesContracts;
//using LibraryManagementSystem.DAL.UOW;
//using OfficeOpenXml;
//using System.Drawing;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace LibraryManagementSystem.BLL.Services
//{
//    public class DoctorSchedueReportServcies : IDoctorSchedueReportServcies
//    {
//        private readonly IUnitOfWork _unitOfWork;
//        private readonly IDepartmentRepository<Department> _department;
//        private readonly List<string> headers = new List<string> { "Day", "Date", "Doctor Name", "Department Name", "Shift" };

//        public DoctorSchedueReportServcies(IUnitOfWork unitOfWork, IDepartmentRepository<Department> department)
//        {
//            _unitOfWork = unitOfWork;
//            _department = department;
//        }

//        public async Task<(byte[] Content, string FileName)> GetExcel()
//        {
//            var clinicName = "All Doctors"; // Default clinic name for all doctors

//            // Get all the schedules from the repository
//            var allSchedules = await _unitOfWork.GetClinicRepository<Clinic_schedule>().GetAllSchedule();

//            // Group the schedules by doctor (DId)
//            var groupedByDoctor = allSchedules
//                .GroupBy(x => x.user.Id)  // Group by doctor ID (DId)
//                .Select(group => new
//                {
//                    DoctorName = group.FirstOrDefault()?.user.FullName,
//                    Schedules = group.Select(x => new ScheduleDTO
//                    {
//                        date = x.schedule.date,
//                        day = x.schedule.day,
//                        shift = x.schedule.shift,
//                        DoctorName = x.user.FullName,
//                        DId = x.user.Id,
//                        DepId = x.schedule.DepID,
//                        DepartmentName = x.Department.DepName
//                    }).ToList()
//                });

//            // Prepare the Excel report
//            var excelFile = await GenerateExcelReport(groupedByDoctor, clinicName);

//            string fileName = $"Schedule Report {clinicName}.xlsx";

//            return (excelFile, fileName);
//        }

//        private async Task<byte[]> GenerateExcelReport(IEnumerable<dynamic> reportData, string clinicName)
//        {
//            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

//            using (var package = new ExcelPackage())
//            {
//                ExcelWorksheet detailedWorksheet = package.Workbook.Worksheets.Add("Detailed");

//                // Setup worksheet headers and title
//                SetupDetailedWorksheet(detailedWorksheet, reportData, clinicName);

//                // Add data to worksheet
//                AddDataToDetailedWorksheet(detailedWorksheet, reportData);

//                // Auto-fit columns
//                detailedWorksheet.Cells.AutoFitColumns();

//                return await package.GetAsByteArrayAsync();
//            }
//        }

//        private void SetupDetailedWorksheet(ExcelWorksheet detailedWorksheet, IEnumerable<dynamic> reportData, string clinicName)
//        {
//            // Define headers
//            var allHeaders = headers;

//            // Set report title
//            SetReportTitle(detailedWorksheet, clinicName, allHeaders.Count);

//            // Add headers to the worksheet
//            AddWorksheetHeaders(detailedWorksheet, allHeaders);

//            // Set font for Arabic compatibility
//            detailedWorksheet.Cells.Style.Font.Name = "Arial";
//        }

//        private void SetReportTitle(ExcelWorksheet worksheet, string clinicName, int headerCount)
//        {
//            string reportTitle = $"Schedule Report for {clinicName}";

//            worksheet.Cells[1, 1].Value = reportTitle;
//            worksheet.Cells[1, 1, 1, headerCount].Merge = true;
//            worksheet.Cells[1, 1].Style.Font.Bold = true;
//            worksheet.Cells[1, 1].Style.Font.Size = 20;
//            worksheet.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
//            worksheet.Cells[1, 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
//            worksheet.Row(1).Height = 100;
//        }

//        private void AddWorksheetHeaders(ExcelWorksheet worksheet, List<string> allHeaders)
//        {
//            // Add headers dynamically
//            for (int col = 0; col < allHeaders.Count; col++)
//            {
//                worksheet.Cells[2, col + 1].Value = allHeaders[col];
//            }

//            using (var headerRange = worksheet.Cells[2, 1, 2, allHeaders.Count])
//            {
//                headerRange.Style.Font.Bold = true;
//                headerRange.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

//                headerRange.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
//                headerRange.Style.Fill.BackgroundColor.SetColor(Color.LightGray);

//                headerRange.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
//                headerRange.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
//                headerRange.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
//                headerRange.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
//            }
//        }

//        private void AddDataToDetailedWorksheet(ExcelWorksheet detailedWorksheet, IEnumerable<dynamic> reportData)
//        {
//            int startRow = 3;
//            int row = startRow;

//            foreach (var doctorGroup in reportData)
//            {
//                // For each doctor, print their schedule
//                foreach (var item in doctorGroup.Schedules)
//                {
//                    detailedWorksheet.Cells[row, 1].Value = item.day;
//                    detailedWorksheet.Cells[row, 2].Value = item.date.ToString("dd-MM-yyyy");
//                    detailedWorksheet.Cells[row, 3].Value = item.DoctorName;
//                    detailedWorksheet.Cells[row, 4].Value = item.DepartmentName;
//                    detailedWorksheet.Cells[row, 5].Value = item.shift;

//                    row++;
//                }
//            }
//        }
//    }
//}


using LibraryManagementSystem.BLL.ServicesContracts;
using LibraryManagementSystem.DAL.DTO;
using LibraryManagementSystem.DAL.Entities;
using LibraryManagementSystem.DAL.Repositories;
using LibraryManagementSystem.DAL.RepositoriesContracts;
using LibraryManagementSystem.DAL.UOW;
using OfficeOpenXml;
using System.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using LibraryManagementSystem.BLL.DTOs;

namespace LibraryManagementSystem.BLL.Services
{
    public class DoctorSchedueReportServcies : IDoctorSchedueReportServcies
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDepartmentRepository<Department> _department;
        private readonly List<string> headers = new List<string> { "Day", "Date", "Doctor Name", "Department Name", "Shift" };

        public DoctorSchedueReportServcies(IUnitOfWork unitOfWork, IDepartmentRepository<Department> department)
        {
            _unitOfWork = unitOfWork;
            _department = department;
        }

        public async Task<(byte[] Content, string FileName)> GetExcel(string doctorId)
        {
            // Get all schedules
            var allSchedules = await _unitOfWork.GetClinicRepository<Clinic_schedule>().GetAllSchedule();

            // Filter the schedules by the doctorId (which is now a string)
            var doctorSchedules = allSchedules
                .Where(x => x.user.Id == doctorId)  // Filter by doctorId as a string
                .Select(x => new ScheduleDTO
                {
                    date = x.schedule.date,
                    day = x.schedule.day,
                    shift = x.schedule.shift,
                    DoctorName = x.user.FullName,
                    DId = x.user.Id,
                    DepId = x.schedule.DepID,
                    DepartmentName = x.Department.DepName
                }).ToList();

            if (!doctorSchedules.Any())
            {
                throw new Exception("No schedules found for the specified doctor.");
            }

            var doctorName = doctorSchedules.FirstOrDefault()?.DoctorName ?? "Unknown Doctor";

            // Prepare the Excel report by passing only the Schedules to the report generation method
            var excelFile = await GenerateExcelReport(doctorSchedules, doctorName);

            string fileName = $"DoctorSchedule_{doctorName}.xlsx";

            return (excelFile, fileName);
        }


        private async Task<byte[]> GenerateExcelReport(List<ScheduleDTO> reportData, string clinicName)
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                ExcelWorksheet detailedWorksheet = package.Workbook.Worksheets.Add("Detailed");

                // Setup worksheet headers and title
                SetupDetailedWorksheet(detailedWorksheet, reportData, clinicName);

                // Add data to worksheet
                AddDataToDetailedWorksheet(detailedWorksheet, reportData);

                // Auto-fit columns
                detailedWorksheet.Cells.AutoFitColumns();

                return await package.GetAsByteArrayAsync();
            }
        }

        private void SetupDetailedWorksheet(ExcelWorksheet detailedWorksheet, List<ScheduleDTO> reportData, string clinicName)
        {
            // Define headers
            var allHeaders = headers;

            // Set report title
            SetReportTitle(detailedWorksheet, clinicName, allHeaders.Count);

            // Add headers to the worksheet
            AddWorksheetHeaders(detailedWorksheet, allHeaders);

            // Set font for Arabic compatibility
            detailedWorksheet.Cells.Style.Font.Name = "Arial";
        }

        private void SetReportTitle(ExcelWorksheet worksheet, string clinicName, int headerCount)
        {
            string reportTitle = $"Schedule Report for {clinicName}";

            worksheet.Cells[1, 1].Value = reportTitle;
            worksheet.Cells[1, 1, 1, headerCount].Merge = true;
            worksheet.Cells[1, 1].Style.Font.Bold = true;
            worksheet.Cells[1, 1].Style.Font.Size = 20;
            worksheet.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            worksheet.Cells[1, 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            worksheet.Row(1).Height = 100;
        }

        private void AddWorksheetHeaders(ExcelWorksheet worksheet, List<string> allHeaders)
        {
            // Add headers dynamically
            for (int col = 0; col < allHeaders.Count; col++)
            {
                worksheet.Cells[2, col + 1].Value = allHeaders[col];
            }

            using (var headerRange = worksheet.Cells[2, 1, 2, allHeaders.Count])
            {
                headerRange.Style.Font.Bold = true;
                headerRange.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                headerRange.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                headerRange.Style.Fill.BackgroundColor.SetColor(Color.LightGray);

                headerRange.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                headerRange.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                headerRange.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                headerRange.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            }
        }

        private void AddDataToDetailedWorksheet(ExcelWorksheet detailedWorksheet, List<ScheduleDTO> reportData)
        {
            int startRow = 3;
            int row = startRow;

            foreach (var doctorGroup in reportData)
            {
                // For each doctor, print their schedule

                detailedWorksheet.Cells[row, 1].Value = doctorGroup.day;
                detailedWorksheet.Cells[row, 2].Value = doctorGroup.date.ToString("dd-MM-yyyy");
                detailedWorksheet.Cells[row, 3].Value = doctorGroup.DoctorName;
                detailedWorksheet.Cells[row, 4].Value = doctorGroup.DepartmentName;
                detailedWorksheet.Cells[row, 5].Value = doctorGroup.shift;

                row++;

            }
        }
    }
}
