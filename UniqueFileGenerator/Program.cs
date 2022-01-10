﻿global using static System.Console;
using Spectre.Console;

namespace UniqueFileGenerator;

public static class Program
{
    public static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            PrintInstructions();
            return;
        }

        Settings settings;
        try
        {
            settings = new Settings(args);

            // If the user requested a high number of files, confirm the operation.
            if (settings.IsHighFileCount && !AnsiConsole.Confirm(ResourceStrings.Warning))
            {
                AnsiConsole.WriteLine(ResourceStrings.Cancelled);
                return;
            }

            var fileHandler = new FileHandler(settings);

            fileHandler.SaveFiles();
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine("[red]The operation was aborted because an error occurred.[/]");
            AnsiConsole.WriteException(ex, ExceptionFormats.Default);
            return;
        }

        WriteLine($"{settings.FileCount} files created.");
    }

    private static void PrintInstructions()
    {
        var outerTable = new Table();
        outerTable.AddColumn("Unique File Generator");
        outerTable.AddRow("Create unique files with unique contents using random numbers.");
        outerTable.AddEmptyRow();

        const string usage = "Usage: Pass in the number of files and, optionally, add additional arguments.\n\n" +
            "Examples:\n" +
            "   uniquefilegen 10  [gray](Creates 10 files with the default settings)[/]\n" +
            "   uniquefilegen 1000 -p TEST- e txt  [gray](Creates 1,000 files in the format \"TEST-12345.txt\")[/]";
        outerTable.AddRow(usage);
        outerTable.AddEmptyRow();

        var argTable = new Table();
        argTable.Border(TableBorder.None);
        argTable.AddColumn("Arg");
        argTable.AddColumn("Description");
        argTable.HideHeaders();
        argTable.Columns[0].PadRight(3);
        argTable.AddRow("-p", "File name prefix. If the last character is alphanumeric, a space will be added after it.");
        argTable.AddRow("-e", "The desired file extension (with no opening period).");
        argTable.AddRow("-o", "Specify an output subfolder. Defaults to \"output\". Multiple terms are okay.");
        argTable.AddRow("-s", "The size of each file in bytes. (They will be populated with random alphanumeric characters.)");

        outerTable.AddRow(argTable);

        AnsiConsole.Write(outerTable);
    }

    private static class ResourceStrings
    {
        public const string Warning = "You've requested the creation of many files. Do you want to continue?";
        public const string Cancelled = "Operation cancelled.";
    }


}