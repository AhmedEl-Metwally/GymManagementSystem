using GymManagementDAL.Data.Context;
using GymManagementDAL.Entities;
using System.Text.Json;

namespace GymManagementDAL.Data.DataSeed
{
    public static class GymDbContextSeeding
    {
        public static bool SeedData(GymDbContext context)
        {
            try
            {
                var HasPlans = context.Plans.Any();
                var HasCategories = context.Categories.Any();
                if (HasPlans && HasCategories)
                    return false;

                if (!HasPlans)
                {
                    var plans = LoadDataFromjsonFile<Plan>("plans.json");
                    if (plans.Any())
                        context.Plans.AddRange(plans);
                }

                if (!HasCategories)
                {
                    var categories = LoadDataFromjsonFile<Category>("Categories.json");
                    if (categories.Any())
                        context.Categories.AddRange(categories);
                }
                return context.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while seeding the database.", ex);
                return false;
            }

        }

        //helpers to load data from json files
        private static List<T> LoadDataFromjsonFile<T>(string fileName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files",fileName);
            if (!File.Exists(filePath))
                throw new FileNotFoundException();

            string data = File.ReadAllText(filePath);
            var options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Deserialize<List<T>>(data, options) ?? new List<T>();
        }

    }
}
