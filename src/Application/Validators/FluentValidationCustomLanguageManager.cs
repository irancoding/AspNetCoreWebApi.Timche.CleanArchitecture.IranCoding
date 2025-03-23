using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators
{
    public class FluentValidationCustomLanguageManager : FluentValidation.Resources.LanguageManager
    {
        public FluentValidationCustomLanguageManager()
        {
            Culture = new System.Globalization.CultureInfo("fa-IR");

            AddTranslation("fa-IR", "NotEmptyValidator", "{PropertyName} الزامی است");
        }
    }
}
