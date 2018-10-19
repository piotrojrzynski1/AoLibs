using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace AoLibs.HttpHelper.Statics
{
    public class ZipUtility
    {
        public static byte[] Zip(string str)
        {
            var bytes = Encoding.UTF8.GetBytes(str);

            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(mso, CompressionMode.Compress))
                {
                    CopyTo(msi, gs);
                }

                return mso.ToArray();
            }
        }

        private static void CopyTo(Stream src, Stream dest)
        {
            byte[] bytes = new byte[src.Length];

            int cnt;

            while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0)
            {
                dest.Write(bytes, 0, cnt);
            }
        }
    }
}
