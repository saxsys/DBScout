namespace DBScout.Model
{
    /// <summary>
    /// Represents a database view definition. 
    /// </summary>
    public class View : AbstractDbObject
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public View()
        {
            ViewDefinition = string.Empty;
        }

        /// <summary>
        /// Definition of the view
        /// </summary>
        public string ViewDefinition { get; set; }
    }
}
