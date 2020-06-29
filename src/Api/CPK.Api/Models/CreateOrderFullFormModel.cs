using System.Collections.Generic;

namespace CPK.Api.Models
{
    public sealed class CreateOrderFullFormModel : CreateOrderShortModel
    {
        public List<LineModel> Lines { get; set; } = new List<LineModel>();
    }
}
