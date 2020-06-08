namespace ToDo.Services.Results
{
    public interface IServiceResult
    {
        bool IsOk { get; }
    }

    public interface IServiceResult<T> : IServiceResult
    {
        T Value { get; }
    }
}