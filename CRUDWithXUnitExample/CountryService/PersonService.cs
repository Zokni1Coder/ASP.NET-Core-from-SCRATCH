using Azure.Identity;
using CsvHelper;
using CsvHelper.Configuration;
using Entities;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using ServiceContract;
using ServiceContract.DTOs;
using ServiceContract.Enums;
using Services.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class PersonService : IPersonService
    {
        //private readonly List<Person>? _persons;
        private readonly ICountryService _countryService;
        private readonly PersonsDbContext _personsDbContext;

        public PersonService(ICountryService countryService, PersonsDbContext personsDbContext)
        {
            this._countryService = countryService;
            this._personsDbContext = personsDbContext;
        }

        //public PersonService(bool initialization = true)
        //{
        //    this._persons = new List<Person>();
        //    this._countryService = new CountryService();

        //    if (initialization)
        //    {
        //        this._persons.AddRange(new List<Person>()
        //        {
        //            new Person()
        //            {
        //                PersonID = Guid.Parse("7D71DA30-4D11-4895-8DE4-EB6C644B1BF0"), PersonName = "Pate", Email = "pdown0@craigslist.org", DateOfBirth = DateTime.Parse("2010-12-22"), Gender = "Male", ReceiveNewsLetters = false, CountryID = Guid.Parse("11C64D36-EC2D-4ADE-99F6-469F98E380CF"), Address = "Apt 1247"
        //            },
        //            new Person()
        //            {
        //                PersonID = Guid.Parse("AD5EE04E-87E2-4DAB-B967-3C25152877DA"), PersonName = "Delphine", Email = "dilymanov1@live.com", DateOfBirth = DateTime.Parse("2009-5-9"), Gender = "Female", ReceiveNewsLetters = true, CountryID = Guid.Parse("456B9BAD-40EA-4A17-85B3-87C2E5555A26"),Address="Apt 177"
        //            },
        //            new Person()
        //            {
        //                PersonID = Guid.Parse("B5C12E51-C168-44E9-87C3-3DB7574DE928"), PersonName = "Sharron", Email = "spiscopiello3@zimbio.com", DateOfBirth = DateTime.Parse("1994-7-24"), Gender = "Female", ReceiveNewsLetters = true, CountryID = Guid.Parse("B4871C6C-6BB8-4CCF-AA16-CF846D036EDF"),Address="Suite 80"
        //            },
        //            new Person()
        //            {
        //                PersonID = Guid.Parse("680AAC5E-AAB0-4603-B836-A0209C3B6D17"), PersonName = "Duffie", Email = "dloades4@house.gov", DateOfBirth = DateTime.Parse("1994-7-24"), Gender = "Female", ReceiveNewsLetters = true, CountryID = Guid.Parse("7ED74F84-21D9-4A9A-A5F2-4390DFD0F40F"), Address="Apt 1503"
        //            },
        //            new Person()
        //            {
        //                PersonID = Guid.Parse("F14082CC-6819-4344-AC77-F17CB3263316"), PersonName = "Buffie", Email = "dcores4@story.rod", DateOfBirth = DateTime.Parse("1993-1-31"), Gender = "Female", ReceiveNewsLetters = true, CountryID = Guid.Parse("C9CCFE13-E61B-485B-ABCB-B953297C6993"),Address="8th Floor"
        //            },
        //            new Person()
        //            {
        //                PersonID = Guid.Parse("6ADA3393-F132-4422-B121-3A5ECAF7B277"), PersonName = "Corenda", Email = "cblakeborough5@cbsnews.com", DateOfBirth = DateTime.Parse("2022-3-16"), Gender = "Male", ReceiveNewsLetters = false, CountryID = Guid.Parse("5716D10D-005A-4347-B27D-F0A50D02279A"),  Address="6th Floor"
        //            }
        //        });
        //    }
        //}
        //private PersonResponse ToPersonResponseWithCountry(Person person)
        //{
        //    PersonResponse personResponse = person.ToPersonResponse();
        //    personResponse.Country = person.Country?.CountryName;

        //    return personResponse;
        //}

        public async Task<PersonResponse> AddPerson(PersonAddRequest? personRequest)
        {
            //Ha null paramétert kap, akkor ArgumentNullException
            //if (personRequest is null)
            //{
            //    throw new ArgumentNullException();
            //}
            //Ha a PersonName property null, akkor ArgumentException
            //if (personRequest.PersonName == null)
            //{
            //    throw new ArgumentException();
            //}

            //Validációs rész:
            ValidationHelper.PersonServiceValidations(personRequest);


            Person tempPerson = personRequest.ToPerson();
            tempPerson.PersonID = Guid.NewGuid();

            /*this._personsDbContext.Add(tempPerson);
           this._personsDbContext.SaveChanges();*/
            //A fenti ugyanazt teszi csak stored procedure nélkül.
            await this._personsDbContext.InsertPerson(tempPerson);

            return tempPerson.ToPersonResponse();
        }

        public async Task<List<PersonResponse>?> GetAllPersons()
        {
            List<PersonResponse>? persons = new List<PersonResponse>();
            //A foreach-ben a ToList() furán néz ki de kötelező, különben DataReader errort kapunk az EF Core miatt, mivel ToList() nélkül minden egyes Person objektumnál újabb kapcsolatot próbál nyitni a db-vel és hibába fut, mert egyszerre csak egy kapcsolat ajánlott (a multikapcsolatot is be lehet állítani, de vannak hátrányai). ToList() esetén végig egy kapcsolat lesz, mert előtte kiolvassa minden és a listát adja át a foreach-nek.

            //foreach (Person person in this._personsDbContext.Persons.ToList())
            //{
            //    persons.Add(ToPersonResponseWithCountry(person));
            //}
            //var temp = this._personsDbContext.Persons.Include("Country").ToList();

            //var tempPersons = await  this._personsDbContext.Persons.Include("Country").ToListAsync();

            persons = await this._personsDbContext.Persons.Include("Country").Select(person => person.ToPersonResponse()).ToListAsync();
            return persons;
        }
        public async Task<PersonResponse?> GetPersonById(Guid? id)
        {
            if (id is null)
            {
                throw new ArgumentNullException();
            }

            //Person? person = this._personsDbContext.Persons.FirstOrDefault(p => p.PersonID == id);
            Person? person = await  this._personsDbContext.Persons.Include("Country").FirstOrDefaultAsync(person => person.PersonID == id);

            if (person == null)
            {
                return null;
            }

            return person.ToPersonResponse();
        }

        public async Task<List<PersonResponse>> GetFilteredPerson(string searchBy, string? searchString)
        {
            List<PersonResponse>? persons_from_GetAll = await GetAllPersons();
            List<PersonResponse>? filtered_persons = new List<PersonResponse>();

            if (string.IsNullOrEmpty(searchBy) || string.IsNullOrEmpty(searchString))
                return persons_from_GetAll;

            switch (searchBy)
            {
                case nameof(PersonResponse.PersonName):
                    filtered_persons = persons_from_GetAll?.Where(person => person.PersonName.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
                    break;
                case nameof(PersonResponse.Email):
                    filtered_persons = persons_from_GetAll?.Where(person => person.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
                    break;
                case nameof(PersonResponse.DateOfBirth):
                    filtered_persons = persons_from_GetAll?.Where(person => person.DateOfBirth.Value.ToString("dd MMMM yyyy").Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
                    break;
                case nameof(PersonResponse.Gender):
                    filtered_persons = persons_from_GetAll?.Where(person => person.Gender.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
                    break;
                case nameof(PersonResponse.CountryId):
                    filtered_persons = persons_from_GetAll?.Where(person => person.CountryId.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
                    break;
                case nameof(PersonResponse.Address):
                    filtered_persons = persons_from_GetAll?.Where(person => person.Address.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
                    break;
                default:
                    break;
            }
            return filtered_persons;
        }

        public async Task<List<PersonResponse>> GetSortedPersons(List<PersonResponse> persons, string sortBy, SortingOptions sortingoption)
        {
            if (string.IsNullOrEmpty(sortBy))
            {
                return persons;
            }
            //Lekérjük a típus (objektum típus) metaadatait.
            Type objectType = typeof(PersonResponse);
            //Megkeresi az adott nevű (sortBy) property-t
            //A PropertyInfo egy propertyleíró objektum. Tartalmazza pl.: property neve, típusa, getter/setter, érték lekérése/beállítása
            PropertyInfo searchByProperty = objectType.GetProperty(sortBy)!;

            switch (sortingoption)
            {
                case SortingOptions.ASC:
                    return persons.OrderBy(person => searchByProperty.GetValue(person)).ToList();
                case SortingOptions.DESC:
                    return persons.OrderByDescending(person => searchByProperty.GetValue(person)).ToList();
                default:
                    return persons;
            }
        }

        public async Task<PersonResponse> UpdatePerson(PersonUpdateRequest? requestPerson)
        {
            //if (requestPerson is null)
            //{
            //    throw new ArgumentNullException();
            //}
            //if (requestPerson.PersonName is null)
            //{
            //    throw new ArgumentException();
            //}
            //Validation
            ValidationHelper.PersonServiceValidations(requestPerson);


            //Person? targetPerson = this._personsDbContext.Persons.FirstOrDefault(person => person.PersonID == requestPerson.PersonId);

            //Itt most használjuk a stored procedure-t is.
            int? affectedRows = await this._personsDbContext.UpdatePerson(requestPerson.ToPerson());

            Person? targetPerson = requestPerson.ToPerson();

            if (affectedRows == null)
                return null;

            //targetPerson.Gender = requestPerson?.Gender.ToString();
            //targetPerson.Address = requestPerson?.Address;
            //targetPerson.DateOfBirth = requestPerson?.DateOfBirth;
            //targetPerson.CountryID = requestPerson?.CountryId;
            //targetPerson.Email = requestPerson?.Email;
            //targetPerson.ReceiveNewsLetters = requestPerson.ReceiveNewsLetter;
            //targetPerson.PersonName = requestPerson.PersonName;

            //Módosítás esetén nem minden propertyt fog újra frissíteni. Minden propertynek van egy attribútuma ami jelzi ha módosítva lett. Csak azokat fogja módosítani a SaveChanges(), ahol ez az attribútum a "modified" állapotban van.
            //this._personsDbContext.SaveChanges();

            return targetPerson.ToPersonResponse();
        }

        public async Task<bool> DeletePerson(Guid? personId)
        {
            Person? temp = await
            this._personsDbContext.Persons.FirstOrDefaultAsync(person => person.PersonID == personId);
            if (temp == null)
                return false;

            this._personsDbContext.Persons?.Remove(temp);
            await this._personsDbContext.SaveChangesAsync(); 
            return true;
        }

        public async Task<MemoryStream> GetPersonsToCSV()
        {
            //A MemoryStream azért kell, mert a CSV tartalmát memóriában hozza létre és visszaadja anélkül, hogy valódi fájlt írna a lemezre.
            MemoryStream memoryStream = new MemoryStream();
            StreamWriter sw = new StreamWriter(memoryStream);

            //Most nem állítunk be semmit de valójában a Config paramétereit állíttjuk és adjuk át a writernek.
            CsvConfiguration csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture);

            //A CultureInfo azt határozza meg, hogy a CSV írás során milyen nyelvi és regionális szabályok szerint legyenek formázva az adatok (pl. tizedesjel, dátumformátum). Az InvariantCulture egy nyelvfüggetlen, egységes formátum, amit ezért gyakran használnak CSV exportnál.
            CsvWriter csvWriter = new CsvWriter(sw, leaveOpen: true, culture: CultureInfo.InvariantCulture);

            //csvWriter.WriteHeader<PersonResponse>();
            //Most saját magunk hozzuk létre a Headert és azt a propertyt íratjuk ki amelyiket szeretnénk
            csvWriter.WriteField(nameof(PersonResponse.PersonId));
            csvWriter.WriteField(nameof(PersonResponse.PersonName));
            csvWriter.WriteField(nameof(PersonResponse.DateOfBirth));
            csvWriter.WriteField(nameof(PersonResponse.Address));
            csvWriter.WriteField(nameof(PersonResponse.Country));
            csvWriter.NextRecord();

            List<PersonResponse> persons = this._personsDbContext.Persons.Include("Country").Select(persons => persons.ToPersonResponse()).ToList();

            //await csvWriter.WriteRecordsAsync(persons);

            foreach (PersonResponse person in persons)
            {
                csvWriter.WriteField(person.PersonId);
                csvWriter.WriteField(person.PersonName);
                if (person.DateOfBirth.HasValue)
                {
                    csvWriter.WriteField(person.DateOfBirth.Value.ToString("yyyy-MM-dd"));
                }
                else
                    csvWriter.WriteField("");
                csvWriter.WriteField(person.Address);
                csvWriter.WriteField(person.Country);
                csvWriter.NextRecord(); //hogy a következő rekord új sorba kerüljön
                await csvWriter.FlushAsync(); //beleírjuk a rekordot a stream-be
            }

            memoryStream.Position = 0;

            return memoryStream;
        }

        public async Task<MemoryStream> GetPersonsToExcel()
        {
            MemoryStream memoryStream = new MemoryStream();

            //Regisztrálni kell EPPlus-ra és a neved írd ide. A linket megtalálod a Linkek.txt-ben. Nekem valamiért az appsettings-ben nem működik.
            ExcelPackage.License.SetNonCommercialPersonal("Erik Kovacs");
            using (ExcelPackage excelPackage = new ExcelPackage())
            {                
                //Létrehozzuk a WorkSheet-et.
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Persons");

                worksheet.Cells["A1"].Value = "Person Name";
                worksheet.Cells["B1"].Value = "Email";
                worksheet.Cells["C1"].Value = "Date of Birth";
                worksheet.Cells["D1"].Value = "Address";
                worksheet.Cells["E1"].Value = "Country";

                List<PersonResponse> persons = await this._personsDbContext.Persons.Include("Country").Select(person => person.ToPersonResponse()).ToListAsync();

                int row = 2;
                foreach (PersonResponse person in persons)
                {
                    worksheet.Cells[row, 1].Value = person.PersonName;
                    worksheet.Cells[row, 2].Value = person.Email;
                    if (person.DateOfBirth.HasValue)
                    {
                        worksheet.Cells[row, 3].Value = person.DateOfBirth.Value.ToString("yyyy-MM-dd");
                    }                    
                    worksheet.Cells[row, 4].Value = person.Address;
                    worksheet.Cells[row, 5].Value = person.Country;
                    row++;
                }
                //Autómatikus méretet vesz fel a cella.
                worksheet.Cells[$"A1:E{row-1}"].AutoFitColumns();
                //Belementjük a stream-be.
                await excelPackage.SaveAsAsync(memoryStream);               
            }

            memoryStream.Position = 0;
            return memoryStream;
        }
    }
}