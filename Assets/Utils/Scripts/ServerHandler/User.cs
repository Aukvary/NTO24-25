using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection.Emit;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using UnityEngine;

#pragma warning disable CS4014
public class User
{
    public static string PlayerID = "penis";

    public static bool Tutorial = false;

    public static string TutorialSeed;

    public const string uuid = "652b951e-8899-4460-ae50-fa9a6c9a188c";

    private static HttpClient _client = new();

    private StringBuilder _resorcesBuilder = new();
    private StringBuilder _logBuilder = new();

    private static string _users = $"https://2025.nti-gamedev.ru/api/games/{uuid}/players/";

    public static readonly string LogURL = $"https://2025.nti-gamedev.ru/api/games/{uuid}/logs/";
    public string UserURL => $"https://2025.nti-gamedev.ru/api/games/{uuid}/players/{Name}/";


    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("resources")]
    public Dictionary<string, int> Resources { get; set; }

    public async Task<bool> Uploaded()
        => (await GetUser()) != null;


    static User()
    {
        PlayerID = PlayerPrefs.GetString(nameof(User), null);
    }

    public User(string name)
    {
        if (Tutorial)
            Name = name.Contains($"{TutorialSeed}_") ? name : $"{TutorialSeed}_{name}";
        else
            Name = name.Contains($"{PlayerID}_") ? name : $"{PlayerID}_{name}";
    }

    public static User JsonToUser(string jsonBody)
        => JsonSerializer.Deserialize<User>(jsonBody);

    public static string UserToJson(User user)
        => JsonSerializer.Serialize(user);

    public static async Task<List<User>> GetUsers()
    {
        var response = await _client.GetAsync(_users);

        response.EnsureSuccessStatusCode();

        return JsonSerializer.Deserialize<List<User>>(await response.Content.ReadAsStringAsync()) ?? new List<User>();
    }

    public static async void Delete()
    {
        if (PlayerID == null)
            return;
        var users = await GetUsers();


        foreach (var user in users.Where(u => u.Name.Contains(PlayerID)))
            await user.DeleteUser();
        PlayerID = null;
    }

    public async Task InitializeUser(params string[] names)
    {
        if (await Uploaded())
        {
            Resources = (await GetUser()).Resources;
            return;
        }

        Resources = names.ToDictionary(s => s, s => 0);

        StringContent content = new(UserToJson(this), Encoding.UTF8, "application/json");
        var response = await _client.PostAsync(_users, content);

        response.EnsureSuccessStatusCode();
    }

    public async Task<User> GetUser()
    {
        var response = await _client.GetAsync(UserURL);

        try
        {
            response.EnsureSuccessStatusCode();

            return JsonToUser(await response.Content.ReadAsStringAsync());
        }
        catch (HttpRequestException)
        {
            return null;
        }
    }

    public async Task UpdateUser()
    {
        Resources = (await GetUser()).Resources;
    }

    public async Task<string> UpdateUser(string resource, int count)
    {
        int oldCount = Resources[resource];
        Resources[resource] = count;

        _resorcesBuilder.Append("{\"resources\":{");

        foreach (var pair in Resources)
            _resorcesBuilder.Append($"\"{pair.Key}\":{pair.Value},");
        _resorcesBuilder.Length--;
        _resorcesBuilder.Append("}}");

        StringContent content = new(_resorcesBuilder.ToString(), Encoding.UTF8, "application/json");
        _resorcesBuilder.Clear();

        var response = await _client.PutAsync(UserURL, content);
        response.EnsureSuccessStatusCode();

        UserLog(
            $"Change {resource} count",
            $"\"new value\": \"{count}\"",
            $"\"old value\": \"{oldCount}\""
            );

        return await response.Content.ReadAsStringAsync();
    }

    public async Task UpdateServerInfo()
    {
        foreach (var pair in Resources)
            _resorcesBuilder.Append($"\"{pair.Key}\":{pair.Value},");
        _resorcesBuilder.Length--;
        _resorcesBuilder.Append("}}");

        StringContent content = new(_resorcesBuilder.ToString(), Encoding.UTF8, "application/json");
        _resorcesBuilder.Clear();
        var response = await _client.PutAsync(UserURL, content);
    }

    public async Task DeleteUser()
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Delete,
            RequestUri = new Uri(UserURL),
            Content = new StringContent("", Encoding.UTF8, "application/json")
        };

        HttpResponseMessage response = await _client.SendAsync(request);

        response.EnsureSuccessStatusCode();
    }

    public async Task<string> UserLog(string comment, params string[] logs)
    {
        _logBuilder.Append("{\"comment\":");
        _logBuilder.Append($"\"{comment}\",");
        _logBuilder.Append($"\"player_name\": \"{Name}\",");
        _logBuilder.Append("\"resources_changed\":{");

        foreach (var log in logs)
            _logBuilder.Append($"{log},");
        _logBuilder.Length--;
        _logBuilder.Append("}}");

        StringContent content = new(_logBuilder.ToString(), Encoding.UTF8, "application/json");

        _logBuilder.Clear();

        var response = await _client.PostAsync(LogURL, content);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }
}