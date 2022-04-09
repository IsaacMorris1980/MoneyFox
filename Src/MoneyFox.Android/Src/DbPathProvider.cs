﻿namespace MoneyFox.Droid.Src
{

    using System;
    using System.IO;
    using Core.Common;
    using Core.Interfaces;

    public class DbPathProvider : IDbPathProvider
    {
        public string GetDbPath()
        {
            return Path.Combine(path1: Environment.GetFolderPath(Environment.SpecialFolder.Personal), path2: DatabaseConfiguration.DatabaseName);
        }
    }

}
