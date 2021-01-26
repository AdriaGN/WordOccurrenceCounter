namespace Application.Services.Interfaces
{
    using System.Collections.Generic;

    public interface IFileManager
    {
        Dictionary<string, string> OperateFilesToAnalyze(string directoryPath); //will return a list with the text filename and the text

        void CheckDirectoryPathAndFilesAreValid(string directoryPath); // checks that the directory path and its files exist

        List<string> GetFileNamesFromDirectory(string directoryPath); //will return a list with filenames to process

        string GetTextFromFile(string filename); //will return the text of a file

        Dictionary<string, string> CreateFileTextDictionary(List<string> fileNameList); //will return a dictionary with every file and its text
    }
}