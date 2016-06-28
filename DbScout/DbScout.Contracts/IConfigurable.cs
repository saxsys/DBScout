namespace DbScout.Contracts
{
    /// <summary>
    /// Definition of functionality to use IConfiguration for configurable implementations.
    /// </summary>
    public interface IConfigurable
    {
        /// <summary>
        /// Assign specified IConfiguration instance
        /// </summary>
        /// <param name="cfg">IConfiguration instance</param>
        void Configure(IConfiguration cfg);
    }
}
