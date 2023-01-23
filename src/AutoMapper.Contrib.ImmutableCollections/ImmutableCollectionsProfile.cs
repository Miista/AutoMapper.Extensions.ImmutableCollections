using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace AutoMapper.Contrib.ImmutableCollections
{
  public class ImmutableCollectionsProfile : Profile
  {
    public ImmutableCollectionsProfile()
    {
      CreateMap(typeof(IEnumerable<>), typeof(ImmutableList<>))
        .ConvertUsing(typeof(ImmutableListTypeConverter<,>));
      CreateMap(typeof(IEnumerable<>), typeof(ImmutableArray<>))
        .ConvertUsing(typeof(ImmutableArrayTypeConverter<,>));
      CreateMap(typeof(IEnumerable<>), typeof(ImmutableStack<>))
        .ConvertUsing(typeof(ImmutableStackTypeConverter<,>));
      CreateMap(typeof(IEnumerable<>), typeof(ImmutableHashSet<>))
        .ConvertUsing(typeof(ImmutableHashSetTypeConverter<,>));
      CreateMap(typeof(IEnumerable<>), typeof(ImmutableSortedSet<>))
        .ConvertUsing(typeof(ImmutableSortedSetSetTypeConverter<,>));
      CreateMap(typeof(IEnumerable<>), typeof(ImmutableQueue<>))
        .ConvertUsing(typeof(ImmutableQueueTyperConverter<,>));
      CreateMap(typeof(IDictionary<,>), typeof(ImmutableDictionary<,>))
        .ConvertUsing(typeof(ImmutableDictionaryTypeConverter<,,,>));
      CreateMap(typeof(IDictionary<,>), typeof(ImmutableSortedDictionary<,>))
        .ConvertUsing(typeof(ImmutableSortedDictionaryTypeConverter<,,,>));
    }

    // System.Collections.Immutable.ImmutableList<>
    private class ImmutableListTypeConverter<TSource, TDestination>
      : ImmutableCollectionTypeConverter<TSource, TDestination, ImmutableList<TDestination>>
    {
      protected override ImmutableList<TDestination> ConvertEnumerable(IEnumerable<TDestination> enumerable)
        => enumerable.ToImmutableList();
    }

    // System.Collections.Immutable.ImmutableHashSet<>
    private class ImmutableHashSetTypeConverter<TSource, TDestination>
      : ImmutableCollectionTypeConverter<TSource, TDestination, ImmutableHashSet<TDestination>>
    {
      protected override ImmutableHashSet<TDestination> ConvertEnumerable(IEnumerable<TDestination> enumerable)
        => enumerable.ToImmutableHashSet();
    }

    // System.Collections.Immutable.ImmutableSortedSet<>
    private class ImmutableSortedSetSetTypeConverter<TSource, TDestination>
      : ImmutableCollectionTypeConverter<TSource, TDestination, ImmutableSortedSet<TDestination>>
    {
      protected override ImmutableSortedSet<TDestination> ConvertEnumerable(IEnumerable<TDestination> enumerable)
        => enumerable.ToImmutableSortedSet();
    }

    // System.Collections.Immutable.ImmutableQueue<>
    private class ImmutableQueueTyperConverter<TSource, TDestination>
      : ImmutableCollectionTypeConverter<TSource, TDestination, ImmutableQueue<TDestination>>
    {
      protected override ImmutableQueue<TDestination> ConvertEnumerable(IEnumerable<TDestination> enumerable)
      {
        var destinationQueue =
          enumerable.Aggregate(ImmutableQueue<TDestination>.Empty, (queue, item) => queue.Enqueue(item));

        return destinationQueue;
      }
    }

    //System.Collections.Immutable.ImmutableArray<>
    private class ImmutableArrayTypeConverter<TSource, TDestination>
      : ImmutableCollectionTypeConverter<TSource, TDestination, ImmutableArray<TDestination>>
    {
      protected override ImmutableArray<TDestination> ConvertEnumerable(IEnumerable<TDestination> enumerable)
        => enumerable.ToImmutableArray();
    }

    // System.Collections.Immutable.ImmutableStack<>
    private class ImmutableStackTypeConverter<TSource, TDestination>
      : ImmutableCollectionTypeConverter<TSource, TDestination, ImmutableStack<TDestination>>
    {
      protected override ImmutableStack<TDestination> ConvertEnumerable(IEnumerable<TDestination> enumerable)
      {
        var destinationStack =
          enumerable.Aggregate(ImmutableStack<TDestination>.Empty, (stack, item) => stack.Push(item));

        return destinationStack;
      }
    }

    // System.Collections.Immutable.ImmutableDictionary<>
    private class ImmutableDictionaryTypeConverter<TSourceKey, TSourceValue, TDestinationKey, TDestinationValue>
      : ITypeConverter<IDictionary<TSourceKey, TSourceValue>, ImmutableDictionary<TDestinationKey, TDestinationValue>>
    {
      public ImmutableDictionary<TDestinationKey, TDestinationValue> Convert(
        IDictionary<TSourceKey, TSourceValue> source,
        ImmutableDictionary<TDestinationKey, TDestinationValue> destination, ResolutionContext context
      )
      {
        var mappedSource = context.Mapper.Map<IDictionary<TDestinationKey, TDestinationValue>>(source);

        return mappedSource.ToImmutableDictionary();
      }
    }

    // System.Collections.Immutable.ImmutableSortedDictionary<>
    private class ImmutableSortedDictionaryTypeConverter<TSourceKey, TSourceValue, TDestinationKey, TDestinationValue>
      : ITypeConverter<IDictionary<TSourceKey, TSourceValue>,
        ImmutableSortedDictionary<TDestinationKey, TDestinationValue>>
    {
      public ImmutableSortedDictionary<TDestinationKey, TDestinationValue> Convert(
        IDictionary<TSourceKey, TSourceValue> source,
        ImmutableSortedDictionary<TDestinationKey, TDestinationValue> destination, ResolutionContext context
      )
      {
        var mappedSource = context.Mapper.Map<IDictionary<TDestinationKey, TDestinationValue>>(source);

        return mappedSource.ToImmutableSortedDictionary();
      }
    }

    private abstract class ImmutableCollectionTypeConverter<TSource, TDestination, TImmutableCollection>
      : ITypeConverter<IEnumerable<TSource>, TImmutableCollection>
    {
      public TImmutableCollection Convert(IEnumerable<TSource> source, TImmutableCollection destination,
        ResolutionContext context)
      {
        return ConvertEnumerable(context.Mapper.Map<IEnumerable<TDestination>>(source));
      }

      protected abstract TImmutableCollection ConvertEnumerable(IEnumerable<TDestination> enumerable);
    }
  }
}