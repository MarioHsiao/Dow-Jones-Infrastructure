using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace DowJones.Web.Mvc.UI
{
    public class EnumSelectList<TEnum> : List<SelectListItem>
        where TEnum : struct
    {
        public EnumSelectList()
            : this(default(TEnum), null)
        {
        }

        public EnumSelectList(TEnum selectedValue, IEnumerable<TEnum> exclusions = null)
            : base(CreateListItems(selectedValue, exclusions))
        {
        }

        private static IEnumerable<SelectListItem> CreateListItems(TEnum selectedValue, IEnumerable<TEnum> exclusions)
        {
            var keyValuePairs = EnumHelper.ToKeyValuePairs<TEnum>(exclusions);
            return keyValuePairs.ToSelectList(selectedValue);
        }
    }

    public class EnumSelectListWithTranslatedText<TEnum> : List<SelectListItem>
        where TEnum : struct
    {
        public EnumSelectListWithTranslatedText()
            : this(default(TEnum), null)
        {
        }

        public EnumSelectListWithTranslatedText(TEnum selectedValue, IEnumerable<TEnum> exclusions = null)
            : base(CreateListItems(selectedValue, exclusions))
        {
        }

        private static IEnumerable<SelectListItem> CreateListItems(TEnum selectedValue, IEnumerable<TEnum> exclusions)
        {
            var keyValuePairs = EnumHelper.ToKeyValuePairsWithTranslatedValue<TEnum>(exclusions);
            return keyValuePairs.ToSelectList(selectedValue.ToString());
        }
    }    
                                                 
}
