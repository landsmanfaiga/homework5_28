using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using homework5_28.Data;
using Faker;
using System.Formats.Asn1;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using homework5_28.Web.Models;
using System.Text;

namespace homework5_28.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly string _connectionString;
        public PersonController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
        }

        [HttpGet]
        [Route("getall")]
        public List<Person> GetAll()
        {
            var repo = new PersonRepo(_connectionString);
            return repo.GetAll();
        }

        [HttpPost]
        [Route("deleteall")]
        public void DeleteAll()
        {
            var repo = new PersonRepo(_connectionString);
            repo.DeleteAll();
        }

        
        [HttpPost("addpeople")]
        public void AddPeople(UploadViewModel vm)
        {
            var repo = new PersonRepo(_connectionString);
            int indexOfComma = vm.Base64.IndexOf(',');
            string base64 = vm.Base64.Substring(indexOfComma + 1);
            byte[] csvBytes = Convert.FromBase64String(base64);
            List<Person> people = GetFromCsvBytes(csvBytes);
            repo.UploadPeople(people);
        }

        [HttpGet]
        [Route("generatepeople")]
        public IActionResult GeneratePeople(int amount)
        {
            List<Person> people = GenerateList(amount);
            var writer = GenerateCsv(people);
            byte[] csvBytes = Encoding.UTF8.GetBytes(writer.ToString());
            return File(csvBytes, "text/csv", "people.csv");
        }
        private static List<Person> GenerateList(int amount)
        {
            return Enumerable.Range(1, amount).Select(_ => new Person
            {
                FirstName = Name.First(),
                LastName = Name.Last(),
                Age = RandomNumber.Next(18, 80),
                Address = Address.StreetAddress(),
                Email = Internet.Email()
            }).ToList(); ;
        }
        private static string GenerateCsv(List<Person> people)
        {
            var writer = new StringWriter();
            var csvWriter = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture));
            csvWriter.WriteRecords(people);

            return writer.ToString();
        }

        [HttpGet("getGeneratedPeople")]
        public IActionResult GetGeneratedPeople()
        {
            return File(System.IO.File.ReadAllBytes("GeneratedPeople/people"), "application/octet-stream", "people.csv");
        }
        static List<Person> GetFromCsvBytes(byte[] csvBytes)
        {
            using var memoryStream = new MemoryStream(csvBytes);
            using var reader = new StreamReader(memoryStream);
            using var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
            return csvReader.GetRecords<Person>().ToList();
        }
    }
}
