using MoviePro.Services.Interfaces;

namespace MoviePro.Services;

public class BasicImageService : IImageService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public BasicImageService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<byte[]> EncodeImageAsync(IFormFile poster)
    {
        if (poster is null) return null;

        using var ms = new MemoryStream();
        await poster.CopyToAsync(ms);
        return ms.ToArray();
    }

    public async Task<byte[]> EncodeImageUrlAsync(string imageUrl)
    {
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync(imageUrl);
        await using Stream stream = await response.Content.ReadAsStreamAsync();

        var ms = new MemoryStream();
        await stream.CopyToAsync(ms);

        return ms.ToArray();
    }

    public string DecodeImage(byte[] poster, string contentType)
    {
        if (poster is null || contentType is null) return null;

        var posterImage = Convert.ToBase64String(poster);

        return $"data:image/{contentType};base64,{posterImage}";
    }
}
