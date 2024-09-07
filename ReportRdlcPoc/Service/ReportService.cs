using AspNetCore.Reporting;
using System.Reflection;
using System.Text;

namespace ReportRdlcPoc.Service
{
    public interface IReportService
    {
        byte[] GenerateReportAsync(string reportName, string reportType);
    }
  
        

        public class ReportService : IReportService
        {

            public byte[] GenerateReportAsync(string reportName, string reportType)
            {

                string rootPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                string fileDirectory = Assembly.GetExecutingAssembly().Location.Replace("ReportRdlcPoc.dll", string.Empty);
                string rdlcFilePath = string.Format("{0}ReportFiles\\{1}.rdlc", fileDirectory, reportName);

         

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                Encoding.GetEncoding("utf-8");


                if (!File.Exists(rdlcFilePath))
                {
                    throw new FileNotFoundException($"The report file was not found at path: {rdlcFilePath}");
                }

                // Register the necessary encoding provider
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                Encoding.GetEncoding("utf-8");

                LocalReport report = new LocalReport(rdlcFilePath);

                // Create parameter values
                Dictionary<string, string> parameters = new Dictionary<string, string>();

                parameters.Add("ReportParameter1", "BURO Bangladesh");


                //List<UserDto> userList = new List<UserDto>();



                //var user1 = new UserDto { FirstName = "jp", LastName = "jan", Email = "jp@gm.com", Phone = "+976666661111" };
                //var user2 = new UserDto { FirstName = "jp2", LastName = "jan", Email = "jp2@gm.com", Phone = "+976666661111" };
                //var user3 = new UserDto { FirstName = "Test", LastName = "Test", Email = "jp3@gm.com", Phone = "+976666661111" };
                //var user4 = new UserDto { FirstName = "Test", LastName = "Test", Email = "jp4@gm.com", Phone = "+976666661111" };
                //var user5 = new UserDto { FirstName = "jp5", LastName = "jan", Email = "jp5@gm.com", Phone = "+976666661111" };

                //userList.Add(user1);
                //userList.Add(user2);
                //userList.Add(user3);
                //userList.Add(user4);
                //userList.Add(user5);

                //report.AddDataSource("DataSet1", userList);


                var result = report.Execute(GetRenderType(reportType), 1, parameters);


                return result.MainStream;
            }

            private RenderType GetRenderType(string reportType)
            {
                return reportType.ToUpper() switch
                {
                    "XLS" => RenderType.Excel,
                    "WORD" => RenderType.Word,
                    _ => RenderType.Pdf
                };
            }
        }
    
}
