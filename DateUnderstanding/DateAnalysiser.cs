using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Recognizers.Text;
using Microsoft.Recognizers.Text.DateTime;
using Newtonsoft.Json;

namespace DateUnderstanding
{
    class DateAnalyzer
    {

        private const string date = "date";
        private const string dateRange = "daterange";
        private const string valuesKey = "values";
        private const string dateTypeValueKey = "type";
        private const string timexValueKey = "timex";
        private const char sep = '-';
        private const string anonymousYear = "XXXX";
        private Config config = new Config();
        public string Culture { get; set; }
        public string YEAR { get; set; }
        public string MONTH { get; set; }
        public string DAY { get; set; }

        private enum DateType { none, year, month, day };


        public DateAnalyzer(string culture)
        {
            this.Culture = culture;
            this.YEAR = config.timeRelatedWords[culture][Config.year].First();
            this.MONTH = config.timeRelatedWords[culture][Config.month].First();
            this.DAY = config.timeRelatedWords[culture][Config.day].First();
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
                case 2: case 3:
                    return BuildHiberarchyFromTable(table, dateColumnIndex);
                default:
                    foreach (var i in dateColumnIndex)
                    {
                        Console.WriteLine(table.Columns[i].ColumnName);
                    }
                    Console.ReadKey();
                    return new DateHiberarchy(Culture);
            }
        }

        private DateType CellValueType(List<string> columnData)
        {
            int yearCount = 0;
            int monthCount = 0;
            int dayCount = 0;
            foreach (var cellValue in columnData)
            {
                var isDate = GetDateTypeAndTimex(cellValue, out string dateType, out string timex);
                var splitedDate = timex.Split(sep);
                if (isDate)
                {
                    yearCount += splitedDate.Length == 1 ? 1 : 0;
                    monthCount += splitedDate.Length == 2 ? 1 : 0;
                }
                else
                {
                    var isNum = int.TryParse(cellValue, out int result);
                    if (isNum)
                    {
                        monthCount += (result >= 1 && result <= 12) ? 1 : 0;
                        dayCount += (result >= 1 && result <= 31) ? 1 : 0;
                    }
                }
            }
            int maxCount = Math.Max(Math.Max(yearCount, monthCount), dayCount);
            if (maxCount == yearCount)
                return DateType.year;
            else if (maxCount == monthCount)
                return DateType.month;
            else
                return DateType.day;
        }


        private DateType IsYearOrMonthOrDay(string columnName, List<string> columnData)
        {
            //columnName = columnName.ToUpper();
            var dateType = CellValueType(columnData);
            if (config.timeRelatedWords[Culture][Config.year].Contains(columnName) && dateType == DateType.year)
            {
                return DateType.year;
            }
            if (config.timeRelatedWords[Culture][Config.month].Contains(columnName) && dateType == DateType.month)
            {
                return DateType.month;
            }
            if (config.timeRelatedWords[Culture][Config.day].Contains(columnName) && dateType == DateType.day)
            {
                return DateType.day;
            }
            return DateType.none;
        }

        private DateHiberarchy BuildHiberarchyFromTable(Table table, List<int> dateColumnIndex)
        {
            DateHiberarchy result = new DateHiberarchy(Culture);
            foreach (int i in dateColumnIndex)
            {
                var columnData = new List<string>(table.Columns[i].ColumnData);
                var columnType = IsYearOrMonthOrDay(table.Columns[i].ColumnName.ToUpper(), columnData);
                switch (columnType)
                {
                    case DateType.year:
                        result.Hiberarchy[YEAR] = columnData;
                        break;
                    case DateType.month:
                        result.Hiberarchy[MONTH] = columnData;
                        break;
                    case DateType.day:
                        result.Hiberarchy[DAY] = columnData;
                        break;
                    default:
                        Console.WriteLine(JsonConvert.SerializeObject(table.Columns[i], Formatting.Indented));
                        break;
                }
            }
            return result;
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
            var result = new DateHiberarchy(Culture);
            foreach (var cellValue in data)
            {
                var isDate = GetDateTypeAndTimex(cellValue, out string dateType, out string timex);
                if (!isDate)
                {
                    Console.Write(cellValue);
                    Console.ReadKey();
                    continue;
                }
                var splitedDate = timex.Split(sep);
                switch (splitedDate.Length)
                {
                    case 1:
                        result.Hiberarchy[YEAR].Add(splitedDate[0]);
                        result.Hiberarchy[MONTH].Add(String.Empty);
                        result.Hiberarchy[DAY].Add(String.Empty);
                        break;
                    case 2:
                        result.Hiberarchy[YEAR].Add(splitedDate[0] != anonymousYear ? splitedDate[0] : String.Empty);
                        result.Hiberarchy[MONTH].Add(splitedDate[1]);
                        result.Hiberarchy[DAY].Add(String.Empty);
                        break;
                    case 3:
                        result.Hiberarchy[YEAR].Add(splitedDate[0]);
                        result.Hiberarchy[MONTH].Add(splitedDate[1]);
                        result.Hiberarchy[DAY].Add(splitedDate[2]);
                        break;
                    default:
                        Console.Write(timex);
                        Console.ReadKey();
                        break;
                }
            }
            return result;
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
                if (dateRelatedWords.Contains(column.ColumnName.ToUpper()))
                {
                    dateColumnIndex.Add(dateColumnNum);
                    dateColumnNum++;
                }
            }
            return dateColumnIndex;
        }
    }
}
