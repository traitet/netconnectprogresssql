using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Npgsql;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aiapinv01.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public List<SampleDatatest> SampleData { get; set; }

        public async Task OnGetAsync()
        {
            SampleData = new List<SampleDatatest>();

            var connString = "Server=localhost;Port=5432;Database=aiap;User Id=postgres;Password=aiap@2023;";
            await using var connection = new NpgsqlConnection(connString);
            await connection.OpenAsync();

            await using var command = new NpgsqlCommand("SELECT * FROM mytable", connection);
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                SampleData.Add(new SampleDatatest
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Description = reader.GetString(2)
                });
            }
        }
    }
}
