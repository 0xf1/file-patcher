using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace patcher
{
    class Program
    {
        static void Main(string[] args)
        {
            CheckArgs(args);
            PathFile(args[0], args[1]);

        }

        private static void PathFile(string bytesFileName, string binaryFileName)
        {
            string searchString, replaceString;
            using (var sr = File.OpenText(bytesFileName))
            {
                searchString = sr.ReadLine();
                replaceString = sr.ReadLine();
            }

            var stringBytes = searchString.Split(' ');
            var searchPattern = new byte[stringBytes.Length];
            var searchPatternSkip = new bool[stringBytes.Length];
            for (int i = 0; i < stringBytes.Length; i++)
            {
                if (stringBytes[i] == "??")
                {
                    searchPatternSkip[i] = true;
                }
                else {
                    searchPattern[i] = byte.Parse(stringBytes[i], System.Globalization.NumberStyles.HexNumber);
                }
                
            }

            stringBytes = replaceString.Split(' ');
            var replacePattern = new byte[stringBytes.Length];
            var replacePatternSkip = new bool[stringBytes.Length];
            for (int i = 0; i < stringBytes.Length; i++)
            {
                if (stringBytes[i] == "??")
                {
                    replacePatternSkip[i] = true;
                }
                else
                {
                    replacePattern[i] = byte.Parse(stringBytes[i], System.Globalization.NumberStyles.HexNumber);
                }
            }

            var offsets = new List<int>();

            var fileBytes = File.ReadAllBytes(binaryFileName);
            for (int filePos = 0; filePos < fileBytes.Length; filePos++)
            {
                if (filePos + searchPattern.Length > fileBytes.Length)
                {
                    break;
                }
                
                for (int patternPos = 0; patternPos < searchPattern.Length; patternPos++)
                {
                    if (fileBytes[filePos] != searchPattern[patternPos] && !searchPatternSkip[patternPos] )
                    {
                        break;
                    }
                    filePos++;

                    if (patternPos == searchPattern.Length -1)
                    {
                        offsets.Add(filePos - patternPos - 1);
                    }
                }    
                
            }
            
            foreach(int offset in offsets)
            {
                //Console.WriteLine($"Offset found: {offset} (hex: {offset.ToString("X10")})");
                for(int replacePatterPos = 0; replacePatterPos < replacePattern.Length; replacePatterPos++)
                {
                    if (!replacePatternSkip[replacePatterPos])
                    {
                        fileBytes[offset + replacePatterPos] = replacePattern[replacePatterPos];
                    }
                }
            }

            File.WriteAllBytes(binaryFileName, fileBytes);
            Console.WriteLine($"Applied total patches count: {offsets.Count}");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private static void CheckArgs(string[] args)
        {
            if (args.Length == 0 || args[0] == "/?" || args[0] == "help" || args[0] == "?" || args.Length == 1)
            {
                Console.WriteLine("Usage:");
                Console.WriteLine("patcher.exe [txt file name contains bytes] [file name to path]");
                Console.WriteLine("[txt file name contains bytes] - Text file with 2 lines:");
                Console.WriteLine("  First line: hex bytes to search");
                Console.WriteLine("  Second line: hex bytes to replace");
                Console.WriteLine("  example: ");
                Console.WriteLine("  mycrack.txt");
                Console.WriteLine("  1B 80 90 90 E6");
                Console.WriteLine("  1B 90 90 90 90");
                Console.WriteLine("[file name to path] - any binary file name. This file will be overwritten.");
                Console.WriteLine();
                Console.WriteLine("Example:");
                Console.WriteLine("patcher.exe mycrack.txt notepad.exe");
                Console.WriteLine();
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                System.Environment.Exit(0);
            }

            if (!File.Exists(args[0]))
            {
                Console.WriteLine($"File {args[0]} not exists.");
                System.Environment.Exit(-1);
            }

            if (!File.Exists(args[1]))
            {
                Console.WriteLine($"File {args[1]} not exists.");
                System.Environment.Exit(-1);
            }
        }
    }
}
