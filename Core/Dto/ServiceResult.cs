using System.Collections.Generic;

namespace Core.Dto
{
    //public class ServiceResult : IServiceResult
    //{
    //    public string MessageType { get; set; }

    //    public List<string> Message { get; set; }

    //    public bool Status { get; set; }


    //    public ServiceResult(bool status, List<string> message = null, string messageType = null)
    //    {
    //        if (string.IsNullOrEmpty(messageType))
    //        {
    //            MessageType = Core.Dto.MessageType.Success;
    //        }
    //        else
    //        {
    //            MessageType = messageType;
    //        }

    //        if (message == null)
    //        {
    //            message = new List<string>();
    //        }

    //        Message = message;
    //        Status = status;
    //    }

    //}


    //public class ServiceResult<T> : ServiceResult
    //{

    //    private T ResposeData { get; set; }
    //    public T Data
    //    {
    //        get => this.ResposeData;
    //        set => this.ResposeData = value;
    //    }


    //    public ServiceResult(bool status, List<string> message = null, string messageType = null)
    //        : base(status, message, messageType)
    //    {
    //    }
    //}

}
