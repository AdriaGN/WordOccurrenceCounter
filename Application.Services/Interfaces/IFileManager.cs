using System.Collections.Generic;
using Domain.FileModelType;

namespace Application.Services.Interfaces
{
    public interface IFileManager
    {
        List<FileToCount> GetFileWordsOccurrencesCounted(string directoryPath);

        string CheckDirectoryPathAndFilesAreValid(string directoryPath);

        List<string> GetFileNamesFromDirectory(string directoryPath);

        List<FileToCount> GetListOfAnalyzedFilesAndResults(string directoryPath, List<string> fileNameList);

        string GetTextFromFile(string textFileNamePath);
    }
}