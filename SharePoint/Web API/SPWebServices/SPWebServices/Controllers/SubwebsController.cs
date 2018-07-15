using Swashbuckle.Swagger.Annotations;
using System.Net;
using System.Web.Http;

namespace SPWebServices.Controllers
{
    public class SubwebsController : ApiController
    {
        /// <summary>
        /// RESTful API for retrieving all subwebs for current user.
        /// <example>
        /// Example 1:
        /// {
        ///     "Url"                     : "https://rivernet2ndev.trg.com/sites/litman",
        ///     "ExcludeKeysWhichContain" : ["__", "vti_"],
        ///     "ResultLimit"             : 5,
        ///     "PropertyValueQuery"      : 
        ///                                {
        ///                                    "LMUserID"           : ["KNEWT", "DBURN", "EKROB"],
        ///                                    "Matter_Status"      : "Closed"
        ///                                },
        ///     "MatchCaseOnQueryValues"  : true
        /// }
        ///
        /// Example 2:
        /// {
        ///     "Url"                     : "https://rivernet2ndev.trg.com/sites/litman",
        ///     "ExcludeKeysWhichContain" : ["__", "vti_", "cachedweb", "moss"],
        ///     "PropertyValueQuery"      : 
        ///                                 {
        ///                                     "Litigation_Manager": ["Quagmire, Glenn", "Joseph Swanson", "Griffin, Stewart"],
        ///                                     "Matter_Status"     : ["Open", "Stayed"],
        ///                                     "State_Filed"       : "CA"
        ///                                 },
        ///     "MatchCaseOnQueryValues"  : false
        /// }
        /// </example>
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost][Route("GetSubwebs")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(Models.WebDetails<object, object>[]))]
        public Models.WebDetails<object, object>[] GetSubwebs(Models.SubwebsRequest request)
        { return new Webs(request).getSubwebs(); }

        /// <summary>
        ///  POST: api/GetWebProperties 
        ///  Body: 
        ///  Syntax 1: { Url: "https://sharepoint/site/web", Properties:["Property1", "Property2"] }
        ///  Syntax 2: { Url: "https://sharepoint/site/web" } //returns all properties for the given website
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost][Route("GetWebProperties")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(Models.WebDetails<object, object>))]
        public Models.WebDetails<object, object> GetWebProperties(Models.WebPropertiesRequest request)
        { return new Webs(request).getWebProperties() ; }
    }
}
