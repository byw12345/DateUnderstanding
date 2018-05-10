using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Recognizers.Text;
using Microsoft.Recognizers.Text.DateTime;

namespace DateUnderstanding
{
    public sealed class Config
    {
        public const string year  = "YEAR";
        public const string month  = "MONTH";
        public const string day = "DAY";

        public readonly Dictionary<string, Dictionary<string, List<string>>> timeRelatedWords = new Dictionary<string, Dictionary<string, List<string>>>()
        {
            {
                Culture.Chinese,
                new Dictionary<string, List<string>>()
                {
                    { year, new List<string>(){ "年", "年代", "年月日", "年份" } },
                    { month, new List<string>(){ "月", "年月日", "月份" } },
                    { day, new List<string>(){ "日", "日期", "年月日", "日子" } }
                }
            },
            {
                Culture.English,
                new Dictionary<string, List<string>>()
                {
                    { year, new List<string>(){ "YEAR" } },
                    { month, new List<string>(){ "MONTH" } },
                    { day, new List<string>(){ "DAY", "DATE", "TIME" } }
                }
            }
        };
    }
}