using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CsvHelper;
using NET_test.Models;
using System.Diagnostics;
using System.Formats.Asn1;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;
using CsvHelper.Configuration;
using NET_test.Data;
using NET_test.Repository.IRepository;
using NET_test.Services;

namespace NET_test.Controllers
{

    public class HomeController : Controller
    {
        private readonly IPersonRepository _dbperson;
        private readonly IFileUploadService _fileUploadService;
                    
        public HomeController(IPersonRepository dbperson, IFileUploadService fileUploadService)
        {
            _dbperson = dbperson;
            _fileUploadService = fileUploadService;

        }

        public async Task<IActionResult> Index()
        {
            var people = await _dbperson.GetAllAsync();
            return View(people);
        }
        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            var result = await _fileUploadService.ProcessCSVFileAsync(file);
            if (!result)
            {
                return BadRequest("Error processing CSV file");
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var person = await _dbperson.GetAsync(u => u.Id == id);
            if (person == null)
            {
                return NotFound();
            }

            _dbperson.DeleteAsync(person);
            await _dbperson.SaveAsync();
            return NoContent();
        }

        [HttpPut("Edit")]
        public async Task<IActionResult> Edit([FromBody] Person person)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _dbperson.EditAsync(person);
            await _dbperson.SaveAsync();
            return NoContent();
        }
    }
}
