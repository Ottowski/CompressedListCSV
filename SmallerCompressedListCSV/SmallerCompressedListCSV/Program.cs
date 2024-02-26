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
            Console.WriteLine("3. Merge create record AdsVariableName's that match and 'Type'");
            Console.WriteLine("0. Exit\n");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.WriteLine("\nCreating a whole combined new CSV list from merging...");
                    MergeWholeCsvFiles();
                    Console.WriteLine("\nReturning to the mainmenu!");
                    break;
                case "2":
                    Console.WriteLine("\nCreating a smaller compressed CSV list from merging certian records...");
                    MergeSmallerCsvFile();
                    Console.WriteLine("\nReturning to the mainmenu!");
                    break;
                case "3":
                    Console.WriteLine("\nCreating a compressed CSV list of matching AdsVariableName's record in both files and type...");
                    AdsVariableNameOnlyMatchCsvFile();
                    Console.WriteLine("\nReturning to the mainmenu!");
                    break;
                case "0":
                    Console.WriteLine("\nYou are exiting CSV File Merger...");
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

        // Find the maximum nr of IdIndex from records
        int maxIdIndex = 0; // set to start at 0
        if (combinedRecords.Any())
        {
            maxIdIndex = combinedRecords.Max(record => int.Parse(record.IdIndex ?? "0")); // Use 0 if null and also parse the string into a integer
        }

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
                ADSDataType = firstRecord.ADSDataType,
                RequestInterval = firstRecord.RequestInterval,
                OutputDataType = firstRecord.OutputDataType
            };
            combinedRecords.Add(combinedRecord);
        }

        // Merge records from the second CSV file
        foreach (var secondRecord in secondCsvRecords)
        {
            var existingRecord = combinedRecords.FirstOrDefault(x => x.AdsVariableName == secondRecord.AdsVariableName);
            if (existingRecord != null)
            {
                // If the record already exists, update it
                existingRecord.Type = secondRecord.Type;
                existingRecord.Description = secondRecord.Description;
                existingRecord.ModbusPermission = secondRecord.ModbusPermission;
                existingRecord.InterpolatePoints = secondRecord.InterpolatePoints ?? false;
                existingRecord.ADSDataType = secondRecord.ADSDataType;
                existingRecord.RequestInterval = secondRecord.RequestInterval;
            }
            else
            {
                // If the record doesn't exist, create a new one
                var combinedRecord = new CombinedCsvRecord
                {
                    IdIndex = (++maxIdIndex).ToString(), // Increased and/or added to maxIdIndex and use the new value of IdIndex
                    AdsVariableName = secondRecord.AdsVariableName,
                    Type = secondRecord.Type,
                    Description = secondRecord.Description,
                    ModbusPermission = secondRecord.ModbusPermission,
                    InterpolatePoints = secondRecord.InterpolatePoints ?? false,
                    ADSDataType = secondRecord.ADSDataType,
                    RequestInterval = secondRecord.RequestInterval
                };
                combinedRecords.Add(combinedRecord);
            }
        }

        return combinedRecords;
    }
    // Method to filter records from "inputlist.csv" based on AdsVariableName values present in "indexlist.csv"
    static void AdsVariableNameOnlyMatchCsvFile()
    {
        try
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string indexCsvFilePath = Path.Combine(baseDirectory, "indexlist.csv");
            string inputCsvFilePath = Path.Combine(baseDirectory, "inputlist.csv");
            string outputCsvFilePath = Path.Combine(baseDirectory, "AdsVariableName-and-type.csv");

            // Read AdsVariableName values from indexlist.csv
            var indexAdsVariableNames = ReadAdsVariableNames(indexCsvFilePath);

            // Read and filter records from inputlist.csv based on indexAdsVariableNames
            var filteredRecords = FilterRecordsByAdsVariableNames(inputCsvFilePath, indexAdsVariableNames);

            // Write the filtered CSV file
            using (var writer = new StreamWriter(outputCsvFilePath))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                // Create ClassMap to configure CSV writing
                csv.Context.RegisterClassMap<SmallerCompressedCsvMap>();
                csv.WriteRecords(filteredRecords);
            }

            Console.WriteLine("\nSuccess!");
            Console.WriteLine("\nFiltered CSV file was created successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error have occurred: {ex.Message}");
        }
    }

    // Method to read AdsVariableName values from "indexlist.csv"
    static List<string> ReadAdsVariableNames(string filePath)
    {
        using (var reader = new StreamReader(filePath))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
            return csv.GetRecords<FirstIndexCsvRecord>()
                      .Select(record => record.AdsVariableName)
                      .ToList();
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.
        }
    }

    // Method to filter records from "inputlist.csv" based on AdsVariableName values present in "indexlist.csv"
    static List<CombinedCsvRecord> FilterRecordsByAdsVariableNames(string filePath, List<string> indexAdsVariableNames)
    {
        using (var reader = new StreamReader(filePath))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            var inputRecords = csv.GetRecords<SecondInputCsvRecord>().ToList();
            return inputRecords.Where(record => indexAdsVariableNames.Contains(record.AdsVariableName))
                               .Select(record => new CombinedCsvRecord
                               {
                                   AdsVariableName = record.AdsVariableName,
                                   Type = record.Type
                               })
                               .ToList();
        }
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
                combinedRecords[i].IdIndex = (i + 1).ToString(); // Starting from 1 without leading zeros
            }


            // Write the combined new CSV file
            using (var writer = new StreamWriter(combinedCsvFilePath))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(combinedRecords);
            }

            Console.WriteLine("\nSuccess!");
            Console.WriteLine("\nCombined merging of CSV files was created successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error have occurred: {ex.Message}");
        }
    }

    static void MergeSmallerCsvFile()
    {
        try
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string firstCsvFilePath = Path.Combine(baseDirectory, "indexlist.csv");
            string secondCsvFilePath = Path.Combine(baseDirectory, "inputlist.csv");
            string combinedCsvFilePath = Path.Combine(baseDirectory, "smaller-compressed-List.csv");

            List<CombinedCsvRecord> combinedRecords = ReadAndMergeCsvFiles(firstCsvFilePath, secondCsvFilePath);

            // Sort the combined records based on the IdIndex
            combinedRecords = combinedRecords.OrderBy(record => record.IdIndex).ToList();

            // Assign new sequential IdIndex values
            for (int i = 0; i < combinedRecords.Count; i++)
            {
                combinedRecords[i].IdIndex = (i + 1).ToString(); // Starting from 1 without leading zeros
            }


            // Write the combined new CSV file
            using (var writer = new StreamWriter(combinedCsvFilePath))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                // Create ClassMap to ignore unwanted properties
                csv.Context.RegisterClassMap<CombinedCsvRecordMap>();
                csv.WriteRecords(combinedRecords);
            }

            Console.WriteLine("\nSuccess!");
            Console.WriteLine("\nCombined merging of CSV files was created successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error have occurred: {ex.Message}");
        }
    }

}
