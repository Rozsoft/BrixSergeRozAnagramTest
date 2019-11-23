using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Brix.SergeRoz.AnagramTest.TextGenerator
{
    public class TextGeneratorHelper : ITextGeneratorHelper
    {
        #region Public Methods

        public async Task<string[]> GenerateTextAsync(int linesCount, int stringLength)
        {
            
            string[] stringList = new string[linesCount];

            ParallelLoopResult result = Parallel.For(0, linesCount, (i) =>
            {
                stringList[i] = (RandomString(stringLength));
            });

            while (result.IsCompleted)
            {
                return stringList;
            }

            return null;
        }

        #endregion

        #region Private Methods

        private string RandomString(int lenth)
        {
            string s = string.Empty;
            for (int i = 0; i < lenth; i++)
            {
                s += ((char)new Random().Next((int)'a', ((int)'z') + 1)).ToString();
            }
            return s;
        }

        #endregion
    }
}
