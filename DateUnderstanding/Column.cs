using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DateUnderstanding
{
    class Column
    {
        public string ColumnName { get; set; }
        public List<string> ColumnData { get; set; }

        public Column()
        {
            this.ColumnData = new List<string>();
            this.ColumnName = String.Empty;
        }

        public Column(string name, List<string> data)
        {
            this.ColumnName = name;
            this.ColumnData = new List<string>(data);
        }
    }
}
