using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace EBook.DataAccess.Services;
public interface IPhotoService
{
    ImageUploadResult AddPhoto(IFormFile file);
    DeletionResult DeletePhoto(string imageUrl);
}

