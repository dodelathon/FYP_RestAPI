﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CS4227_Database_API.Models.Containers;
using CS4227_Database_API.Factories.Abstract_Factories;

namespace CS4227_Database_API.Factories.Concrete_Factories
{
    public class LeaderboardObjectFactory : DBObjectAbstractFactory
    {
        override
        public IDBContainer GetDBContainer(List<string> _params)
        {
            //Console.WriteLine("I can write to console");
            return LeaderboardObject.Creator(this, _params[0], _params[1], _params[2], _params[3]);
        }
    }
}
