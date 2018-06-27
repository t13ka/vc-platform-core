using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using VirtoCommerce.Platform.Core.PushNotifications;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.SearchModule.Core;
using VirtoCommerce.SearchModule.Core.Model;
using VirtoCommerce.SearchModule.Core.Services;
using VirtoCommerce.SearchModule.Web.BackgroundJobs;

namespace VirtoCommerce.SearchModule.Web.Controllers
{
    [Route("api/search/indexes")]
    [Produces("application/json")]
    public class SearchIndexationModuleController : Controller
    {
        private readonly IndexDocumentConfiguration[] _documentConfigs;
        private readonly ISearchProvider _searchProvider;
        private readonly IIndexingManager _indexingManager;
        private readonly IUserNameResolver _userNameResolver;
        private readonly IPushNotificationManager _pushNotifier;

        public SearchIndexationModuleController(IndexDocumentConfiguration[] documentConfigs, ISearchProvider searchProvider, IIndexingManager indexingManager, IUserNameResolver userNameResolver, IPushNotificationManager pushNotifier)
        {
            _documentConfigs = documentConfigs;
            _searchProvider = searchProvider;
            _indexingManager = indexingManager;
            _userNameResolver = userNameResolver;
            _pushNotifier = pushNotifier;
        }

        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(IndexState[]), 200)]
        public async Task<IActionResult> GetAllIndexes()
        {
            var documentTypes = _documentConfigs.Select(c => c.DocumentType).Distinct().ToList();
            var result = await Task.WhenAll(documentTypes.Select(_indexingManager.GetIndexStateAsync));
            return Ok(result);
        }

        /// <summary>
        /// Get search index for specified document type and document id.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("index/{documentType}/{documentId}")]
        [ProducesResponseType(typeof(IndexDocument[]), 200)]
        public async Task<IActionResult> GetDocumentIndexAsync(string documentType, string documentId)
        {
            var request = new SearchRequest
            {
                Filter = new IdsFilter
                {
                    Values = new[] { documentId },
                },
            };

            var result = await _searchProvider.SearchAsync(documentType, request);
            return Ok(result.Documents);
        }

        /// <summary>
        /// Run indexation process for specified options
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("index")]
        [ProducesResponseType(typeof(IndexProgressPushNotification), 200)]
        [Authorize(ModuleConstants.Security.Permissions.IndexRebuild)]
        public IActionResult IndexDocuments([FromBody] IndexingOptions[] options)
        {
            var currentUserName = _userNameResolver.GetCurrentUserName();
            var notification = IndexingJobs.Enqueue(currentUserName, options);
            _pushNotifier.Send(notification);
            return Ok(notification);
        }


        [HttpGet]
        [Route("tasks/{taskId}/cancel")]
        public IActionResult CancelIndexationProcess(string taskId)
        {
            IndexingJobs.CancelIndexation();
            return Ok();
        }
    }
}
