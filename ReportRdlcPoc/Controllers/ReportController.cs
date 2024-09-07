using AspNetCore.Reporting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReportRdlcPoc.Service;
using System.Net.Mime;

namespace ReportRdlcPoc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {

        private IReportService _reportService;
        private IWebHostEnvironment Environment;

        public ReportController(IReportService reportService, IWebHostEnvironment _environment)
        {
            _reportService = reportService;
            this.Environment = _environment;
        }
        [HttpGet("{reportName}/{reportType}")]
        public ActionResult Get(string reportName, string reportType)
        {
            var reportNameWithLang = reportName;
            var reportFileByteString = _reportService.GenerateReportAsync(reportNameWithLang, reportType);
            return File(reportFileByteString, MediaTypeNames.Application.Octet, getReportName(reportNameWithLang, reportType));
        }

        [HttpGet("print")]
        public IActionResult Print()
        {
            string mimetype = "";
            int extension = 1;
            var path = $"{this.Environment.ContentRootPath}\\ReportFiles\\Report1.rdlc";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            //parameters.Add("ReportParameter1", "RDLC in Blazor Web Application.");
            LocalReport localReport = new LocalReport(path);
            var result = localReport.Execute(RenderType.Pdf, extension, parameters, mimetype);
            return File(result.MainStream, "application/pdf");
        }


        private string getReportName(string reportName, string reportType)
        {
            var outputFileName = reportName + ".pdf";

            switch (reportType.ToUpper())
            {
                default:
                case "PDF":
                    outputFileName = reportName + ".pdf";
                    break;
                case "XLS":
                    outputFileName = reportName + ".xls";
                    break;
                case "WORD":
                    outputFileName = reportName + ".doc";
                    break;
            }

            return outputFileName;
        }
    }
}
