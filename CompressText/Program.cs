using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompressText
{
    class Program
    {
        public static void CopyTo(Stream src, Stream dest)
        {
            byte[] bytes = new byte[4096];

            int cnt;

            while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0)
            {
                dest.Write(bytes, 0, cnt);
            }
        }

        public static byte[] Zip(string str)
        {
            var bytes = Encoding.UTF8.GetBytes(str);

            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(mso, CompressionMode.Compress))
                {
                    //msi.CopyTo(gs);
                    CopyTo(msi, gs);
                }

                return mso.ToArray();
            }
        }

        public static string Unzip(byte[] bytes)
        {
            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(msi, CompressionMode.Decompress))
                {
                    //gs.CopyTo(mso);
                    CopyTo(gs, mso);
                }

                return Encoding.UTF8.GetString(mso.ToArray());
            }
        }

        static void Main(string[] args)
        {
            var fileIn = @"f:\Sync.txt";
            var fileOut = @"f:\compressSync.txt";
            var fileOut2 = @"f:\Sync2.txt";

            // COMPRESS
            var text = File.ReadAllText(fileIn, Encoding.UTF8);

            byte[] r1 = Zip(text);
            File.WriteAllBytes(fileOut, r1);

            // DECOMPRESS
            var textBytes = File.ReadAllBytes(fileOut);

            string r2 = Unzip(textBytes);
            File.WriteAllText(fileOut2, r2);
        }
    }
}
