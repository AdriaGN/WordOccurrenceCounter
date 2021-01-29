using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Moq;
using Application.Services;
using Application.Services.Interfaces;
using Domain.FileModelType;

namespace Application.Testing
{
    [TestClass]
    public class FileManagerTests
    {
        // Path Directories
        private static string ProjectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
        private static string SingleSample = ProjectDirectory + "\\TextSamples\\SingleSampleText\\";
        private static string MultipleSamples = ProjectDirectory + "\\TextSamples\\MultipleSamplesText\\";
        private static string EmptySample = ProjectDirectory + "\\TextSamples\\EmptySampleText\\";

        // Number of existing files
        private static int numberOfSimpleFiles = 1;
        private static int numberOfMultipleFiles = 3;

        // Lists to compare
        private static List<string> simpleFilesList = new List<string>{ "SingleSample.txt" };
        private static List<string> multipleFilesList = new List<string> { "Text_1.txt", "Text_2.txt", "Text_3.txt" };

        // Dictionary to mock
        private static readonly Dictionary<string, int> MockDictionary = new Dictionary<string, int>
                                                                             { { "hello", 1 }, { "there", 1 }, { "i", 1 }, { "am", 1 },
                                                                                 { "a", 1 }, { "text", 1 }, { "file", 1 } };

        // DI
        private IFileManager fileManager;
        private IFileTextParser fileTextParser;

        [TestInitialize]
        public void TestInitialization()
        { 
            var mock = new Mock<IFileTextParser>();
            mock.Setup(m => 
                m.GetOccurrencesWordDictionary(It.IsAny<string>())).Returns(MockDictionary);

            this.fileTextParser = mock.Object;
            this.fileManager = new FileManager(this.fileTextParser);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetFileWordsOccurrencesCounted_DirectoryPathIsNull_ThrowsArgumentNullException()
        {
            this.fileManager.GetFileWordsOccurrencesCounted(null);
        }

        [TestMethod]
        [ExpectedException(typeof(DirectoryNotFoundException))]
        public void GetFileWordsOccurrencesCounted_DirectoryPathIsEmpty_ThrowsDirectoryNotFoundException()
        {
            this.fileManager.GetFileWordsOccurrencesCounted(string.Empty);
        }

        [TestMethod]
        public void GetFileWordsOccurrencesCounted_AllInputsAreCorrect_ReturnsAListOfFilesCounted()
        {
            FileToCount expectedFileRead = new FileToCount()
                                               {
                                                   Name = "SingleSample.txt",
                                                   WordOccurrences = MockDictionary
                                               };

            List<FileToCount> returnedList = this.fileManager.GetFileWordsOccurrencesCounted(SingleSample);

            Assert.AreEqual(expectedFileRead.Name, returnedList.First().Name);
            CollectionAssert.AreEqual(expectedFileRead.WordOccurrences, returnedList.First().WordOccurrences);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CheckDirectoryPathAndFilesAreValid_DirectoryPathIsNull_ThrowsArgumentNullException()
        {
            this.fileManager.CheckDirectoryPathAndFilesAreValid(null);
        }

        [TestMethod]
        [ExpectedException(typeof(DirectoryNotFoundException))]
        public void CheckDirectoryPathAndFilesAreValid_DirectoryPathIsWrong_ThrowsDirectoryNotFoundException()
        {
            this.fileManager.CheckDirectoryPathAndFilesAreValid("WrongPath");
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void CheckDirectoryPathAndFilesAreValid_DirectoryPathIsEmpty_ThrowsFileNotFoundException()
        {
            this.fileManager.CheckDirectoryPathAndFilesAreValid(ProjectDirectory);
        }

        [TestMethod]
        public void CheckDirectoryPathAndFilesAreValid_DirectoryPathIsCorrect_NoExceptionThrown()
        {
            try
            {
                this.fileManager.CheckDirectoryPathAndFilesAreValid(SingleSample);
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetFileNamesFromDirectory_DirectoryPathIsNull_ThrowsArgumentNullException()
        {
            this.fileManager.GetFileNamesFromDirectory(null);
        }

        [TestMethod]
        public void GetFileNamesFromDirectory_DirectoryPathIsWrong_ReturnsEmptyFileNameList()
        {
            List<string> returnedFileNameList = this.fileManager.GetFileNamesFromDirectory(ProjectDirectory);

            Assert.IsTrue(returnedFileNameList.Count == 0);
        }

        [TestMethod]
        public void GetFileNamesFromDirectory_DirectoryPathIsCorrectAndSingleFile_ReturnsOneFileNameList()
        {
            List<string> returnedFileNameList = this.fileManager.GetFileNamesFromDirectory(SingleSample);

            Assert.IsTrue(returnedFileNameList.Count == numberOfSimpleFiles);
            CollectionAssert.AreEqual(simpleFilesList, returnedFileNameList);
        }

        [TestMethod]
        public void GetFileNamesFromDirectory_DirectoryPathIsCorrectAndMultipleFile_ReturnsMultipleFileNameList()
        {
            List<string> returnedFileNameList = this.fileManager.GetFileNamesFromDirectory(MultipleSamples);

            Assert.IsTrue(returnedFileNameList.Count == numberOfMultipleFiles);
            CollectionAssert.AreEqual(multipleFilesList, returnedFileNameList);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void GetListOfAnalyzedFilesAndResults_DirectoryPathIsNull_ThrowsFileNotFoundException()
        {
            this.fileManager.GetListOfAnalyzedFilesAndResults(null, simpleFilesList);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void GetListOfAnalyzedFilesAndResults_DirectoryPathIsEmpty_ThrowsFileNotFoundException()
        {
            this.fileManager.GetListOfAnalyzedFilesAndResults(string.Empty, simpleFilesList);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void GetListOfAnalyzedFilesAndResults_FileNameListIsNull_ThrowsArgumentNullException()
        {
            this.fileManager.GetListOfAnalyzedFilesAndResults(string.Empty, null);
        }

        [TestMethod]
        public void GetListOfAnalyzedFilesAndResults_AllInputsAreCorrect_ReturnsAListOfFilesCounted()
        {
            FileToCount expectedFileRead = new FileToCount()
                                               {
                                                   Name = "SingleSample.txt",
                                                   WordOccurrences = MockDictionary
                                               };

            List<FileToCount> returnedList = this.fileManager.GetListOfAnalyzedFilesAndResults(SingleSample, simpleFilesList);

            Assert.AreEqual(expectedFileRead.Name, returnedList.First().Name);
            CollectionAssert.AreEqual(expectedFileRead.WordOccurrences, returnedList.First().WordOccurrences);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetTextFromFile_TextFilePathIsNull_ThrowsArgumentNullException()
        {
            this.fileManager.GetTextFromFile(null);
        }

        [TestMethod]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public void GetTextFromFile_TextFilePathIsEmpty_ThrowsUnauthorizedAccessException()
        {
            this.fileManager.GetTextFromFile(ProjectDirectory);
        }

        [TestMethod]
        public void GetTextFromFile_TextFileIsCorrectAndIsEmpty_ReturnsEmptyReadedText()
        {
            var filePathToRead = EmptySample + "EmptySample.txt";
            var expectedText = string.Empty;
            var returnedReadText = this.fileManager.GetTextFromFile(filePathToRead);
            Assert.AreEqual(expectedText, returnedReadText);
        }

        [TestMethod]
        public void GetTextFromFile_TextFileIsCorrectAndNotEmpty_ReturnsFullReadedText()
        {
            var filePathToRead = SingleSample + "SingleSample.txt";
            var expectedText = "Hello there! I'm a text file!";
            var returnedReadText = this.fileManager.GetTextFromFile(filePathToRead);
            Assert.AreEqual(expectedText, returnedReadText);
        }
    }
}
