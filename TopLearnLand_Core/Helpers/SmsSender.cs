//using Gheytaran.Data;
//using Microsoft.EntityFrameworkCore;
//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Text.Encodings.Web;
//using System.Threading.Tasks;

//namespace Helpers
//{
//    public class SMSSender
//    {
//        private readonly IHttpClientFactory _clientFactory;

//        public SMSSender(IHttpClientFactory clientFactory)
//        {
//            _clientFactory = clientFactory;
//        }

//        public async Task<string> SendPattern(string receiver, string message, string type)
//        {
//            string from;
//            string uname;
//            string pass;
//            string domain;
//            string patternCode = "";
//            string input_data = "";

//            using (var db = new ApplicationDbContext())
//            {
//                var c = db.Config.Select(a => new { a.SmsCenter, a.SmsPass, a.SmsUser, a.Domain }).AsNoTracking().FirstOrDefault();
//                from = c.SmsCenter;
//                uname = c.SmsUser;
//                pass = c.SmsPass;
//                domain = c.Domain;
//            }

//            if (type.Equals("activate"))
//            {
//                patternCode = "msyylckn6v";
//                input_data = JsonConvert.SerializeObject(new Dictionary<string, string>
//                {
//                    ["code"] = message
//                });
//            }
//            else if (type.Equals("order-admin"))
//            {
//                patternCode = "plbtckfs8x";
//                input_data = JsonConvert.SerializeObject(new Dictionary<string, string>
//                {
//                    ["number"] = message
//                });
//            }
//            else if (type.Equals("reset"))
//            {
//                patternCode = "fqy7yecw8e";
//                input_data = JsonConvert.SerializeObject(new Dictionary<string, string>
//                {
//                    ["code"] = message
//                });
//            }
//            else if (type.Equals("order0"))
//            {
//                patternCode = "5cy4lopq26";
//                var msg = message.Split('|');
//                input_data = JsonConvert.SerializeObject(new Dictionary<string, string>
//                {
//                    ["order-number"] = msg[0],
//                    ["customer-name"] = msg[1],
//                    ["link"] = $"{domain}/profile/order"
//                });

//            }
//            else if (type.Equals("order1"))
//            {
//                patternCode = "gqtu379dxa";
//                var msg = message.Split('|');
//                input_data = JsonConvert.SerializeObject(new Dictionary<string, string>
//                {
//                    ["order-number"] = msg[0],
//                    ["customer-name"] = msg[1],
//                    ["link"] = $"{domain}/profile/order"
//                });

//            }
//            else if (type.Equals("order2"))
//            {
//                patternCode = "0ulzx436v6";
//                var msg = message.Split('|');
//                input_data = JsonConvert.SerializeObject(new Dictionary<string, string>
//                {
//                    ["order-number"] = msg[0],
//                    ["customer-name"] = msg[1],
//                    ["link"] = $"{domain}/profile/order"
//                });
//            }
//            string to = JsonConvert.SerializeObject(new string[] { receiver });

//            string url = $"http://188.0.240.110/patterns/pattern?username={uname}&password={UrlEncoder.Default.Encode(pass)}&from={from}&to={to}&input_data={UrlEncoder.Default.Encode(input_data)}&pattern_code={patternCode}";
//            var request = new HttpRequestMessage(HttpMethod.Post, url);
//            using var client = _clientFactory.CreateClient();
//            var response = await client.SendAsync(request);
//            if (response.IsSuccessStatusCode)
//            {
//                response.Dispose();
//                return "ok";
//            }
//            else
//            {
//                response.Dispose();
//                return "nok";
//            }
//        }

//        public async Task<string> SendPattern(string receiver, string patternCode, string input_data, string smsCenter, string smsPass, string smsUser)
//        {
//            string to = JsonConvert.SerializeObject(new string[] { receiver });
//            string url = $"http://188.0.240.110/patterns/pattern?username={smsUser}&password={UrlEncoder.Default.Encode(smsPass)}&from={smsCenter}&to={to}&input_data={UrlEncoder.Default.Encode(input_data)}&pattern_code={patternCode}";
//            var request = new HttpRequestMessage(HttpMethod.Post, url);
//            using var client = _clientFactory.CreateClient();
//            var response = await client.SendAsync(request);
//            if (response.IsSuccessStatusCode)
//            {
//                response.Dispose();
//                return "ok";
//            }
//            else
//            {
//                response.Dispose();
//                return "nok";
//            }
//        }

//        public string SendSms(string reciever, string message)
//        {
//            string from = "";
//            string uname = "";
//            string pass = "";

//            using (var db = new ApplicationDbContext())
//            {
//                var c = db.Config.Select(a => new { a.SmsCenter, a.SmsPass, a.SmsUser }).AsNoTracking().FirstOrDefault();
//                from = c.SmsCenter;
//                uname = c.SmsUser;
//                pass = c.SmsPass;
//            }

//            string json = "";
//            json = JsonConvert.SerializeObject(reciever);
//            WebRequest request = WebRequest.Create("http://ippanel.com/services.jspd");
//            request.Method = "POST";
//            string postData = $"op=send&uname={uname}&pass={pass}&message={message}&to={json}&from={from}";
//            byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(postData);
//            request.ContentType = "application/x-www-form-urlencoded";
//            request.ContentLength = byteArray.Length;
//            Stream dataStream = request.GetRequestStream();
//            dataStream.Write(byteArray, 0, byteArray.Length);
//            dataStream.Close();
//            WebResponse response = request.GetResponse();
//            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
//            dataStream = response.GetResponseStream();
//            StreamReader reader = new StreamReader(dataStream);
//            string responseFromServer = reader.ReadToEnd();
//            Console.WriteLine(responseFromServer);
//            reader.Close();
//            dataStream.Close();
//            response.Close();
//            List<string> smsResult = JsonConvert.DeserializeObject<List<string>>(responseFromServer);
//            return (smsResult.First()) switch
//            {
//                "0" => "ok",
//                "1" => "متن پیام خالی می باشد.",
//                "2" => "کاربر محدود گردیده است",
//                "3" => "شماره ارسالی به شما تعلق ندارد",
//                "4" => "دریافت کننده وارد نگردیده است",
//                "5" => "اعتبار شما ناکافی است",
//                "6" => "تعدا رشته پیام نامناسب می باشد",
//                "7" => "خط مورد نظر برای ارسال انبوه مناسب نمیباشد",
//                "98" => "حد بالای دریافت کننده رعایت نگردیده است",
//                "99" => "اپراتور شماره ارسال کننده قطع میباشد",
//                "962" => "نام کاربری یا رمز عبور اشتباه می باشد.",
//                "963" => "کاربر شما محدود گردیده است.",
//                "301" => "از حرف ویژه در نام کاربری استفاده گردیده است",
//                "302" => "قیمت گذاری انجام نشده است",
//                "303" => "نام کاربری وارد نگردیده است",
//                "304" => "نام کاربری قبلا انتخاب گردیده است",
//                "305" => "نام کاربری وارد نگردیده است",
//                "306" => "کد ملی وارد نگردیده است",
//                "307" => "کد ملی به خطا وارد شده است",
//                "308" => "شماره شناسنامه نا معتبر است",
//                "309" => "شماره شناسنامه وارد نگردیده است",
//                "310" => "ایمیل کاربر وارد نگردیده است",
//                "311" => "شماره تلفن وارد نگردیده است",
//                "312" => "تلفن به درست یوارد نگردیده است",
//                "313" => "آدرس شما وارد نگردیده است",
//                "314" => "شماره موبایل را وارد نکرده اید",
//                "315" => "شماره موبایل به نادرستی وارد گردیده است",
//                "316" => "سطح دسترسی به نادرستی وارد گردیده است",
//                _ => "خطای ناشناخته",
//            };
//        }
//    }
//}
