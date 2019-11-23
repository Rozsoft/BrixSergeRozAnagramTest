using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Brix.SergeRoz.AnagramTest.AnagramFinder
{
    public interface IAnagramFinder
    {
        string[] StringLines { get; }
        Dictionary<int, string> StringLinesDictionary { get; }

        Stopwatch LoadStopwatch { get; }
        Stopwatch SortStopwatch { get; }
        Stopwatch FindStopwatch { get; }

        Task LoadAsync(string[] stringLines);
        Task SortAsync();
        Task<Dictionary<string, int[]>> GetAnagramsAsync(string inputString);
    }
}
