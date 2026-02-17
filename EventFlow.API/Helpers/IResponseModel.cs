namespace EventFlow.API.Helpers
{
    public class IResponseModel<T> //Class type not object
    {
        public T? Data { get; set; }
        public string  Message { get; set; }
        public bool Error { get; set; } = false;
    }
}
