using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace conFake64Coder
{
    /// <summary>
    /// Base64 Cypher
    /// </summary>
    class Fake64
    {
        public static string Encode(string base64str)
        {
            Fake64Coder coder = new Fake64Coder();
            return coder.Encode(base64str);
        }

        public static string Deocde(string fake64str)
        {
            Fake64Coder coder = new Fake64Coder();
            return coder.Deocde(fake64str);
        }

        class Fake64Coder
        {
            private static Random randor = new Random((int)DateTime.UtcNow.Ticks);
            private static string Code64 = @"ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";

            internal string IntToBase64(int[] int64)
            {
                var base64str = new StringBuilder(new String(' ', int64.Length));
                for (int i = 0; i < int64.Length; i++)
                {
                    base64str[i] = Code64[int64[i]];
                }
                return base64str.ToString();
            }

            internal int[] Base64ToInt(string base64str)
            {
                var i64 = new int[base64str.Length];
                for (int i = 0; i < i64.Length; i++)
                    i64[i] = Code64.IndexOf(base64str[i]);
                return i64;
            }

            internal int[] Encode(int[] base64int)
            {
                var fake64int = new List<int>();
                int chksum = randor.Next(64);
                int tailCnt = 0;

                // header
                fake64int.Add(randor.Next(64));
                // base
                fake64int.Add(chksum);
                // content
                for (int i = 0; i < base64int.Length; i++)
                {
                    if (base64int[i] == 64)
                    {
                        tailCnt++;
                        continue;
                    }
                    int fake = (base64int[i] + fake64int[i + 1]) % 64;
                    chksum = (fake + chksum) % 64;
                    fake64int.Add(fake);
                }

                // checksum
                chksum = 63 - chksum;
                fake64int.Add(chksum);

                // tailer
                for (int j = 0; j < tailCnt; j++)
                    fake64int.Add(64);

                return fake64int.ToArray();
            }

            internal int[] Decode(int[] fake64int)
            {
                var base64int = new List<int>();
                var en = fake64int.GetEnumerator();
                en.MoveNext();
                en.MoveNext();
                int bs = (int)en.Current; // base
                int chksum = bs, chksum_pre = 64;
                while (en.MoveNext()) {
                    int fake = (int)en.Current;
                    if (fake > 63) {
                        base64int.RemoveAt(base64int.Count - 1);
                        chksum = chksum_pre;
                        base64int.Add(64);
                        while (en.MoveNext()) {
                            base64int.Add(64);
                        }
                        break;
                    }
                    chksum_pre = chksum;
                    chksum = (fake + chksum) % 64;
                    int value = fake + 64 - bs;
                    if (value > 63)
                        value = fake - bs;
                    base64int.Add(value);
                    bs = fake;
                }

                /// check checksum
                if (bs != 63 - chksum)
                {
                    throw new ApplicationException("invalid checksum!");
                }

                // success
                return base64int.ToArray();
            }

            internal string Encode(string base64str)
            {
                Fake64Coder coder = new Fake64Coder();
                var bake64int = coder.Base64ToInt(base64str);
                var fake64int = coder.Encode(bake64int);
                var fake64str = coder.IntToBase64(fake64int);
                return fake64str;
            }

            internal string Deocde(string fake64str)
            {
                Fake64Coder coder = new Fake64Coder();
                var fake64int = coder.Base64ToInt(fake64str.EndsWith("=") ? fake64str : fake64str + "===");
                var base64int = coder.Decode(fake64int);
                var base64str = coder.IntToBase64(base64int);
                if (base64str.EndsWith("==="))
                    base64str = base64str.Substring(0, base64str.Length - 3);
                return base64str;
            }
        }
    }
}
