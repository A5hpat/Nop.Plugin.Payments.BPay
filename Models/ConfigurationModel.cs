using System.Collections.Generic;
using System.Web.Mvc;
using Nop.Web.Framework;
using Nop.Web.Framework.Localization;
using Nop.Web.Framework.Mvc;

namespace Nop.Plugin.Payments.BPay.Models
{
    public class ConfigurationModel : BaseNopModel, ILocalizedModel<ConfigurationModel.ConfigurationLocalizedModel>
    {
        public ConfigurationModel()
        {
            Locales = new List<ConfigurationLocalizedModel>();
        }

        public int ActiveStoreScopeConfiguration { get; set; }

        [AllowHtml]
        [NopResourceDisplayName("Plugins.Payment.BPay.DescriptionText")]
        public string DescriptionText { get; set; }
        public bool DescriptionText_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Payment.BPay.AdditionalFee")]
        public decimal AdditionalFee { get; set; }
        public bool AdditionalFee_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Payment.BPay.AdditionalFeePercentage")]
        public bool AdditionalFeePercentage { get; set; }
        public bool AdditionalFeePercentage_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Payment.BPay.ShippableProductRequired")]
        public bool ShippableProductRequired { get; set; }
        public bool ShippableProductRequired_OverrideForStore { get; set; }

        [AllowHtml]
        [NopResourceDisplayName("Plugins.Payment.BPay.BillerCode")]
        public string BillerCode { get; set; }
        public bool BillerCode_OverrideForStore { get; set; }

        [AllowHtml]
        [NopResourceDisplayName("Plugins.Payment.BPay.RefNumberBaseOn")]
        public string RefNumberBaseOn { get; set; }
        public bool RefNumberBaseOn_OverrideForStore { get; set; }

        public IEnumerable<SelectListItem> RefNumberBaseOnTypeList
        {
            get
            {
                return new List<SelectListItem>
                {
                    new SelectListItem { Text = "OrderNumber", Value = "OrderNumber"},
                    new SelectListItem { Text = "CustomerNumber", Value = "CustomerNumber"}
                };
            }
        }

        public IList<ConfigurationLocalizedModel> Locales { get; set; }

        #region Nested class

        public partial class ConfigurationLocalizedModel : ILocalizedModelLocal
        {
            public int LanguageId { get; set; }

            [AllowHtml]
            [NopResourceDisplayName("Plugins.Payment.BPay.DescriptionText")]
            public string DescriptionText { get; set; }
        }

        #endregion

    }
}