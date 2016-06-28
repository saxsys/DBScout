using DbScout.Contracts;

namespace DbScout.Commands
{
    public class TestCommand : ICommand,IConfigurable
    {
        public void Execute()
        {
        }

        public void Configure(IConfiguration cfg)
        {
        }
    }
}
