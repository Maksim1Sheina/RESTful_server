using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RESTful_server.Services
{
    public class HelperService
    {
        public byte[] FileStreamToByteArray(Stream stream, long length)
        {
            byte[] fileBytesArray = new byte[length];

            stream.Read(fileBytesArray, 0, (int)length);

            return fileBytesArray;
        }
    }
}
