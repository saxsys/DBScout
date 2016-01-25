namespace DataDictionary.Services.Model
{
    /// <summary>
    /// Interface zur Definition der Schnittstelle für SQL Datei Exporter-Klassen
    /// Für jeden Objekttyp wird eine separate Klasse bereitgestellt, die dieses Interface
    /// implementiert.
    /// </summary>
    public interface ISqlScriptCreator
    {
        /// <summary>
        /// Liefert den SQL-String für das konkrete Datenbank-Objekt
        /// </summary>
        /// <returns>SQL-String bzw. SQL-Fragment</returns>
        string GetSqlString();
    }
}
