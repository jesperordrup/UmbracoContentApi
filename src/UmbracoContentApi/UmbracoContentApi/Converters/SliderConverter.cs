﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UmbracoContentApi.Converters
{
    internal class SliderConverter : IConverter
    {
        public string EditorAlias => "Umbraco.Slider";

        public object Convert(object value)
        {
            return value;
        }
    }
}