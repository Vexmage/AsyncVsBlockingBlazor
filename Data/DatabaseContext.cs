using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace AsyncVsBlockingBlazor.Data
{
    // Model for storing API responses
    public class ApiResponse
    {
        public int Id { get; set; }  // Auto-increment primary key
        public string Data { get; set; }  // API response data
        public DateTime RetrievedAt { get; set; }  // Timestamp
    }

    // Database Context for managing SQLite connection
    public class DatabaseContext : DbContext
    {
        public DbSet<ApiResponse> ApiResponses { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
            {
                options.UseSqlite("Data Source=AsyncDemo.db");  // SQLite database file
            }
        }

        // Ensure the database is created when the app starts
        public async Task InitializeDatabaseAsync()
        {
            await Database.EnsureCreatedAsync();
        }
    }
}
