using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net;

namespace PLM.Controllers
{
    public class ImageEditorController : Controller
    {
        // GET: ImageEditor
        [HttpGet]
        public ActionResult ImageEditor()
        {
            return View();
        }

        
        [HttpPost]
        [ActionName("ImageEditor")]
        public ActionResult ImageEditorPOST()
        {
            //Image Editor post string format:
            //If the image is saved as a jpeg, the post results in: "data:image/jpeg;base64,[IMAGEDATA]", 
            //where "[IMAGEDATA]" is a base64 string that converts to a jpeg image.
            //Otherwise, if the image is saved as a png, the post results in: "data:image/png;base64,[IMAGEDATA]",
            //where "[IMAGEDATA]" is a base64 string that converts to a png image.

            Image image;
            string dirPath = (Path.Combine(Server.MapPath("~/Content/Images/tempUploads/")));
            
            //gets the actual image data as a base64 string.
            string imageBase64 = Request.Form.Get("imgData");
            
            //gets the image format from the post
            string imageFormat = imageBase64.Substring(imageBase64.IndexOf('/') + 1, imageBase64.IndexOf(';') - 11);
            
            //gets the file data as a Base64 string
            imageBase64 = imageBase64.Substring(imageBase64.LastIndexOf(',') + 1);
            
            //converts the file data to a byte array
            byte[] img = Convert.FromBase64String(imageBase64);

            //if the image is greater than 200KB, reject it and return a response.
            //if (img.Length > 200000)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.RequestEntityTooLarge, 
            //    "Image file size larger than 200 KB");
            //}

            //sets up the filename, guid part taken from Mark Synowiec at http://stackoverflow.com/questions/730268/unique-random-string-generation
            Guid g = Guid.NewGuid();
            string TempFileName = Convert.ToBase64String(g.ToByteArray());
            TempFileName = TempFileName.Replace("=","");
            TempFileName = TempFileName.Replace("+","");
            TempFileName = TempFileName.Replace(@"/","");

            TempFileName = TempFileName + "." + imageFormat;
            
            using (MemoryStream ms = new MemoryStream(img,0,img.Length))
            {
                image = Image.FromStream(ms, true);
            
                if (imageFormat == "jpeg")
	            {
                        image.Save(dirPath + TempFileName, ImageFormat.Jpeg);
	            }
                else if (imageFormat == "png")
                {
                    image.Save(dirPath + TempFileName, ImageFormat.Png);
                }
                else
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, 
                        "Something went wrong with your request. Contact an administrator");
                }
            }

            return View();
        }
    }
}