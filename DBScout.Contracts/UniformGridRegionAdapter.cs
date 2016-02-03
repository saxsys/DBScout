using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls.Primitives;
using Prism.Regions;

namespace DBScout.Contracts
{
    public class UniformGridRegionAdapter : RegionAdapterBase<UniformGrid>
    {
        public UniformGridRegionAdapter(IRegionBehaviorFactory regionBehaviorFactory) : base(regionBehaviorFactory)
        {
        }

        protected override void Adapt(IRegion region, UniformGrid regionTarget)
        {
            region.Views.CollectionChanged += (sender, args) =>
            {
                if (args.Action == NotifyCollectionChangedAction.Add)
                {
                    foreach (FrameworkElement element in args.NewItems)
                    {
                        regionTarget.Children.Add(element);
                    }
                }
                else if (args.Action == NotifyCollectionChangedAction.Remove)
                {
                    foreach (FrameworkElement element in args.NewItems)
                    {
                        regionTarget.Children.Remove(element);
                    }
                }
            };
        }

        protected override IRegion CreateRegion()
        {
            return  new AllActiveRegion();
        }
    }
}
