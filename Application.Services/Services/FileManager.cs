using System;
using System.IO;
using System.Collections.Generic;
using Application.Services.Interfaces;
using Domain.FileModelType;

namespace Application.Services
{
    public class FileManager : IFileManager
    {
        private readonly IFileTextParser fileTextParser;

        public FileManager(IFileTextParser fileTextParser)
        {
            this.fileTextParser = fileTextParser;
        }

        public List<FileToCount> GetFileWordsOccurrencesCounted(string directoryPath)
        {
            this.CheckDirectoryPathAndFilesAreValid(directoryPath);

            var fileNames = this.GetFileNamesFromDirectory(directoryPath);
            
            var filesWithWordsCounted = this.GetListOfAnalyzedFilesAndResults(directoryPath, fileNames);

            return filesWithWordsCounted;
        }

        public void CheckDirectoryPathAndFilesAreValid(string directoryPath)
        {
            if (directoryPath == null)
            {
                throw new ArgumentNullException("The directory path is incorrect or doesn't exist.");
            }

            this.AdaptDirectoryPath(directoryPath);

            if (!Directory.Exists(directoryPath))
            {
                throw new DirectoryNotFoundException("The directory path is incorrect or doesn't exist.");
            }

            if (Directory.GetFiles(directoryPath, "*.txt").Length == 0)
            {
                throw new FileNotFoundException("No text files where found in the directory");
            }
        }

        private string AdaptDirectoryPath(string directoryPath)
        {
            if (!(directoryPath.EndsWith("/") || (directoryPath.EndsWith("\\"))))
            {
                directoryPath = directoryPath + "\\";
            }

            return directoryPath;
        }

        public List<string> GetFileNamesFromDirectory(string directoryPath)
        {
            var files = Directory.GetFiles(directoryPath, "*.txt");
            var fileNamesList = new List<string>();

            foreach (var file in files)
            {
              fileNamesList.Add(Path.GetFileName(file));  
            }

            return fileNamesList;
        }

        public List<FileToCount> GetListOfAnalyzedFilesAndResults(string directoryPath, List<string> fileNameList)
        {
            List<FileToCount> analyzedFilesList = new List<FileToCount>();

            foreach (var currentFile in fileNameList)
            {
                var filePath = directoryPath + currentFile;

                var fileText = this.GetTextFromFile(filePath);


                var occurrencesWordsDictionary = this.fileTextParser.GetOccurrencesWordDictionary(fileText);

                FileToCount file = new FileToCount() 
                {
                    Name = currentFile,
                    WordOccurrences = occurrencesWordsDictionary
                };

                analyzedFilesList.Add(file);
            }

            return analyzedFilesList;
        }

        public string GetTextFromFile(string textFileNamePath)
        {
            var textFromFile = File.ReadAllText(textFileNamePath);
            return textFromFile;
        }
    }
}
