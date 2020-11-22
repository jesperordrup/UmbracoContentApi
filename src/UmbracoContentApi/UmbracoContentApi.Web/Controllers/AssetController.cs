using System;
using System.Web.Http;
using System.Web.Http.Description;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web.WebApi;
using UmbracoContentApi.Core.Models;
using UmbracoContentApi.Core.Resolvers;

namespace UmbracoContentApi.Web.Controllers
{
    [RoutePrefix("api/assets")]
    public class AssetController : UmbracoApiController
    {
        private readonly IMediaResolver _mediaResolver;

        public AssetController(
            IMediaResolver mediaResolver)
        {
            _mediaResolver = mediaResolver;
        }

        [Route("{id:guid}")]
        [ResponseType(typeof(ContentModel))]
        public IHttpActionResult Get(string id)
        {
            IPublishedContent media = Umbraco.Media(id);
            AssetModel mediaModel = _mediaResolver.ResolveMedia(media);

            return Ok(mediaModel);
        }
    }
}