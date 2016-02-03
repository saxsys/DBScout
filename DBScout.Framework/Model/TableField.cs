namespace DBScout.Model
{
    /// <summary>
    /// Specifies a data structure that represents a field or column definition of a database table or view.
    /// </summary>
    public class TableField : AbstractField
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public TableField()
        {
            DataType = string.Empty;
            DefaultValue = string.Empty;
            IsNullable = true;
        }
        
        /// <summary>
        /// Data type as reported by the database management system (e.g. Oracle - VARCHAR2(255)) 
        /// </summary>
        public string DataType { get; set; }

        /// <summary>
        /// Default field value
        /// </summary>
        public string DefaultValue { get; set; }

        /// <summary>
        /// True if the field may contain NULL values, otherwise false.
        /// </summary>
        public bool IsNullable { get; set; }
    }
}
