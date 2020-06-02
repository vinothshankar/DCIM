using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DCIM.Extension
{
    public static class DCIMExtension
    {
        public static string ConvertToBase64(this Stream stream)
        {
            var bytes = new Byte[(int)stream.Length];
            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(bytes, 0, (int)stream.Length);
            return Convert.ToBase64String(bytes);
        }
    }
}
