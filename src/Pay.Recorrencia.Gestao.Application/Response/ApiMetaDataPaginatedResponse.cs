namespace Pay.Recorrencia.Gestao.Application.Response
{
    public class Pagination
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
    }
    public class ItemsData
    {
        public IEnumerable<dynamic> Items { get; set; }
        public Pagination Pagination { get; set; }
    }
    public class ItemsDataNonPaginated
    {
        public IEnumerable<dynamic> Items { get; set; }
    }
    public class ApiMetaDataPaginatedResponse
    {
        public int StatusCode { get; set; }
        public string Status { get; set; }
        public ItemsData Data { get; set; }
        public string Message { get; set; }
    }
    public class ApiMetaDataNonPaginatedResponse<T> : ApiMetaDataPaginatedResponse
    {
        public new T Data { get; set; }
    }
    public class TypedItemsData<T>
    {
        public IEnumerable<T> Items { get; set; }
        public Pagination Pagination { get; set; }
    }

    public class TypedApiMetaDataPaginatedResponse<T> : ApiMetaDataPaginatedResponse
    {
        public new TypedItemsData<T> Data { get; set; }
    }
    public class TypedApiMetaDataNonPaginatedResponse<T> : ApiMetaDataNonPaginatedResponse<T>
    {
        public new T Data { get; set; }
    }

    public class Error
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public string Status { get; set; }
        public Error Error { get; set; }
    }
}