using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Markup;

namespace Transleet.Desktop
{
    public class TypeGetter
    {
        public Type Type { get; set; }

        public static implicit operator Type(TypeGetter typeGetter)=>typeGetter.Type;
    }
}
