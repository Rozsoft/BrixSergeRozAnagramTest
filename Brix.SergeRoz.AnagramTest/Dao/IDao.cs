using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Brix.SergeRoz.AnagramTest.Dao
{
    public interface IDao
    {   
        string LinesSource { get; }
        
        Task<string[]> GetLinesAsync();
        Task SetLinesAsync(string[] lines, bool append);
    }
}
