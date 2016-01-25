using DataDictionary.Services.Tools;
using EFModel.DataDictionary;

namespace DataDictionary.Services.Model
{
    public abstract class SqlFileCreator : ISqlFileCreator
    {
        /// <summary>
        /// Standard-Dateinamenerweiterung für SQL-Dateien
        /// </summary>
        protected const string SqlFileExtension = ".sql";

        /// <summary>
        /// Bezeichnung des Sub-Folders, der die SQL-Dateien der jeweiligen Objekttypen
        /// beinhaltet. Standardmäßig wird an den Objekttyp ein Plural-s angefügt.
        /// </summary>
        public string SubFolderName { get { return ObjectType + "s"; } }

        /// <summary>
        /// Instanz der Abhängigkeits-Matrix
        /// </summary>
        public IDependencyMatrix DependencyMatrix { get; set; }

        /// <summary>
        /// Behandelter Objekttyp.
        /// </summary>
        public string ObjectType { get; protected set; }

        /// <summary>
        /// Datenbank-Kontext, der für die DataDictionary-Abfragen verwendet wird.
        /// </summary>
        protected readonly DataDictionaryDbContext DbContext;

        /// <summary>
        /// FileSystemTools Instanz für Datei- und Verzeichniszugriffe
        /// </summary>
        protected readonly IFileSystemHelper FileSystemHelper;

        /// <summary>
        /// Konstruktor zur Initialisierung der privaten Member
        /// </summary>
        /// <param name="dbContext">Datenbank-Kontext</param>
        /// <param name="fileSystemHelper">Instanz der Implementierung für Filesystem-Zugriffe</param>
        /// <param name="dependencyMatrix">Instanz der Abhängigkeitsmatrix</param>
        protected SqlFileCreator(
            DataDictionaryDbContext dbContext, 
            IFileSystemHelper fileSystemHelper,
            IDependencyMatrix dependencyMatrix)
        {
            DbContext = dbContext;
            FileSystemHelper = fileSystemHelper;
            DependencyMatrix = dependencyMatrix;
        }

        /// <summary>
        /// Erstellen der SQL-Dateien im angegebenen Ziel-Pfad
        /// </summary>
        /// <param name="schemaName">Schema Name</param>
        public abstract void CreateFiles(string schemaName);
    }
}
