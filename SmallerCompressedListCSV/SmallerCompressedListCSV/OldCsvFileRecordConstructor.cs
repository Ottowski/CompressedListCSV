using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

//namespace SmallerCompressedListCSV;

public class FirstIndexCsvRecord
{
    [Name("IdIndex")]
    public string? IdIndex { get; set; }
    [Name(" AdsVariableName")]
    public string? AdsVariableName { get; set; }

    [Name(" ModbusAddress")]
    public int? ModbusAddress { get; set; }

    [Name(" ModbusPermission")]
    public string? ModbusPermission { get; set; }

    [Name("AddUnit")]
    public bool? AddUnit { get; set; }

    [Name("ADS data type")]
    public string? ADSDataType { get; set; }

    [Name("Request Interval")]
    public int? RequestInterval { get; set; }

    [Name("Output Data Type")]
    public string? OutputDataType { get; set; }
}


public class SecondInputCsvRecord
{
    [Name("IdIndex")]
    public string? IdIndex { get; set; }
    [Name(" AdsVariableName")]
    public string? AdsVariableName { get; set; }

    [Name(" Type")]
    public string? Type { get; set; }

    [Name(" ModbusPermission")]
    public string? ModbusPermission { get; set; }

    [Name("Description")]
    public string? Description { get; set; }

    [Name("InterpolatePoints")]
    public bool? InterpolatePoints { get; set; }

    [Name("ADS data type")]
    public string? ADSDataType { get; set; }

    [Name("Request Interval")]
    public int? RequestInterval { get; set; }
}