using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using RepositoryContracts;
using ServiceContract;
using ServiceContract.DTOs;
using System.Runtime.InteropServices;

namespace Services
{
    public class CountryService : ICountryService
    {
        //fieldként elmentjük a contextot, hogy a megfelelő adatbázisra tudjunk hivatkozni.
        private readonly ICountriesRepository _countryRepository;

        public CountryService(ICountriesRepository countryRepository)
        {
            this._countryRepository = countryRepository;
        }


        //Így tudsz paraméternek default értéket megadni.
        //public CountryService(bool initialization = true)
        //{
        //    this._countries = new List<Country>();

        //    //Ha a konstruktorban az érték 1, akkor lefutt az inicializáció és ezzel dolgozik. Ha a tesztben is hozzáadunk újabb objektumokat, akkor az is bele lesz téva a listába. A két helyen inicializált objektumok összeadódnak végén. 
        //    if (initialization)
        //    {
        //        _countries.AddRange(new List<Country> { new Country() { CountryID = Guid.Parse("11C64D36-EC2D-4ADE-99F6-469F98E380CF"), CountryName = "Hungary" }, new Country() { CountryID = Guid.Parse("456B9BAD-40EA-4A17-85B3-87C2E5555A26"), CountryName = "Austria" }, new Country() { CountryID = Guid.Parse("B4871C6C-6BB8-4CCF-AA16-CF846D036EDF"), CountryName = "Serbia" }, new Country() { CountryID = Guid.Parse("7ED74F84-21D9-4A9A-A5F2-4390DFD0F40F"), CountryName = "Germany" }, new Country() { CountryID = Guid.Parse("C9CCFE13-E61B-485B-ABCB-B953297C6993"), CountryName = "Italy" }, new Country() { CountryID = Guid.Parse("5716D10D-005A-4347-B27D-F0A50D02279A"), CountryName = "England" } });
        //    }
        //}
        public async Task<CountryResponse> AddCountry(CountryAddRequest? countryRequest)
        {
            //Ha null a metódus paraméter akkor Exception 
            if (countryRequest is null)
            {
                throw new ArgumentNullException();
            }
            //Ha a Name üres, akkor Exception
            if (countryRequest.Name is null)
            {
                throw new ArgumentException();
            }

            //Alul meghagyom a korábbi verziót összehasonlításnak.

            //if (await this._countryService.GetAllCountries().CountAsync(country => country.CountryName == countryRequest.Name) > 0)
            //{
            //    throw new Exception("The given Country name is already exists!");
            //}
            if (await this._countryRepository.GetCountryByName(countryRequest.Name) != null)
            {
                throw new ArgumentException("The given Country name is already exists!");
            }

            //if (this._dbContext.Where(x => x.CountryName == countryRequest.Name).Count() > 0)
            //{
            //    throw new Exception("The given Country name is already exists!");
            //}

            //Ahogyláthatod a "Extension" metódus sikeresen hozzá lett addva a Country Entity-hez.
            //Átalakítjuk a countryRequest objketumot Country egyeddé
            Country country = countryRequest.ToCountry();
            //Generálunk neki Guid-t
            country.CountryID = Guid.NewGuid();
            //Hozzáadjuk a belső listához
            //this._countryRepository.Add(country);
            await this._countryRepository.AddCountry(country);
            //Mikor insert történik kötelessek vagyunk elmenteni a változtatást.
            //Ez a SaveChangesAsync már nem fog kelleni nekünk, mert a CountryRepositoryban meghívjuk.
            //await this._countryRepository.SaveChangesAsync();

            //Azért célszerű nem a Country egyedet visszaadni és inkább csak a Service-en belül hagyni, hogy kívülről ne legyen látható, csak amit engedünk a CountryResponse-zal.            
            return country.ToCountryResponse();
        }

        public async Task<int> FromExcelDataUpload(IFormFile formFile)
        {
            //Azért kell a MemoryStream, mert az bármilyen adatot tud tárolni.
            MemoryStream memoryStream = new MemoryStream();
            //Elhelyezzük a memorystream-be.
            await formFile.CopyToAsync(memoryStream);
            int insertedCountry = 0;
            ExcelPackage.License.SetNonCommercialPersonal("Erik Kovacs");
            using (ExcelPackage excelPackage = new ExcelPackage(memoryStream))
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets["Countries"];

                //Összeszámolja sorokat amik nem csak üres cellákat tartalmaz.
                int row = worksheet.Dimension.Rows;

                for (int i = 2; i <= row; i++)
                {
                    string? countryName = worksheet.Cells[i, 1].Value.ToString();

                    if (!string.IsNullOrEmpty(countryName))
                    {
                        //List<string?> countryNames = await this._countryRepository.Countries.Select(country => country.CountryName).ToListAsync();
                        List<string?> countryNames = (await this._countryRepository.GetAllCountries()).Select(country => country.CountryName).ToList();

                        if (!countryNames.Contains(countryName))
                        {
                            CountryAddRequest countryAddRequest = new CountryAddRequest()
                            {
                                Name = countryName
                            };
                            Country country = countryAddRequest.ToCountry();
                            //await this._countryRepository.Countries.AddAsync(country);
                            await this._countryRepository.AddCountry(country);

                            //await this._countryRepository.SaveChangesAsync();                       
                            insertedCountry++;
                        }
                    }
                }
            }
            ;
            return insertedCountry;
        }

        public async Task<List<CountryResponse>> GetAllCountries()
        {
            //List<CountryResponse> countries = await this._countryRepository.Countries.Select(country => country.ToCountryResponse()).ToListAsync();
            List<CountryResponse> countries = (await this._countryRepository.GetAllCountries()).Select(country => country.ToCountryResponse()).ToList();

            return countries;

            //return (List<CountryResponse>)this._dbContext.Select(x => x.ToCountryResponse()).ToList();
        }

        public async Task<CountryResponse?> GetCountryById(Guid? countryId)
        {
            if (countryId is null)
            {
                return null;
            }

            //Előbb kiszűri a megfelelő elemeket és uttána kiválasztja az elsőt. 
            //return this._countries.Where(country => country.Guid == countryId).FirstOrDefault().ToCountryResponse();

            //Amint megtalál egy megfelelő elemet, visszaadja.
            //Country? country = this._dbContext.FirstOrDefault(country => country.CountryID == countryId);

            //Country? country = await this._countryRepository.Countries.FirstOrDefaultAsync(country => country.CountryID == countryId);
            Country? country = await this._countryRepository.GetCountryById(countryId.Value);

            if (country is null)
            {
                return null;
            }
            return country.ToCountryResponse();
        }


    }
}
