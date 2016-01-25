namespace DataDictionary.Services.Model
{
    public interface ISqlFileCreator
    {
        /// <summary>
        /// Erstellen von SQL-Files
        /// </summary>
        /// <param name="schemaName"></param>
        void CreateFiles(string schemaName);

        /// <summary>
        /// Liefert den Objekttyp, den die Implementierung behandelt.
        /// </summary>
        string ObjectType { get; }

        /// <summary>
        /// Liefert den Namen des Unterordners für die SQL Files des entsprechenden Objekttyps
        /// </summary>
        string SubFolderName { get; }

        /// <summary>
        /// Referenz zur Dependency Matrix, die durch den FileCreator aktualisiert wird
        /// </summary>
        IDependencyMatrix DependencyMatrix { get; set; }
    }
}
