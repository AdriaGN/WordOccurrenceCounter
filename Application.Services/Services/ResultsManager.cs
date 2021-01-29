using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using Domain.FileModelType;
using Application.Services.Interfaces;

namespace Application.Services
{
    public class ResultsManager : IResultsManager
    {
        public void ShowResults(string wordToFind, List<FileToCount> listOfCountedFiles)
        {
            var filesWithOccurrences = this.CountResults(wordToFind, listOfCountedFiles);

            var orderedMatchingFiles = this.OrderTopFilesWithOccurrences(filesWithOccurrences);

            foreach (var file in orderedMatchingFiles)
            {
                Console.Write(file.Key + ": " + file.Value + " occurrences" + Environment.NewLine);
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