using System.Collections.Generic;

namespace DbScout.Contracts
{
    public interface ICommand
    {
        void Configure(IDictionary<string, string> cfgDictionary);

        void Execute();
    }
}
