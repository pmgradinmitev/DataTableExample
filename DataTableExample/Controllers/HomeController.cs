using DataTableExample.Data;
using DataTableExample.Data.Entities;
using DataTableExample.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Globalization;

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
            string? draw = null;

            try
            {
                draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();

                int pageSize = length != null ? Convert.ToInt32(length) : 10;
                int skip = start != null ? Convert.ToInt32(start) : 0;

                IQueryable<Medicine> query = _context.Medicines;
                // Total records in table (before filtering)
                int recordsTotal = await query.CountAsync();

                // Search
                if (!string.IsNullOrEmpty(searchValue))
                {
                    searchValue = searchValue.ToLower(); // For case-insensitive search

                    bool isPriceSearch = decimal.TryParse(
                                           searchValue.Replace(',', '.'), // allow "5,99" as well as "5.99"
                                           NumberStyles.Number,
                                           CultureInfo.InvariantCulture,
                                           out decimal priceValue
                                         );

                    query = query.Where(m =>
                       m.Name.ToLower().Contains(searchValue) ||
                       m.ActiveIngredient.ToLower().Contains(searchValue) ||
                       m.Manufacturer.ToLower().Contains(searchValue) ||
                       (isPriceSearch && m.Price == priceValue)
                   );
                }

                // Total records after filtering
                int recordsFiltered = await query.CountAsync();

                // Sorting
                bool descending = sortColumnDirection == "desc";
                IOrderedQueryable<Medicine> ordered = sortColumn switch
                {
                    "Price" => descending ? query.OrderByDescending(c => c.Price) : query.OrderBy(c => c.Price),
                    "ActiveIngredient" => descending ? query.OrderByDescending(c => c.ActiveIngredient) : query.OrderBy(c => c.ActiveIngredient),
                    "Manufacturer" => descending ? query.OrderByDescending(c => c.Manufacturer) : query.OrderBy(c => c.Manufacturer),
                    _ => descending ? query.OrderByDescending(c => c.Name) : query.OrderBy(c => c.Name),
                };
                query = ordered.ThenBy(c => c.Id); // stable order between pages when values are equal

                // Paging (DataTables sends length = -1 for "All")
                query = query.Skip(skip);
                if (pageSize > 0)
                    query = query.Take(pageSize);

                var data = await query.Select(e => new
                {
                    id = e.Id,
                    name = e.Name,
                    activeIngredient = e.ActiveIngredient,
                    manufacturer = e.Manufacturer,
                    price = e.Price
                }).ToListAsync();

                return Json(new { draw, recordsFiltered, recordsTotal, data });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading medicine for DataTable");
                // DataTables shows the message only from a 200 response with an "error" field
                return Json(new { draw, error = "Възникна грешка при обработката на вашата заявка" });
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
