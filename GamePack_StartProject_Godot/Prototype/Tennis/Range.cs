using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamePackStartProjectGodot.Prototype.Tennis
{
    public class Range
    {
        public double Min { get; set; }
        public double Max { get; set; }
        public double Value { get; set; }

        public Range(double min, double max, double value)
        {
            Min = min;
            Max = max;
            Value = value;
        }

        public double Contains(double val)
        {
            return (val >= Min && val <= Max) ? Value : -1;
        }

        public static double GetRangeValue(double input, List<Range> ranges)
        {
            foreach (var r in ranges)
            {
                double result = r.Contains(input);
                if (result != -1)
                    return result;
            }
            return -1;
        }
    }
}
