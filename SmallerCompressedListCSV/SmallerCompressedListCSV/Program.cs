using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace SmallerCompressedListCSV
{

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
                Console.WriteLine("2. Merge only some records to new CSV file");
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
                        Console.WriteLine("\nCreating an even smaller CSV list...");
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
            try
            {
                string baseDirectory = @"C:\Users\ottoa\OneDrive\Skrivbord\SmallerCompressedListCSV\SmallerCompressedListCSV\SmallerCompressedListCSV\bin\Debug\net8.0";
                string firstCsvFilePath = Path.Combine(baseDirectory, "indexlist.csv");
                string secondCsvFilePath = Path.Combine(baseDirectory, "inputlist.csv");
                string combinedCsvFilePath = Path.Combine(baseDirectory, "even-smaller-combined-List.csv");

                List<FirstIndexCsvRecord> firstCsvRecords;
                List<SecondInputCsvRecord> secondCsvRecords;

                // Read both CSV files

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
                        existingRecord.ADSDataType = secondRecord.ADSDataType;
                    }
                    else
                    {
                        var combinedRecord = new CombinedCsvRecord
                        {
                            AdsVariableName = secondRecord.AdsVariableName,
                            Type = secondRecord.Type,
                            ADSDataType = secondRecord.ADSDataType,
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
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
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
            try
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
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }

}
