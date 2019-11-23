using Brix.SergeRoz.AnagramTest.AnagramFinder;
using Brix.SergeRoz.AnagramTest.Common;
using Brix.SergeRoz.AnagramTest.Dao;
using Brix.SergeRoz.AnagramTest.TextGenerator;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Brix.SergeRoz.AnagramTest
{
    class Program
    {  
        static async Task  Main(string[] args)
        {
            try
            {
                ITextGeneratorHelper textGeneratorHelper = new TextGeneratorHelper();
                IDao dao = DaoFactory.Create(eDaoType.FileDao);
                IAnagramFinder anagramFinder = new AnagramFinderByHash();

                AnagramConsoleExecutor anagramExecutor = new AnagramConsoleExecutor(textGeneratorHelper, dao, anagramFinder);

                await anagramExecutor.Run();
                
            }
            catch(Exception e)
            {
                Console.WriteLine(UiMessages.ErrorMsg);
                Console.WriteLine(e.Message);
            }

            Console.ReadKey();
        }
    }
}
