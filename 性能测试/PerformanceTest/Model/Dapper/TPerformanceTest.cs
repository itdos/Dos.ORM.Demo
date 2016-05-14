using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dos.ORM;

namespace Dapper.Model
{
    public class TPerformanceTest :Entity
    {
        public System.Guid Id { get; set; }
        public string T1 { get; set; }
        public Nullable<int> T2 { get; set; }
        public Nullable<decimal> T3 { get; set; }
        public Nullable<double> T4 { get; set; }
        public string T5 { get; set; }
        public System.DateTime T6 { get; set; }
        public Nullable<bool> T7 { get; set; }
        public Nullable<decimal> T8 { get; set; }
    }
}
