using NameParser;
using System.Linq;

namespace SPWebServices
{
    public class Util
    {
        /// <summary>
        /// Given a varying format of Fullname, return a name formatted consistently as
        /// 'lastName, firstName' or 'lastName, firstName (nickname)'
        /// </summary>
        /// <param name="fullName">(Accepts most common conventions of entry)</param>
        /// <returns>Standardized formatted name in lastName, firstName format.</returns>
        public static string standardizedName(string fullName)
        {
            HumanName nameObj = new HumanName(fullName);

            string firstName = nameObj.First;
            string lastName = nameObj.Last;
            string nickName = nameObj.Nickname;
            string suffix = nameObj.Suffix;

            #region Fix Mal-identified Suffixes due to bad storage in db.
            if (string.IsNullOrWhiteSpace(lastName) && !string.IsNullOrWhiteSpace(nameObj.Suffix))
            { 
                //Last Name was empty, but suffix wasn't

                if (!string.IsNullOrWhiteSpace(nameObj.First)) // First Name wasn't empty, but should be last name.
                {
                    lastName = nameObj.First;
                    if (nameObj.Suffix.Split(new char[0]) != null
                        && nameObj.Suffix.Split(new char[0]).Count() > 1) // Suffix could be split into at least two parts,
                    {
                        suffix = nameObj.Suffix.Split(new char[0])[0]; // presume the first part is the actual suffix.
                        firstName = nameObj.Suffix.Replace((nameObj.Suffix.Split(new char[0])[0]), "").Trim(); // Replace last name with the string, less the suffix.
                    }
                    else
                    {
                        lastName = nameObj.Suffix;
                    }
                }
            }
            #endregion

            if (!string.IsNullOrEmpty(nickName))
            {
                return lastName + ", " + firstName + " (" +
                    nickName + ")"; // Morel, Jeremy (Bob)
            }
            if (!string.IsNullOrEmpty(suffix))
            {
                return lastName + " " + suffix + ", " + firstName; // Morel Jr., Jeremy
            }
            return lastName + ", " + firstName; // Morel, Jeremy
        }
        public static bool stringsMatch (string string1, string string2, bool caseSensitive = false)
        {
            if (!caseSensitive) return (string1.ToLower() == string2.ToLower());
            return (string1 == string2);
        }
    }
}