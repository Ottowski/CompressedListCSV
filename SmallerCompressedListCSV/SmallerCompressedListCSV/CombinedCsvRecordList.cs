using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//namespace SmallerCompressedListCSV;

public class CombinedCsvRecord
{
    //remember it's not index, but regular order placement of this that determine the list order outcome.
    [Index(0)]
    public string? AdsVariableName { get; set; }
    [Index(1)]
    public bool? AddUnit { get; set; }
    [Index(2)]
    public string? ModbusPermission { get; set; }
    [Index(3)]
    public string? Type { get; set; }
    [Index(4)]
    public string? Description { get; set; }
    [Index(5)]
    public int? ModbusAddress { get; set; }
    [Index(6)]
    public bool? InterpolatePoints { get; set; }
    [Index(7)]
    public string? ADSDataType { get; set; }
}