using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Recognizers.Text;
using Microsoft.Recognizers.Text.DateTime;

namespace DateUnderstanding
{
    class DateHiberarchy
    {
        public string Culture { get; set; }
        //public List<Column> Hiberarchy { get; set; }
        public Dictionary<string, List<string>> Hiberarchy { get; set; } = new Dictionary<string, List<string>>();

        public DateHiberarchy(string culture)
        {
            this.Culture = culture;
            var config = new Config();
            this.Hiberarchy.Add(config.timeRelatedWords[culture][Config.year].First(), new List<string>());
            this.Hiberarchy.Add(config.timeRelatedWords[culture][Config.month].First(), new List<string>());
            this.Hiberarchy.Add(config.timeRelatedWords[culture][Config.day].First(), new List<string>());
        }

    }
}
