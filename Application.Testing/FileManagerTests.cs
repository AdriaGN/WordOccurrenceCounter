using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using Moq;
using Application.Services;
using Application.Services.Interfaces;
using Domain.FileModelType;

namespace Application.Testing
{
    using System.Linq;

    [TestClass]
    public class FileManagerTests
    {
        // Directories
        private static string _ProjectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
        private static string _SingleSample = _ProjectDirectory + "\\TextSamples\\SingleSampleText\\";
        private static string _MultipleSamples = _ProjectDirectory + "\\TextSamples\\MultipleSamplesText\\";
        private static string _EmptySample = _ProjectDirectory + "\\TextSamples\\EmptySampleText\\";

        // Number of existing files
        private static int _numberOfSimpleFiles = 1;
        private static int _numberOfMultipleFiles = 3;

        // Lists to compare
        private static List<string> _simpleFilesList = new List<string>{ "SingleSample.txt" };
        private static List<string> _multipleFilesList = new List<string> { "Text_1.txt", "Text_2.txt", "Text_3.txt" };

        // IoC
        private IFileManager _fileManager;
        private IFileTextParser _fileTextParser;

        [TestInitialize]
        public void TestInitialization()
        {
            var mock = new Mock<IFileTextParser>();
            this._fileTextParser = mock.Object;
            this._fileManager = new FileManager(this._fileTextParser);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CheckDirectoryPathAndFilesAreValid_DirectoryPathIsNull_ThrowsArgumentNullException()
        {
            this._fileManager.CheckDirectoryPathAndFilesAreValid(null);
        }

        [TestMethod]
        [ExpectedException(typeof(DirectoryNotFoundException))]
        public void CheckDirectoryPathAndFilesAreValid_DirectoryPathIsWrong_ThrowsDirectoryNotFoundException()
        {
            this._fileManager.CheckDirectoryPathAndFilesAreValid("WrongPath");
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void CheckDirectoryPathAndFilesAreValid_DirectoryPathIsEmpty_ThrowsFileNotFoundException()
        {
            this._fileManager.CheckDirectoryPathAndFilesAreValid(_ProjectDirectory);
        }

        [TestMethod]
        public void CheckDirectoryPathAndFilesAreValid_DirectoryPathIsCorrect_NoExceptionThrown()
        {
            try
            {
                this._fileManager.CheckDirectoryPathAndFilesAreValid(_SingleSample);
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
            this._fileManager.GetFileNamesFromDirectory(null);
        }

        [TestMethod]
        public void GetFileNamesFromDirectory_DirectoryPathIsWrong_ReturnsEmptyFileNameList()
        {
            List<string> returnedFileNameList = new List<string>();
            returnedFileNameList = this._fileManager.GetFileNamesFromDirectory(_ProjectDirectory);
            Assert.IsTrue(returnedFileNameList.Count == 0);
        }

        [TestMethod]
        public void GetFileNamesFromDirectory_DirectoryPathIsCorrectAndSingleFile_ReturnsOneFileNameList()
        {
            List<string> returnedFileNameList = new List<string>();
            returnedFileNameList = this._fileManager.GetFileNamesFromDirectory(_SingleSample);
            Assert.IsTrue(returnedFileNameList.Count == _numberOfSimpleFiles);
            CollectionAssert.AreEqual(_simpleFilesList, returnedFileNameList);
        }

        [TestMethod]
        public void GetFileNamesFromDirectory_DirectoryPathIsCorrectAndMultipleFile_ReturnsMultipleFileNameList()
        {
            List<string> returnedFileNameList = new List<string>();
            returnedFileNameList = this._fileManager.GetFileNamesFromDirectory(_MultipleSamples);
            Assert.IsTrue(returnedFileNameList.Count == _numberOfMultipleFiles);
            CollectionAssert.AreEqual(_multipleFilesList, returnedFileNameList);
        }



        // HERE GOES THE GetListOfAnalyzedFilesAndResults TESTS




        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetTextFromFile_TextFilePathIsNull_ThrowsArgumentNullException()
        {
            this._fileManager.GetTextFromFile(null);
        }

        [TestMethod]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public void GetTextFromFile_TextFilePathIsEmpty_ThrowsUnauthorizedAccessException()
        {
            this._fileManager.GetTextFromFile(_ProjectDirectory);
        }

        [TestMethod]
        public void GetTextFromFile_TextFileIsCorrectAndIsEmpty_ReturnsEmptyReadedText()
        {
            var filePathToRead = _EmptySample + "EmptySample.txt";
            var expectedText = string.Empty;
            var returnedReadText = this._fileManager.GetTextFromFile(filePathToRead);
            Assert.AreEqual(expectedText, returnedReadText);
        }

        [TestMethod]
        public void GetTextFromFile_TextFileIsCorrectAndNotEmpty_ReturnsFullReadedText()
        {
            var filePathToRead = _SingleSample + "SingleSample.txt";
            var expectedText = "Hello there! I'm a text file!";
            var returnedReadText = this._fileManager.GetTextFromFile(filePathToRead);
            Assert.AreEqual(expectedText, returnedReadText);
        }
    }
}
