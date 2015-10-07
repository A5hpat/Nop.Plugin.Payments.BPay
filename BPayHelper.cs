using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Payments.BPay
{
    public class BPayHelper
    {
        /// <summary>
        /// Gets a BPAY Customer Reference Number (CRN)
        /// </summary>
        /// <param name="Number">Generate CRN from number</param>      
        /// <returns>CRN number</returns>
        /// 
        public static string GenerateBPayCRN(string Number)
        {
            int checkDigit = 0;
            int digit = 0;

            if (Number.Length < 2)
            {
                Number = Number.PadLeft(2, '0');
            }         

            try
            {
                for (int i = 0; i < Number.Length; i++)
                {
                    digit = int.Parse(Number.Substring(i, 1));
                    checkDigit += digit * (i + 1);
                }
                checkDigit = checkDigit % 10;
            }
            catch
            {
            }
            return Number + checkDigit.ToString();            
        }
    }
}
