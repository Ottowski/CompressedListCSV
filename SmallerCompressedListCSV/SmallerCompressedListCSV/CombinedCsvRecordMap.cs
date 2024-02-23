using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//namespace SmallerCompressedListCSV

// ClassMap to configure CsvHelper to ignore unwanted properties
public class CombinedCsvRecordMap : ClassMap<CombinedCsvRecord>
{
    public CombinedCsvRecordMap()
    {
        Map(m => m.IdIndex);
        Map(m => m.AdsVariableName);
        Map(m => m.ModbusAddress);
        Map(m => m.Type);
        Map(m => m.ADSDataType);
    }
}