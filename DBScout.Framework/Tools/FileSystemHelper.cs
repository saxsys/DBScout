using System;
using System.IO;

namespace DataDictionary.Services.Tools
{
    public class FileSystemHelper : IFileSystemHelper
    {
        /// <summary>
        /// Wurzel-Pfad für die Ausgabedateien
        /// </summary>
        public string RootPath { get; set; }

        /// <summary>
        /// Rekursives Löschen aller Inhalte aus dem angegebenen Basisverzeichnis.
        /// </summary>
        /// <param name="rootPath">Basisverzeichnis</param>
        public void DeleteContentRecursiv(string rootPath)
        {
            if (string.IsNullOrEmpty(rootPath))
            {
                return;
            }

            if (!Directory.Exists(rootPath))
            {
                return;
            }

            try
            {
                Directory.Delete(rootPath, true);
                OnNotifyFileSavedEventHandler(new NotifyFileSavedEventArgs { Message = "Content of directory '" + rootPath + "' successfully removed." });
            }
            catch (Exception e)
            {
                OnNotifyFileSavedEventHandler(new NotifyFileSavedEventArgs { Message = "Exception: " + e.Message });
            }
        }

        /// <summary>
        /// Speichern des angegebenen Inhaltes in die angegebene Datei.
        /// </summary>
        /// <param name="filePath">Vollständiger File-Pfad</param>
        /// <param name="fileContent">Inhalt, der in die Datei zu schreiben ist.</param>
        public void SaveContentToFile(string filePath, string fileContent)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filePath) ?? ".");
            using (var file = new StreamWriter(filePath))
            {
                file.Write(fileContent);
                OnNotifyFileSavedEventHandler(new NotifyFileSavedEventArgs { FileName = filePath, Message = "File '" + filePath + "' created."});
            }
        }

        /// <summary>
        /// Event für Nachrichten an Clients
        /// </summary>
        public event NotifyFileSavedEventHandler NotifyFileSavedEventHandler;

        /// <summary>
        /// Ausführen des Event Handlers
        /// </summary>
        /// <param name="e">Parameter der Nachricht</param>
        protected virtual void OnNotifyFileSavedEventHandler(NotifyFileSavedEventArgs e)
        {
            if (NotifyFileSavedEventHandler != null)
            {
                NotifyFileSavedEventHandler(this, e);
            }
        }

        /// <summary>
        /// Liefert den Ausgabe-Pfad für den angegebenen Objekttyp
        /// </summary>
        /// <param name="objectType">Datenbank-Objekttyp (Tabelle, View, ...)</param>
        /// <param name="schemaName">Schema-Name</param>
        /// <returns>Liefert den mit <see cref="Path.DirectorySeparatorChar"/> abgeschlossenen Ausgabepfad.</returns>
        public string GetOutputPath(string objectType, string schemaName)
        {
            return RootPath + Path.DirectorySeparatorChar +
                (string.IsNullOrEmpty(schemaName) ? string.Empty : schemaName + Path.DirectorySeparatorChar) +
                (string.IsNullOrEmpty(objectType) ? string.Empty : objectType + Path.DirectorySeparatorChar);
        }
    }
}
