using System.Drawing;
using System.Text.Json;
namespace Pie_Chart
{
    internal class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            try
            {
                // Fetch JSON data from the REST endpoint
                string json = await FetchJsonData();

                // Parse the JSON data
                var employees = ParseJsonData(json);


                // Generate the pie chart
                GeneratePieChart(getUniqueEmployees(employees));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        static async System.Threading.Tasks.Task<string> FetchJsonData()
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync("https://rc-vault-fap-live-1.azurewebsites.net/api/gettimeentries?code=vO17RnE8vuzXzPJo5eaLLjXjmRW07law99QTD90zat9FfOQJKKUcgQ==");
                response.EnsureSuccessStatusCode();
                string json = await response.Content.ReadAsStringAsync();
                return json;
            }
        }

        static List<Employee> ParseJsonData(string json)
        {
            var employees = new List<Employee>();
            JsonDocument document = JsonDocument.Parse(json);

            // Assuming the JSON data is an array of employee objects
            foreach (JsonElement element in document.RootElement.EnumerateArray())
            {
                string name = element.GetProperty("EmployeeName").GetString();
                string id = element.GetProperty("Id").GetString();
                string startTimeUtcString = element.GetProperty("StarTimeUtc").GetString();
                string endTimeUtcString = element.GetProperty("EndTimeUtc").GetString();

                // Parse the DateTime values
                DateTime startTimeUtc = DateTime.Parse(startTimeUtcString);
                DateTime endTimeUtc = DateTime.Parse(endTimeUtcString);

                employees.Add(new Employee(name, id, startTimeUtc, endTimeUtc));

            }

            return employees;
        }

        static void GeneratePieChart(Dictionary<string, int> employees)
        {
            using (Bitmap image = new Bitmap(800, 600))
            {
                using (Graphics graphics = Graphics.FromImage(image))
                {
                    // Clear the image with a white background
                    graphics.Clear(Color.White);

                    // Calculate the total time worked by all employees
                    int totalWorkedTime = 0;
                    foreach (var employee in employees)
                    {
                        totalWorkedTime += employee.Value;

                    }

                    // Draw the pie chart
                    int startAngle = 0;
                    foreach (var employee in employees)
                    {
                        float percentage = (float)employee.Value / totalWorkedTime;

                        int sweepAngle = (int)(percentage * 360);

                        // Set a random color for each employee
                        Color color = Color.FromArgb(new Random().Next(256), new Random().Next(256), new Random().Next(256));

                        // Draw the pie slice
                        graphics.FillPie(new SolidBrush(color), new Rectangle(200, 100, 400, 400), startAngle, sweepAngle);

                        // Update the start angle for the next slice
                        startAngle += sweepAngle;
                    }

                    // Save the image as a PNG file
                    image.Save("pie_chart.png");
                }
            }

            Console.WriteLine("Pie chart generated successfully!");
        }

        static public Dictionary<string, int> getUniqueEmployees(List<Employee> employees)
        {
            Dictionary<string, int> totalTimeWorked = new Dictionary<string, int>();
            foreach (var employee in employees)
            {
                if (totalTimeWorked.ContainsKey(employee.Name ?? "No name"))
                {
                    // Employee already exists in the dictionary, update the total time worked
                    totalTimeWorked[employee.Name ?? "No name"] += employee.getDurationPerDay();
                }
                else
                {
                    // New employee, add to the dictionary
                    totalTimeWorked.Add(employee.Name ?? "No name", employee.getDurationPerDay());
                }
            }
            return totalTimeWorked;
        }
    }




    class Employee
    {
        public string Id { get; set; }
        public string EmployeeName { get; set; }
        public DateTime StarTimeUtc { get; set; }
        public DateTime EndTimeUtc { get; set; }
        public string EntryNotes { get; set; }
        public string Name { get; set; }
        public int TotalTimeWorked { get; set; }

        public Employee(string name, string id, DateTime starTimeUtc, DateTime endTimeUtc)
        {
            Name = name;
            Id = id;
            StarTimeUtc = starTimeUtc;
            EndTimeUtc = endTimeUtc;
        }
        public int getDurationPerDay()
        {
            TimeSpan duration = EndTimeUtc - StarTimeUtc;
            return (int)duration.TotalHours;
        }
    }


}

