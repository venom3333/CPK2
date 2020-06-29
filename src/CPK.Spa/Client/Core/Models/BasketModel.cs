using System.Collections.Generic;

namespace CPK.Spa.Client.Core.Models
{
    public class BasketModel
    {
        public List<LineModel> Lines { get; set; } = new List<LineModel>();
    }
}
