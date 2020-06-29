using System.Threading.Tasks;

namespace CPK.Spa.Client.Core.HttpContext
{
    public interface IHttpContext
    {
        Task<(T, string)> PostAsync<T>(string url, object data, bool auth = true);
        Task<(T, string)> GetAsync<T>(string url, bool auth = true);
        Task<(T, string)> DeleteAsync<T>(string url, bool auth = true);
        Task<(T, string)> PutAsync<T>(string url, object data, bool auth = true);
    }
}