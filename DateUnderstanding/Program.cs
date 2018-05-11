using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

using Microsoft.Recognizers.Text;
using Microsoft.Recognizers.Text.DateTime;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.NumberWithUnit;
using Microsoft.Recognizers.Text.Choice;
using Microsoft.Recognizers.Text.Sequence;

namespace DateUnderstanding
{
    class Program
    {
        // Use English for the Recognizers culture
        private static string defaultCulture = Culture.English;
        private static List<string> testData = new List<string>()
        {
            "2009",
            "2012",
            "2007",
            "2007",
            "2007"
        };
        private static List<string> testData2 = new List<string>()
        {
            "1",
            "1",
            "2",
            "3",
            "7"
        };

        static void Main(string[] args)
        {
            var test = new DateAnalyzer(defaultCulture);
            var result = test.DateSplit(testData);
            var aaa = "2009年".ToUpper();
            Table testTable = new Table();
            testTable.Columns.Add(new Column("year", testData));
            testTable.Columns.Add(new Column("month", testData2));
            var r = test.GetDateHiberarchy(testTable);


        }
    }
}