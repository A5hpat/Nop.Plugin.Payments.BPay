using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Nop.Core;
using Nop.Plugin.Payments.BPay.Models;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Payments;
using Nop.Services.Stores;
using Nop.Web.Framework.Controllers;

namespace Nop.Plugin.Payments.BPay.Controllers
{
    public class PaymentBPayController : BasePaymentController
    {
        private readonly IWorkContext _workContext;
        private readonly IStoreService _storeService;
        private readonly IStoreContext _storeContext;
        private readonly ISettingService _settingService;
        private readonly ILocalizationService _localizationService;
        private readonly ILanguageService _languageService;

        public PaymentBPayController(IWorkContext workContext,
            IStoreService storeService,
            ISettingService settingService,
            IStoreContext storeContext,
            ILocalizationService localizationService,
            ILanguageService languageService)
        {
            this._workContext = workContext;
            this._storeService = storeService;
            this._settingService = settingService;
            this._storeContext = storeContext;
            this._localizationService = localizationService;
            this._languageService = languageService;

        }

        [AdminAuthorize]
        [ChildActionOnly]
        public ActionResult Configure()
        {
            //load settings for a chosen store scope
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var bPayPaymentSettings = _settingService.LoadSetting<BPayPaymentSettings>(storeScope);

            var model = new ConfigurationModel();
            model.DescriptionText = bPayPaymentSettings.DescriptionText;
            //locales
            AddLocales(_languageService, model.Locales, (locale, languageId) =>
            {
                locale.DescriptionText = bPayPaymentSettings.GetLocalizedSetting(x => x.DescriptionText, languageId, false, false);
            });
            model.AdditionalFee = bPayPaymentSettings.AdditionalFee;
            model.AdditionalFeePercentage = bPayPaymentSettings.AdditionalFeePercentage;
            model.ShippableProductRequired = bPayPaymentSettings.ShippableProductRequired;
            model.BillerCode = bPayPaymentSettings.BillerCode;
            model.RefNumberBaseOn = bPayPaymentSettings.RefNumberBaseOn;

            model.ActiveStoreScopeConfiguration = storeScope;
            if (storeScope > 0)
            {
                model.DescriptionText_OverrideForStore = _settingService.SettingExists(bPayPaymentSettings, x => x.DescriptionText, storeScope);
                model.AdditionalFee_OverrideForStore = _settingService.SettingExists(bPayPaymentSettings, x => x.AdditionalFee, storeScope);
                model.AdditionalFeePercentage_OverrideForStore = _settingService.SettingExists(bPayPaymentSettings, x => x.AdditionalFeePercentage, storeScope);
                model.ShippableProductRequired_OverrideForStore = _settingService.SettingExists(bPayPaymentSettings, x => x.ShippableProductRequired, storeScope);
                model.BillerCode_OverrideForStore = _settingService.SettingExists(bPayPaymentSettings, x => x.BillerCode, storeScope);
                model.RefNumberBaseOn_OverrideForStore = _settingService.SettingExists(bPayPaymentSettings, x => x.RefNumberBaseOn, storeScope);
            }

            return View("~/Plugins/Payments.BPay/Views/PaymentBPay/Configure.cshtml", model);
        }

        [HttpPost]
        [AdminAuthorize]
        [ChildActionOnly]
        public ActionResult Configure(ConfigurationModel model)
        {
            if (!ModelState.IsValid)
                return Configure();

            //load settings for a chosen store scope
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var bPayPaymentSettings = _settingService.LoadSetting<BPayPaymentSettings>(storeScope);

            if (string.IsNullOrWhiteSpace(model.BillerCode))
            {
                ModelState.AddModelError("BillerCode", "Biller code Required!");
                return Configure();
            }
            // get valid values for RefNumberBaseOn
            List<string> allowdValueforRefNumberBaseOn = new List<string>();          
            foreach (var item in model.RefNumberBaseOnTypeList)
            {
                allowdValueforRefNumberBaseOn.Add(item.Value.ToLower());
            }
            if (string.IsNullOrWhiteSpace(model.RefNumberBaseOn) || (!allowdValueforRefNumberBaseOn.Contains(model.RefNumberBaseOn.ToLower())))
            {
                ModelState.AddModelError("RefNumberBaseOn", "Required! Allowed Value (OrderNumber or CustomerNumber)");
                return Configure();
            }
            //save settings
            bPayPaymentSettings.DescriptionText = model.DescriptionText;
            bPayPaymentSettings.AdditionalFee = model.AdditionalFee;
            bPayPaymentSettings.AdditionalFeePercentage = model.AdditionalFeePercentage;
            bPayPaymentSettings.ShippableProductRequired = model.ShippableProductRequired;
            bPayPaymentSettings.BillerCode = model.BillerCode;
            bPayPaymentSettings.RefNumberBaseOn = model.RefNumberBaseOn;

            /* We do not clear cache after each setting update.
             * This behavior can increase performance because cached settings will not be cleared 
             * and loaded from database after each update */
            if (model.DescriptionText_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(bPayPaymentSettings, x => x.DescriptionText, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(bPayPaymentSettings, x => x.DescriptionText, storeScope);

            if (model.AdditionalFee_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(bPayPaymentSettings, x => x.AdditionalFee, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(bPayPaymentSettings, x => x.AdditionalFee, storeScope);

            if (model.AdditionalFeePercentage_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(bPayPaymentSettings, x => x.AdditionalFeePercentage, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(bPayPaymentSettings, x => x.AdditionalFeePercentage, storeScope);

            if (model.ShippableProductRequired_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(bPayPaymentSettings, x => x.ShippableProductRequired, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(bPayPaymentSettings, x => x.ShippableProductRequired, storeScope);

            if (model.BillerCode_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(bPayPaymentSettings, x => x.BillerCode, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(bPayPaymentSettings, x => x.BillerCode, storeScope);

            if (model.RefNumberBaseOn_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(bPayPaymentSettings, x => x.RefNumberBaseOn, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(bPayPaymentSettings, x => x.RefNumberBaseOn, storeScope);

            //now clear settings cache
            _settingService.ClearCache();

            //localization. no multi-store support for localization yet.
            foreach (var localized in model.Locales)
            {
                bPayPaymentSettings.SaveLocalizedSetting(x => x.DescriptionText,
                    localized.LanguageId,
                    localized.DescriptionText);
            }

            SuccessNotification(_localizationService.GetResource("Admin.Plugins.Saved"));

            return Configure();
        }

        [ChildActionOnly]
        public ActionResult PaymentInfo()
        {
            var bPayPaymentSettings = _settingService.LoadSetting<BPayPaymentSettings>(_storeContext.CurrentStore.Id);

            var model = new PaymentInfoModel
            {
                DescriptionText = bPayPaymentSettings.GetLocalizedSetting(x => x.DescriptionText, _workContext.WorkingLanguage.Id)
            };

            return View("~/Plugins/Payments.BPay/Views/PaymentBPay/PaymentInfo.cshtml", model);
        }

        [NonAction]
        public override IList<string> ValidatePaymentForm(FormCollection form)
        {
            var warnings = new List<string>();
            return warnings;
        }
        [NonAction]
        public override ProcessPaymentRequest GetPaymentInfo(FormCollection form)
        {
            var paymentInfo = new ProcessPaymentRequest();
            return paymentInfo;
        }

        
    }
}
