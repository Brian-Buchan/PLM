using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;


namespace PLM
{
    public class UserGameSession
    {
        public int Score;
        public int currentGuess;
        public Module currentModule;
        public List<Picture> Pictures;
        public List<AnsPicIndex> PictureIndicies;
        public ApplicationUser currentUser { get; set; }

        public UserGameSession()
        {
            currentModule = new Module();
            Pictures = new List<Picture>();
            PictureIndicies = new List<AnsPicIndex>();
            //if (!System.Security.Principal.WindowsIdentity.GetCurrent().IsGuest)
            //{

            //}
            //currentUser.Id = System.Web.HttpContext.Current.User.Identity.GetUserId();
        }
    }
}