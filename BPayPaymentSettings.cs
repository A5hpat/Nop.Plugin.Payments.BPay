using Nop.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Payments.BPay
{
    public class BPayPaymentSettings : ISettings
    {
        public string DescriptionText { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether to "additional fee" is specified as percentage. true - percentage, false - fixed value.
        /// </summary>
        public bool AdditionalFeePercentage { get; set; }
        /// <summary>
        /// Additional fee
        /// </summary>
        public decimal AdditionalFee { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether shippable products are required in order to display this payment method during checkout
        /// </summary>
        public bool ShippableProductRequired { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether BPay RefNumber Base on OrderID or CustomerID
        /// </summary>
        public string RefNumberBaseOn { get; set; }

        /// <summary>
        /// Gets or sets a value indicating BPay Biller Code
        /// </summary>
        public string BillerCode { get; set; }
    }
}
