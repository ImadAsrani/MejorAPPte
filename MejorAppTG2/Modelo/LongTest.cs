using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace MejorAppTG2.Modelo
{
    public class LongTest
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public String user_id { get; set; }
        public String user_sex { get; set; }
        public int user_age { get; set; }
        public int fact01 { get; set; }
        public int fact02 { get; set; }
        public int fact03 { get; set; }
        public int fact04 { get; set; }
        public DateTime date_done { get; set; }
        public Boolean is_uploaded { get; set; }
    }
}
