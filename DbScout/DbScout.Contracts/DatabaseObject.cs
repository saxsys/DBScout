using System.Collections.Generic;

namespace DbScout.Contracts
{
    public class DatabaseObject : IDatabaseObject
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public IDatabaseObject ParentObject { get; set; }
        public ICollection<IDatabaseObject> ChildObjects { get; set; }
        public ICollection<IDatabaseObject> DependsOn { get; set; }
        public IDictionary<string, IDictionary<string, string>> Properties { get; set; }
    }
}
