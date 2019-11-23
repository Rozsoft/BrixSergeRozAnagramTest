using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Brix.SergeRoz.AnagramTest.AnagramFinder
{
    public abstract class AnagramFinderBase
    {
        public string[] StringLines { get; set; }
        public Dictionary<int, string> StringLinesDictionary { get; set; }        


        public abstract Task LoadAsync(string[] stringLines);
        public abstract Task SortAsync();
        public abstract Task<Dictionary<string, int[]>> GetAnagramsAsync(string inputString);
    }
}
