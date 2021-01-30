using System.Collections.Generic;
using System.Text.RegularExpressions;
using Application.Services.Interfaces;
using Domain.FileModelType;

namespace Application.Services
{
    public class FileTextParser : IFileTextParser
    {
        public Dictionary<string, int> GetOccurrencesWordDictionary(string textToAnalyze)
        {
            var cleanedTextToCount = this.RemoveSpecialCharactersFromText(textToAnalyze);
            var wordOccurrencesDictionary = this.CountWordsFromText(cleanedTextToCount.ToLower());

            return wordOccurrencesDictionary;
        }

        public string RemoveSpecialCharactersFromText(string textToClean)
        {
            Regex regexNewLines = new Regex("(?:(\\r\\n\\r\\n))");
            var cleanedText = regexNewLines.Replace(textToClean, " ");

            Regex regexSpecialCharactersRegex = new Regex("(?:[^\\p{L}0-9 ]|(?<=[\'])s)");
            cleanedText = regexSpecialCharactersRegex.Replace(cleanedText, "");
            
            return cleanedText;
        }

        public Dictionary<string, int> CountWordsFromText(string textPreparedToCount)
        {
            var listWithMappedWords = this.Mapper(textPreparedToCount);
            var dictionaryWithOccurrences = this.Reducer(listWithMappedWords);

            return dictionaryWithOccurrences;
        }

        private List<MapReduceWord> Mapper(string textPreparedToCount)
        {
            var listWithMappedWords = new List<MapReduceWord>();

            foreach (var word in textPreparedToCount.Split())
            {
                if (word != string.Empty)
                {
                    var wordCounter = new MapReduceWord(word, 1);
                    listWithMappedWords.Add(wordCounter);
                }
            }

            return listWithMappedWords;
        }

        private Dictionary<string, int> Reducer(List<MapReduceWord> mappedWordsList)
        {
            var dictionaryWithCountedWords = new Dictionary<string, int>();

            foreach (var occurrence in mappedWordsList)
            {
                if (dictionaryWithCountedWords.ContainsKey(occurrence.Word) == false)
                {
                    dictionaryWithCountedWords.Add(occurrence.Word, occurrence.Occurrences);
                }
                else
                {
                    dictionaryWithCountedWords[occurrence.Word] += 1;
                }
            }

            return dictionaryWithCountedWords;
        }
    }
}
