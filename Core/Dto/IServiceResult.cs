using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto
{
    public interface IServiceResult
    {
        bool Status { get; set; }
        List<string> Message { get; set; }
        string MessageType { get; set; }
    }
}
