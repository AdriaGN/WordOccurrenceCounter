using System.Collections.Generic;
using System.Text.RegularExpressions;
using Application.Services.Interfaces;
using Domain.FileModelType;

namespace Application.Services
{

    public class FileParser : IFileTextParser
    {
        public Dictionary<string, int> GetOccurrencesWordDictionary(string textToAnalyze)
        {
            var cleanedTextToCount = this.RemoveSpecialCharactersFromText(textToAnalyze);

            var wordOccurrencesDictionary = this.CountWordsFromText(cleanedTextToCount.ToLower());

            return wordOccurrencesDictionary;
        }

        public string RemoveSpecialCharactersFromText(string textToClean)
        {
            Regex regexTool = new Regex("(?:[^a-zA-Z0-9 ]|(?<=[\'])s)");

            var cleanedText = regexTool.Replace(textToClean, " ");

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
                var wordCounter = new MapReduceWord(word, 1);
                
                listWithMappedWords.Add(wordCounter);
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
