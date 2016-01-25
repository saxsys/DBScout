using System;
using DataDictionary.Services.Model;
using DataDictionary.Services.Model.Oracle;
using DataDictionary.Services.Tools;
using EFModel.DataDictionary;

namespace DataDictionary.Services.Controller
{
    public delegate void CreateSqlFiles(string schemaName);

    public class DataDictionaryController
    {
        private readonly IFileSystemHelper _fileSystemHelper;

        private readonly CreateSqlFiles _createSqlFiles;

        private readonly IDependencyMatrix _dependencyMatrix;

        /// <summary>
        /// Constructor der Instanz. Da bei der Erstellung die Factory zur Erstellung
        /// des Datenbank-Kontextes erzeugt wird, muss der Oracle-ConnectionString 
        /// angegeben werden.
        /// </summary>
        /// <param name="oracleConnectionString"></param>
        public DataDictionaryController(string oracleConnectionString)
        {
            if (string.IsNullOrEmpty(oracleConnectionString))
            {
                throw new InvalidOperationException("ConnectionString must not be empty or null!");
            }

            _fileSystemHelper = new FileSystemHelper();
            _dependencyMatrix = new DependencyMatrix(_fileSystemHelper);

            var dbContext = new DataDictionaryDbContextFactory(oracleConnectionString).Create();

            _fileSystemHelper.NotifyFileSavedEventHandler += _fileSystemHelper_NotifyFileSavedEventHandler;

            _createSqlFiles += s => new DbLinkFileContentCreator(dbContext, _fileSystemHelper, _dependencyMatrix).CreateFiles(s);
            _createSqlFiles += s => new SequenceFileContentCreator(dbContext, _fileSystemHelper, _dependencyMatrix).CreateFiles(s);
            _createSqlFiles += s => new TypeFileContentCreator(dbContext, _fileSystemHelper, _dependencyMatrix).CreateFiles(s);
            _createSqlFiles += s => new TableFileContentGenerator(dbContext, _fileSystemHelper, _dependencyMatrix).CreateFiles(s);
            _createSqlFiles += s => new ForeignKeyConstraintFileCreator(dbContext, _fileSystemHelper, _dependencyMatrix).CreateFiles(s);
            _createSqlFiles += s => new ViewFileContentGenerator(dbContext, _fileSystemHelper, _dependencyMatrix).CreateFiles(s);
            _createSqlFiles += s => new MaterializedViewFileContentGenerator(dbContext, _fileSystemHelper, _dependencyMatrix).CreateFiles(s);
            _createSqlFiles += s => new PackagesFileContentGenerator(dbContext, _fileSystemHelper, _dependencyMatrix).CreateFiles(s);
            _createSqlFiles += s => new ProceduresFileContentCreator(dbContext, _fileSystemHelper, _dependencyMatrix).CreateFiles(s);
            _createSqlFiles += s => new FunctionsFileContentCreator(dbContext, _fileSystemHelper, _dependencyMatrix).CreateFiles(s);
            _createSqlFiles += s => new TriggerFileContentCreator(dbContext, _fileSystemHelper, _dependencyMatrix).CreateFiles(s);
            _createSqlFiles += s => new SynonymFileContentCreator(dbContext, _fileSystemHelper, _dependencyMatrix).CreateFiles(s);
            _createSqlFiles += s => new DbmsSchedulerJobFileContentCreator(dbContext, _fileSystemHelper, _dependencyMatrix).CreateFiles(s);
        }

        void _fileSystemHelper_NotifyFileSavedEventHandler(object sender, NotifyFileSavedEventArgs e)
        {
            if (e == null)
            {
                return;
            }

            if (e.FileName == null && e.Message == null)
            {
                return;
            }

            Console.Out.WriteLine(e.FileName ?? e.Message);
        }

        /// <summary>
        /// basisverzeichnis für die Ausgabedateien
        /// </summary>
        public string RootPath { get; set; }

        /// <summary>
        /// Schema Name
        /// </summary>
        public string SchemaName { get; set; }

        /// <summary>
        /// Einlesen des Datenmodells aus der Datenbank, die durch den Oracle ConnectionString 
        /// angegeben worden ist, und Erzeugen von SQL-Dateien in der eingestellten Ordnerstruktur. 
        /// Der Datenbank-Kontext wird erst beim beim Aufruf erstellt.
        /// </summary>
        public void CreateSqlFilesFromDatabaseModel()
        {
            _fileSystemHelper.RootPath = RootPath;
            var schemaPath = _fileSystemHelper.GetOutputPath(string.Empty, SchemaName);

            _fileSystemHelper.DeleteContentRecursiv(schemaPath);

            if (_createSqlFiles != null)
            {
                _createSqlFiles(SchemaName);
            }

            _dependencyMatrix.SchemaName = SchemaName;
            _dependencyMatrix.CreateDependencyFile(schemaPath + "create_datamodel.sql");
        }
    }
}
