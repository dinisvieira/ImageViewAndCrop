using System.Threading.Tasks;

namespace ImageCrop.Services
{
    public interface IImageCropper
    {
        Task<byte[]> CropImage(string path);
    }
}
