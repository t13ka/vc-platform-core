using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.StoreModule.Core.Model.Search
{
    public class StoreSearchResult : GenericSearchResult<Store>
    {
        public IList<Store> Stores => Results;
    }
}
