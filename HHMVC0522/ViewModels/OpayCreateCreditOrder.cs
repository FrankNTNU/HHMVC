using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Web;

public class OpayCreateCreditOrder
    {
        public string MerchantID { get; set; } //測試用商家ID：2000132
        public string MerchantTradeNo { get; set; } //用日期時間毫秒來產生不重複的編號
        public string MerchantTradeDate { get; set; } //DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        public string PaymentType { get; set; } //固定使用"aio"
        public int TotalAmount { get; set; }
        public string TradeDesc { get; set; }
        public string ItemName { get; set; }
        public string ReturnURL { get; set; } //付款完成通知回傳網址
        public string ChoosePayment { get; set; } //"Credit"
        public string StoreID { get; set; } //使用空字串 ""
        public string ClientBackURL { get; set; } //完成訂單確認後要導回到的專題網站網址
        public string CreditInstallment { get; set; } //刷卡分期期數
        public string InstallmentAmount { get; set; } //使用刷卡分期的付款金額
        public string Redeem { get; set; } //信用卡是否使用紅利折抵 string (1)
        public int EncryptType { get; set; } //固定使用1

        private string _checkMacValue = "";
        public string CheckMacValue { get { return this.generateMacValue(); } }
        private string generateMacValue() //sha256加密
        {
            var prop_value = new Dictionary<string, string>();
            var sortedKey = new List<string>();
            var props = this.GetType().GetProperties();
            string data = "";
            foreach (var prop in props)
            {
                if (prop.Name != "CheckMacValue")
                {
                    if (prop.GetValue(this) == null)
                    {
                        prop_value[prop.Name] = "";
                    }
                    else
                    {
                        prop_value[prop.Name] = prop.GetValue(this).ToString();
                    }
                    sortedKey.Add(prop.Name);
                }
            }
            sortedKey.Sort();
            data += "HashKey=5294y06JbISpM5x9";
            foreach (var key in sortedKey)
            {
                data += "&";
                data += key;
                data += "=";
                data += prop_value[key];
            }
            data += "&HashIV=v77hoKGq4kWxNNIS";
            data = HttpUtility.UrlEncode(data).ToLower();
            this._checkMacValue = sha256_hash(data).ToUpper();
            return this._checkMacValue;
        }
        public static String sha256_hash(String value)
        {
            StringBuilder Sb = new StringBuilder();

            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                Byte[] result = hash.ComputeHash(enc.GetBytes(value));

                foreach (Byte b in result)
                    Sb.Append(b.ToString("x2"));
            }
            return Sb.ToString();
        }
    }
