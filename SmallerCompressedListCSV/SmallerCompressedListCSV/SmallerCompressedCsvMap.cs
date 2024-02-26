using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//namespace SmallerCompressedListCSV
public class SmallerCompressedCsvMap : ClassMap<CombinedCsvRecord>
{
    public SmallerCompressedCsvMap()
    {
        Map(m => m.AdsVariableName);
        Map(m => m.Type);
    }
}