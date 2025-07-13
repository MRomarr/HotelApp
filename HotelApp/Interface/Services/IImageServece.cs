
namespace HotelApp.Interface.Services
{
    public interface IImageServece
    {
        Task<List<string>> AddImagesAsync(IFormFileCollection files,string src);
        Task<bool> DeleteImageAsync(string imageUrl);
        
    }
}
