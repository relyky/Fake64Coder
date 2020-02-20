using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace conFake64Coder
{
    class Program
    {
        static void Main(string[] args)
        {
            test1();
            test2();
            test3();

            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }

        static void test1() {
            var info = new TestInfo
            {
                userId = "Z01234.56789",
                userName = "今天天氣真好。"
            };
            string json = JsonConvert.SerializeObject(info);
            string base64str = Convert.ToBase64String(ASCIIEncoding.UTF8.GetBytes(json));
            string fake64str = Fake64.Encode(base64str);

            //=================================================================

            string base64str2 = Fake64.Deocde(fake64str);
            string json2 = ASCIIEncoding.UTF8.GetString(Convert.FromBase64String(base64str2));
            var info2 = JsonConvert.DeserializeObject<TestInfo>(json2);

            if (json == json2)
                Console.WriteLine("Yes.1");
            else
                Console.WriteLine("No.1");
        }

        static void test2()
        {
            var info = new TestInfo
            {
                userId = "Z01234.56789",
                userName = "I am fine."
            };
            string json = JsonConvert.SerializeObject(info);
            string base64str = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(json));
            string fake64str = Fake64.Encode(base64str);

            //=================================================================

            string base64str2 = Fake64.Deocde(fake64str);
            string json2 = ASCIIEncoding.ASCII.GetString(Convert.FromBase64String(base64str2));
            var info2 = JsonConvert.DeserializeObject<TestInfo>(json2);

            if (json == json2)
                Console.WriteLine("Yes.2");
            else
                Console.WriteLine("No.2");
        }

        static void test3()
        {
            string info = @"userId=Z01234.56789&userName=I am fine";
            string base64str = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(info));
            string fake64str = Fake64.Encode(base64str);

            //=================================================================
            
            try
            {
                string base64str2 = Fake64.Deocde(fake64str);
                string info2 = ASCIIEncoding.ASCII.GetString(Convert.FromBase64String(base64str2));

                if (info == info2)
                    Console.WriteLine("Yes.3");
                else
                    Console.WriteLine("No.3");
            }
            catch (Exception ex) {
                Console.WriteLine("Exception >> " + ex.Message);
            }
        }
    }

    class TestInfo {
        public string userId { get; set; }
        public string userName { get; set; }
    }
}
