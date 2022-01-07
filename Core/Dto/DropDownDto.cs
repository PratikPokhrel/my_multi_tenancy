using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto
{
    public class DropDownDto<T>
    {
        public DropDownDto()
        {

        }
        public DropDownDto(T value, string text)
        {
            Value = value;
            Text = text;
        }
        public T Value { get; set; }
        public string Text { get; set; }
    }
}
