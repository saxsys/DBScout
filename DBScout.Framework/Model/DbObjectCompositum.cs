using System.Collections.Generic;

namespace DBScout.Model
{
    /// <summary>
    /// Definition of compositum of 
    /// </summary>
    public class DbObjectCompositum : AbstractDbObject
    {
        /// <summary>
        /// Collection of child database objects
        /// </summary>
        private ICollection<IDbObject> _children;

        /// <summary>
        /// Accessor to children collection
        /// </summary>
        public ICollection<IDbObject> Children
        {
            get { return _children ?? (_children = new List<IDbObject>()); }
        }

        /// <summary>
        /// Add new item to child objects collection
        /// </summary>
        /// <param name="newItem">Item to be added</param>
        public void Add(IDbObject newItem)
        {
            Children.Add(newItem);
        }

        /// <summary>
        /// Remove specified item from child objects collection
        /// </summary>
        /// <param name="itemToRemove">Item to be removed</param>
        public void Remove(IDbObject itemToRemove)
        {
            Children.Remove(itemToRemove);
        }
    }
}
