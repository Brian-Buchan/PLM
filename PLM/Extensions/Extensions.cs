using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Claims;
using System.Security.Principal;

namespace PLM
{
    public static class ShuffleExtension
    {
        //this randomization method, based on the Fisher-Yates shuffle, was taken from http://stackoverflow.com/questions/273313/randomize-a-listt-in-c-sharp
        private static Random rand = new Random();
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rand.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }

    public static class TopTenScore
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private List<Score> scores = new List<Score>();
            //ViewBag.UserID = User.Identity.GetUserId();

        public TopTenScore()
        {
            scores = db.Scores.ToList();
        }
    }

    public static class IdentityExtensions
    {
        public static string GetFirstName(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("FirstName");
            // Test for null to avoid issues during local testing
            return (claim != null) ? claim.Value : string.Empty;
        }

        public static string GetLastName(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("LastName");
            // Test for null to avoid issues during local testing
            return (claim != null) ? claim.Value : string.Empty;
        }

        public static string GetInstution(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("Instution");
            // Test for null to avoid issues during local testing
            return (claim != null) ? claim.Value : string.Empty;
        }
    }
}