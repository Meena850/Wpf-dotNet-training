using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace ToDoApplication.MarkUpExtension
{
    internal class PlaceHolderTextExtension : MarkupExtension
    {
        private readonly string _placeHolderString;
        public int TextLength { get; set; }
        public PlaceHolderTextExtension()
        {
            _placeHolderString = "Meena";
            TextLength = _placeHolderString.Length;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _placeHolderString.Substring(0, TextLength);
        }


    }
}
