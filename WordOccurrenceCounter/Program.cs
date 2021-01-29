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
            IUnityContainer container = new UnityContainer();
            container.RegisterType<IFileManager, FileManager>();
            container.RegisterType<IFileTextParser, FileTextParser>();
            container.RegisterType<IResultsManager, ResultsManager>();

            var filesPath = args[0];

            var filesToParse = container.Resolve<IFileManager>();
            var countedOccurrences = filesToParse.GetFileWordsOccurrencesCounted(filesPath);

            string inputWord = string.Empty;

            while (inputWord != ":q!")
            {
                Console.WriteLine("Put your word here: ");
                inputWord = Console.ReadLine();

                if (inputWord != ":q!")
                {
                    var results = container.Resolve<IResultsManager>();
                    results.ShowResults(inputWord?.ToLower(), countedOccurrences);
                }
            }
        }
    }
}