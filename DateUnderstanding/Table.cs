using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DateUnderstanding
{
    class Table
    {
        public List<Column> Columns { get; set; }

        public Table(List<Column> tableData)
        {
            this.Columns = new List<Column>(tableData);
        }

        public Table()
        {
            this.Columns = new List<Column>();
        }
    }
}
