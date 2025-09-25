namespace TodoApi.DTOs
{
    public class ApiResponse<T>
    {
        public int StatusCode { get; set; } = 200;
        public string ErrorMessage { get; set; } = string.Empty;
        public T Data { get; set; }
    }
}
