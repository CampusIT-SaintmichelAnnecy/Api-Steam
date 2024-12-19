// Services/SteamApiService.cs
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using APISteam.Models;

namespace APISteam.Services
{
    public class SteamApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public SteamApiService(string apiKey)
        {
            _httpClient = new HttpClient();
            _apiKey = apiKey;
        }

        public async Task<List<int>> GetGameIdsAsync()
        {
            var requestUrl = $"https://api.steampowered.com/ISteamChartsService/GetMostPlayedGames/v1/";
            var responseString = await _httpClient.GetStringAsync(requestUrl);

            var jsonResponse = JObject.Parse(responseString);
            var gameIds = new List<int>();

            if (jsonResponse["response"]?["ranks"] != null)
            {
                foreach (var item in jsonResponse["response"]["ranks"])
                {
                    gameIds.Add((int)item["appid"]);
                }
            }
            else
            {
                throw new HttpRequestException("Failed to load game IDs: Response format incorrect");
            }

            return gameIds;
        }

        public async Task<int> GetNumberOfCurrentPlayersAsync(int appId)
        {
            var requestUrl = $"https://api.steampowered.com/ISteamUserStats/GetNumberOfCurrentPlayers/v1/?appid={appId}";
            var response = await _httpClient.GetAsync(requestUrl);

            if (!response.IsSuccessStatusCode)
            {
                return -1;
            }

            var responseString = await response.Content.ReadAsStringAsync();
            var jsonResponse = JObject.Parse(responseString);
            
            return jsonResponse["response"]?.Value<int>("player_count") ?? -1;
        }

        public async Task<SteamGame> GetGameDetailsAsync(int appId)
        {
            var requestUrl = $"https://store.steampowered.com/api/appdetails?appids={appId}";
            var response = await _httpClient.GetAsync(requestUrl);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var responseString = await response.Content.ReadAsStringAsync();
            var jsonResponse = JObject.Parse(responseString);

            var gameData = jsonResponse[$"{appId}"];
            if (gameData != null && gameData["success"].Value<bool>())
            {
                var data = gameData["data"];
                return new SteamGame
                {
                    AppId = appId,
                    Name = data["name"].Value<string>(),
                    Description = data["short_description"].Value<string>(),
                    ImageUrl = data["header_image"].Value<string>(),
                    Genres = string.Join(", ", data["genres"].Select(g => g["description"].Value<string>())),
                    ReleaseDate = data["release_date"]["date"].Value<string>(),
                    Website = data["website"]?.Value<string>() ?? string.Empty
                };
            }

            return null;
        }

        public async Task<List<SteamGame>> GetTopGamesAsync()
        {
            var gameIds = await GetGameIdsAsync();

            // Paralléliser les requêtes pour récupérer les informations des jeux
            var getPlayerCountTasks = gameIds.Select(async gameId =>
            {
                var playerCount = await GetNumberOfCurrentPlayersAsync(gameId);
                if (playerCount >= 0)
                {
                    var gameDetails = await GetGameDetailsAsync(gameId);
                    if (gameDetails != null)
                    {
                        gameDetails.PlayerCount = playerCount;
                        return gameDetails;
                    }
                }
                return null;
            }).ToList();

            var games = await Task.WhenAll(getPlayerCountTasks);

            return games.Where(g => g != null).OrderByDescending(g => g.PlayerCount).Take(10).ToList();
        }
    }
}