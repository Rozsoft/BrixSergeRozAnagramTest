using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brix.SergeRoz.AnagramTest.AnagramFinder
{
    public class AnagramFinderByHash : AnagramFinderBase, IAnagramFinder
    {

        #region Properties / Members 

        public Hashtable StringLinesHashtable { get; private set; }

        public Stopwatch LoadStopwatch { get; private set; }
        public Stopwatch SortStopwatch { get; private set; }
        public Stopwatch FindStopwatch { get; private set; }

        #endregion

        #region Constructor

        public AnagramFinderByHash()
        {
            base.StringLinesDictionary = new Dictionary<int, string>();
            StringLinesHashtable = new System.Collections.Hashtable();

            SortStopwatch = new Stopwatch();
            FindStopwatch = new Stopwatch();
            LoadStopwatch = new Stopwatch();
        }

        #endregion

        #region Public Methods

        public override async Task LoadAsync(string[] stringLines)
        {
            LoadStopwatch.Start();

            base.StringLines = stringLines;

            int i = 0;

            foreach (var line in StringLines)
            {
                StringLinesDictionary.Add(i, line);

                i++;
            };

            LoadStopwatch.Stop();
        }

        public override async Task SortAsync()
        {
            SortStopwatch.Start();

            foreach (var stringLine in StringLinesDictionary)
            {
                var stringHashPair = await CalculateStringHashAsync(stringLine.Value);

                var duplicateInHashTbl = (Dictionary<string, int[]>)StringLinesHashtable[stringHashPair.Key];

                if (duplicateInHashTbl == null)
                {
                    var stringLineNumPair = new Dictionary<string, int[]>();
                    stringLineNumPair.Add(stringLine.Value, new int[] { stringLine.Key });

                    StringLinesHashtable.Add(stringHashPair.Key, stringLineNumPair);
                }
                else
                {
                    var stringLineDic = duplicateInHashTbl;

                    stringLineDic =
                        await ManageSameStringsAndLinesInColectionAsync(stringLineDic, new KeyValuePair<string, int>(stringLine.Value, stringLine.Key));
                }
            }

            SortStopwatch.Stop();

            return;
        }

        public override async Task<Dictionary<string, int[]>> GetAnagramsAsync(string inputString)
        {
            FindStopwatch.Start();

            var inputStringHash = await CalculateStringHashAsync(inputString);
            var anagramsDic = StringLinesHashtable[inputStringHash.Key];

            if (anagramsDic != null)
            {
                return (Dictionary<string, int[]>)anagramsDic;
            }

            FindStopwatch.Stop();

            return null;
        }

        #endregion

        #region Private Methods

        private async Task<KeyValuePair<long, string>> CalculateStringHashAsync(string stringToCalculate)
        {
            char[] letters = stringToCalculate.ToLower().ToCharArray();
            Array.Sort(letters);

            long hashSum = new string(letters).GetHashCode();

            return (new KeyValuePair<long, string>(hashSum, stringToCalculate));
        }

        private async Task<Dictionary<string, int[]>> ManageSameStringsAndLinesInColectionAsync(Dictionary<string, int[]> colection, KeyValuePair<string, int> stringLinePair)
        {
            try
            {
                var same = colection.Where(c => c.Key == stringLinePair.Key);

                if (same.FirstOrDefault().Value != null)
                {
                    List<int> lineNumsList = same.FirstOrDefault().Value.ToList();
                    lineNumsList.Add(stringLinePair.Value);

                    colection[stringLinePair.Key] = lineNumsList.ToArray();
                }
                else
                {
                    colection.Add(stringLinePair.Key, new int[] { stringLinePair.Value });
                }

                return colection;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #endregion

    }
}
