using System;

namespace VirtoCommerce.CoreModule.Core.Common
{
    [Flags]
    public enum AddressType
    {
        Undef = 0,
        Billing = 1,
        Shipping = 2,
        Pickup = 3,
        BillingAndShipping = Billing | Shipping
    }
}
