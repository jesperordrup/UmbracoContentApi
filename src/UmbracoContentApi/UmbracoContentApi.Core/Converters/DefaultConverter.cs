using System.Collections.Generic;

namespace UmbracoContentApi.Core.Converters
{
    public class DefaultConverter : IConverter
    {
        public string EditorAlias => "DefaultConverter";

        public object Convert(object value, Dictionary<string, object> options = null)
        {
            return value;
        }
    }
}
