namespace EventFlow.API.Helpers
{
    public class IResponseModel<T> //how will it work interms of inheriing or mapping
    {
        public T? Data { get; set; }
        public string  Message { get; set; }
        public bool Error { get; set; } = false;
    }
}
