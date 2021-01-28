using System.Collections.Generic;
using System.Linq;
using Domain.FileModelType;

namespace Application.Services.Interfaces
{
    public interface IResultsManager
    {
        void ShowResults(string wordToFind, List<FileToCount> listOfCountedFiles);

        Dictionary<string, int> CountResults(string wordToFind, List<FileToCount> listOfCountedFiles);

        IEnumerable<KeyValuePair<string, int>> OrderTopFilesWithOccurrences(Dictionary<string, int> listOfMatchingFiles);
    }
}