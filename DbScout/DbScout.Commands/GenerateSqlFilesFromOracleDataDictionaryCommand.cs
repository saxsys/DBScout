using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbScout.Contracts;

namespace DbScout.Commands
{
    public class GenerateSqlFilesFromOracleDataDictionaryCommand : ICommand
    {
        public void Execute()
        {
            ConnectToDatabase();
            CollectData();
            GenerateOutputFiles();
        }

        private void ConnectToDatabase()
        {
        }

        private void CollectData()
        {
        }

        private void GenerateOutputFiles()
        {
        }
    }
}
