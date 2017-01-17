namespace DbScout.Contracts
{
    public interface IStrategy
    {
        void Run();

        void AddCommand(ICommand command);
    }
}
