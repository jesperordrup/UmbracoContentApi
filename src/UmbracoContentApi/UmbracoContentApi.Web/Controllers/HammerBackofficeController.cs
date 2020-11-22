using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Http;
using System.Web.Http.Description;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web.WebApi;
using UmbracoContentApi.Core.Models;
using UmbracoContentApi.Core.Resolvers;

namespace UmbracoContentApi.Web.Controllers
{

    public class HammerBackofficeController : UmbracoAuthorizedApiController
    {
        private readonly Lazy<IContentResolver> _contentResolver;
        private readonly string _excludeDoctypes;


        public HammerBackofficeController(
            Lazy<IContentResolver> contentResolver)
        {
            _contentResolver = contentResolver;
            _excludeDoctypes = ConfigurationManager.AppSettings["HammerBackofficeExcludeDoctypes"] != null ? ConfigurationManager.AppSettings["HammerBackofficeExcludeDoctypes"] : "";
        }
      

     
        public IHttpActionResult Get(string id, int levels = 1)
        {

            IPublishedContent content = Umbraco.Content(id);
            var dictionary = new Dictionary<string, object>
            {
                { "addUrl", true }
            };
            dictionary.Add("level", levels);

            var contentResolved = _contentResolver.Value.ResolveContent(content, dictionary);
            if (_excludeDoctypes.Contains(content.ContentType.Alias))
                contentResolved.Fields = null;

            return Ok(new { content = contentResolved, children = content.Children != null && levels > 1 ? GetTree(content, false, levels - 1) : null });
        }
   
        public IHttpActionResult GetHeaders(int id, int levels = 1)
        {

            IPublishedContent content = Umbraco.Content(id);
            var dictionary = new Dictionary<string, object>
            {
                { "addUrl", true }
            };
            dictionary.Add("level", levels);

            var contentResolved = _contentResolver.Value.ResolveContent(content, dictionary);
            contentResolved.Fields = null;
         
            return Ok(new { content = contentResolved, children = content.Children != null && levels > 1 ? GetTree(content, true, levels - 1) : null });
        }
    
        private List<dynamic> GetTree(IPublishedContent content, bool excludeFields, int levels = 1)
        {
            var childModels = new List<dynamic>();

            foreach (var c in content.Children)
            {
                var dictionary = new Dictionary<string, object>{
                    { "addUrl", true }
                };
                dictionary.Add("levels", levels);
                var contentResolved = _contentResolver.Value.ResolveContent(c, dictionary);
                if (excludeFields || _excludeDoctypes.Contains(c.ContentType.Alias))
                    contentResolved.Fields = null;
                childModels.Add(new { content = contentResolved, children = c.Children != null && levels > 1 ? GetTree(c, excludeFields, levels - 1) : null });

            };
            return childModels;
        }
    }
}