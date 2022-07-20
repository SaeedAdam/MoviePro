namespace MoviePro.Services.Interfaces;

public interface IImageService
{
    Task<byte[]> EncodeImageAsync(IFormFile poster);
    Task<byte[]> EncodeImageUrlAsync(string imageUrl);
    string DecodeImage(byte[] poster, string contentType);
}
