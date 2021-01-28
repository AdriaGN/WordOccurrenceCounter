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
        private IFileTextParser fileManager;

        [TestInitialize]
        public void TestInitialization()
        {
            this.fileManager = new FileTextParser();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetOccurrencesWordDictionary_TextIsNull_ThrowsArgumentNullException()
        {
            this.fileManager.GetOccurrencesWordDictionary(null);
        }

        [TestMethod]
        public void GetOccurrencesWordDictionary_TextIsEmpty_ReturnsEmptyDictionary()
        {
            Dictionary<string, int> expectedReturn = new Dictionary<string, int>();
            var returnedDictionary = this.fileManager.GetOccurrencesWordDictionary(string.Empty);
            CollectionAssert.AreEqual(expectedReturn, returnedDictionary);
        }

        [TestMethod]
        public void GetOccurrencesWordDictionary_TextIsCorrect_ReturnsEmptyDictionary()
        {
            var inputString = "this is a x x x x !!!! ???? %%%%";
            Dictionary<string, int> expectedReturn = new Dictionary<string, int> { { "this", 1 }, { "is", 1 }, { "a", 1 }, { "x", 4 } };
            Dictionary<string, int> returnedDictionary = this.fileManager.GetOccurrencesWordDictionary(inputString);
            Assert.IsTrue(returnedDictionary.Keys.Count == numberOfkeys);
            CollectionAssert.AreEqual(expectedReturn, returnedDictionary);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RemoveSpecialCharactersFromText_TextIsNull_ThrowsArgumentNullException()
        {
            this.fileManager.RemoveSpecialCharactersFromText(null);
        }

        [TestMethod]
        public void RemoveSpecialCharactersFromText_TextIsEmpty_ReturnsEmptyText()
        {
            this.fileManager.RemoveSpecialCharactersFromText(string.Empty);
        }

        [TestMethod]
        public void RemoveSpecialCharactersFromText_TextHasNoSpecialCharacters_ReturnsSameText()
        {
            var inputText = "I am a normal text with no special characters";
            var returnedText = this.fileManager.RemoveSpecialCharactersFromText(inputText);
            Assert.AreEqual(inputText, returnedText);
        }

        [TestMethod]
        public void RemoveSpecialCharactersFromText_TextHasSpecialCharacters_ReturnsFilteredText()
        {
            var inputText = "Only 12345this6789 text?_!·$@~à will be ãáçñvisible";
            var expectedText = "Only this text will be visible";
            var returnedText = this.fileManager.RemoveSpecialCharactersFromText(inputText);
            Assert.AreEqual(expectedText, returnedText);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void CountWordsFromText_TextIsNull_ThrowsNullReferenceException()
        {
            this.fileManager.CountWordsFromText(null);
        }

        [TestMethod]
        public void CountWordsFromText_TextIsEmpty_ReturnsEmptyMapReduceWord()
        {
            Dictionary<string, int> expectedReturn = new Dictionary<string, int>();
            Dictionary<string, int> returnedDictionary = this.fileManager.CountWordsFromText(string.Empty);
            Assert.IsTrue(returnedDictionary.Keys.Count == 0);
            CollectionAssert.AreEqual(expectedReturn, returnedDictionary);
        }

        [TestMethod]
        public void CountWordsFromText_TextIsCorrect_ReturnsFullMapReduceWord()
        {
            var inputString = "this is a x x x x";
            Dictionary<string, int> expectedReturn = new Dictionary<string, int> { {"this", 1}, { "is", 1 }, { "a", 1 }, { "x", 4 } };
            Dictionary<string, int> returnedDictionary = this.fileManager.CountWordsFromText(inputString);
            Assert.IsTrue(returnedDictionary.Keys.Count == numberOfkeys);
            CollectionAssert.AreEqual(expectedReturn, returnedDictionary);
        }
    }
}
