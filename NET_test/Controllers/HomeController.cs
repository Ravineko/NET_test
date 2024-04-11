using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CsvHelper;
using NET_test.Models;
using System.Diagnostics;
using System.Formats.Asn1;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;
using CsvHelper.Configuration;

namespace NET_test.Controllers
{

    public class HomeController : Controller
    {
        private readonly PersonDbContext _context;
                    
        public HomeController(PersonDbContext context)
        {
            _context = context;
        }

        // GET: Home
        public async Task<IActionResult> Index()
        {
            var people = await _context.People.ToListAsync();
            return View(people);
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("File is empty");
            }
            var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                MissingFieldFound = null,
            };
            using (var streamReader = new StreamReader(file.OpenReadStream()))
            using (var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture))
            {
                var records = new List<Person>();
                csvReader.Read();
                csvReader.ReadHeader();
                while (csvReader.Read())
                {
                    try
                    {
                        var record = new Person
                        {
                            /* Id = csvReader.GetField<int>("Id"),*/
                            Name = csvReader.GetField("Name"),
                            DateOfBirth = csvReader.GetField<DateTime>("Date_of_Birth"),
                            Married = csvReader.GetField<bool>("Married"),
                            Phone = csvReader.GetField("Phone"),
                            Salary = csvReader.GetField<decimal>("Salary")

                        };
                        records.Add(record);
                    }
                    catch (CsvHelperException ex)
                    {
                        return BadRequest($"Error parsing CSV file: {ex.Message}");
                    }

                }

                if (records.Any())
                {
                    await _context.People.AddRangeAsync(records);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    return BadRequest("No records found in CSV file");
                }
            }

            return RedirectToAction(nameof(Index));
        }
        
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var person = await _context.People.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }

            _context.People.Remove(person);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("Edit")]
        public async Task<IActionResult> Edit([FromBody] Person person)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Entry(person).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
