using System.Threading.Tasks;
using Oshop.API.Dtos;

namespace Oshop.API.Interfaces
{
    public interface ICartService
    {
         Task<Response> ClearCart(string userId);
         Task<Response> GetCart(string userId);
         Task<Response> AddToCart(string userId, ProductDto productDto);
         Task<Response> RemoveFromCart(string userId, ProductDto productDto);
    }
}