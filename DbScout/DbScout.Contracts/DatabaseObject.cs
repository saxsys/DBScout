using System.Collections.Generic;

namespace DbScout.Contracts
{
    public class DatabaseObject : IDatabaseObject
    {
        public string Type { get; set; }

        public string Name { get; set; }

        public IDatabaseObject ParentObject { get; set; }

        private ICollection<IDatabaseObject> _childObjects;

        public ICollection<IDatabaseObject> ChildObjects
        {
            get { return _childObjects ?? (_childObjects = new List<IDatabaseObject>()); }
            set { _childObjects = value; }
        }

        private ICollection<IDatabaseObject> _dependsOn;

        public ICollection<IDatabaseObject> DependsOn
        {
            get { return _dependsOn ?? (_dependsOn = new List<IDatabaseObject>()); }
            set { _dependsOn = value; }
        }

        private IDictionary<string, IDictionary<string, object>> _properties;

        public IDictionary<string, IDictionary<string, object>> Properties
        {
            get { return _properties ?? (_properties = new Dictionary<string, IDictionary<string, object>>()); }
            set { _properties = value; }
        }
    }
}
