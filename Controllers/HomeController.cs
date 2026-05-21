using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using fictional_journey.Models;

namespace fictional_journey.Controllers;

public class HomeController : Controller
{
    private readonly IWebHostEnvironment _environment;

    public HomeController(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        var dataPath = Path.Combine(_environment.WebRootPath, "data", "sampledata.json");

        if (!System.IO.File.Exists(dataPath))
        {
            return View(new List<EmployeeRecord>());
        }

        var json = System.IO.File.ReadAllText(dataPath);
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        using var document = JsonDocument.Parse(json);
        var employees = new List<EmployeeRecord>();

        foreach (var item in document.RootElement.EnumerateArray())
        {
            employees.Add(new EmployeeRecord
            {
                FirstName = item.GetProperty("FirstName").GetString() ?? string.Empty,
                LastName = item.GetProperty("LastName").GetString() ?? string.Empty,
                HiringDate = item.GetProperty("HiringDate").GetString() ?? string.Empty,
                Department = item.GetProperty("Department").GetString() ?? string.Empty,
                JobTitle = item.GetProperty("Job Title").GetString() ?? string.Empty
            });
        }

        return View(employees);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
