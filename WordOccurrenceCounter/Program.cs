using System;
using Unity;
using Application.Services;
using Application.Services.Interfaces;

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

            try
            {
                var filesPath = args[0];
                var inputWord = string.Empty;

                var fileManager = container.Resolve<IFileManager>();
                var countedOccurrences = fileManager.GetFileWordsOccurrencesCounted(filesPath);

                while (inputWord != ":q!")
                {
                    Console.Write("Input the word you want to search (type :q! to exit): ");
                    inputWord = Console.ReadLine();

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
                Console.WriteLine("ERROR: " + exception.Message + Environment.NewLine);
            }

            Console.Write("Press enter key to exit...");
            Console.ReadLine();
        }
    }
}