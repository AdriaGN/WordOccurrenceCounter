using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Application.Services;
using Application.Services.Interfaces;
using Domain.FileModelType;

namespace Application.Testing
{
    [TestClass]
    public class ResultsManagerTests
    {
        // Dictionary to mock the results, dictionary to mock the parsed files (simple and multiple) and the word to search
        private static readonly Dictionary<string, int> MockDictionary = new Dictionary<string, int>
            { 
                { "hello", 6 },
                { "there", 5 },
                { "i", 4 },
                { "am", 1 },
                { "a", 2 },
                { "text", 3 },
                { "file", 1 }
            };
        private static readonly Dictionary<string, int> mockParsedFilesSimpleDictionary = new Dictionary<string, int>()
            {
                { "TextName", 6 }
            };
        private static readonly Dictionary<string, int> mockParsedFilesMultipleDictionary = new Dictionary<string, int>()
            {
                { "TextName_1", 10 },
                { "TextName_2", 9 },
                { "TextName_3", 8 },
                { "TextName_4", 7 },
                { "TextName_5", 6 },
                { "TextName_10", 5 },
                { "TextName_9", 4 },
                { "TextName_8", 3 },
                { "TextName_7", 2 },
                { "TextName_6", 1 },
                { "TextName_11", 1 }
            };
        private static string wordToSearch = "hello";

        // DI
        private IResultsManager resultManager;

        [TestInitialize]
        public void TestInitialization()
        {
            this.resultManager = new ResultsManager();
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void ShowResults_ListOfCountedFilesIsNull_ThrowsNullReferenceException()
        {
            this.resultManager.ShowResults(wordToSearch, null);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void ShowResults_ListOfCountedFilesIsEmpty_ThrowsNullReferenceException()
        {
            FileToCount fileToCount = new FileToCount();
            List<FileToCount> listToCount = new List<FileToCount> { fileToCount };

            this.resultManager.ShowResults(wordToSearch, listToCount);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShowResults_WordToFindIsNull_ThrowsArgumentNullException()
        {
            FileToCount fileToCount = new FileToCount()
            {
                Name = "TextName",
                WordOccurrences = MockDictionary
            };
            List<FileToCount> listToCount = new List<FileToCount> { fileToCount };

            this.resultManager.ShowResults(null, listToCount);
        }

        [TestMethod]
        public void ShowResults_WordToFindIsEmpty_PrintsEmptyResultsOnConsole()
        {
            using (StringWriter consoleWritter = new StringWriter())
            {
                Console.SetOut(consoleWritter);

                FileToCount fileToCount = new FileToCount()
                                              {
                                                  Name = "TextName",
                                                  WordOccurrences = MockDictionary
                                              };
                List<FileToCount> listToCount = new List<FileToCount> { fileToCount };
                var expectedConsoleOutput = ("");

                this.resultManager.ShowResults("", listToCount);

                Assert.AreEqual(expectedConsoleOutput, consoleWritter.ToString());
            }
        }

        [TestMethod]
        public void ShowResults_AllInputsAreCorrect_PrintsCorrectResultsOnConsole()
        {
            using (StringWriter consoleWritter = new StringWriter())
            {
                Console.SetOut(consoleWritter);

                FileToCount fileToCount = new FileToCount()
                                              {
                                                  Name = "TextName",
                                                  WordOccurrences = MockDictionary
                                              };
                List<FileToCount> listToCount = new List<FileToCount> { fileToCount };
                var expectedConsoleOutput = ("TextName: 6 occurrences" + Environment.NewLine);

                this.resultManager.ShowResults(wordToSearch, listToCount);

                Assert.AreEqual(expectedConsoleOutput, consoleWritter.ToString());
            }
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void CountResults_ListOfCountedFilesIsNull_ThrowsNullReferenceException()
        {
            this.resultManager.CountResults(wordToSearch, null);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void CountResults_ListOfCountedFilesIsEmpty_ThrowsNullReferenceException()
        {
            FileToCount fileToCount = new FileToCount();
            List<FileToCount> listToCount = new List<FileToCount> { fileToCount };

            this.resultManager.CountResults(wordToSearch, listToCount);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CountResults_WordToFindIsNull_ThrowsArgumentNullException()
        {
            FileToCount fileToCount = new FileToCount()
                                          {
                                              Name = "TextName",
                                              WordOccurrences = MockDictionary
                                          };
            List<FileToCount> listToCount = new List<FileToCount> { fileToCount };

            this.resultManager.CountResults(null, listToCount);
        }

        [TestMethod]
        public void CountResults_WordToFindIsEmpty_ReturnsEmptyDictionary()
        {
            FileToCount fileToCount = new FileToCount()
                                          {
                                              Name = "TextName",
                                              WordOccurrences = MockDictionary
                                          };
            List<FileToCount> listToCount = new List<FileToCount> { fileToCount };
            Dictionary<string, int> expectedEmptyDictionary = new Dictionary<string, int>();

            var countedResults = this.resultManager.CountResults("", listToCount);

            CollectionAssert.AreEqual(expectedEmptyDictionary, countedResults);
        }

        [TestMethod]
        public void CountResults_AllInputsAreCorrect_ReturnsDictionaryWithCorrectResults()
        {
            FileToCount fileToCount = new FileToCount()
                                          {
                                              Name = "TextName",
                                              WordOccurrences = MockDictionary
                                          };
            List<FileToCount> listToCount = new List<FileToCount> { fileToCount };

            var countedResults = this.resultManager.CountResults(wordToSearch, listToCount);

            Assert.AreEqual(countedResults.Count, 1);
            Assert.AreEqual(mockParsedFilesSimpleDictionary.Keys.First(), countedResults.Keys.First());
            CollectionAssert.AreEqual(mockParsedFilesSimpleDictionary.Values, countedResults.Values);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void OrderTopFilesWithOccurrences_DictionaryOfMatchesIsNull_ThrowsArgumentNullException()
        {
            this.resultManager.OrderTopFilesWithOccurrences(null);
        }

        [TestMethod]
        public void OrderTopFilesWithOccurrences_DictionaryOfMatchesIsEmpty_ReturnsEmptyDictionary()
        {
            Dictionary<string, int> emptyDictionary = new Dictionary<string, int>();
            IEnumerable<KeyValuePair<string, int>> emptyEnumerable = new List<KeyValuePair<string, int>>();

            var returnedEnumerable = this.resultManager.OrderTopFilesWithOccurrences(emptyDictionary);

            foreach (var pair in returnedEnumerable)
            {
                Assert.AreEqual(emptyEnumerable, pair);
            }
        }

        [TestMethod]
        public void OrderTopFilesWithOccurrences_DictionaryOfMatchesIsCorrect_ReturnsCorrectDictionaryWithCorrectPair()
        {
            KeyValuePair<string, int> expectedPair = new KeyValuePair<string, int>("TextName", 6);

            var expectedValuePairsList = this.resultManager.OrderTopFilesWithOccurrences(mockParsedFilesSimpleDictionary);

            foreach (var pair in expectedValuePairsList)
            {
                Assert.AreEqual(expectedPair, pair);
            }
        }

        [TestMethod]
        public void OrderTopFilesWithOccurrences_DictionaryOfMatchesIsCorrectAndTheresElevenFiles_ReturnCorrectResultsForTheTopTen()
        {
            IEnumerable<KeyValuePair<string, int>> expectedValuePairsList =
                new List<KeyValuePair<string, int>>
                    {
                        new KeyValuePair<string, int>("TextName_1", 10),
                        new KeyValuePair<string, int>("TextName_2", 9),
                        new KeyValuePair<string, int>("TextName_3", 8),
                        new KeyValuePair<string, int>("TextName_4", 7),
                        new KeyValuePair<string, int>("TextName_5", 6),
                        new KeyValuePair<string, int>("TextName_10", 5),
                        new KeyValuePair<string, int>("TextName_9", 4),
                        new KeyValuePair<string, int>("TextName_8", 3),
                        new KeyValuePair<string, int>("TextName_7", 2),
                        new KeyValuePair<string, int>("TextName_6", 1)
                    };

            var returnedPairValuesList = this.resultManager.OrderTopFilesWithOccurrences(mockParsedFilesMultipleDictionary);

            using (var expectedPair = expectedValuePairsList.GetEnumerator())
            using (var returnedPair = returnedPairValuesList.GetEnumerator())
            {
                while (expectedPair.MoveNext() && returnedPair.MoveNext())
                {
                    Assert.AreEqual(expectedPair.Current, returnedPair.Current);
                }
            }
        }
    }
}
