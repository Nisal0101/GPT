using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Configuration;
using Newtonsoft.Json;
class Program
{
    private static readonly HttpClient client = new HttpClient();
    private static string apiKey;
    private static string apiEndpoint;
    static async Task Main(string[] args)
    {
        LoadConfiguration();
        Console.WriteLine("Welcome to the AI Chat Program!");
        while (true)
        {
            Console.Write("You: ");
            string userInput = Console.ReadLine();
            if (userInput.ToLower() == "exit")
                break;
            string response = await GetAIResponse(userInput);
            Console.WriteLine($"AI: {response}");
        }
    }
    private static void LoadConfiguration()
    {
        apiKey = ConfigurationManager.AppSettings["API_KEY"];
        apiEndpoint = ConfigurationManager.AppSettings["API_ENDPOINT"];
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
    }
    private static async Task<string> GetAIResponse(string message)
    {
        var content = new StringContent(JsonConvert.SerializeObject(new { input = message }));
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        var response = await client.PostAsync(apiEndpoint, content);
        response.EnsureSuccessStatusCode();
        var jsonResponse = await response.Content.ReadAsStringAsync();
        dynamic result = JsonConvert.DeserializeObject(jsonResponse);
        return result.response;
    }
}