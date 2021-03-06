using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.MarketingModule.Core.Model.Promotions
{
    public abstract class PromotionReward : ValueObject
    {
        public PromotionReward()
        {
            Id = GetType().Name;
        }
        //Copy constructor
        protected PromotionReward(PromotionReward other)
        {
            Id = GetType().Name;
            IsValid = other.IsValid;
            Description = other.Description;
            CouponAmount = other.CouponAmount;
            Coupon = other.Coupon;
            CouponMinOrderAmount = other.CouponMinOrderAmount;
            Promotion = other.Promotion;
        }

        public string Id { get; set; }
        /// <summary>
        /// Flag for applicability
        /// </summary>
        public bool IsValid { get; set; }
        /// <summary>
        /// Promo information. (user instructions, current promo description)
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Coupon amount
        /// </summary>
        public decimal CouponAmount { get; set; }
        /// <summary>
        /// Coupon
        /// </summary>
        public string Coupon { get; set; }

        /// <summary>
        /// Minimal amount in order to apply a coupon
        /// </summary>
        public decimal? CouponMinOrderAmount { get; set; }

        //Promotion 
        public string PromotionId { get; set; }

        public Promotion Promotion { get; set; }

    }
}
