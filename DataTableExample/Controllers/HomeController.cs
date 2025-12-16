using DataTableExample.Data;
using DataTableExample.Data.Entities;
using DataTableExample.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace DataTableExample.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetMedicines()
        {
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();

                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                IQueryable<Medicine> query = _context.Medicines.AsQueryable();

                // Search
                if (!string.IsNullOrEmpty(searchValue))
                {
                    searchValue = searchValue.ToLower(); // For case-insensitive search

                    bool isPriceSearch = decimal.TryParse(
                                           searchValue,
                                           System.Globalization.NumberStyles.Any,
                                           System.Globalization.CultureInfo.InvariantCulture,
                                           out decimal priceValue
                                         );

                    query = query.Where(m =>
                       (m.Name.ToLower().Contains(searchValue)) ||
                       (m.ActiveIngredient.ToLower().Contains(searchValue)) ||
                       (m.Manufacturer.ToLower().Contains(searchValue)) ||
                       (isPriceSearch && m.Price == priceValue)
                   );
                }

                // Total records after filtering
                recordsTotal = await query.CountAsync();

                // Sorting
                if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDirection))
                {
                    if (sortColumn == "Name")
                        query = sortColumnDirection == "asc" ? query.OrderBy(c => c.Name) : query.OrderByDescending(c => c.Name);
                    else if (sortColumn == "Price")
                        query = sortColumnDirection == "asc" ? query.OrderBy(c => c.Price) : query.OrderByDescending(c => c.Price);
                    else if (sortColumn == "ActiveIngredient")
                        query = sortColumnDirection == "asc" ? query.OrderBy(c => c.ActiveIngredient) : query.OrderByDescending(c => c.ActiveIngredient);
                    else if (sortColumn == "Manufacturer")
                        query = sortColumnDirection == "asc" ? query.OrderBy(c => c.Manufacturer) : query.OrderByDescending(c => c.Manufacturer);
                    else
                        query = query.OrderByDescending(c => c.Name);
                }
                else
                {
                    query = query.OrderByDescending(c => c.Name);
                }

                // Paging
                var data = await query.Skip(skip).Take(pageSize).Select(e => new
                {
                    id = e.Id,
                    name = e.Name,
                    activeIngredient = e.ActiveIngredient,
                    manufacturer = e.Manufacturer,
                    price = e.Price
                }).ToListAsync();

                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading medicine for DataTable");
                return StatusCode(500, new { error = "Възникна грешка при обработката на вашата заявка" });
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
