using CsvHelper.Configuration;
using CsvHelper;
using NET_test.Models;
using System.Globalization;
using NET_test.Repository.IRepository;

namespace NET_test.Services
{
    public class FileUploadService : IFileUploadService
    {
        private readonly IPersonRepository _personRepository;

        public FileUploadService(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public async Task<bool> ProcessCSVFileAsync(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return false;
                }

                var records = await ParseCSVFileAsync(file);
                if (records == null || !records.Any())
                {
                    return false;
                }

                await _personRepository.AddRangeAsync(records);
                await _personRepository.SaveAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                // Обробка помилок
                return false;
            }
        }

        private async Task<List<Person>?> ParseCSVFileAsync(IFormFile file)
        {
            var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                MissingFieldFound = null,
            };

            using (var streamReader = new StreamReader(file.OpenReadStream()))
            using (var csvReader = new CsvReader(streamReader, csvConfig))
            {
                var records = new List<Person>();
                await csvReader.ReadAsync();
                csvReader.ReadHeader();
                while (await csvReader.ReadAsync())
                {
                    try
                    {
                        var record = new Person
                        {
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
                        Console.WriteLine($"{ex.Message}");
                        return null;
                    }
                }
                return records;
            }
        }
    }


}
