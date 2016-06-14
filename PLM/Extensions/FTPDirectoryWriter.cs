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

            WebRequest request = WebRequest.Create("plm.nmc.edu" + directory);
            request.Method = WebRequestMethods.Ftp.MakeDirectory;
            request.Credentials = new NetworkCredential("PLMAdmin", "Password1");
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
        public static bool FTPDelete(string directory)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create("plm.nmc.edu" + directory);

            //If you need to use network credentials
            request.Credentials = new NetworkCredential("PLMAdmin", "Password1");
            //additionally, if you want to use the current user's network credentials, just use:
            //System.Net.CredentialCache.DefaultNetworkCredentials

            request.Method = WebRequestMethods.Ftp.DeleteFile;
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            Console.WriteLine("Delete status: {0}", response.StatusDescription);
            response.Close();
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