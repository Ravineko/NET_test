using CsvHelper.Configuration;
using CsvHelper;
using NET_test.Data;
using NET_test.Models;
using System.Globalization;

namespace NET_test.Services
{
    public class FileUploadService : IFileUploadService
    {
        private readonly PersonDbContext _context;

        public FileUploadService(PersonDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ProcessCSVFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return false;
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
                        // Обробка помилок
                        return false;
                    }
                }

                if (records.Any())
                {
                    await _context.People.AddRangeAsync(records);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    return false;
                }
            }

            return true;
        }
    }

}
