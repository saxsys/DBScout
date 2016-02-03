using System;
namespace DBScout.Model
{
    public class DatabaseSchema : AbstractDbObject
    {
        #region Member variables

        /// <summary>
        /// Tables collection
        /// </summary>
        private DbObjectCompositum _tables;

        /// <summary>
        /// Views collection
        /// </summary>
        private DbObjectCompositum _views;

        /// <summary>
        /// Materialized views collection
        /// </summary>
        private DbObjectCompositum _materializedViews;

        /// <summary>
        /// Packages collection
        /// </summary>
        private DbObjectCompositum _packages;

        /// <summary>
        /// Procedures collection
        /// </summary>
        private DbObjectCompositum _procedures;

        /// <summary>
        /// Functions collection
        /// </summary>
        private DbObjectCompositum _functions;

        /// <summary>
        /// Database triggers collection (not assigned to a table or view)
        /// </summary>
        private DbObjectCompositum _triggers;

        /// <summary>
        /// Types collection
        /// </summary>
        private DbObjectCompositum _types;

        /// <summary>
        /// Sequences collection
        /// </summary>
        private DbObjectCompositum _sequences;

        /// <summary>
        /// Synonyms collection
        /// </summary>
        private DbObjectCompositum _synonyms;

        /// <summary>
        /// Database links collection
        /// </summary>
        private DbObjectCompositum _databaseLinks;

        /// <summary>
        /// Database jobs collection
        /// </summary>
        private DbObjectCompositum _databaseJobs;

        #endregion

        #region Properties

        /// <summary>
        /// Accessor to tables collection.
        /// </summary>
        public DbObjectCompositum Tables { get { return _tables ?? (_tables = new DbObjectCompositum()); } }

        /// <summary>
        /// Accessor to views collection.
        /// </summary>
        public DbObjectCompositum Views { get { return _views ?? (_views = new DbObjectCompositum()); } }

        /// <summary>
        /// Accessor to materialized views collection.
        /// </summary>
        public DbObjectCompositum MaterializedViews { get { return _materializedViews ?? (_materializedViews = new DbObjectCompositum()); } }

        /// <summary>
        /// Accessor to packages collection
        /// </summary>
        public DbObjectCompositum Packages { get { return _packages ?? (_packages = new DbObjectCompositum()); } }

        /// <summary>
        /// Accessor to procedures collection
        /// </summary>
        public DbObjectCompositum Procedures { get { return _procedures ?? (_procedures = new DbObjectCompositum()); } }

        /// <summary>
        /// Accessor to functions collection
        /// </summary>
        public DbObjectCompositum Functions { get { return _functions ?? (_functions = new DbObjectCompositum()); } }

        /// <summary>
        /// Accessor to database triggers collection 
        /// </summary>
        public DbObjectCompositum Triggers { get { return _triggers ?? (_triggers = new DbObjectCompositum()); } }

        /// <summary>
        /// Accessor to types collection 
        /// </summary>
        public DbObjectCompositum Types { get { return _types ?? (_types = new DbObjectCompositum()); } }

        /// <summary>
        /// Accessor to sequences collection 
        /// </summary>
        public DbObjectCompositum Sequences { get { return _sequences ?? (_sequences = new DbObjectCompositum()); } }

        /// <summary>
        /// Accessor to synonyms collection 
        /// </summary>
        public DbObjectCompositum Synonyms { get { return _synonyms ?? (_synonyms = new DbObjectCompositum()); } }

        /// <summary>
        /// Accessor to database links collection 
        /// </summary>
        public DbObjectCompositum DatabaseLinks { get { return _databaseLinks ?? (_databaseLinks = new DbObjectCompositum()); } }

        /// <summary>
        /// Accessor to database jobs collection 
        /// </summary>
        public DbObjectCompositum DatabaseJobs { get { return _databaseJobs ?? (_databaseJobs = new DbObjectCompositum()); } }

        /// <summary>
        /// Timestamp when the database schema was created 
        /// </summary>
        public DateTime? Created { get; set; }
        
        #endregion
    }
}
