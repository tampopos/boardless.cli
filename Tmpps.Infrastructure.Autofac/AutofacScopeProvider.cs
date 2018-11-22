using Tmpps.Infrastructure.Common.DependencyInjection.Interfaces;
using Tmpps.Infrastructure.Common.ValueObjects;

namespace Tmpps.Infrastructure.Autofac
{
    public class AutofacScopeProvider : IScopeProvider
    {
        public IScope CurrentScope { get; set; }

        /// <inheritdoc/>
        public IScope BeginLifetimeScope(params TypeValuePair[] inheritances)
        {
            return this.CurrentScope.BeginLifetimeScope(inheritances);
        }

        /// <inheritdoc/>
        public IScope BeginLifetimeScope(object tag, params TypeValuePair[] inheritances)
        {
            return this.CurrentScope.BeginLifetimeScope(tag, inheritances);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            this.CurrentScope = null;
        }
    }
}