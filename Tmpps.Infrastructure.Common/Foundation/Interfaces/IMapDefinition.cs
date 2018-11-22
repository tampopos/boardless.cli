namespace Tmpps.Infrastructure.Common.Foundation.Interfaces
{
    public interface IMapDefinition<TSource, TDestination>
    {
        void Map(TSource source, TDestination destination);
        void MapReverse(TDestination source, TSource destination);
    }
}