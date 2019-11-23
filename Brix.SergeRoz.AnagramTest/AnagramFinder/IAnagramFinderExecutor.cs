using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Brix.SergeRoz.AnagramTest.AnagramFinder
{
    interface IAnagramFinderExecutor
    {
        string[] StringLines { get; }        
        Dictionary<int, string> StringLinesDictionary { get; }
        Hashtable StringLinesHashtable { get; }

        Task Load();
        Task Sort();
        Task GetAnagrams();
    }
}
