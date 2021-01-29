using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Application.Services;
using Application.Services.Interfaces;

namespace Application.Testing
{
    [TestClass]
    public class FileTextParserTests
    {
        // Number of existing words in dictionaries
        private static int numberOfkeys = 4;

        // DI
        private IFileTextParser fileTextParser;

        [TestInitialize]
        public void TestInitialization()
        {
            this.fileTextParser = new FileTextParser();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetOccurrencesWordDictionary_TextIsNull_ThrowsArgumentNullException()
        {
            this.fileTextParser.GetOccurrencesWordDictionary(null);
        }

        [TestMethod]
        public void GetOccurrencesWordDictionary_TextIsEmpty_ReturnsEmptyDictionary()
        {
            Dictionary<string, int> expectedReturn = new Dictionary<string, int>();
            var returnedDictionary = this.fileTextParser.GetOccurrencesWordDictionary(string.Empty);
            CollectionAssert.AreEqual(expectedReturn, returnedDictionary);
        }

        [TestMethod]
        public void GetOccurrencesWordDictionary_TextIsCorrect_ReturnsEmptyDictionary()
        {
            var inputString = "this is a x x x x !!!! ???? %%%%";
            Dictionary<string, int> expectedReturn = new Dictionary<string, int> { { "this", 1 }, { "is", 1 }, { "a", 1 }, { "x", 4 } };
            Dictionary<string, int> returnedDictionary = this.fileTextParser.GetOccurrencesWordDictionary(inputString);
            Assert.IsTrue(returnedDictionary.Keys.Count == numberOfkeys);
            CollectionAssert.AreEqual(expectedReturn, returnedDictionary);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RemoveSpecialCharactersFromText_TextIsNull_ThrowsArgumentNullException()
        {
            this.fileTextParser.RemoveSpecialCharactersFromText(null);
        }

        [TestMethod]
        public void RemoveSpecialCharactersFromText_TextIsEmpty_ReturnsEmptyText()
        {
            this.fileTextParser.RemoveSpecialCharactersFromText(string.Empty);
        }

        [TestMethod]
        public void RemoveSpecialCharactersFromText_TextHasNoSpecialCharacters_ReturnsSameText()
        {
            var inputText = "I am a normal text with no special characters";
            var returnedText = this.fileTextParser.RemoveSpecialCharactersFromText(inputText);
            Assert.AreEqual(inputText, returnedText);
        }

        [TestMethod]
        public void RemoveSpecialCharactersFromText_TextHasSpecialCharacters_ReturnsFilteredText()
        {
            var inputText = "Only 12345this6789 text?_!·$@~à will be ãáçñvisible, the numbers and stresses too!";
            var expectedText = "Only 12345this6789 textà will be ãáçñvisible the numbers and stresses too";
            var returnedText = this.fileTextParser.RemoveSpecialCharactersFromText(inputText);
            Assert.AreEqual(expectedText, returnedText);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void CountWordsFromText_TextIsNull_ThrowsNullReferenceException()
        {
            this.fileTextParser.CountWordsFromText(null);
        }

        [TestMethod]
        public void CountWordsFromText_TextIsEmpty_ReturnsEmptyMapReduceWord()
        {
            Dictionary<string, int> expectedReturn = new Dictionary<string, int>();
            Dictionary<string, int> returnedDictionary = this.fileTextParser.CountWordsFromText(string.Empty);
            Assert.IsTrue(returnedDictionary.Keys.Count == 0);
            CollectionAssert.AreEqual(expectedReturn, returnedDictionary);
        }

        [TestMethod]
        public void CountWordsFromText_TextIsCorrect_ReturnsFullMapReduceWord()
        {
            var inputString = "this is a x x x x";
            Dictionary<string, int> expectedReturn = new Dictionary<string, int> { {"this", 1}, { "is", 1 }, { "a", 1 }, { "x", 4 } };
            Dictionary<string, int> returnedDictionary = this.fileTextParser.CountWordsFromText(inputString);
            Assert.IsTrue(returnedDictionary.Keys.Count == numberOfkeys);
            CollectionAssert.AreEqual(expectedReturn, returnedDictionary);
        }
    }
}
