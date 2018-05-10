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
            "2012",
            "今晚8点",
            "2007-1-1",
            "2007",
            "2007-1"
        };

        static void Main(string[] args)
        {
            var test = new DateAnalyzer(defaultCulture);
            var result = test.DateSplit(testData);
        }
    }
}