using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto
{
    #region "Interface"
    public interface IResult
    {
        List<string> Messages { get; set; }

        bool Succeeded { get; set; }

        public string MessageType { get; set; }
    }

    public interface IResult<out T> : IResult
    {
        T Data { get; }
    }

    #endregion
    public class Result : IResult
    {
        public Result()
        {
        }
        
        public Result(bool succeeded,List<string> messages)
        {
            Succeeded = succeeded;
            Messages = messages;
        }

       

        public List<string> Messages { get; set; } = new List<string>();

        public bool Succeeded { get; set; }
        public string MessageType { get; set; }

        public static IResult Fail()
        {
            return new Result { Succeeded = false };
        }

        public static IResult Fail(string message)
        {
            return new Result { Succeeded = false, Messages = new List<string> { message } };
        }

        public static IResult Fail(string message,string messageType)
        {
            return new Result { Succeeded = false,MessageType=messageType, Messages = new List<string> { message } };
        }

        public static IResult Fail(List<string> messages)
        {
            return new Result { Succeeded = false, Messages = messages };
        }

        public static IResult Fail(List<string> messages, string messageType)
        {
            return new Result { Succeeded = false,MessageType=messageType, Messages = messages };
        }

        public static Task<IResult> FailAsync()
        {
            return Task.FromResult(Fail());
        }

        public static Task<IResult> FailAsync(string message)
        {
            return Task.FromResult(Fail(message));
        }


        public static Task<IResult> FailAsync(string message, string messageType)
        {
            return Task.FromResult(Fail(message,messageType));
        }

        public static Task<IResult> FailAsync(List<string> messages)
        {
            return Task.FromResult(Fail(messages));
        }

        public static Task<IResult> FailAsync(List<string> messages,string messageType)
        {
            return Task.FromResult(Fail(messages,messageType));
        }

        public static IResult Success()
        {
            return new Result { Succeeded = true ,MessageType=MessageTypes.Success};
        }

        public static IResult Success(string message)
        {
            return new Result { Succeeded = true, MessageType = MessageTypes.Success, Messages = new List<string> { message } };
        }

        public static Task<IResult> SuccessAsync()
        {
            return Task.FromResult(Success());
        }

        public static Task<IResult> SuccessAsync(string message)
        {
            return Task.FromResult(Success(message));
        }
    }

    public class Result<T> : Result, IResult<T>
    {
        public Result()
        {
        }

        public Result(bool succeded, List<string> messages,string messageType)
        {
            Succeeded = succeded;
            Messages = messages;
            MessageType = messageType;
        }
        public Result(bool succeded,T data, List<string> messages, string messageType)
        {
            Succeeded = succeded;
            Data = data;
            Messages = messages;
            MessageType = messageType;
        }

        public T Data { get; set; }

        public new static Result<T> Fail()
        {
            return new Result<T> { Succeeded = false };
        }

        public new static Result<T> Fail(string message)
        {
            return new Result<T> { Succeeded = false, Messages = new List<string> { message } };
        }
        public new static Result<T> Fail(string message, string messageType)
        {
            return new Result<T> { Succeeded = false,MessageType=messageType, Messages = new List<string> { message } };
        }
        public new static Result<T> Fail(List<string> messages)
        {
            return new Result<T> { Succeeded = false, Messages = messages };
        }

        public new static Result<T> Fail(List<string> messages,string messageType)
        {
            return new Result<T> { Succeeded = false,MessageType=messageType, Messages = messages };
        }

        public new static Task<Result<T>> FailAsync()
        {
            return Task.FromResult(Fail());
        }

        public new static Task<Result<T>> FailAsync(string message)
        {
            return Task.FromResult(Fail(message));
        }

        public new static Task<Result<T>> FailAsync(List<string> messages)
        {
            return Task.FromResult(Fail(messages));
        }

        public new static Task<Result<T>> FailAsync(List<string> messages,string messageType)
        {
            return Task.FromResult(Fail(messages,messageType));
        }

        public new static Result<T> Success()
        {
            return new Result<T> { Succeeded = true,MessageType=MessageTypes.Success };
        }

        public new static Result<T> Success(string message)
        {
            return new Result<T> { Succeeded = true, Messages = new List<string> { message }, MessageType = MessageTypes.Success };
        }

        public static Result<T> Success(T data)
        {
            return new Result<T> { Succeeded = true, Data = data, MessageType = MessageTypes.Success };
        }

        public static Result<T> Success(T data, string message)
        {
            return new Result<T> { Succeeded = true, Data = data, Messages = new List<string> { message }, MessageType = MessageTypes.Success };
        }

        public static Result<T> Success(T data, List<string> messages)
        {
            return new Result<T> { Succeeded = true, Data = data, Messages = messages, MessageType = MessageTypes.Success };
        }

        public new static Task<Result<T>> SuccessAsync()
        {
            return Task.FromResult(Success());
        }

        public new static Task<Result<T>> SuccessAsync(string message)
        {
            return Task.FromResult(Success(message));
        }

        public static Task<Result<T>> SuccessAsync(T data)
        {
            return Task.FromResult(Success(data));
        }

        public static Task<Result<T>> SuccessAsync(T data, string message)
        {
            return Task.FromResult(Success(data, message));
        }
    }

    public static class MessageTypes
    {
        public static string Success = "Success";
        public static string Warning = "Warning";
        public static string Danger = "Danger";
    }
}
