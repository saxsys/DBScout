using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataDictionary.Services.Tools;
using EFModel.DataDictionary;

namespace DataDictionary.Services.Model.Oracle
{
    public class DependenciesSqlGenerator : SqlScriptCreator
    {
        private readonly DBA_VIEWS _view;
        private readonly DBA_MVIEWS _mview;
        private readonly DBA_PROCEDURES _procedure;
        private readonly DBA_TYPES _type;
        private readonly DBA_SYNONYMS _synonym;
        private readonly DBA_SCHEDULER_JOBS _schedulerJob;
        private readonly DBA_CONSTRAINTS _foreignKey;

        private string _objectType;
        private string _owner;
        private string _objectName;

        private readonly IDependencyMatrix _dependencyMatrix;

        private ICollection<DBA_OBJECTS> _referencedObjectsCollection;

        public DependenciesSqlGenerator(DataDictionaryDbContext dbContext, DBA_CONSTRAINTS foreignKey, IDependencyMatrix dependencyMatrix)
            : base(dbContext)
        {
            _foreignKey = foreignKey;
            _dependencyMatrix = dependencyMatrix;
        }

        public DependenciesSqlGenerator(DataDictionaryDbContext dbContext, DBA_SCHEDULER_JOBS schedulerJob, IDependencyMatrix dependencyMatrix)
            : base(dbContext)
        {
            _schedulerJob = schedulerJob;
            _dependencyMatrix = dependencyMatrix;
        }

        public DependenciesSqlGenerator(DataDictionaryDbContext dbContext, DBA_VIEWS view, IDependencyMatrix dependencyMatrix)
            : base(dbContext)
        {
            _view = view;
            _dependencyMatrix = dependencyMatrix;
        }

        public DependenciesSqlGenerator(DataDictionaryDbContext dbContext, DBA_MVIEWS mview, IDependencyMatrix dependencyMatrix)
            : base(dbContext)
        {
            _mview = mview;
            _dependencyMatrix = dependencyMatrix;
        }

        public DependenciesSqlGenerator(DataDictionaryDbContext dbContext, DBA_PROCEDURES procedure, IDependencyMatrix dependencyMatrix)
            : base(dbContext)
        {
            _procedure = procedure;
            _dependencyMatrix = dependencyMatrix;
        }

        public DependenciesSqlGenerator(DataDictionaryDbContext dbContext, DBA_TYPES type, IDependencyMatrix dependencyMatrix)
            : base(dbContext)
        {
            _type = type;
            _dependencyMatrix = dependencyMatrix;
        }

        public DependenciesSqlGenerator(DataDictionaryDbContext dbContext, DBA_SYNONYMS synonym, IDependencyMatrix dependencyMatrix)
            : base(dbContext)
        {
            _synonym = synonym;
            _dependencyMatrix = dependencyMatrix;
        }

        public override string GetSqlString()
        {
            LoadReferencedObjects();
            UpdateDependencyMatrix();

            var sqlStringBuilder = new StringBuilder();
            var dependenciesString = 
                _referencedObjectsCollection == null
                    ? string.Empty
                    : new CollectionToString(
                        _referencedObjectsCollection
                            .Select(
                                o =>
                                    "-- - " +
                                    o.OBJECT_TYPE.ToLowerInvariant() +
                                    " " +
                                    (o.OWNER != _owner ? o.OWNER.ToLowerInvariant() + "." : string.Empty) +
                                    o.OBJECT_NAME.ToLowerInvariant())
                            .Distinct()
                            .ToList(),
                        string.Empty,
                        true).GetAsString();

            sqlStringBuilder.AppendLine(HeaderString("Abhängigkeiten"));
            sqlStringBuilder.AppendLine(string.IsNullOrEmpty(dependenciesString) ? "-- keine Abhängigkeiten" : dependenciesString);

            return sqlStringBuilder.ToString();
        }

        private void UpdateDependencyMatrix()
        {
            if (_dependencyMatrix == null)
            {
                return;
            }

            var dbObject = _dependencyMatrix.GetDbObject(_objectType, _owner, _objectName);
                
            foreach (var refDbObject in _referencedObjectsCollection
                .Select(
                    refObject => 
                        _dependencyMatrix.GetDbObject(refObject.OBJECT_TYPE, refObject.OWNER, refObject.OBJECT_NAME)))
            {
                if (!refDbObject.UsedBy.Contains(dbObject))
                {
                    refDbObject.UsedBy.Add(dbObject);
                }
                
                if (!dbObject.DependsOn.Contains(refDbObject))
                {
                    dbObject.DependsOn.Add(refDbObject);
                }
            }
        }

        private void LoadReferencedObjects()
        {
            if (_view != null)
            {
                LoadViewDependencies();
            }

            if (_mview != null)
            {
                LoadMaterializedViewDependencies();
            }

            if (_procedure != null)
            {
                LoadProcedureDependencies();
            }

            if (_type != null)
            {
                LoadTypeDependencies();
            }

            if (_synonym != null)
            {
                LoadSynonymDependencies();
            }

            if (_schedulerJob != null)
            {
                LoadSchedulerJobDependencies();
            }

            if (_foreignKey != null)
            {
                LoadForeignKeyDependencies();
            }
        }

        private void LoadForeignKeyDependencies()
        {
            _owner = _foreignKey.OWNER;
            _objectType = "FOREIGN KEY";
            _objectName = _foreignKey.CONSTRAINT_NAME;

            var referencedConstraint = DbContext.DBA_CONSTRAINTS
                .FirstOrDefault(
                    r => 
                        r.OWNER != "SYS" && 
                        r.OWNER != "PUBLIC" && 
                        r.OWNER == _foreignKey.R_OWNER && 
                        r.CONSTRAINT_NAME == _foreignKey.R_CONSTRAINT_NAME);

            if (referencedConstraint != null)
            {
                _referencedObjectsCollection = DbContext.DBA_OBJECTS
                    .Where(
                        o =>
                            (o.OWNER == _foreignKey.OWNER && o.OBJECT_NAME == _foreignKey.TABLE_NAME) ||
                            (o.OWNER == referencedConstraint.OWNER && o.OBJECT_NAME == referencedConstraint.TABLE_NAME))
                    .OrderBy(o => o.OWNER)
                    .ThenBy(o => o.OBJECT_NAME)
                    .ToList();
            }
        }

        private void LoadSchedulerJobDependencies()
        {
            _owner = _schedulerJob.OWNER;
            _objectType = "JOB";
            _objectName = _schedulerJob.JOB_NAME;
        }

        private void LoadSynonymDependencies()
        {
            _owner = _synonym.OWNER;
            _objectType = "SYNONYM";
            _objectName = _synonym.SYNONYM_NAME;

            _referencedObjectsCollection = (
                from d in DbContext.DBA_DEPENDENCIES
                from o in DbContext.DBA_OBJECTS
                where
                    d.OWNER == _owner &&
                    d.NAME == _objectName &&
                    d.TYPE == _objectType &&
                    d.OWNER == d.REFERENCED_OWNER &&
                    d.REFERENCED_OWNER == o.OWNER &&
                    d.REFERENCED_NAME == o.OBJECT_NAME &&
                    d.REFERENCED_TYPE == o.OBJECT_TYPE
                orderby o.OBJECT_TYPE, o.OWNER, o.OBJECT_NAME
                select o)
                .ToList();
        }

        private void LoadTypeDependencies()
        {
            _owner = _type.OWNER;
            _objectType = "TYPE";
            _objectName = _type.TYPE_NAME;

            _referencedObjectsCollection = (
                from d in DbContext.DBA_DEPENDENCIES
                from o in DbContext.DBA_OBJECTS
                where
                    d.OWNER == _owner &&
                    d.NAME == _objectName &&
                    d.TYPE == _objectType &&
                    d.OWNER == d.REFERENCED_OWNER &&
                    d.REFERENCED_OWNER == o.OWNER &&
                    d.REFERENCED_NAME == o.OBJECT_NAME &&
                    d.REFERENCED_TYPE == o.OBJECT_TYPE
                orderby o.OBJECT_TYPE, o.OWNER, o.OBJECT_NAME
                select o)
                .ToList();
        }

        private void LoadProcedureDependencies()
        {
            _owner = _procedure.OWNER;
            _objectType = _procedure.OBJECT_TYPE;
            _objectName = _procedure.OBJECT_NAME;

            _referencedObjectsCollection = (
                from d in DbContext.DBA_DEPENDENCIES
                from o in DbContext.DBA_OBJECTS
                where
                    d.OWNER == _owner &&
                    d.NAME == _objectName &&
                    d.OWNER == d.REFERENCED_OWNER &&
                    d.REFERENCED_OWNER == o.OWNER &&
                    d.REFERENCED_NAME == o.OBJECT_NAME &&
                    d.REFERENCED_TYPE == o.OBJECT_TYPE &&
                    o.OBJECT_NAME != _procedure.OBJECT_NAME
                orderby o.OBJECT_TYPE, o.OWNER, o.OBJECT_NAME
                select o)
                .ToList();
        }

        private void LoadViewDependencies()
        {
            _owner = _view.OWNER;
            _objectType = "VIEW";
            _objectName = _view.VIEW_NAME;

            _referencedObjectsCollection = (
                from d in DbContext.DBA_DEPENDENCIES
                from o in DbContext.DBA_OBJECTS
                where
                    d.OWNER == _owner &&
                    d.NAME == _objectName &&
                    d.OWNER == d.REFERENCED_OWNER &&
                    d.REFERENCED_OWNER == o.OWNER &&
                    d.REFERENCED_NAME == o.OBJECT_NAME &&
                    d.REFERENCED_TYPE == o.OBJECT_TYPE
                orderby o.OBJECT_TYPE, o.OWNER, o.OBJECT_NAME
                select o)
                .ToList();
        }

        private void LoadMaterializedViewDependencies()
        {
            _owner = _mview.OWNER;
            _objectType = "MVIEW";
            _objectName = _mview.MVIEW_NAME;

            _referencedObjectsCollection = (
                from d in DbContext.DBA_DEPENDENCIES
                from o in DbContext.DBA_OBJECTS
                where
                    d.OWNER == _owner &&
                    d.NAME == _objectName &&
                    d.OWNER == d.REFERENCED_OWNER &&
                    d.REFERENCED_OWNER == o.OWNER &&
                    d.REFERENCED_NAME == o.OBJECT_NAME &&
                    d.REFERENCED_TYPE == o.OBJECT_TYPE
                orderby o.OBJECT_TYPE, o.OWNER, o.OBJECT_NAME
                select o)
                .ToList();
        }
    }
}
