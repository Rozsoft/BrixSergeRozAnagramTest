using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Brix.SergeRoz.AnagramTest.TextGenerator
{
    public interface ITextGeneratorHelper
    {
        Task<string[]> GenerateTextAsync(int linesCount, int stringLength);
    }
}
