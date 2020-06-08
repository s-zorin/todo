namespace ToDo.Services.Results
{
    public class ServiceResult : IServiceResult
    {
        public bool IsOk { get; }

        public ServiceResult(bool isOk)
        {
            IsOk = isOk;
        }
    }

    public class ServiceResult<T> : ServiceResult, IServiceResult<T>
    {
        public T Value { get; }

        public ServiceResult(bool isOk, T value) : base(isOk)
        {
            Value = value;
        }
    }
}