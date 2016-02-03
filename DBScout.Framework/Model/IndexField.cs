namespace DBScout.Model
{
    public class IndexField : AbstractField
    {
        /// <summary>
        /// Sort direction definition
        /// </summary>
        public enum SortDirection
        {
            Ascending,
            Descending
        }

        /// <summary>
        /// Sort direction of index field
        /// </summary>
        public SortDirection Sort { get; set; }
    }
}
