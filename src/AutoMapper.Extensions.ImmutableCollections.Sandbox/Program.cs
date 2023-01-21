using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace AutoMapper.Extensions.ImmutableCollections.Sandbox
{
  class Program
  {
    // System.Collections.Immutable.ImmutableList<>
    public class ImmutableListTypeConverter<TSource, TDestination>
      : ImmutableCollectionTypeConverter<TSource, TDestination, ImmutableList<TDestination>>
    {
      protected override ImmutableList<TDestination> ConvertEnumerable(IEnumerable<TDestination> enumerable)
        => enumerable.ToImmutableList();
    }
    
    // System.Collections.Immutable.ImmutableHashSet<>
    public class ImmutableHashSetTypeConverter<TSource, TDestination>
      : ImmutableCollectionTypeConverter<TSource, TDestination, ImmutableHashSet<TDestination>>
    {
      protected override ImmutableHashSet<TDestination> ConvertEnumerable(IEnumerable<TDestination> enumerable)
        => enumerable.ToImmutableHashSet();
    }
    
    // System.Collections.Immutable.ImmutableSortedSet<>
    public class ImmutableSortedSetSetTypeConverter<TSource, TDestination>
      : ImmutableCollectionTypeConverter<TSource, TDestination, ImmutableSortedSet<TDestination>>
    {
      protected override ImmutableSortedSet<TDestination> ConvertEnumerable(IEnumerable<TDestination> enumerable)
        => enumerable.ToImmutableSortedSet();
    }

    // System.Collections.Immutable.ImmutableQueue<>
    public class ImmutableQueueTyperConverter<TSource, TDestination>
      : ImmutableCollectionTypeConverter<TSource, TDestination, ImmutableQueue<TDestination>>
    {
      protected override ImmutableQueue<TDestination> ConvertEnumerable(IEnumerable<TDestination> enumerable)
      {
        var destinationQueue = enumerable.Aggregate(ImmutableQueue<TDestination>.Empty, (queue, item) => queue.Enqueue(item));

        return destinationQueue;
      }
    }
    
    //System.Collections.Immutable.ImmutableArray<>
    public class ImmutableArrayTypeConverter<TSource, TDestination>
      : ImmutableCollectionTypeConverter<TSource, TDestination, ImmutableArray<TDestination>>
    {
      protected override ImmutableArray<TDestination> ConvertEnumerable(IEnumerable<TDestination> enumerable)
        => enumerable.ToImmutableArray();
    }
    
    
    // System.Collections.Immutable.ImmutableStack<>
    public class ImmutableStackTypeConverter<TSource, TDestination>
      : ImmutableCollectionTypeConverter<TSource, TDestination, ImmutableStack<TDestination>>
    {
      protected override ImmutableStack<TDestination> ConvertEnumerable(IEnumerable<TDestination> enumerable)
      {
        var destinationStack = enumerable.Aggregate(ImmutableStack<TDestination>.Empty, (stack, item) => stack.Push(item));

        return destinationStack;
      }
    }
    
    // System.Collections.Immutable.ImmutableDictionary<>
    public class ImmutableDictionaryTypeConverter<TSourceKey, TSourceValue, TDestinationKey, TDestinationValue>
      : ITypeConverter<IDictionary<TSourceKey, TSourceValue>, ImmutableDictionary<TDestinationKey, TDestinationValue>>
    {
      public ImmutableDictionary<TDestinationKey, TDestinationValue> Convert(
        IDictionary<TSourceKey, TSourceValue> source,
        ImmutableDictionary<TDestinationKey, TDestinationValue> destination, ResolutionContext context
      )
      {
        return source
          .Select(keyValuePair =>
          {
            var convertedKey = context.Mapper.Map<TDestinationKey>(keyValuePair.Key);
            var convertedValue = context.Mapper.Map<TDestinationValue>(keyValuePair.Value);

            return new KeyValuePair<TDestinationKey, TDestinationValue>(convertedKey, convertedValue);
          })
          .ToDictionary(
            keySelector: keyValuePair => keyValuePair.Key,
            elementSelector: keyValuePair => keyValuePair.Value
          )
          .ToImmutableDictionary();
      }
    }

        //System.Collections.Immutable.ImmutableSortedDictionary<>
      
    
      

    public abstract class ImmutableCollectionTypeConverter<TSource, TDestination, TImmutableCollection>
      : ITypeConverter<IEnumerable<TSource>, TImmutableCollection>
    {
      public TImmutableCollection Convert(IEnumerable<TSource> source, TImmutableCollection destination,
        ResolutionContext context)
      {
        var convertedEnumerable = source.Select(context.Mapper.Map<TSource, TDestination>);
        var immutableCollection = ConvertEnumerable(convertedEnumerable);

        return immutableCollection;
      }

      protected abstract TImmutableCollection ConvertEnumerable(IEnumerable<TDestination> enumerable);
    }
    
    public static void Main(string[] args)
    {
      var source = new List<int> { 1, 2, 3 };
      var mapper = new MapperConfiguration(configuration =>
      {
        configuration
          .CreateMap(typeof(IEnumerable<>), typeof(ImmutableList<>))
          .ConvertUsing(typeof(ImmutableListTypeConverter<,>));
        configuration
          .CreateMap(typeof(IEnumerable<>), typeof(ImmutableHashSet<>))
          .ConvertUsing(typeof(ImmutableHashSetTypeConverter<,>));
        configuration
          .CreateMap(typeof(IEnumerable<>), typeof(ImmutableQueue<>))
          .ConvertUsing(typeof(ImmutableQueueTyperConverter<,>));
        configuration
          .CreateMap(typeof(IDictionary<,>), typeof(ImmutableDictionary<,>))
          .ConvertUsing(typeof(ImmutableDictionaryTypeConverter<,,,>));
      }).CreateMapper();

      var immutableList = mapper.Map<ImmutableList<int>>(new List<int>{1,2,3});
      var immutableList1 = mapper.Map<ImmutableList<int>>(new[]{1,2,3});
      var immutableList2 = mapper.Map<ImmutableList<int>>(new HashSet<int>{1,2,3});
      var immutableSet = mapper.Map<ImmutableHashSet<int>>(new List<int>{1,2,3});
      var immutableSet1 = mapper.Map<ImmutableHashSet<int>>(new[]{1,2,3});
      var immutableSet2 = mapper.Map<ImmutableHashSet<int>>(new HashSet<int>{1,2,3});
      /*var immutableQueue = mapper.Map<ImmutableQueue<int>>(new List<int>{1,2,3});
      var immutableQueue1 = mapper.Map<ImmutableQueue<int>>(new[]{1,2,3});
      var immutableQueue2 = mapper.Map<ImmutableQueue<int>>(new HashSet<int>{1,2,3});*/
      var immutableDict = mapper.Map<ImmutableDictionary<string, int>>(new Dictionary<string, int>{{"1", 1}});
      var immutableDict1 = mapper.Map<ImmutableDictionary<int, int>>(new Dictionary<string, int>{{"1", 1}});

      IEnumerable<int> ints = mapper.Map<IEnumerable<int>>(immutableList);

      Console.WriteLine("Hello World!");
    }
  }
}