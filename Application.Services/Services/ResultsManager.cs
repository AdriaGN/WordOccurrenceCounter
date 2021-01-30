using System;
using System.Collections.Generic;
using System.Linq;
using Domain.FileModelType;
using Application.Services.Interfaces;
using Infrastructure.Services.Interfaces;

namespace Application.Services
{
    public class ResultsManager : IResultsManager
    {
        private ILoggerApp loggerApp;

        public ResultsManager(ILoggerApp loggerApp)
        {
            this.loggerApp = loggerApp;
        }

        public void ShowResults(string wordToFind, List<FileToCount> listOfCountedFiles)
        {
            var filesWithOccurrences = this.CountResults(wordToFind, listOfCountedFiles);

            var orderedMatchingFiles = this.OrderTopFilesWithOccurrences(filesWithOccurrences);

            if (orderedMatchingFiles.Count().Equals(0))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("No matches found");
                Console.ForegroundColor = ConsoleColor.Gray;
                this.loggerApp.Trace("No matches found");
            }

            foreach (var file in orderedMatchingFiles)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(file.Key + ": " + file.Value + " occurrences");
                Console.ForegroundColor = ConsoleColor.Gray;
                this.loggerApp.Trace(file.Key + ": " + file.Value + " occurrences");
            }
        }

        public Dictionary<string, int> CountResults(string wordToFind, List<FileToCount> listOfCountedFiles)
        {
            Dictionary<string, int> dictOfMatchingFiles = new Dictionary<string, int>();

            foreach (var file in listOfCountedFiles)
            {
                if (file.WordOccurrences.ContainsKey(wordToFind))
                {
                    dictOfMatchingFiles.Add(file.Name, file.WordOccurrences[wordToFind]);
                }
            }

            return dictOfMatchingFiles;
        }

        public IEnumerable<KeyValuePair<string, int>> OrderTopFilesWithOccurrences(Dictionary<string, int> dictOfMatchingFiles)
        {
            var orderedResults =
                (from resultFile in dictOfMatchingFiles orderby resultFile.Value descending select resultFile).Take(10);

            return orderedResults;
        }
    }
}