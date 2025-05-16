namespace Pay.Recorrencia.Gestao.Application.Response
{
    public class ApiResponse
    {
        public ApiDataResponse data { get; set; }

        public ApiMetaDataResponse metadata { get; set; }

        public ApiResponse(ApiDataResponse ApiDataResponse, ApiMetaDataResponse ApiMetaDataResponse)
        {
            data = ApiDataResponse;
            metadata = ApiMetaDataResponse;
        }
    }
}
