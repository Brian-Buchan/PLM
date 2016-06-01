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

            WebRequest request = WebRequest.Create("waws-prod-hk1-017.ftp.azurewebsites.windows.net" + directory);
            request.Method = WebRequestMethods.Ftp.MakeDirectory;
            request.Credentials = new NetworkCredential("PLMBeta\\$PLMBeta", "jEdRCP0G3DxglPTxGeTPTHcXuschgCEl0awntxe1RlPZ5iqvJEpPnotM0PtD");
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
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create("waws-prod-hk1-017.ftp.azurewebsites.windows.net/site/wwwroot" + directory);

            //If you need to use network credentials
            request.Credentials = new NetworkCredential("PLMBeta\\$PLMBeta", "jEdRCP0G3DxglPTxGeTPTHcXuschgCEl0awntxe1RlPZ5iqvJEpPnotM0PtD");
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