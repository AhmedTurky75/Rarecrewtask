using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace HTML_Page
{




    class Employee
    {
        public string Id { get; set; }
        public string EmployeeName { get; set; }
        public string StarTimeUtc { get; set; }
        public string EndTimeUtc { get; set; }
        public string EntryNotes { get; set; }
        public string Name { get; set; }
        public int TotalTimeWorked { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            {
                // Download the Bootstrap CSS file
                string bootstrapCssUrl = "https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css";
                string bootstrapCssFilePath = "bootstrap.min.css";
                DownloadFile(bootstrapCssUrl, bootstrapCssFilePath);

                // Make the GET request to the API endpoint
                string apiUrl = "https://rc-vault-fap-live-1.azurewebsites.net/api/gettimeentries?code=vO17RnE8vuzXzPJo5eaLLjXjmRW07law99QTD90zat9FfOQJKKUcgQ==";
                string json = MakeGetRequest(apiUrl);

                // Deserialize the JSON response into a list of Employee objects
                List<Employee> employees = JsonConvert.DeserializeObject<List<Employee>>(json);

                // Group employees by name and calculate total time worked for each employee
                var groupedEmployees = employees.GroupBy(emp => emp.EmployeeName)
                                                .Select(group => new Employee
                                                {
                                                    EmployeeName = group.Key,
                                                    TotalTimeWorked = group.Sum(emp => CalculateTimeDifference(emp))
                                                })
                                                .ToList();

                // Sort the employees by total time worked in descending order
                groupedEmployees.Sort((emp1, emp2) => emp2.TotalTimeWorked.CompareTo(emp1.TotalTimeWorked));

                // Generate the HTML table with Bootstrap classes
                string tableHtml = GenerateHtmlTable(groupedEmployees);

                // Create an HTML file and write the table data
                string filePath = "employee_table.html";
                File.WriteAllText(filePath, tableHtml);

                // Add Bootstrap CSS file link to the HTML file
                AddBootstrapCssLink(filePath, bootstrapCssFilePath);

                Console.WriteLine("HTML file created: " + filePath);
            }

            static void DownloadFile(string url, string filePath)
            {
                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(url, filePath);
                }
            }

            static string MakeGetRequest(string url)
            {
                using (WebClient client = new WebClient())
                {
                    return client.DownloadString(url);
                }
            }

            static int CalculateTimeDifference(Employee employee)
            {
                DateTime startTime = DateTime.Parse(employee.StarTimeUtc);
                DateTime endTime = DateTime.Parse(employee.EndTimeUtc);
                TimeSpan timeDifference = endTime - startTime;
                return (int)timeDifference.TotalHours;
            }

            static string GenerateHtmlTable(List<Employee> employees)
            {
                StringBuilder sb = new StringBuilder();

                // Open the table tag and add Bootstrap classes
                sb.AppendLine("<table class=\"table table-bordered table-striped\">");

                // Create table header with Bootstrap classes
                sb.AppendLine("<tr class=\"thead-dark\"><th>Name</th><th>Total Time Worked</th></tr>");

                // Create table rows with Bootstrap classes
                foreach (Employee employee in employees)
                {
                    string rowColor = employee.TotalTimeWorked < 100 ? "table-danger" : "";
                    sb.AppendLine($"<tr class=\"{rowColor}\"><td>{employee.EmployeeName}</td><td>{employee.TotalTimeWorked}</td></tr>");
                }

                // Close the table tag
                sb.AppendLine("</table>");

                return sb.ToString();
            }

            static void AddBootstrapCssLink(string htmlFilePath, string cssFilePath)
            {
                string htmlContent = File.ReadAllText(htmlFilePath);

                string cssLinkTag = $"<link rel=\"stylesheet\" href=\"{cssFilePath}\">";
                int headIndex = htmlContent.IndexOf("<head>");

                if (headIndex != -1)
                {
                    int insertIndex = headIndex + "<head>".Length;
                    htmlContent = htmlContent.Insert(insertIndex, cssLinkTag);
                }
                else
                {
                    int bodyIndex = htmlContent.IndexOf("<body>");
                    if (bodyIndex != -1)
                    {
                        htmlContent = htmlContent.Insert(bodyIndex, $"<head>{cssLinkTag}</head>");
                    }
                    else
                    {
                        htmlContent = $"<head>{cssLinkTag}</head>{htmlContent}";
                    }
                }

                File.WriteAllText(htmlFilePath, htmlContent);
            }
        }
    }
}