using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Application.Services;

public class CoinCapService
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "https://pro.coincap.io";

    public CoinCapService(string apiKey)
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(BaseUrl)
        };

        // Add API key as Bearer token
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", apiKey);

        _httpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task<List<CryptoAsset>> GetAllAssetsAsync()
    {
        var response = await _httpClient.GetAsync("/v1/assets");
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();

        var result = JsonConvert.DeserializeObject<CoinCapAssetsResponse>(json);
        return result?.Data ?? new List<CryptoAsset>();
    }

    public async Task<CryptoAsset?> GetAssetByIdAsync(string assetId)
    {
        var response = await _httpClient.GetAsync($"/v1/assets/{assetId}");
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<CoinCapSingleAssetResponse>(json);

        return result?.Data;
    }


    public class CoinCapAssetsResponse
    {
        public List<CryptoAsset> Data { get; set; }
    }

    public class CoinCapSingleAssetResponse
    {
        public CryptoAsset Data { get; set; }
    }


    public class CryptoAsset
    {
        public string Id { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public decimal PriceUsd { get; set; }
    }
}