using System.Collections.Generic;

namespace DataDictionary.Services.Model
{
    public interface IDependencyMatrix
    {
        ICollection<DbObject> DbObjects { get; set; }
        string SchemaName { get; set; }

        void CreateDependencyFile(string filePath);

        DbObject GetDbObject(string objectType, string objectOwner, string objectName);
    }
}
