using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Mixer_Controller
{

    /// <summary>
    /// utility class purely static
    /// </summary>
    public static class Utils
    {

        /// <summary>
        /// used to capitalize the first letter of a word
        /// </summary>
        /// <param name="input">input string</param>
        /// <returns>captitalized string</returns>
        public static string capitalizeFirst(string input)
        {
            char[] letters = input.ToCharArray();
            letters[0] = letters[0].ToString().ToUpper().ToCharArray()[0];
            return new string(letters);
        }

    }
}
