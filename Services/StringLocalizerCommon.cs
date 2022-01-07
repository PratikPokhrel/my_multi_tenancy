using Core;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class StringLocalizerCommon : ICommonLocalizer
    {
        private readonly IStringLocalizerFactory _factory;
        public StringLocalizerCommon(IStringLocalizerFactory factory)
        {
            _factory = factory;
        }
        public IStringLocalizer Localize =>
            _factory.Create("Common.Common",Assembly.GetExecutingAssembly().GetName().Name);
    }
}
