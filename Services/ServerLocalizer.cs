using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    internal class ServerLocalizer<T> where T : class
    {
        public IStringLocalizer<T> Localizer { get; }
        public ServerLocalizer(IStringLocalizer<T> localizer)
        {
            Localizer = localizer;
        }
    }
}
