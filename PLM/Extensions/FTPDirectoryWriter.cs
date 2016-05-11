using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;

namespace PLM.Extensions
{
    public class FTPDirectoryWriter
    {
        public static bool FTPNewDirectory(string directory)
        {

            WebRequest request = WebRequest.Create("ftp://waws-prod-bn1-007.ftp.azurewebsites.windows.net/" + directory);
            request.Method = WebRequestMethods.Ftp.MakeDirectory;
            request.Credentials = new NetworkCredential("PLMGamesite2\\FTPClient12345", "PLM12345");
            using (var resp = (FtpWebResponse)request.GetResponse())
            {
                if (resp.StatusCode == FtpStatusCode.CommandOK)
                {
                    return true;
                }
                else
                {
                    return false;
                }
                //Console.WriteLine(resp.StatusCode);
            }
        }
    }
}