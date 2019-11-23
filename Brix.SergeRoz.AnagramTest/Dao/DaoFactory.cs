using Brix.SergeRoz.AnagramTest.Common;
using System;
using System.Collections.Generic;
using System.Text;


namespace Brix.SergeRoz.AnagramTest.Dao
{
    public class DaoFactory
    {
        static public IDao Create(eDaoType daoType)
        {
            switch(daoType)
            {
                case eDaoType.FileDao :
                    return new FileDao();
                
                default:
                    throw new ArgumentException();
            }
        }
    }
}
