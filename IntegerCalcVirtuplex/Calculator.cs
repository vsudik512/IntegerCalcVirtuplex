using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace IntegerCalcVirtuplex
{
    public static class Calculator
    {
        private static  readonly Regex sWhitespace = new Regex(@"\s+");
        private static DataTable dataTable = new DataTable();

        private static char[] validCharacters = new char[14] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '+', '-', '/', '*' };

        public static async Task<string> CalculateFromString(string expression)
        {
            expression = sWhitespace.Replace(expression, "");

            //Find out if the expression has got any invalid characters and store them in string 
            string invalidCharacters = string.Join("", expression.Split(validCharacters));

            if (invalidCharacters.Length > 0)
                return string.Format("Error - Invalid characters: {0}", invalidCharacters);

            int result = Convert.ToInt32(Math.Floor(Convert.ToDouble(dataTable.Compute(expression, null).ToString())));
            return result.ToString();
        }

        public static async Task CalculateFromFile(string inputPath, string outputPath)
        {
            string[] lines = await File.ReadAllLinesAsync(inputPath);

            for (int i = 0; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i]))
                {
                    lines[i] = "0";
                    continue;
                }
                lines[i] = await CalculateFromString(lines[i]);
            }

            await File.WriteAllLinesAsync(outputPath, lines);
        }
    }
}
