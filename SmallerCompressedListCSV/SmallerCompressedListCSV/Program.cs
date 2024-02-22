using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;


public class FirstIndexCsvRecord
{
    [Name("AdsVariableName")]
    public string? AdsVariableName { get; set; }

    [Name(" ModbusAddress")]
    public int? ModbusAddress { get; set; }

    [Name(" ModbusPermission")]
    public string? ModbusPermission { get; set; }

    [Name("AddUnit")] 
    public bool? AddUnit { get; set; }
    [Name("ADS data type")]
    public string? ADSDataType { get; set; }
}

public class SecondInputCsvRecord
{
    [Name("AdsVariableName")]
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
}

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

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Welcome to CSV File Merger!");

        while (true)
        {
            Console.WriteLine("\nMainmenu");
            Console.WriteLine("Select an option: ");
            Console.WriteLine("\n1. Merge CSV files");
            Console.WriteLine($"2. Merge only some records to new CSV file");
            Console.WriteLine("0. Exit\n");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.WriteLine("\nCreating a new CSV list from merging...");
                    MergeCsvFiles();
                    Console.WriteLine("Returning to the mainmenu!");
                    break;
                case "2":
                    Console.WriteLine("\nCreating a even smaller CSV list...");
                    MergeSmallerCsvFile();
                    Console.WriteLine("Returning to the mainmenu!");
                    break;
                case "0":
                    Console.WriteLine("\nExiting CSV File Merger...");
                    return;
                default:
                    Console.WriteLine("\nInvalid option. Please try again.");
                    break;
            }
        }
    }



    static void MergeSmallerCsvFile()
    {
        string baseDirectory = @"C:\Users\ottoa\OneDrive\Skrivbord\SmallerCompressedListCSV\SmallerCompressedListCSV\SmallerCompressedListCSV\bin\Debug\net8.0";
        string firstCsvFilePath = Path.Combine(baseDirectory, "indexlist.csv");
        string secondCsvFilePath = Path.Combine(baseDirectory, "inputlist.csv");
        string combinedCsvFilePath = Path.Combine(baseDirectory, "even-smaller-combined-List.csv");

        // Read both CSV files
        List<FirstIndexCsvRecord> firstCsvRecords;
        List<SecondInputCsvRecord> secondCsvRecords;

        using (var reader = new StreamReader(firstCsvFilePath))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            firstCsvRecords = csv.GetRecords<FirstIndexCsvRecord>().ToList();
        }

        using (var reader = new StreamReader(secondCsvFilePath))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            secondCsvRecords = csv.GetRecords<SecondInputCsvRecord>().ToList();
        }

        // Merge records from first CSV file
        var combinedRecords = new List<CombinedCsvRecord>();

        foreach (var firstRecord in firstCsvRecords)
        {
            var combinedRecord = new CombinedCsvRecord
            {
                AdsVariableName = firstRecord.AdsVariableName,
                ModbusAddress = firstRecord.ModbusAddress,
                ADSDataType = firstRecord.ADSDataType
            };
            combinedRecords.Add(combinedRecord);
        }

        // Merge records from second CSV file
        foreach (var secondRecord in secondCsvRecords)
        {
            var existingRecord = combinedRecords.FirstOrDefault(x => x.AdsVariableName == secondRecord.AdsVariableName);
            if (existingRecord != null)
            {
                existingRecord.Type = secondRecord.Type;
            }
            else
            {
                var combinedRecord = new CombinedCsvRecord
                {
                    AdsVariableName = secondRecord.AdsVariableName,
                    Type = secondRecord.Type,
                };
                combinedRecords.Add(combinedRecord);
            }
        }

        // Write the combined new CSV file
        using (var writer = new StreamWriter(combinedCsvFilePath))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            // Create ClassMap to ignore unwanted properties
            csv.Context.RegisterClassMap<CombinedCsvRecordMap>();
            csv.WriteRecords(combinedRecords);
        }

        Console.WriteLine("\nCombined merging of CSV files was created successfully.");
    }

    // ClassMap to configure CsvHelper to ignore unwanted properties
    public class CombinedCsvRecordMap : ClassMap<CombinedCsvRecord>
    {
        public CombinedCsvRecordMap()
        {
            Map(m => m.AdsVariableName);
            Map(m => m.ModbusAddress);
            Map(m => m.Type);
            Map(m => m.ADSDataType);
        }
    }



    static void MergeCsvFiles()
    {
        string baseDirectory = @"C:\Users\ottoa\OneDrive\Skrivbord\SmallerCompressedListCSV\SmallerCompressedListCSV\SmallerCompressedListCSV\bin\Debug\net8.0";
        string firstCsvFilePath = Path.Combine(baseDirectory, "indexlist.csv");
        string secondCsvFilePath = Path.Combine(baseDirectory, "inputlist.csv");
        string combinedCsvFilePath = Path.Combine(baseDirectory, "smaller-combined-List.csv");

        // Read both CSV files
        List<FirstIndexCsvRecord> firstCsvRecords;
        List<SecondInputCsvRecord> secondCsvRecords;

        using (var reader = new StreamReader(firstCsvFilePath))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            firstCsvRecords = csv.GetRecords<FirstIndexCsvRecord>().ToList();
        }

        using (var reader = new StreamReader(secondCsvFilePath))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            secondCsvRecords = csv.GetRecords<SecondInputCsvRecord>().ToList();
        }

        // Attempt to merge the data
        var combinedRecords = new List<CombinedCsvRecord>();

        // Merge records from first CSV file
        foreach (var firstRecord in firstCsvRecords)
        {
            // Check if the record already exists 
            var existingRecord = combinedRecords.FirstOrDefault(x => x.AdsVariableName == firstRecord.AdsVariableName);

            if (existingRecord == null)
            {
                // If not, create new
                var combinedRecord = new CombinedCsvRecord
                {
                    AdsVariableName = firstRecord.AdsVariableName,
                    ModbusAddress = firstRecord.ModbusAddress,
                    ModbusPermission = firstRecord.ModbusPermission,
                    AddUnit = firstRecord.AddUnit ?? false, // Default false if  null
                    ADSDataType = firstRecord.ADSDataType
            };
                combinedRecords.Add(combinedRecord);
            }
            else
            {
                // If exists, update the existing record
                existingRecord.ModbusAddress = firstRecord.ModbusAddress;
                existingRecord.ModbusPermission = firstRecord.ModbusPermission;
                existingRecord.AddUnit = firstRecord.AddUnit ?? false; // Default false if  null
                existingRecord.ADSDataType = firstRecord.ADSDataType;

        }
        }

        // Add missing records from first CSV file
        foreach (var firstRecord in firstCsvRecords)
        {
            if (!combinedRecords.Any(x => x.AdsVariableName == firstRecord.AdsVariableName))
            {
                var combinedRecord = new CombinedCsvRecord
                {
                    AdsVariableName = firstRecord.AdsVariableName,
                    ModbusAddress = firstRecord.ModbusAddress,
                    ModbusPermission = firstRecord.ModbusPermission,
                    AddUnit = firstRecord.AddUnit ?? false, // Default false if  null
                    ADSDataType = firstRecord.ADSDataType
};
                combinedRecords.Add(combinedRecord);
            }
        }

        // Merge records from second CSV file
        foreach (var secondRecord in secondCsvRecords)
        {
            // Check if the record already exists 
            var existingRecord = combinedRecords.FirstOrDefault(x => x.AdsVariableName == secondRecord.AdsVariableName);

            if (existingRecord == null)
            {
                // If not, create new
                var combinedRecord = new CombinedCsvRecord
                {
                    AdsVariableName = secondRecord.AdsVariableName,
                    Type = secondRecord.Type,
                    Description = secondRecord.Description,
                    ModbusPermission = secondRecord.ModbusPermission,
                    InterpolatePoints = secondRecord.InterpolatePoints ?? false, // Default false if  null
                    ADSDataType = secondRecord.ADSDataType
                };
                combinedRecords.Add(combinedRecord);
            }
            else
            {
                // If exists, update the existing record
                existingRecord.Type = secondRecord.Type;
                existingRecord.Description = secondRecord.Description;
                existingRecord.ModbusPermission = secondRecord.ModbusPermission;
                existingRecord.InterpolatePoints = secondRecord.InterpolatePoints ?? false; // Default false if  null
                existingRecord.ADSDataType = secondRecord.ADSDataType;
            }
        }

        // Add missing records from second CSV file
        foreach (var secondRecord in secondCsvRecords)
        {
            if (!combinedRecords.Any(x => x.AdsVariableName == secondRecord.AdsVariableName))
            {
                var combinedRecord = new CombinedCsvRecord
                {
                    AdsVariableName = secondRecord.AdsVariableName,
                    Type = secondRecord.Type,
                    Description = secondRecord.Description,
                    ModbusPermission = secondRecord.ModbusPermission,
                    InterpolatePoints = secondRecord.InterpolatePoints ?? false, // Default false if  null
                    ADSDataType = secondRecord.ADSDataType
                };
                combinedRecords.Add(combinedRecord);
            }
        }

        // Write the combined new CSV file
        using (var writer = new StreamWriter(combinedCsvFilePath))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteRecords(combinedRecords);
        }

        Console.WriteLine("\nCombined merging of CSV files was created successfully.");
    }

}
