namespace MoviesAPI.Utilities
{
    public class ServiceResult<T>
    {
        public ServiceResult(T result)
        {
            Result = result;
            Success = true;
        }

        public ServiceResult(string message)
        {
            Message = message;
        }

        public T Result { get; }
        public bool Success { get; }
        public string Message { get; }
    }
}
