using Microsoft.EntityFrameworkCore;
using Net6Features.Api.Models;
using Net6Features.Shared.Models;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace Net6Features.Api.Utils
{
    public class DataGenerator
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using (var context = new SampleDatabaseContext(
                serviceProvider.GetRequiredService<DbContextOptions<SampleDatabaseContext>>()))
            {
                if (!context.Contributions.Any())
                {
                    var contributions = await LoadDataAsync<Contribution>(context);
                    await context.Contributions.AddRangeAsync(contributions);
                }

                if (!context.Conferences.Any())
                {
                    var conferences = await LoadDataAsync<Conference>(context);
                    await context.Conferences.AddRangeAsync(conferences);
                }
                if (!context.Speakers.Any())
                {
                    var speakers = await LoadDataAsync<Speaker>(context);
                    await context.Speakers.AddRangeAsync(speakers);
                }

                await context.SaveChangesAsync();
            }
        }

        private static async Task<List<T>> LoadDataAsync<T>(SampleDatabaseContext context)
            where T : class, IModelId
        {
            var assembly = Assembly.GetEntryAssembly();
            var currentType = typeof(T);
            var resourceStream = assembly?.GetManifestResourceStream($"Net6Features.Api.Data.{currentType.Name}.json");
            var root = new Root<T>();
            if (resourceStream != null)
            {
                using var reader = new StreamReader(resourceStream, Encoding.UTF8);
                var jsonString = await reader.ReadToEndAsync();
                root = JsonSerializer.Deserialize<Root<T>>(jsonString, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            // This is needed because the sample data has duplicate entries
            if (root != null)
            {
                var index = 1;
                root.Items.ForEach(c =>
                {
                    c.Id = index;
                    index++;
                });
            }
            return root?.Items ?? new List<T>();
        }
    }
}
