using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CS4227_Database_API.Models.Containers;

namespace CS4227_Database_API.Factories.Abstract_Factories
{
    public interface IAbFactory
    {
       IDBContainer GetDBContainer(List<string> _params);
    }
}
