using HotelApp.Interface.Services;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;

namespace HotelApp.Services
{
    public class ImageServece : IImageServece
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ImageServece(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            
        }
        public Task<List<string>> AddImagesAsync(IFormFileCollection files, string src)
        {
            var imageUrls = new List<string>();
            if (files == null || files.Count == 0)
            {
                throw new ArgumentException("No files provided for upload.");
            }
            if (string.IsNullOrEmpty(src))
            {
                throw new ArgumentException("Source path cannot be null or empty.");
            }
            var uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, src);
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }
            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var filePath = Path.Combine(uploadPath, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    imageUrls.Add(Path.Combine(src, fileName));
                }
            }
            return Task.FromResult(imageUrls);
        }

        public Task<bool> DeleteImageAsync(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
            {
                throw new ArgumentException("Image URL cannot be null or empty.");
            }
            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, imageUrl.TrimStart('/'));
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Image file not found.", filePath);
            }
            try
            {
                File.Delete(filePath);
                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                // Log the exception as needed
                return Task.FromResult(false);
            }
        }
    }
}
