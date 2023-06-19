using System.CodeDom.Compiler;

namespace CodeFly.DTO;

public class Result<T> where T : class
{
    public static Result<T> GenerateSuccess(T data)
    {
        return new Result<T>() { Data = data, Error = "", Status = 200 };
    }

    public static Result<T> GenerateFailure(string message, int status = 500)
    {
        return new Result<T>() { Data = null, Error = message, Status = status };
    }

    public T Data { get; set; }
    public string Error { get; set; }
    public int Status { get; set; }
}