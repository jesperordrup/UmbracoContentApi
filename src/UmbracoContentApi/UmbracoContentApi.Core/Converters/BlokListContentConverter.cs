using System;
using System.Collections.Generic;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.Models.Blocks;
using UmbracoContentApi.Core.Models;
using UmbracoContentApi.Core.Resolvers;

namespace UmbracoContentApi.Core.Converters
{
    public class BlokListContentConverter : IConverter
    {
        private readonly Lazy<IContentResolver> _contentResolver;

        public BlokListContentConverter(Lazy<IContentResolver> contentResolver)
        {
            _contentResolver = contentResolver;
        }

        public string EditorAlias => "Umbraco.BlockList";

        public object Convert(object value, Dictionary<string, object> options = null)
        {

            try
            {

                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value), $"A value for {EditorAlias} is required.");
                }

                var models = new List<ContentModel>();

                foreach (var block in (IEnumerable<BlockListItem>)value)
                {

                    ContentModel content = _contentResolver.Value.ResolveContent(block.Content, null);
                    content.Settings = block.Settings != null ? _contentResolver.Value.ResolveContent(block.Settings, null) : null;
                    models.Add(content);

                }



                return models;
            }
            catch (Exception e)
            {

                throw;
            }
        
        }
    }
}