using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

public static class RequestSender
{
    private static string _userURL = $"https://2025.nti-gamedev.ru/api/games/{Ñonstants.UUID}/players/";

    public static async Task<string> CreateUser(string jsonBody)
    {
        using (HttpClient client = new HttpClient())
        {
            HttpContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(_userURL, content);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

    }

    public static async Task<string> GetUsers()
    {
        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = await client.GetAsync(_userURL);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }

    public static async Task<string> GetUser(string jsonBody, string username)
    {
        using (HttpClient client = new HttpClient())
        {
            HttpContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(_userURL + $"{username}/", content);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }

    public static async Task<string> UpdateUser(string jsonBody, string username)
    {
        using (HttpClient client = new HttpClient())
        {
            HttpContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PutAsync(_userURL + $"{username}/", content);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }

    public static async Task<string> DeleteUser(string jsonBody, string username)
    {
        using (HttpClient client = new HttpClient())
        {
            HttpRequestMessage request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri(_userURL + $"{username}/"),
                Content = new StringContent(jsonBody, Encoding.UTF8, "application/json")
            };

            HttpResponseMessage response = await client.SendAsync(request);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
}