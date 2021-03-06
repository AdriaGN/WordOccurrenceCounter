﻿using System.Collections.Generic;

namespace Application.Services.Interfaces
{
    public interface IFileTextParser
    {
        Dictionary<string, int> GetOccurrencesWordDictionary(string textToAnalyze);

        string RemoveSpecialCharactersFromText(string textToClean);

        Dictionary<string, int> CountWordsFromText(string textPreparedToCount);
    }
}