using System.IO;

namespace DataDictionary.Services.Tools
{
    public delegate void NotifyFileSavedEventHandler(object sender, NotifyFileSavedEventArgs e);

    public interface IFileSystemHelper
    {
        /// <summary>
        /// Event für Nachrichten an Clients
        /// </summary>
        event NotifyFileSavedEventHandler NotifyFileSavedEventHandler;
        
        /// <summary>
        /// Wurzel-Pfad für die Ausgabedateien
        /// </summary>
        string RootPath { get; set; }

        /// <summary>
        /// Rekursives Löschen aller Inhalte aus dem angegebenen Basisverzeichnis.
        /// </summary>
        /// <param name="rootPath">Basisverzeichnis</param>
        void DeleteContentRecursiv(string rootPath);

        /// <summary>
        /// Speichern des angegebenen Inhaltes in die angegebene Datei.
        /// </summary>
        /// <param name="filePath">Vollständiger File-Pfad</param>
        /// <param name="fileContent">Inhalt, der in die Datei zu schreiben ist.</param>
        void SaveContentToFile(string filePath, string fileContent);

        /// <summary>
        /// Liefert den Ausgabe-Pfad für den angegebenen Objekttyp
        /// </summary>
        /// <param name="objectType">Datenbank-Objekttyp (Tabelle, View, ...)</param>
        /// <param name="schemaName">Schema-Name</param>
        /// <returns>Liefert den mit <see cref="Path.DirectorySeparatorChar"/> abgeschlossenen Ausgabepfad.</returns>
        string GetOutputPath(string objectType, string schemaName);
    }
}