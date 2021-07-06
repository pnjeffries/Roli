using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Binding
{
    /// <summary>
    /// Abstract base class for data bindings which bind to multiple source properties
    /// </summary>
    public abstract class MultiBindingBase : DataBindingBase
    {
        public List<SourceBinding> Bindings = new List<SourceBinding>() { new SourceBinding(), new SourceBinding() };

        protected override bool BindingRefreshRequired
        {
            get
            {
                foreach (var binding in Bindings)
                {
                    if (binding.BindingRefreshRequired) return true;
                }
                return false;
            }
            set
            {
                foreach (var binding in Bindings) binding.BindingRefreshRequired = value;
            }
        }

        protected override bool UIRefreshRequired
        {
            get
            {
                foreach (var binding in Bindings)
                {
                    if (binding.UIRefreshRequired) return true;
                }
                return false;
            }
            set
            {
                foreach (var binding in Bindings) binding.UIRefreshRequired = value;
            }
        }

        public void AddNewSubBinding()
        {
            var newSub = new SourceBinding();
            Bindings.Add(newSub);
        }

        protected override void RefreshBinding()
        {
            base.RefreshBinding();
            foreach (var binding in Bindings)
            {
                binding.RefreshBinding();
            }
        }

        public override void NotifyDataContextUpdated(DataContext context)
        {
            base.NotifyDataContextUpdated(context);
            foreach (var binding in Bindings)
            {
                binding.NotifyDataContextUpdated(context);
            }
        }

        protected override void CleanupOnDestroy()
        {
            base.CleanupOnDestroy();
            foreach (var binding in Bindings)
            {
                binding.CleanupOnDestroy();
            }
        }

        protected override void InitialiseBinding()
        {
            base.InitialiseBinding();
            foreach (var binding in Bindings)
            {
                binding.Owner = this;
            }
        }

        /// <summary>
        /// Get an array of all current bound values
        /// </summary>
        /// <returns></returns>
        public virtual object[] GetBoundValues()
        {
            var result = new object[Bindings.Count];
            for (int i = 0; i < Bindings.Count; i++)
            {
                result[i] = Bindings[i].GetBoundValue();
            }
            return result;
        }

        protected override void BindingUpdates()
        {
            base.BindingUpdates();

        }
    }
}
