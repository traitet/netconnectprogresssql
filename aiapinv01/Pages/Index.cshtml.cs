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
            // test commit 1:54
            //var connString = "Server=localhost;Port=5432;Database=postgres;User Id=dbadmin;Password=aiap@2023;";
            //var connString = "Server=localhost;Port=5432;Database=aiap;User Id=postgres;Password=aiap@2023;";
            var connString = "Server=test-investmentdb.postgres.database.azure.com;Port=5432;Database=aiap;User Id=dbadmin;Password=aiap@2023;SslMode=Require;";
            //var connString = "Server=test-investmentdb.postgres.database.azure.com;Port=5432;Database=postgres;User Id=dbadmin@Server=test-investmentdb.postgres.database.azure.com;Password=aiap@2023;SslMode=Require;";
            await using var connection = new NpgsqlConnection(connString);
            await connection.OpenAsync();
            //await using var command = new NpgsqlCommand("select *, CURRENT_TIME as CTime from myTable", connection);
            await using var command = new NpgsqlCommand("SELECT 1 as id, 'abc' as Name, 'desc' as description, to_char(CURRENT_TIME::time without time zone, 'HH:MI:SS.US') as CTime", connection);
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                SampleData.Add(new SampleDatatest
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Description = reader.GetString(2),
                    CTime = reader.GetString(3)
                });
            }
        }
    }
}
