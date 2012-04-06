using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SI.Other
{
    public static class OpenBetaFunctions
    {
        /// <summary>
        /// Verifies the key given by the website.
        /// </summary>
        /// <param name="name">The name of the player.</param>
        /// <param name="key">The key generated.</param>
        /// <returns></returns>
        public static bool VerifyBetaKey(string name, string key)
        {
            if (key.Length < 16)
                return false;

            int sum = 0, keySum = 0;

            foreach (var letter in name)
                sum += (int)letter;

            for (int i = 0; i < key.Length; ++i)
            {
                if (i > 12 && key[i] == '-')
                    break;
                if (key[i] == '-')
                    continue;

                keySum += (int)key[i] + 32;
            }

            char[] offset = new char[5];
            for (int i = 15; i < key.Length; ++i)
                offset[i - 15] = key[i];
            int off;
            if (!int.TryParse(new string(offset), out off))
                return false;

            if (keySum + off == sum || keySum - off == sum)
                return true;
            return false;
        }
    }
}
