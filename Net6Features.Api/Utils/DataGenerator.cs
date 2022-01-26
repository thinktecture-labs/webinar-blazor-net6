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
                    var rest = 10000 - contributions.Count;
                    if (rest > 0)
                    {
                        for (int i = 0; i < rest; i++)
                        {
                            var id = contributions.Count  + 1;
                            contributions.Add(new Contribution
                            {
                                Id = id,
                                Title = $"Contribution {id}",
                                Date = DateTime.Now.ToShortDateString(),
                                Abstract = "Lorem ipsum dolor...",
                                Language = "deutsch",
                                PreviewSrc = String.Empty,
                                Type = "Talk"
                            });
                        }
                    }
                    await context.Contributions.AddRangeAsync(contributions);
                }

                if (!context.Conferences.Any())
                {
                    var conferences = await LoadDataAsync<Conference>(context);
                    var rest = 10000 - conferences.Count;
                    if (rest > 0)
                    {
                        for (int i = 0; i < rest; i++)
                        {
                            var id = conferences.Count  + 1;
                            conferences.Add(new Conference
                            {
                                Id = id,
                                Title = $"Conference {id}",
                                City = "Rodalben",
                                Country = "Deutschland",
                                Url = String.Empty,
                            });
                        }
                    }
                    await context.Conferences.AddRangeAsync(conferences);
                }
                if (!context.Speakers.Any())
                {
                    var speakers = await LoadDataAsync<Speaker>(context);
                    var rest = 10000 - speakers.Count;
                    if (rest > 0)
                    {
                        for (int i = 0; i < rest; i++)
                        {
                            var id = speakers.Count  + 1;
                            speakers.Add(new Speaker
                            {
                                Id = id,
                                FirstName = $"First {id}",
                                LastName = $"Last generated {id}",
                                Email = $"speaker{id}@tt.com"
                            });
                        }
                    }
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
