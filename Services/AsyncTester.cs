namespace AsyncVsBlockingBlazor.Services 
{
    using System;
    using System.Diagnostics;
    using System.Net.Http;
    using System.Net;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;
    using AsyncVsBlockingBlazor.Data;  

    public class AsyncTester
    {
        private readonly Stopwatch _stopwatch = new Stopwatch();
        private static readonly HttpClient _httpClient = new HttpClient();
        private readonly DatabaseContext _dbContext;

        public AsyncTester(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Simulated Blocking Test (Loop Instead of Thread.Sleep)
        public async Task<string> RunBlockingTest()
        {
            _stopwatch.Restart();
            var startTime = DateTime.UtcNow;
            while ((DateTime.UtcNow - startTime).TotalMilliseconds < 5000)
            {
                await Task.Delay(1);  
            }
            _stopwatch.Stop();
            return $"Simulated Blocking Method Time: {_stopwatch.ElapsedMilliseconds} ms";
        }

        // Async Test (Task.Delay for Non-Blocking Execution)
        public async Task<string> RunAsyncTest()
        {
            _stopwatch.Restart();
            await Task.Delay(5000);
            _stopwatch.Stop();
            return $"Async Method Time: {_stopwatch.ElapsedMilliseconds} ms";
        }

        // Parallel Async Test (Task.WhenAll to Run Two Async Tasks Simultaneously)
        public async Task<string> RunParallelAsyncTest()
        {
            _stopwatch.Restart();
            Task task1 = Task.Delay(5000);
            Task task2 = Task.Delay(5000);
            await Task.WhenAll(task1, task2);
            _stopwatch.Stop();
            return $"Parallel Async Execution Time: {_stopwatch.ElapsedMilliseconds} ms";
        }

        // API Call (Fetching Data from External Service)
        public async Task<string> FetchDataFromApi()
        {
            try
            {
                _stopwatch.Restart();
                HttpResponseMessage response = await _httpClient.GetAsync("https://jsonplaceholder.typicode.com/posts");
                response.EnsureSuccessStatusCode();
                string data = await response.Content.ReadAsStringAsync();
                _stopwatch.Stop();
                return $"API Call Time: {_stopwatch.ElapsedMilliseconds} ms\nFirst 100 chars: {data.Substring(0, 100)}...";
            }
            catch (Exception ex)
            {
                return $"❌ API call failed: {ex.Message}";
            }
        }

        // Save API Response to SQLite Database
        public async Task<string> SaveApiResponseToDatabaseAsync()
        {
            string data = await FetchDataFromApi();
            var response = new ApiResponse { Data = data, RetrievedAt = DateTime.UtcNow };

            await _dbContext.Database.EnsureCreatedAsync();  
            _dbContext.ApiResponses.Add(response);
            await _dbContext.SaveChangesAsync();

            return $"API Response Saved to Database!";
        }

        // Retrieve Stored API Responses from SQLite Database
        public async Task<List<ApiResponse>> RetrieveApiResponsesAsync()
        {
            return await _dbContext.ApiResponses.ToListAsync();
        }

        // Parallel API Calls (Three Requests at Once)
        public async Task<string> RunParallelApiCalls()
        {
            _stopwatch.Restart();
            try
            {
                Task<string> task1 = FetchDataFromApi();
                Task<string> task2 = FetchDataFromApi();
                Task<string> task3 = FetchDataFromApi();

                string[] results = await Task.WhenAll(task1, task2, task3);
                _stopwatch.Stop();
                return $"Parallel API Calls Completed in: {_stopwatch.ElapsedMilliseconds} ms";
            }
            catch (Exception ex)
            {
                return $"One or more API calls failed: {ex.Message}";
            }
        }
    } 
}  
