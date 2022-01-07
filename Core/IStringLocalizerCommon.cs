using Microsoft.Extensions.Localization;

namespace Core
{
    public interface ICommonLocalizer
    {
        public IStringLocalizer Localize { get; }
    }
}
