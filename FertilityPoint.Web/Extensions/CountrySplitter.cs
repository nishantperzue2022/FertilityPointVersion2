using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FertilityPoint.Web.Extensions
{
    public static class CountrySplitter
    {
        public static string  get(string Name)
        {
            int index = Name.IndexOfAny(new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' });

            string chars = Name.Substring(0, index);

            int num = int.Parse(Name.Substring(index));

            var code =("+" + num).ToString();

            return code;
        }

    }
}
