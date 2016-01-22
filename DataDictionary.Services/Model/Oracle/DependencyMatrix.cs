using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DataDictionary.Services.Tools;

namespace DataDictionary.Services.Model.Oracle
{
    /// <summary>
    /// Implementierung der Funktionalität der Abhängigkeitsmatrix.
    /// </summary>
    public class DependencyMatrix : IDependencyMatrix
    {
        /// <summary>
        /// Schema-Name. Wenn leer, werden alle vorhandenen Schemata erstellt, ansonsten nur das bezeichnete Schema.
        /// </summary>
        public string SchemaName { get; set; }
        
        /// <summary>
        /// Collection der Datenbankobjekte
        /// </summary>
        public ICollection<DbObject> DbObjects { get; set; }

        /// <summary>
        /// Referenz zur FileSystemHelper Instanz
        /// </summary>
        public IFileSystemHelper FileSystemHelper { get; set; }

        /// <summary>
        /// Standard-Konstruktor. Wenn der Standardwert für die IFileSystemHelper beibehalten wird,
        /// muss dieser spätestens vor dem Aufruf von CreateDependencyFile zugewiesen worden sein.
        /// </summary>
        /// <param name="fileSystemHelper">Instanz des IFileSystemHelper</param>
        /// <param name="schemaName">Schema-Bezeichnung</param>
        public DependencyMatrix(IFileSystemHelper fileSystemHelper = null, string schemaName = null)
        {
            FileSystemHelper = fileSystemHelper;
            DbObjects = new List<DbObject>();
            SchemaName = schemaName;
        }

        /// <summary>
        /// Geordnete Collection der Datenbankobjekte in deren Abhängigkeits-Reihenfolge. Wird durch die Funktion 
        /// GetSortedDependencies implizit erzeugt und durch CreateDependencyFile ausgegeben.
        /// </summary>
        private ICollection<DbObject> _sortedDependencies;
        
        /// <summary>
        /// Zwischenspeicher des aktuell bearbeiteten Datenbank-Objekts bei der Erzeugung der sortierten Liste _sortedDependencies.
        /// </summary>
        private DbObject _currentObject;

        /// <summary>
        /// Rekursive Funktion zum Hinzufügen von Abhängigkeiten zur sortierten Datenbankobjekt-Liste _sortedDependencies.
        /// Bevor das angegebene Datenbank-Objekt an die Liste angefügt wird, wird sichergestellt, dass alle Datenbank-Objekte,
        /// von denen das aktuelle DB-Objekt abhängt, bereits in der Liste enthalten sind. Diese Funktion wird rekursiv
        /// aufgerufen, um ebenfalls die Abhängigkeiten der referenzierten Objekte zu berücksichtigen.
        /// </summary>
        /// <param name="dbObject">Aktuelles Datenbank-Objekt</param>
        private void AddObjectToSortedDependencies(DbObject dbObject)
        {
            if (_sortedDependencies == null)
            {
                _sortedDependencies = new List<DbObject>();
            }

            foreach (var refObject in dbObject.DependsOn.Where(o => !_sortedDependencies.Contains(o) && _currentObject != o))
            {
                AddObjectToSortedDependencies(refObject);
            }

            if (!_sortedDependencies.Contains(dbObject))
            {
                _sortedDependencies.Add(dbObject);
            }
        }

        /// <summary>
        /// Erzeugt und liefert die Liste der Datenbank-Objekte in der reihenfolge der Berücksichtigung von Abhängigkeiten.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<DbObject> GetSortedDependencies()
        {
            _sortedDependencies = null;

            foreach (var dbObject in DbObjects)
            {
                _currentObject = dbObject;
                AddObjectToSortedDependencies(dbObject);
            }

            return _sortedDependencies;
        }
        
        /// <summary>
        /// Erstellen der Abhängigkeiten-Datei anhand der vorher erzeugten sortierten Objekt-Liste.
        /// </summary>
        /// <param name="filePath">Ausgabe-Pfad der Datei.</param>
        public void CreateDependencyFile(string filePath)
        {
            var fileFolderPath = Path.GetDirectoryName(filePath) ?? string.Empty;
            var fileContentBuilder = new StringBuilder();
            var objectIndex = 0;
            var orderedCollection = GetSortedDependencies();

            foreach (var dbObject in orderedCollection
                .Where(
                    o => 
                        !string.IsNullOrEmpty(o.FilePath) && 
                        (string.IsNullOrEmpty(SchemaName) || o.OwnerName == SchemaName)))
            {
                fileContentBuilder.Append("-- ");
                fileContentBuilder.AppendFormat("{0:000}: ", ++objectIndex);
                fileContentBuilder.Append(dbObject.TypeName.ToLowerInvariant());
                fileContentBuilder.Append(" ");
                fileContentBuilder.Append(dbObject.OwnerName.ToLowerInvariant());
                fileContentBuilder.Append(".");
                fileContentBuilder.AppendLine(dbObject.ObjectName.ToLowerInvariant());

                fileContentBuilder.Append("-- depends on: ");
                fileContentBuilder.AppendLine(
                    new CollectionToString(
                        dbObject.DependsOn
                            .Select(
                                o =>
                                    (o.OwnerName + "." + o.ObjectName).ToLowerInvariant()), ", ").GetAsString());

                fileContentBuilder.Append("-- used by: ");
                fileContentBuilder.AppendLine(
                    new CollectionToString(
                        dbObject.UsedBy
                            .Select(
                                o =>
                                    (o.OwnerName + "." + o.ObjectName).ToLowerInvariant()), ", ").GetAsString());

                fileContentBuilder.Append("start \"");
                fileContentBuilder.Append(
                    dbObject.FilePath.Replace(fileFolderPath + Path.DirectorySeparatorChar, string.Empty));
                fileContentBuilder.AppendLine("\";");
                fileContentBuilder.AppendLine();
            }

            FileSystemHelper.SaveContentToFile(filePath,fileContentBuilder.ToString());
        }

        /// <summary>
        /// Liefert das spezifizierte DB-Objekt aus der internen Collection zurück. Wenn es nicht enthalten ist,
        /// wird das Objekt neu erzeugt, in die Collection eingefügt, und die neue Referenz zurückgegeben.
        /// </summary>
        /// <param name="objectType">Objekttyp-Bezeichnung</param>
        /// <param name="objectOwner">Owner des DB-Objektes</param>
        /// <param name="objectName">Name des DB-Objektes</param>
        /// <returns></returns>
        public DbObject GetDbObject(string objectType, string objectOwner, string objectName)
        {
            var dbObject = DbObjects
                .FirstOrDefault(
                    o =>
                        o.OwnerName == objectOwner &&
                        o.TypeName == objectType &&
                        o.ObjectName == objectName);

            if (null == dbObject)
            {
                DbObjects.Add(
                    dbObject =
                        new DbObject
                        {
                            OwnerName = objectOwner,
                            TypeName = objectType,
                            ObjectName = objectName
                        });
            }
            
            return dbObject;
        }
    }
}
