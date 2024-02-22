/*using CsvHelper;
using CsvHelper.Configuration.Attributes;
using System.Globalization;


public class FirstCsvRecord
{
    [Name("AdsVariableName")]
    public string AdsVariableName { get; set; }

    [Name(" ModbusAddress")]
    public int? ModbusAddress { get; set; }

    [Name(" ModbusPermission")]
    public string ModbusPermission { get; set; }
}

public class SecondCsvRecord
{
    [Name("AdsVariableName")]
    public string AdsVariableName { get; set; }

    [Name(" Type")]
    public string Type { get; set; }

    [Name(" ModbusPermission")]
    public string ModbusPermission { get; set; }

    [Name("Description")]
    public string Description { get; set; }
}

public class CombinedCsvRecord
{
    public string AdsVariableName { get; set; }
    public int? ModbusAddress { get; set; }
    public string ModbusPermission { get; set; }
    public string Type { get; set; }
    public string Description { get; set; }
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Welcome to CSV File Merger!");

        while (true)
        {
            Console.WriteLine("\nMainmenu options:");
            Console.WriteLine("\nSelect an option: ");
            Console.WriteLine("\n1. Merge CSV files");
            Console.WriteLine("2. Exit");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.WriteLine("\nCreating a new CSV list from merging...");
                    MergeCsvFiles();
                    Console.WriteLine("\nReturning to the mainmenu!");
                    break;
                case "2":
                    Console.WriteLine("\nExiting CSV File Merger...");
                    return;
                default:
                    Console.WriteLine("\nInvalid option. Please try again.");
                    break;
            }
        }
    }

    static void MergeCsvFiles()
    {
        string baseDirectory = @"C:\Users\ottoa\OneDrive\Skrivbord\SmallerCompressedListCSV\SmallerCompressedListCSV\SmallerCompressedListCSV\bin\Debug\net8.0";
        string firstCsvFilePath = Path.Combine(baseDirectory, "indexlist.csv");
        string secondCsvFilePath = Path.Combine(baseDirectory, "inputlist.csv");
        string combinedCsvFilePath = Path.Combine(baseDirectory, "smaller-combined-List.csv");

        // Read both CSV files
        List<FirstCsvRecord> firstCsvRecords;
        List<SecondCsvRecord> secondCsvRecords;

        using (var reader = new StreamReader(firstCsvFilePath))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            firstCsvRecords = csv.GetRecords<FirstCsvRecord>().ToList();
        }

        using (var reader = new StreamReader(secondCsvFilePath))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            secondCsvRecords = csv.GetRecords<SecondCsvRecord>().ToList();
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
                    ModbusPermission = firstRecord.ModbusPermission
                };
                combinedRecords.Add(combinedRecord);
            }
            else
            {
                // If exists, update the existing record
                existingRecord.ModbusAddress = firstRecord.ModbusAddress;
                existingRecord.ModbusPermission = firstRecord.ModbusPermission;
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
                    ModbusPermission = firstRecord.ModbusPermission
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
                    ModbusPermission = secondRecord.ModbusPermission
                };
                combinedRecords.Add(combinedRecord);
            }
            else
            {
                // If exists, update the existing record
                existingRecord.Type = secondRecord.Type;
                existingRecord.Description = secondRecord.Description;
                existingRecord.ModbusPermission = secondRecord.ModbusPermission;
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
                    ModbusPermission = secondRecord.ModbusPermission
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
}*/
