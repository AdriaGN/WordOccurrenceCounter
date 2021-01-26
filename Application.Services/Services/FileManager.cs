using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Services.Interfaces;

namespace Application.Services
{
    public class FileManager : IFileManager
    {
        public Dictionary<string, string> OperateFilesToAnalyze(string directoryPath)
        {
            // before anything, we'll check that the directory exists and has .txt files
            this.CheckDirectoryPathAndFilesAreValid(directoryPath);

            // first we'll get the filenames from the user-specified directory
            var fileNames = this.GetFileNamesFromDirectory(directoryPath);

            // after getting the filenames we'll get the text from the file and return it with its text
            var filesWithTextProcessed = new Dictionary<string, string>();

            filesWithTextProcessed = this.CreateFileTextDictionary(fileNames);

            // after processing the texts, we return them
            return filesWithTextProcessed;
        }

        public void CheckDirectoryPathAndFilesAreValid(string directoryPath)
        {
            if (Directory.Exists(directoryPath))
            {
                throw new ArgumentException("The directory path is incorrect or doesn't exist.");
            }

            if (Directory.GetFiles(directoryPath, "*.txt").Length == 0)
            {
                throw new FileNotFoundException("No text files where found in the directory");
            }
        } //completed

        public List<string> GetFileNamesFromDirectory(string directoryPath)
        {
            var files = Directory.GetFiles(directoryPath, "*.txt");
            var fileNamesList = new List<string>();

            foreach (var file in files)
            {
              fileNamesList.Add(Path.GetFileName(file));  
            }

            return fileNamesList;
        } // completed

        public string GetTextFromFile(string textFileName)
        {
            var textFromFile = File.ReadAllText(textFileName);
            return textFromFile;
        } // completed

        public Dictionary<string, string> CreateFileTextDictionary(List<string> fileNameList)
        {
            var fileTextDictionary = new Dictionary<string, string>();
            Parallel.ForEach(
                fileNameList,
                (currentFile) =>
                    {
                        fileTextDictionary.Add(currentFile, this.GetTextFromFile(currentFile));
                    });

            return fileTextDictionary;
        } // completed
    }
}
