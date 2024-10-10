namespace Ensure.Entities.Constant;

public class Response
{
    public Response(string message, bool status)
    {
        this.message = message;
        this.status = status;
    }
    public bool status { get; }
    public string message { get; }
}

public class Response<T> : Response
{
    public Response(T data, string message,bool status) : base(
        message,
        status)
    {
        this.data = data;
    }
    public T data { get; }
}