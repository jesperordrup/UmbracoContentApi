using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web.WebApi;
using UmbracoContentApi.Core.Models;
using UmbracoContentApi.Core.Resolvers;

namespace UmbracoContentApi.Web.Controllers
{
    [RoutePrefix("api/content")]
    public class ContentApiController : UmbracoAuthorizedApiController
    {
        private readonly Lazy<IContentResolver> _contentResolver;

        public ContentApiController(
            Lazy<IContentResolver> contentResolver)
        {
            _contentResolver = contentResolver;
        }
        [ResponseType(typeof(ContentModel))]
        public IHttpActionResult GetById(int id)
        {
            IPublishedContent content = Umbraco.Content(id);
            var dictionary = new Dictionary<string, object>
            {
                { "addUrl", true }
            };
            return Ok(_contentResolver.Value.ResolveContent(content, dictionary));
        }

        public IHttpActionResult GetFullTree(int id, int level = 1)
        {

            IPublishedContent content = Umbraco.Content(id);
            var dictionary = new Dictionary<string, object>
            {
                { "addUrl", true }
            };
            dictionary.Add("level", level);
            var root = _contentResolver.Value.ResolveContent(content, dictionary);
            var res = new { content = root, children = content.Children != null && level > 1 ? GetTree(content, false, level - 1) : null };
          
            return Ok(res);
         


        }

        public IHttpActionResult GetSystemTree(int id, int level = 1)
        {

            IPublishedContent content = Umbraco.Content(id);
            var dictionary = new Dictionary<string, object>
            {
                { "addUrl", true }
            };
            dictionary.Add("level", level);
            var contentResolved = _contentResolver.Value.ResolveContent(content, dictionary);
            contentResolved.Fields = null;
            var res = new { content = contentResolved, children = content.Children != null && level > 1 ? GetTree(content, true, level - 1) : null };

            return Ok(res);



        }
        private List<dynamic> GetTree(IPublishedContent content, bool excludeFields, int level = 1)
        {
            var childModels = new List<dynamic>();

            foreach (var c in content.Children)
            {
                var dictionary = new Dictionary<string, object>{
                    { "addUrl", true }
                };
                dictionary.Add("level", level);
                var contentResolved =  _contentResolver.Value.ResolveContent(c, dictionary);
                if (excludeFields) 
                    contentResolved.Fields = null;
                childModels.Add(new { content = contentResolved, children = c.Children != null && level > 1 ? GetTree(c, excludeFields, level - 1) : null }); 

            };

            return childModels;
        }

        [Route("{id:guid}")]
        [ResponseType(typeof(ContentModel))]
        public IHttpActionResult Get(Guid id, int level = 0)
        {
            IPublishedContent content = Umbraco.Content(id);
            var dictionary = new Dictionary<string, object>
            {
                { "addUrl", true }
            };

            if (level <= 0)
            {
                return Ok(_contentResolver.Value.ResolveContent(content, dictionary));
            }

            dictionary.Add("level", level);

            return Ok(_contentResolver.Value.ResolveContent(content, dictionary));
        }
    }
}