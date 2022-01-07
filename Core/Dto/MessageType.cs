using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto
{
    public static class MessageType
    {
        public static string Success => "Success";
        public static string Info => "Info";
        public static string Danger => "Danger";
        public static string Warning => "Warning";
        public static string ValidationFailed => "ValidationFailed";
    }
}
