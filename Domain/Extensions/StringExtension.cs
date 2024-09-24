using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Extensions;
public static class StringExtension
{
    public static string ToSnakeCase(this string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return input;
        }

        var stringBuilder = new StringBuilder();
        for (int i = 0; i < input.Length; i++)
        {
            if (char.IsUpper(input[i]))
            {
                if (i > 0)
                {
                    stringBuilder.Append('_');
                }
                stringBuilder.Append(char.ToLower(input[i]));
            }
            else
            {
                stringBuilder.Append(input[i]);
            }
        }

        return stringBuilder.ToString();
    }
}