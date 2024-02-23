using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

//namespace SmallerCompressedListCSV;

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
                    Console.WriteLine("\nCreating a whole combined new CSV list from merging...");
                    MergeWholeCsvFiles();
                    Console.WriteLine("Returning to the mainmenu!");
                    break;
                case "2":
                    Console.WriteLine("\nCreating a smaller compressed CSV list from merging certian records...");
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
    static List<CombinedCsvRecord> ReadAndMergeCsvFiles(string firstCsvFilePath, string secondCsvFilePath)
    {
        List<FirstIndexCsvRecord> firstCsvRecords;
        List<SecondInputCsvRecord> secondCsvRecords;

        // Read first CSV file
        using (var reader = new StreamReader(firstCsvFilePath))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            firstCsvRecords = csv.GetRecords<FirstIndexCsvRecord>().ToList();
        }

        // Read second CSV file
        using (var reader = new StreamReader(secondCsvFilePath))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            secondCsvRecords = csv.GetRecords<SecondInputCsvRecord>().ToList();
        }

        // Merge records from both CSV files
        var combinedRecords = new List<CombinedCsvRecord>();

        // Merge records from the first CSV file
        foreach (var firstRecord in firstCsvRecords)
        {
            var combinedRecord = new CombinedCsvRecord
            {
                IdIndex = firstRecord.IdIndex,
                AdsVariableName = firstRecord.AdsVariableName,
                ModbusAddress = firstRecord.ModbusAddress,
                ModbusPermission = firstRecord.ModbusPermission,
                AddUnit = firstRecord.AddUnit ?? false,
                ADSDataType = firstRecord.ADSDataType
            };
            combinedRecords.Add(combinedRecord);
        }

        // Merge records from the second CSV file
        foreach (var secondRecord in secondCsvRecords)
        {
            var existingRecord = combinedRecords.FirstOrDefault(x => x.IdIndex == secondRecord.IdIndex && x.AdsVariableName == secondRecord.AdsVariableName);
            if (existingRecord != null)
            {
                existingRecord.Type = secondRecord.Type;
                existingRecord.Description = secondRecord.Description;
                existingRecord.ModbusPermission = secondRecord.ModbusPermission;
                existingRecord.InterpolatePoints = secondRecord.InterpolatePoints ?? false;
                existingRecord.ADSDataType = secondRecord.ADSDataType;
            }
            else
            {
                var combinedRecord = new CombinedCsvRecord
                {
                    IdIndex = secondRecord.IdIndex,
                    AdsVariableName = secondRecord.AdsVariableName,
                    Type = secondRecord.Type,
                    Description = secondRecord.Description,
                    ModbusPermission = secondRecord.ModbusPermission,
                    InterpolatePoints = secondRecord.InterpolatePoints ?? false,
                    ADSDataType = secondRecord.ADSDataType
                };
                combinedRecords.Add(combinedRecord);
            }
        }

        return combinedRecords;
    }


    static void MergeWholeCsvFiles()
    {
        try
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string firstCsvFilePath = Path.Combine(baseDirectory, "indexlist.csv");
            string secondCsvFilePath = Path.Combine(baseDirectory, "inputlist.csv");
            string combinedCsvFilePath = Path.Combine(baseDirectory, "combinedlist.csv");

            List<CombinedCsvRecord> combinedRecords = ReadAndMergeCsvFiles(firstCsvFilePath, secondCsvFilePath);

            // Sort the combined records based on the IdIndex
            combinedRecords = combinedRecords.OrderBy(record => record.IdIndex).ToList();

            // Assign new sequential IdIndex values
            for (int i = 0; i < combinedRecords.Count; i++)
            {
                combinedRecords[i].IdIndex = i + 1; // Assuming IdIndex starts from 1
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

    static void MergeSmallerCsvFile()
    {
        try
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string firstCsvFilePath = Path.Combine(baseDirectory, "indexlist.csv");
            string secondCsvFilePath = Path.Combine(baseDirectory, "inputlist.csv");
            string combinedCsvFilePath = Path.Combine(baseDirectory, "even-smaller-combined-List.csv");

            List<CombinedCsvRecord> combinedRecords = ReadAndMergeCsvFiles(firstCsvFilePath, secondCsvFilePath);

            // Sort the combined records based on the IdIndex
            combinedRecords = combinedRecords.OrderBy(record => record.IdIndex).ToList();

            // Assign new sequential IdIndex values
            for (int i = 0; i < combinedRecords.Count; i++)
            {
                combinedRecords[i].IdIndex = i + 1; // Assuming IdIndex starts from 1
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

}