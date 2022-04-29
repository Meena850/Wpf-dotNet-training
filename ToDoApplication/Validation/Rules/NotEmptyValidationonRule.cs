using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ToDoApplication.Validation.Rules
{
    internal class NotEmptyValidationonRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value is string str)
            {
                if (!String.IsNullOrWhiteSpace(str))
                {
                    return ValidationResult.ValidResult;
                }
                else
                {
                    return new ValidationResult(false, "This Field cannot be Empty or WhiteSpace.");
                }
            }
            else
            {
                return new ValidationResult(false, "Value must be a string");
            }
        }
    }
}
