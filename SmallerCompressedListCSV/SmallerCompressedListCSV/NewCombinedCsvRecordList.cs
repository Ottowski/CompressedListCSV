using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

//namespace SmallerCompressedListCSV;

public class CombinedCsvRecord
{
    //remember it's not index, but the regular row order (top to down) placement of this that determine the list order outcome (e.g. in Excel).
    [Index(0)]
    public string? IdIndex { get; set; }
    [Index(1)]
    public string? AdsVariableName { get; set; }
    [Index(2)]
    public bool? AddUnit { get; set; }
    [Index(3)]
    public string? ModbusPermission { get; set; }
    [Index(4)]
    public string? Type { get; set; }
    [Index(5)]
    public string? Description { get; set; }
    [Index(6)]
    public int? ModbusAddress { get; set; }
    [Index(7)]
    public bool? InterpolatePoints { get; set; }
    [Index(8)]
    public string? ADSDataType { get; set; }
    [Index(9)]
    public int? RequestInterval { get; set; }
    [Index(10)]
    public string? OutputDataType { get; set; }
}