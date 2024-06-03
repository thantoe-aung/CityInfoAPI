namespace CityInfoAPI.Models
{
    public class PaginationMetaData
    {
        public int TotalItemCount { get; set; }

        public int TotalPageCount { get; set; } 

        public int PageSize { get; set; }

        public int CurrentPage { get; set; }

        public PaginationMetaData(int totalItemCount,int pageSize,int currentPage)
        {
            TotalItemCount = totalItemCount;
            TotalPageCount = pageSize;
            CurrentPage = currentPage;

            TotalPageCount =(int)Math.Ceiling(TotalItemCount / (double)pageSize);
        }
    }
}
