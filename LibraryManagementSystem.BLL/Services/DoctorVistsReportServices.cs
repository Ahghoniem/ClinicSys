using LibraryManagementSystem.BLL.DTOs;
using LibraryManagementSystem.BLL.ServicesContracts;
using LibraryManagementSystem.DAL.DTO;
using LibraryManagementSystem.DAL.Entities;
using LibraryManagementSystem.DAL.UOW;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.IO;
using System.ComponentModel;
using QuestPDF.Companion;
using LibraryManagementSystem.DAL.Entities.IdentityEntities;

namespace LibraryManagementSystem.BLL.Services
{
    public class DoctorVistsReportServices : IDoctorVistsReport
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly List<string> headers = new List<string> { "Day", "Date", "Doctor Name", "Patient Name", "Shift", "Medicine", "Status" };


        public DoctorVistsReportServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<(byte[] Content, string FileName)> GetExcel(string doctorId)
        {
            var Schedule = await _unitOfWork.GetPatientRepository<Patient_schedule>().GetDoctorScheduleById(doctorId);
            var res = Schedule.Select(x => new GetAllBookAppointmentDTO
            {
                Date = x.schedule.date,
                Day = x.schedule.day,
                Shift = x.schedule.shift,
                DName = x.doctor.FullName,
                DId = x.doctor.Id,
                PId = x.patient.Id,
                PName = x.patient.FullName,
                Medicine = x.schedule.PrescriptionMedicine,
                Status = x.schedule.status
            }).ToList();

            if (!Schedule.Any())
            {
                throw new Exception("No Appointments found for the specified doctor.");
            }
            var doctorName = Schedule.FirstOrDefault()?.doctor.FullName ?? "Unknown Doctor";

            // Prepare the Excel report by passing only the Schedules to the report generation method
            var excelFile = await GenerateExcelReport(res, doctorName);

            string fileName = $"DoctorAppointments_{doctorName}.xlsx";

            return (excelFile, fileName);
        }



        private async Task<byte[]> GenerateExcelReport(List<GetAllBookAppointmentDTO> reportData, string clinicName)
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

        private void SetupDetailedWorksheet(ExcelWorksheet detailedWorksheet, List<GetAllBookAppointmentDTO> reportData, string clinicName)
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
            string reportTitle = $"Appointment Report for {clinicName}";

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
                headerRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);

                headerRange.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                headerRange.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                headerRange.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                headerRange.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            }
        }

        private void AddDataToDetailedWorksheet(ExcelWorksheet detailedWorksheet, List<GetAllBookAppointmentDTO> reportData)
        {
            int startRow = 3;
            int row = startRow;

            foreach (var doctorGroup in reportData)
            {
                // For each doctor, print their schedule

                detailedWorksheet.Cells[row, 1].Value = doctorGroup.Day;
                detailedWorksheet.Cells[row, 2].Value = doctorGroup.Date.ToString("dd-MM-yyyy");
                detailedWorksheet.Cells[row, 3].Value = doctorGroup.DName;
                detailedWorksheet.Cells[row, 4].Value = doctorGroup.PName;
                detailedWorksheet.Cells[row, 5].Value = doctorGroup.Shift;
                detailedWorksheet.Cells[row, 6].Value = doctorGroup.Medicine;
                detailedWorksheet.Cells[row, 7].Value = doctorGroup.Status;

                row++;

            }
        }

        public async Task<byte[]> GenerateSamplePdf(String doctorId)
        {
            var Schedule = await _unitOfWork.GetPatientRepository<Patient_schedule>().GetDoctorScheduleById(doctorId);
            var sch = new List<GetAllBookAppointmentDTO> { };
            var res = Schedule.Select(x => new GetAllBookAppointmentDTO
            {
                Date = x.schedule.date,
                Day = x.schedule.day,
                Shift = x.schedule.shift,
                DName = x.doctor.FullName,
                DId = x.doctor.Id,
                PId = x.patient.Id,
                PName = x.patient.FullName,
                Medicine = x.schedule.PrescriptionMedicine,
                Status = x.schedule.status
            }).ToList();



            QuestPDF.Settings.License = LicenseType.Community;
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);

                    // Header Section
                    page.Header().Row(row =>
                    {
                        row.RelativeItem().Column(column =>
                        {
                            column.Item().Text("ELITE CARE")
                                .FontFamily("Arial")
                                .FontSize(20)
                                .Bold();

                            column.Item().Text("Nasr City")
                                .FontFamily("Arial")
                                .FontSize(10);
                        });

                        row.RelativeItem()
                            .ShowOnce()
                            .Text("Hello")
                            .AlignRight()
                            .FontFamily("Arial")
                            .FontSize(20)
                            .ExtraBlack();
                    });

                    // Content Section
                    page.Content().PaddingTop(50).Column(column =>
                    {
                        column.Item().Row(row =>
                        {
                            row.RelativeItem().Column(col =>
                            {
                                col.Item().Text("Doctor Name: Dr . " + res[0].DName).Bold();

                            });
                        });

                        // Table Section
                        column.Item().PaddingTop(50).Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(40);   // #
                                columns.RelativeColumn();     // Description
                                columns.ConstantColumn(50);   // Qty
                                columns.ConstantColumn(60);   // Rate
                                columns.ConstantColumn(70);   // Total
                            });

                            // Table Header
                            table.Header(header =>
                            {
                                header.Cell().Text("#").Bold();
                                header.Cell().Text("Patient Name").Bold();
                                header.Cell().Text("day").AlignRight().Bold();
                                header.Cell().Text("shift").AlignRight().Bold();
                                header.Cell().Text("date").AlignRight().Bold();

                                header.Cell().ColumnSpan(5)
                                    .PaddingVertical(5)
                                    .BorderBottom(1)
                                    .BorderColor(Colors.Black);
                            });



                            for (int i = 0; i < res.Count(); i++)
                            {
                                var color = i % 2 == 0 ? QuestPDF.Infrastructure.Color.FromHex("#ffffff") :
                                QuestPDF.Infrastructure.Color.FromHex("#f0f0f0");

                                table.Cell().Background(color).Text("" + (+i + 1));
                                table.Cell().Background(color).Text(res[i].PName);
                                table.Cell().Background(color).Text(res[i].Day).AlignRight();
                                table.Cell().Background(color).Text("" + res[i].Shift).AlignRight();
                                table.Cell().Background(color).Text("" + DateOnly.FromDateTime(res[i].Date)).AlignRight();
                            }

                        });
                    });

                    // Footer Section
                    page.Footer().AlignCenter().Text(txt =>
                    {
                        txt.Span("Page ");
                        txt.CurrentPageNumber();
                        txt.Span(" of ");
                        txt.TotalPages();
                    });
                });
            });

            return document.GeneratePdf();
        }
    }
}
