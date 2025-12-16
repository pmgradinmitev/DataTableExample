using DataTableExample.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataTableExample.Data.Seed
{
    public class MedicineSeed
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());

            if (!context.Medicines.Any())
            {
                var medicines = new List<Medicine>
                {
                    new Medicine { Name = "Paracetamol", ActiveIngredient = "Paracetamol", Manufacturer = "Teva", Price = 5.99m },
                    new Medicine { Name = "Ibuprofen", ActiveIngredient = "Ibuprofen", Manufacturer = "Bayer", Price = 7.49m },
                    new Medicine { Name = "Aspirin", ActiveIngredient = "Acetylsalicylic Acid", Manufacturer = "Bayer", Price = 6.20m },
                    new Medicine { Name = "Amoxicillin", ActiveIngredient = "Amoxicillin", Manufacturer = "Pfizer", Price = 12.50m },
                    new Medicine { Name = "Ciprofloxacin", ActiveIngredient = "Ciprofloxacin", Manufacturer = "Teva", Price = 15.00m },
                    new Medicine { Name = "Metformin", ActiveIngredient = "Metformin", Manufacturer = "Sandoz", Price = 8.75m },
                    new Medicine { Name = "Atorvastatin", ActiveIngredient = "Atorvastatin", Manufacturer = "Pfizer", Price = 10.50m },
                    new Medicine { Name = "Omeprazole", ActiveIngredient = "Omeprazole", Manufacturer = "AstraZeneca", Price = 9.30m },
                    new Medicine { Name = "Lisinopril", ActiveIngredient = "Lisinopril", Manufacturer = "Teva", Price = 11.25m },
                    new Medicine { Name = "Simvastatin", ActiveIngredient = "Simvastatin", Manufacturer = "Sandoz", Price = 7.90m },
                    new Medicine { Name = "Levothyroxine", ActiveIngredient = "Levothyroxine Sodium", Manufacturer = "Pfizer", Price = 14.00m },
                    new Medicine { Name = "Losartan", ActiveIngredient = "Losartan Potassium", Manufacturer = "Sandoz", Price = 13.20m },
                    new Medicine { Name = "Prednisone", ActiveIngredient = "Prednisone", Manufacturer = "Teva", Price = 6.80m },
                    new Medicine { Name = "Hydrochlorothiazide", ActiveIngredient = "Hydrochlorothiazide", Manufacturer = "Pfizer", Price = 5.50m },
                    new Medicine { Name = "Citalopram", ActiveIngredient = "Citalopram", Manufacturer = "Sandoz", Price = 9.75m },
                    new Medicine { Name = "Alprazolam", ActiveIngredient = "Alprazolam", Manufacturer = "Teva", Price = 8.40m },
                    new Medicine { Name = "Metoprolol", ActiveIngredient = "Metoprolol Tartrate", Manufacturer = "Pfizer", Price = 10.00m },
                    new Medicine { Name = "Gabapentin", ActiveIngredient = "Gabapentin", Manufacturer = "Sandoz", Price = 12.80m },
                    new Medicine { Name = "Furosemide", ActiveIngredient = "Furosemide", Manufacturer = "Teva", Price = 7.10m },
                    new Medicine { Name = "Warfarin", ActiveIngredient = "Warfarin Sodium", Manufacturer = "Pfizer", Price = 11.90m }
                };

                context.Medicines.AddRange(medicines);
                context.SaveChanges();
            }
        }
    }
}
