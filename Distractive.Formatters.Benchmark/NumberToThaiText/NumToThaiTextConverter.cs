using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumberToThaiText
{
    public class NumToThaiTextConverter
    {
        private ThaiText thaiText = new ThaiText { Interger=string.Empty,Satang=string.Empty  };

        public ThaiText Convert(string strNumber, bool bahtSuffix,bool satangSuffix, bool IsTrillion = false)
        {
            ThaiBahtText(strNumber, IsTrillion);

            if (bahtSuffix && !string.IsNullOrEmpty(this.thaiText.Interger))
            {
                this.thaiText.Interger = this.thaiText.Interger + "บาท";
            }

            if (satangSuffix && !string.IsNullOrEmpty(this.thaiText.Satang))
            {
                this.thaiText.Satang = this.thaiText.Satang + "สตางค์";
            }

            return this.thaiText;
        }
        private string ThaiBahtText(string strNumber, bool IsTrillion = false)
        {            
            string BahtText = "";
            string strTrillion = "";
            string[] strThaiNumber = { "ศูนย์", "หนึ่ง", "สอง", "สาม", "สี่", "ห้า", "หก", "เจ็ด", "แปด", "เก้า", "สิบ" };
            string[] strThaiPos = { "", "สิบ", "ร้อย", "พัน", "หมื่น", "แสน", "ล้าน" };

            decimal decNumber = 0;
            decimal.TryParse(strNumber, out decNumber);

            //if (decNumber == 0)
            //{
            //    return "ศูนย์บาทถ้วน";
            //}
            if (decNumber == 0)
            {
                return "ศูนย์";
            }

            strNumber = decNumber.ToString("0.00");
            string strInteger = strNumber.Split('.')[0];
            string strSatang = strNumber.Split('.')[1];

            if (strInteger.Length > 13)
                throw new Exception("Whole number part exceed 13 digits");

            bool _IsTrillion = strInteger.Length > 7;
            if (_IsTrillion)
            {
                strTrillion = strInteger.Substring(0, strInteger.Length - 6);
                BahtText = ThaiBahtText(strTrillion, _IsTrillion);
                strInteger = strInteger.Substring(strTrillion.Length);
            }

            int strLength = strInteger.Length;
            for (int i = 0; i < strInteger.Length; i++)
            {
                string number = strInteger.Substring(i, 1);
                if (number != "0")
                {
                    if (i == strLength - 1 && number == "1" && strLength != 1)
                    {
                        BahtText += "เอ็ด";
                    }
                    else if (i == strLength - 2 && number == "2" && strLength != 1)
                    {
                        BahtText += "ยี่";
                    }
                    else if (i != strLength - 2 || number != "1")
                    {
                        BahtText += strThaiNumber[int.Parse(number)];
                    }

                    BahtText += strThaiPos[(strLength - i) - 1];
                }
            }

            if (IsTrillion)
            {
                return BahtText + "ล้าน";
            }

            //if (strInteger != "0")
            //{
            //    BahtText += "บาท";
            //}
            if (strInteger != "0")
            {
                BahtText += "";
            }

            //if (strSatang == "00")
            //{
            //    BahtText += "ถ้วน";
            //}
            if (strSatang == "00")
            {
                BahtText += "";
                thaiText.Interger = BahtText;
            }
            else
            {
                thaiText.Interger = BahtText;
                strLength = strSatang.Length;
                var satang = string.Empty;
                for (int i = 0; i < strSatang.Length; i++)
                {
                    string number = strSatang.Substring(i, 1);
                    if (number != "0")
                    {
                        if (i == strLength - 1 && number == "1" && strSatang[0].ToString() != "0")
                        {
                            //BahtText += "เอ็ด";
                            satang += "เอ็ด";
                        }
                        else if (i == strLength - 2 && number == "2" && strSatang[0].ToString() != "0")
                        {
                            //BahtText += "ยี่";
                            satang += "ยี่";
                        }
                        else if (i != strLength - 2 || number != "1")
                        {
                            //BahtText += strThaiNumber[int.Parse(number)];
                            satang += strThaiNumber[int.Parse(number)];
                        }

                        //BahtText += strThaiPos[(strLength - i) - 1];
                        satang += strThaiPos[(strLength - i) - 1];
                    }
                }

                thaiText.Satang = satang;

                //BahtText += "สตางค์";
            }

            return BahtText;
        }
    }
}
