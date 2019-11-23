using Brix.SergeRoz.AnagramTest.AnagramFinder;
using Brix.SergeRoz.AnagramTest.Common;
using Brix.SergeRoz.AnagramTest.Dao;
using Brix.SergeRoz.AnagramTest.TextGenerator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Brix.SergeRoz.AnagramTest
{
    public class AnagramConsoleExecutor
    {
        #region Private Members

        private ITextGeneratorHelper _TextGeneratorHelper;
        private IDao _Dao;
        private IAnagramFinder _AnagramFinder;

        private Dictionary<string, int[]> _anagramFinderResult;

        private string[] _lines;
        private int _linesCount;

        private string _continueFindDialogAnsw = string.Empty;
        private string _createFIleDialogAnsw = string.Empty;
        private string _userInputToFind = string.Empty;

        #endregion

        #region Constructor

        public AnagramConsoleExecutor(ITextGeneratorHelper textGeneratorHelper, IDao dao, IAnagramFinder anagramFinder)
        {
            _TextGeneratorHelper = textGeneratorHelper;
            _Dao = dao;
            _AnagramFinder = anagramFinder;
        }

        #endregion

        #region Public Methods 

        public async Task Run()
        {
            Console.WriteLine(UiMessages.BorderMsg);
            Console.WriteLine(UiMessages.BorderMsg);
            Console.WriteLine(UiMessages.WelcomeMsg);
            Console.WriteLine(UiMessages.BorderMsg);
            Console.WriteLine(UiMessages.BorderMsg);

            Console.WriteLine();

            await RunFileCreationFlow();

            await RunDaoLoadFlow();

            Console.WriteLine();
            Console.WriteLine(UiMessages.StartExecutingAnagramLoading);

            await _AnagramFinder.LoadAsync(_lines);

            Console.WriteLine(UiMessages.TimeElapsedMsg + _AnagramFinder.LoadStopwatch.Elapsed);

            Console.WriteLine();

            Console.WriteLine(UiMessages.StartExecutingAnagramSorting);

            await _AnagramFinder.SortAsync();

            Console.WriteLine(UiMessages.TimeElapsedMsg + _AnagramFinder.SortStopwatch.Elapsed);


            await RunFindAnagramFlow();

        }

        #endregion

        #region Private Methods

        private async Task RunDaoLoadFlow()
        {
            try
            {
                Console.WriteLine(UiMessages.GettingLinesMsg);

                _lines = await _Dao.GetLinesAsync();
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine();
                Console.WriteLine(UiMessages.FileNotFoundMsg + _Dao.LinesSource);
                Console.ReadKey();
                Environment.Exit(-1);
            }
        }

        private async Task RunFileCreationFlow()
        {
            await RunCreateFileDialogAsync();

            if (_createFIleDialogAnsw == "y")
            {
                Console.WriteLine();
                await RunLinesCountDialogAsync();

                Console.WriteLine();
                Console.WriteLine(UiMessages.LoadingMsg);
                Console.WriteLine();

                _lines = await _TextGeneratorHelper.GenerateTextAsync(_linesCount, 5);
                await _Dao.SetLinesAsync(_lines, false);

                await RunFileCreatedDialogAsync();
            }
        }

        private async Task RunFindAnagramFlow()
        {
            while (_continueFindDialogAnsw != "n")
            {

                Console.WriteLine();
                Console.WriteLine(UiMessages.FindMsg);

                _continueFindDialogAnsw = Console.ReadLine().ToLower();

                if (_continueFindDialogAnsw != "y" && _continueFindDialogAnsw != "n")
                {
                    Console.WriteLine(UiMessages.WrongInputMsg);
                    continue;
                }

                if (_continueFindDialogAnsw == "n")
                {
                    Environment.Exit(0);
                }

                Console.WriteLine();

                await RunGetUserInputDialogAsync();

                Console.WriteLine();
                Console.WriteLine(UiMessages.StartExecutingAnagramFinding);

                _anagramFinderResult = await _AnagramFinder.GetAnagramsAsync(_userInputToFind);

                Console.WriteLine(UiMessages.TimeElapsedMsg + _AnagramFinder.FindStopwatch.Elapsed);

                await RunAnagramsFoundDialogAsync();
            }
        }

        private async Task RunCreateFileDialogAsync()
        {
            Console.WriteLine(UiMessages.CreateFileQuestionMsg);

            while (_createFIleDialogAnsw != "y" && _createFIleDialogAnsw != "n")
            {

                _createFIleDialogAnsw = Console.ReadLine().ToLower();

                if (_createFIleDialogAnsw != "y" && _createFIleDialogAnsw != "n")
                {
                    Console.WriteLine(UiMessages.WrongInputMsg);
                }
            }
        }

        private async Task RunLinesCountDialogAsync()
        {
            while (true)
            {
                try
                {
                    Console.WriteLine(UiMessages.CreateFileLinesCountMsg);

                    _linesCount = int.Parse(Console.ReadLine());

                    if (_linesCount > 1000000)
                    {
                        _linesCount = 1000000;
                    }

                    break;
                }
                catch
                {

                }
            }
        }

        private async Task RunFileCreatedDialogAsync()
        {
            Console.WriteLine(UiMessages.FileCreatedMsg);
            Console.WriteLine(UiMessages.CreatedLinesCountMsg + _linesCount.ToString());
            Console.WriteLine(UiMessages.FilePathMsg + " " + _Dao.LinesSource);
            Console.WriteLine();
        }

        private async Task RunGetUserInputDialogAsync()
        {
            _userInputToFind = string.Empty;

            while (_userInputToFind.Length != 5)
            {
                Console.WriteLine(UiMessages.EnterInputString);

                _userInputToFind = Console.ReadLine();
            }

            if (_userInputToFind.Length > 5)
            {
                _userInputToFind = _userInputToFind.Substring(0, 5);
            }

            _userInputToFind = _userInputToFind.ToLower();

        }

        private async Task RunAnagramsFoundDialogAsync()
        {
            Console.WriteLine();

            if (_anagramFinderResult == null)
            {

                Console.WriteLine(UiMessages.AnagramsNotFoundMsg);
                Console.WriteLine();
                return;
            }

            Console.WriteLine("\"" + _userInputToFind + "\" " + UiMessages.AnagramsFoundMsg);
            Console.WriteLine(UiMessages.BorderMsg);

            foreach (var anagram in _anagramFinderResult)
            {
                string linesNums = string.Empty;
                foreach (var line in anagram.Value) linesNums += " " + line.ToString() + ".";

                Console.WriteLine("\"" + anagram.Key + "\"" + " lines: " + linesNums);
            }
            Console.WriteLine(UiMessages.BorderMsg);
        }

        #endregion
    }
}
