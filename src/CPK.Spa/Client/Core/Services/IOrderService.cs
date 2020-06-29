using System.Collections.Generic;
using System.Threading.Tasks;
using CPK.Spa.Client.Core.Models;

namespace CPK.Spa.Client.Core.Services
{
    public interface IOrderService
    {
        string Error { get; }
        IReadOnlyList<OrderModel> Orders { get; }
        Task Create(IEnumerable<LineModel> lines, string address);
        Task Load();
    }
}