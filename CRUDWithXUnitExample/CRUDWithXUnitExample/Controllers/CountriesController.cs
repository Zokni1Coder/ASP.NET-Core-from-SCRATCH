using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceContract;

namespace CRUDWithXUnitExample.Controllers
{
    [Route("[controller]")]
    public class CountriesController : Controller
    {
        private ICountryService _countryService { get; }

        public CountriesController(ICountryService countryService)
        {
            _countryService = countryService;
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> ExcelUpload()
        {
            return View();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ExcelUpload(IFormFile formFile)
        {
            if (formFile is null || formFile.Length == 0)
            {
                ViewBag.ErrorMessage = "Please select an xlsx file";
                return View();
            }
            if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                ViewBag.ErrorMessage = "Unsupported file. Please select an xlsx file";
                return View();
            }
            int insertedCountries = await this._countryService.FromExcelDataUpload(formFile);

            ViewBag.Message = $"{insertedCountries} Countries uploaded";
            return View();
        }

    }
}
