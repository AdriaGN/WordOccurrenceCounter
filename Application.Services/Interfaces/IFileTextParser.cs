namespace Application.Services.Interfaces
{
    using System.Collections.Generic;

    public interface IFileTextParser
    {
        Dictionary<string, int> GetOccurrencesWordDictionary(string textToAnalyze);

        string RemoveSpecialCharactersFromText(string textToClean);

        Dictionary<string, int> CountWordsFromText(string textPreparedToCount);
    }
}