using System;

namespace Tmpps.Infrastructure.Common.ValueObjects
{
    public class MapKey
    {
        public MapKey(Type sourceType, Type destinationType)
        {
            this.SourceType = sourceType;
            this.DestinationType = destinationType;
        }
        public Type SourceType { get; }
        public Type DestinationType { get; }
        public override bool Equals(object obj)
        {
            if (obj is MapKey mapKey)
            {
                return mapKey.SourceType == this.SourceType && mapKey.DestinationType == this.DestinationType;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return this.SourceType.GetHashCode() ^ this.DestinationType.GetHashCode();
        }

        public static bool operator ==(MapKey z, MapKey w)
        {
            return z.Equals(w);
        }
        public static bool operator !=(MapKey z, MapKey w)
        {
            return !z.Equals(w);
        }

        public bool IsAssignableFrom(MapKey key)
        {
            return this.SourceType.IsAssignableFrom(key.SourceType) && this.DestinationType.IsAssignableFrom(key.DestinationType);
        }
    }
    public class MapKey<TSource, TDestination> : MapKey
    {
        public MapKey() : base(typeof(TSource), typeof(TDestination)) { }
    }
}