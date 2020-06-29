namespace CPK.Spa.Client.Models
{
    public sealed class PaginatorModel
    {
        public int Size { get; set; }
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; } = 10;
        public int ItemsTotalCount { get; set; }
    }
}