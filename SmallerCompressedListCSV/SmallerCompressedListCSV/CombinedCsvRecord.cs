using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallerCompressedListCSV
{
    public class CombinedCsvRecord
    {
        public string? AdsVariableName { get; set; }
        public int? ModbusAddress { get; set; }
        public string? ModbusPermission { get; set; }
        public string? Type { get; set; }
        public string? Description { get; set; }
        public bool? AddUnit { get; set; }
        public bool? InterpolatePoints { get; set; }
        public string? ADSDataType { get; set; }
    }
}