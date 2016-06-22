using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PLM.Models;

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
        public static List<Score> GetTopTenScores(int moduleID, string userID)
        {
            if (userID == null)
            {
                //The current Guest Account ID
                // TODO - Set up a txt file that this can be read from, or find another way of selecting the guest account if not logged in
                userID = "5b853c48-424f-455e-b731-f24e102cdc6d";
            }

            ApplicationDbContext db = new ApplicationDbContext();
            List<Score> scores = db.Scores.ToList();
            scores.Where(x => x.Module.ModuleID == moduleID);
            scores.Where(y => y.User.Id == userID);
            scores.OrderBy(x => (x.TotalAnswers / x.CorrectAnswers)).ToList();
            return((List<Score>)scores.Take(10));
        }
    }

    public static class FileManipExtensions
    {
        /// <summary>
        /// Move a number of files to a single directory, keeping their names
        /// and overwriting if the switch is toggled.
        /// Will ignore nonexistent files, and return false if the specified directory does not exist.
        /// Returns true if it succeeded, false if it did not.
        /// </summary>
        /// <param name="filePaths">An array of filepath strings, </param>
        /// <param name="saveDirectory">The path to the directory to use</param>
        /// <param name="overWrite">Optional, defaults to false. Whether or not 
        /// to overwrite any existing files with the same name in the new directory. 
        /// If false, skips files that already exist in destination.</param>
        /// <returns>bool</returns>
        public static bool MoveSpecificFiles(string[] filePaths, string saveDirectory, bool overWrite = false)
        {
            //If the directory doesn't exist, error out.
            if (!Directory.Exists(saveDirectory))
            {
                return false;
            }
            string fileName;

            try
            {
                foreach (string filePath in filePaths)
                {
                    //Check if the file to be moved exists. If it doesn't, skip it and go to the next one.
                    if (File.Exists(filePath))
                    {
                        fileName = Path.GetFileName(filePath);

                        //if the overwrite flag is set to true and the file exists in the directory, delete it.
                        if (overWrite && File.Exists(saveDirectory + fileName))
                        {
                            File.Delete(saveDirectory + fileName);
                        }
                        //If the file to be moved does not exist in the new location, move it there.
                        //This means that duplicate files will not be moved.
                        if (!File.Exists(saveDirectory + fileName))
                        {
                            File.Move(filePath, saveDirectory + fileName);
                        }
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Delete multiple specific files. Returns true if it succeeds, false otherwise
        /// </summary>
        /// <param name="filePaths">An array of filepath strings that 
        /// each refer to a file to be deleted</param>
        /// <returns>bool</returns>
        public static bool DeleteSpecificFiles(string[] filePaths)
        {
            try
            {
                foreach (string filePath in filePaths)
                {
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
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
        public static string GetLocation(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("ProfilePicture");
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

    public static class PagingExtensions
    {
        //used by LINQ to SQL
        public static IQueryable<TSource> Page<TSource>(this IQueryable<TSource> source, int page, int pageSize)
        {
            return source.Skip((page - 1) * pageSize).Take(pageSize);
        }

        //used by LINQ
        public static IEnumerable<TSource> Page<TSource>(this IEnumerable<TSource> source, int page, int pageSize)
        {
            return source.Skip((page - 1) * pageSize).Take(pageSize);
        }

    }

    //public class ProfanityFilter
    //{        
    //    // METHOD: containsProfanity
    //    public bool containsProfanity(string checkStr)
    //    {
    //        bool badwordpresent = false;

    //        string[] inStrArray = checkStr.Split(new char[] { ' ' });

    //        string[] words = this.profanityArray();

    //        // LOOP THROUGH WORDS IN MESSAGE
    //        for (int x = 0; x < inStrArray.Length; x++)
    //        {
    //            // LOOP THROUGH PROFANITY WORDS
    //            for (int i = 0; i < words.Length; i++)
    //            {
    //                // IF WORD IS PROFANITY, SET FLAG AND BREAK OUT OF LOOP
    //                //if (inStrArray[x].toString().toLowerCase().equals(words[i]))
    //                if( inStrArray[x].ToLower() == words[i].ToLower() )
    //                {
    //                    badwordpresent = true;
    //                    break;
    //                }
    //            }
    //            // IF FLAG IS SET, BREAK OUT OF OUTER LOOP
    //            if (badwordpresent == true) break;
    //        }

    //        return badwordpresent;
    //    }
    //    // ************************************************************************




    //    // ************************************************************************
    //    // METHOD: profanityArray()
    //    // METHOD OF PROFANITY WORDS
    //    private string[] profanityArray()
    //    {
    //        var profanity = Enum.GetValues(typeof(Profanity));
    //        // THESE WERE UPDATED TO USE THE SAME BADWORDS FROM FACESOFMBCFBAPP
    //        foreach(Word w in Enum.GetValues(typeof(Profanity))
    //        string[] words = 
    //        return words;
    //    }
    //}


    //Taken from http://stackoverflow.com/questions/3216496/c-sharp-how-to-determine-if-a-number-is-a-multiple-of-another
    public static class MathExtensions
    {
        /// <summary>
        /// Check if this number is evenly divisible by another number
        /// </summary>
        /// <param name="dividend">The number this method is being applied to</param>
        /// <param name="divisor">The number to divide by</param>
        /// <returns>bool</returns>
        public static bool IsDivisible(this int dividend, int divisor)
        {
            return (dividend % divisor) == 0;
        }
    }
}