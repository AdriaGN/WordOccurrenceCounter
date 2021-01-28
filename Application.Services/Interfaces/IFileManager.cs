using System.Collections.Generic;
using Domain.FileModelType;

namespace Application.Services.Interfaces
{
    

    public interface IFileManager
    {
        List<FileToCount> GetFileWordsOccurrencesCounted(string directoryPath); //will return a list with the a file type with its name and a dictionary with counted words

        void CheckDirectoryPathAndFilesAreValid(string directoryPath); // checks that the directory path and its files exist

        List<string> GetFileNamesFromDirectory(string directoryPath); //will return a list with filenames to process

        List<FileToCount> GetListOfAnalyzedFilesAndResults(string directoryPath, List<string> fileNameList); //will return a dictionary with every file and its text

        string GetTextFromFile(string textFileNamePath); //will return the text of a file
    }
}