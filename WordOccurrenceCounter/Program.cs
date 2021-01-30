using System;
using Unity;
using Application.Services;
using Application.Services.Interfaces;
using Infrastructure.Services;
using Infrastructure.Services.Interfaces;

namespace Presentation.UI
{
    class Program
    { 
        static void Main(string[] args)
        {
            // DI registration
            IUnityContainer container = new UnityContainer();
            container.RegisterType<IFileManager, FileManager>();
            container.RegisterType<IFileTextParser, FileTextParser>();
            container.RegisterType<IResultsManager, ResultsManager>();
            container.RegisterType<ILoggerApp, LoggerApp>();

            var loggerApp = container.Resolve<ILoggerApp>();
            loggerApp.Debug("Starting application");

            try
            {
                var filesPath = args[0];
                var inputWord = string.Empty;

                var fileManager = container.Resolve<IFileManager>();
                var countedOccurrences = fileManager.GetFileWordsOccurrencesCounted(filesPath);

                while (inputWord != ":q!")
                {
                    loggerApp.Info("Input the word you want to search (type :q! to exit): ");
                    inputWord = Console.ReadLine();
                    loggerApp.Trace(inputWord);

                    if (inputWord != ":q!")
                    {
                        var results = container.Resolve<IResultsManager>();
                        results.ShowResults(inputWord?.ToLower(), countedOccurrences);
                    }
                    Console.WriteLine();
                }
            }
            catch (Exception exception)
            {
                loggerApp.Fatal("ERROR: " + exception.Message + Environment.NewLine + exception.StackTrace + Environment.NewLine);
            }

            loggerApp.Info("Press enter key to exit...");
            Console.ReadLine();
        }
    }
}