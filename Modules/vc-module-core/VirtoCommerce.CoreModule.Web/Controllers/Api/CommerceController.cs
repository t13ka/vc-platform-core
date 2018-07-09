using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VirtoCommerce.CoreModule.Core.Model;
using VirtoCommerce.CoreModule.Core.Services;

namespace VirtoCommerce.CoreModule.Web.Controllers.Api
{
    [Route("api")]
    public class CommerceController : Controller
    {
        private readonly ISeoService _seoService;
        private readonly ICurrencyService _currencyService;
        //private readonly IStoreService _storeService;
        //private readonly ISeoDuplicatesDetector _seoDuplicateDetector;

        public CommerceController(ISeoService seoService, ICurrencyService currencyService/*, IStoreService storeService, ISeoDuplicatesDetector seoDuplicateDetector*/)
        {
            _seoService = seoService;
            _currencyService = currencyService;
            //_storeService = storeService;
            //_seoDuplicateDetector = seoDuplicateDetector;
        }


        ///// <summary>
        ///// Evaluate and return all tax rates for specified store and evaluation context 
        ///// </summary>
        ///// <param name="storeId"></param>
        ///// <param name="evalContext"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[ResponseType(typeof(coreTaxModel.TaxRate[]))]
        //[Route("taxes/{storeId}/evaluate")]
        //public IHttpActionResult EvaluateTaxes(string storeId, [FromBody]coreTaxModel.TaxEvaluationContext evalContext)
        //{
        //    var retVal = new List<coreTaxModel.TaxRate>();
        //    var store = _storeService.GetById(storeId);
        //    if (store != null)
        //    {
        //        var activeTaxProvider = store.TaxProviders.FirstOrDefault(x => x.IsActive);
        //        if (activeTaxProvider != null)
        //        {
        //            evalContext.Store = store;
        //            retVal.AddRange(activeTaxProvider.CalculateRates(evalContext));
        //        }
        //    }
        //    return Ok(retVal);
        //}

        /// <summary>
        /// Batch create or update seo infos
        /// </summary>
        /// <param name="seoInfos"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(void), 200)]
        [Route("seoinfos/batchupdate")]
        public async Task<IActionResult> BatchUpdateSeoInfos(SeoInfo[] seoInfos)
        {
            await _seoService.SaveSeoInfosAsync(seoInfos);
            return Ok();
        }

        [HttpGet]
        [ProducesResponseType(typeof(SeoInfo[]), 200)]
        [Route("seoinfos/duplicates")]
        public IActionResult GetSeoDuplicates(string objectId, string objectType)
        {
            //TODO
            return Ok(new SeoInfo[] { });
            //var retVal = _seoDuplicateDetector.DetectSeoDuplicates(objectType, objectId, _commerceService.GetAllSeoDuplicates());
            //return Ok(retVal.ToArray());
        }

        /// <summary>
        /// Find all SEO records for object by slug 
        /// </summary>
        /// <param name="slug">slug</param>
        [HttpGet]
        [ProducesResponseType(typeof(SeoInfo[]), 200)]
        [Route("seoinfos/{slug}")]
        public async Task<IActionResult> GetSeoInfoBySlug(string slug)
        {
            var retVal = await _seoService.GetSeoByKeywordAsync(slug);
            return Ok(retVal.ToArray());
        }

        /// <summary>
        /// Return all currencies registered in the system
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(Currency[]), 200)]
        [Route("currencies")]
        public async Task<IActionResult> GetAllCurrencies()
        {
            var retVal = await _currencyService.GetAllCurrenciesAsync();
            return Ok(retVal.ToArray());
        }

        /// <summary>
        ///  Update a existing currency 
        /// </summary>
        /// <param name="currency">currency</param>
        [HttpPut]
        [ProducesResponseType(typeof(void), 200)]
        [Route("currencies")]
        //TODO
        //[CheckPermission(Permission = CommercePredefinedPermissions.CurrencyUpdate)]
        public async Task<IActionResult> UpdateCurrency([FromBody]Currency currency)
        {
            await _currencyService.SaveChangesAsync(new[] { currency });
            return Ok();
        }

        /// <summary>
        ///  Create new currency 
        /// </summary>
        /// <param name="currency">currency</param>
        [HttpPost]
        [ProducesResponseType(typeof(void), 200)]
        [Route("currencies")]
        //TODO
        //[CheckPermission(Permission = CommercePredefinedPermissions.CurrencyCreate)]
        public async Task<IActionResult> CreateCurrency([FromBody]Currency currency)
        {
            await _currencyService.SaveChangesAsync(new[] { currency });
            return Ok();
        }

        /// <summary>
        ///  Delete currencies 
        /// </summary>
        /// <param name="codes">currency codes</param>
        [HttpDelete]
        [ProducesResponseType(typeof(void), 200)]
        [Route("currencies")]
        //TODO
        //[CheckPermission(Permission = CommercePredefinedPermissions.CurrencyDelete)]
        public async Task<IActionResult> DeleteCurrencies([FromQuery] string[] codes)
        {
            await _currencyService.DeleteCurrenciesAsync(codes);
            return Ok();
        }


        ///// <summary>
        ///// Return all package types registered in the system
        ///// </summary>
        //[HttpGet]
        //[ResponseType(typeof(coreModel.PackageType[]))]
        //[Route("packageTypes")]
        //public IHttpActionResult GetAllPackageTypes()
        //{
        //    var retVal = _commerceService.GetAllPackageTypes().ToArray();
        //    return Ok(retVal);
        //}

        ///// <summary>
        /////  Update a existing package type 
        ///// </summary>
        ///// <param name="packageType">package type</param>
        //[HttpPut]
        //[ResponseType(typeof(void))]
        //[Route("packageTypes")]
        //[CheckPermission(Permission = CommercePredefinedPermissions.PackageTypeUpdate)]
        //public IHttpActionResult UpdatePackageType(coreModel.PackageType packageType)
        //{
        //    _commerceService.UpsertPackageTypes(new[] { packageType });
        //    return StatusCode(HttpStatusCode.NoContent);
        //}

        ///// <summary>
        /////  Create new package type 
        ///// </summary>
        ///// <param name="packageType">package type</param>
        //[HttpPost]
        //[ResponseType(typeof(void))]
        //[Route("packageTypes")]
        //[CheckPermission(Permission = CommercePredefinedPermissions.PackageTypeCreate)]
        //public IHttpActionResult CreatePackageType(coreModel.PackageType packageType)
        //{
        //    _commerceService.UpsertPackageTypes(new[] { packageType });
        //    return StatusCode(HttpStatusCode.NoContent);
        //}

        ///// <summary>
        /////  Delete package types 
        ///// </summary>
        ///// <param name="ids">package type ids</param>
        //[HttpDelete]
        //[ResponseType(typeof(void))]
        //[Route("packageTypes")]
        //[CheckPermission(Permission = CommercePredefinedPermissions.PackageTypeDelete)]
        //public IHttpActionResult DeletePackageTypes([FromUri] string[] ids)
        //{
        //    _commerceService.DeletePackageTypes(ids);
        //    return StatusCode(HttpStatusCode.NoContent);
        //}
    }
}
