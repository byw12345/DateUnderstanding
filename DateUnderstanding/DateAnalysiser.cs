using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Recognizers.Text;
using Microsoft.Recognizers.Text.DateTime;

namespace DateUnderstanding
{
    class DateAnalyzer
    {

        private const string date = "date";
        private const string dateRange = "daterange";
        private const string valuesKey = "values";
        private const string dateTypeValueKey = "type";
        private const string timexValueKey = "timex";
        private Config config = new Config();
        //private const int typeIndex = 1;
        //private const int timexIndex = 0;
        public string Culture { get; set; }

        public DateAnalyzer(string culture)
        {
            this.Culture = culture;
        }

        public DateHiberarchy GetDateHiberarchy(Table table)
        {
            var dateColumnIndex = GetDateColumnIndex(table);
            switch (dateColumnIndex.Count)
            {
                case 0:
                    return new DateHiberarchy(Culture);
                case 1:
                    return DateSplit(table.Columns[dateColumnIndex.First()].ColumnData);
                default:
                    return 
            }
        }

        private bool GetDateTypeAndTimex(string cellValue, out string dateType, out string timex)
        {
            dateType = String.Empty;
            timex = String.Empty;
            var recgonizeResult = DateTimeRecognizer.RecognizeDateTime(cellValue, Culture);
            if (recgonizeResult.Count == 0)
            {
                return false;
            }
            Dictionary<string, string> formatDate = ((List<Dictionary<string, string>>)recgonizeResult.First().Resolution[valuesKey]).First();
            dateType = formatDate[dateTypeValueKey];
            timex = formatDate[timexValueKey];
            return true;
        }

        public DateHiberarchy DateSplit(List<string> data)
        {
            //foreach (var column in data.Columns)
            //{
                foreach (var cellValue in data)//column.ColumnData)
                {
                
                    var test = GetDateTypeAndTimex(cellValue, out string dateType, out string timex);
                    Console.Write(dateType);
                }
            //}
            return new DateHiberarchy(Culture);
        }

        private List<int> GetDateColumnIndex(Table table)
        {
            HashSet<string> dateRelatedWords = new HashSet<string>();
            var dateColumnIndex = new List<int>();
            foreach (var wordsList in config.timeRelatedWords[Culture].Values)
            {
                wordsList.ForEach(word => dateRelatedWords.Add(word));
            }
            int dateColumnNum = 0;
            foreach (var column in table.Columns)
            {
                if (dateRelatedWords.Contains(column.ColumnName))
                {
                    dateColumnIndex.Add(dateColumnNum);
                    dateColumnNum++;
                }
            }
            return dateColumnIndex;
        }
    }
}
