using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using AutoFixture;
using FluentAssertions;
using Xunit;

namespace AutoMapper.Contrib.ImmutableCollections.Tests
{
  public class ImmutableCollectionsProfileTests
  {
    [Theory]
    [MemberData(nameof(Supports_converting_to_immutable_collection_Data))]
    public void Supports_converting_to_immutable_collection<TSourceCollection, TExpectedCollection>(
      TSourceCollection sourceCollection,
      TExpectedCollection expectedCollection
    )
      where TSourceCollection : IEnumerable
      where TExpectedCollection : IEnumerable
    {
      // Arrange
      var sut = CreateMapper();
      
      // Act
      var result = sut.Map<TExpectedCollection>(sourceCollection);
      
      // Assert
      result.Should().NotBeNull();
      result.Should().BeEquivalentTo(expectedCollection);
    }

    // ReSharper disable once InconsistentNaming
    public static IEnumerable<object[]> Supports_converting_to_immutable_collection_Data
    {
      get
      {
        var fixture = new Fixture();

        yield return TestCase<IEnumerable<int>, ImmutableList<int>>(ImmutableList.ToImmutableList);
        yield return TestCase<IEnumerable<int>, ImmutableQueue<int>>(ImmutableQueue.CreateRange);
        yield return TestCase<IEnumerable<int>, ImmutableStack<int>>(ImmutableStack.CreateRange);
        yield return TestCase<IEnumerable<int>, ImmutableHashSet<int>>(ImmutableHashSet.ToImmutableHashSet);
        yield return TestCase<IEnumerable<int>, ImmutableSortedSet<int>>(ImmutableSortedSet.ToImmutableSortedSet);
        yield return TestCase<IEnumerable<int>, ImmutableArray<int>>(ImmutableArray.ToImmutableArray);
        yield return TestCase<Dictionary<int, int>, ImmutableDictionary<int, int>>(ImmutableDictionary.ToImmutableDictionary);
        yield return TestCase<Dictionary<int, int>, ImmutableSortedDictionary<int, int>>(ImmutableSortedDictionary.ToImmutableSortedDictionary);

        object[] TestCase<TSourceCollection, TCollection>(
          Func<TSourceCollection, TCollection> creator
        )
        {
          var source = fixture.Create<TSourceCollection>();

          return new object[]
          {
            source,
            creator(source)
          };
        }
      }
    }

    private static IMapper CreateMapper() =>
      new MapperConfiguration(expression =>
          expression.AddProfile(new ImmutableCollectionsProfile()))
        .CreateMapper();
    
    [Theory]
    [MemberData(nameof(Supports_converting_from_immutable_collection_Data))]
    public void Supports_converting_from_immutable_collection<TSourceCollection, TExpectedCollection>(
      TSourceCollection sourceCollection,
      TExpectedCollection expectedCollection
    )
      where TSourceCollection : IEnumerable
      where TExpectedCollection : IEnumerable
    {
      // Arrange
      var sut = CreateMapper();
      
      // Act
      var result = sut.Map<TExpectedCollection>(sourceCollection);
      
      // Assert
      result.Should().NotBeNull();
      result.Should().BeEquivalentTo(expectedCollection);
    }

    // ReSharper disable once InconsistentNaming
    public static IEnumerable<object[]> Supports_converting_from_immutable_collection_Data
    {
      get
      {
        var fixture = new Fixture();

        // Mapping from concrete class to concrete class
        yield return TestCase<ImmutableArray<int>, List<int>>(ImmutableArray.ToImmutableArray);
        yield return TestCase<ImmutableDictionary<int, int>, Dictionary<int, int>>(ImmutableDictionary.ToImmutableDictionary);
        yield return TestCase<ImmutableHashSet<int>, List<int>>(ImmutableHashSet.ToImmutableHashSet);
        yield return TestCase<ImmutableList<int>, List<int>>(ImmutableList.ToImmutableList);
        yield return TestCase<ImmutableSortedDictionary<int, int>, Dictionary<int, int>>(ImmutableSortedDictionary.ToImmutableSortedDictionary);
        yield return TestCase<ImmutableSortedSet<int>, List<int>>(ImmutableSortedSet.ToImmutableSortedSet);
        yield return TestCase<ImmutableStack<int>, List<int>>(ImmutableStack.CreateRange);
        yield return TestCase<ImmutableQueue<int>, List<int>>(ImmutableQueue.CreateRange);

        // Mapping from interface to concrete class
        yield return TestCase<IImmutableList<int>, List<int>>(ImmutableList.ToImmutableList);
        yield return TestCase<IImmutableQueue<int>, List<int>>(ImmutableQueue.CreateRange);
        yield return TestCase<IImmutableStack<int>, List<int>>(ImmutableStack.CreateRange);
        yield return TestCase<IImmutableSet<int>, List<int>>(ImmutableHashSet.ToImmutableHashSet);
        yield return TestCase<IImmutableDictionary<int, int>, Dictionary<int, int>>(ImmutableDictionary.ToImmutableDictionary);
        
        // Interfaces (System.Collections.Generic)
        yield return TestCase<ImmutableArray<int>, ICollection<int>>(ImmutableArray.ToImmutableArray);
        yield return TestCase<ImmutableArray<int>, IList<int>>(ImmutableArray.ToImmutableArray);
        yield return TestCase<ImmutableArray<int>, IReadOnlyCollection<int>>(ImmutableArray.ToImmutableArray);
        yield return TestCase<ImmutableArray<int>, IReadOnlyList<int>>(ImmutableArray.ToImmutableArray);
        yield return TestCase<ImmutableArray<int>, ISet<int>>(ImmutableArray.ToImmutableArray);
        
        yield return TestCase<ImmutableDictionary<int, int>, IDictionary<int, int>>(ImmutableDictionary.ToImmutableDictionary);
        
        yield return TestCase<ImmutableHashSet<int>, ICollection<int>>(ImmutableHashSet.ToImmutableHashSet);
        yield return TestCase<ImmutableHashSet<int>, IList<int>>(ImmutableHashSet.ToImmutableHashSet);
        yield return TestCase<ImmutableHashSet<int>, IReadOnlyCollection<int>>(ImmutableHashSet.ToImmutableHashSet);
        yield return TestCase<ImmutableHashSet<int>, IReadOnlyList<int>>(ImmutableHashSet.ToImmutableHashSet);
        yield return TestCase<ImmutableHashSet<int>, ISet<int>>(ImmutableHashSet.ToImmutableHashSet);
        
        yield return TestCase<ImmutableList<int>, ICollection<int>>(ImmutableList.ToImmutableList);
        yield return TestCase<ImmutableList<int>, IList<int>>(ImmutableList.ToImmutableList);
        yield return TestCase<ImmutableList<int>, IReadOnlyCollection<int>>(ImmutableList.ToImmutableList);
        yield return TestCase<ImmutableList<int>, IReadOnlyList<int>>(ImmutableList.ToImmutableList);
        yield return TestCase<ImmutableList<int>, ISet<int>>(ImmutableList.ToImmutableList);

        yield return TestCase<ImmutableSortedDictionary<int, int>, IDictionary<int, int>>(ImmutableSortedDictionary.ToImmutableSortedDictionary);
        
        yield return TestCase<ImmutableSortedSet<int>, ICollection<int>>(ImmutableSortedSet.ToImmutableSortedSet);
        yield return TestCase<ImmutableSortedSet<int>, IList<int>>(ImmutableSortedSet.ToImmutableSortedSet);
        yield return TestCase<ImmutableSortedSet<int>, IReadOnlyCollection<int>>(ImmutableSortedSet.ToImmutableSortedSet);
        yield return TestCase<ImmutableSortedSet<int>, IReadOnlyList<int>>(ImmutableSortedSet.ToImmutableSortedSet);
        yield return TestCase<ImmutableSortedSet<int>, ISet<int>>(ImmutableSortedSet.ToImmutableSortedSet);
        
        yield return TestCase<ImmutableStack<int>, ICollection<int>>(ImmutableStack.CreateRange);
        yield return TestCase<ImmutableStack<int>, IList<int>>(ImmutableStack.CreateRange);
        yield return TestCase<ImmutableStack<int>, IReadOnlyCollection<int>>(ImmutableStack.CreateRange);
        yield return TestCase<ImmutableStack<int>, IReadOnlyList<int>>(ImmutableStack.CreateRange);
        yield return TestCase<ImmutableStack<int>, ISet<int>>(ImmutableStack.CreateRange);
        
        yield return TestCase<ImmutableQueue<int>, ICollection<int>>(ImmutableQueue.CreateRange);
        yield return TestCase<ImmutableQueue<int>, IList<int>>(ImmutableQueue.CreateRange);
        yield return TestCase<ImmutableQueue<int>, IReadOnlyCollection<int>>(ImmutableQueue.CreateRange);
        yield return TestCase<ImmutableQueue<int>, IReadOnlyList<int>>(ImmutableQueue.CreateRange);
        yield return TestCase<ImmutableQueue<int>, ISet<int>>(ImmutableQueue.CreateRange);
        
        //yield return TestCase<ImmutableList<int>, IEnumerable<int>>(ImmutableList.ToImmutableList);
        yield return TestCase<ImmutableQueue<int>, IReadOnlyCollection<int>>(ImmutableQueue.CreateRange);
        
        yield return TestCase<ImmutableStack<int>, List<int>>(ImmutableStack.CreateRange);
        yield return TestCase<ImmutableHashSet<int>, List<int>>(ImmutableHashSet.ToImmutableHashSet);
        yield return TestCase<ImmutableSortedSet<int>, List<int>>(ImmutableSortedSet.ToImmutableSortedSet);
        yield return TestCase<ImmutableArray<int>, List<int>>(ImmutableArray.ToImmutableArray);
        yield return TestCase<ImmutableDictionary<int, int>, IDictionary<int, int>>(ImmutableDictionary.ToImmutableDictionary);
        yield return TestCase<ImmutableSortedDictionary<int, int>, IDictionary<int, int>>(ImmutableSortedDictionary.ToImmutableSortedDictionary);
        
        // Interfaces (System.Collections)
        //yield return TestCase<IEnumerable, ImmutableList<int>>(ImmutableList.ToImmutableList);

        object[] TestCase<TSourceCollection, TCollection>(
          Func<TCollection, TSourceCollection> creator
        )
        {
          var source = fixture.Create<TCollection>();

          return new object[]
          {
            creator(source),
            source
          };
        }
      }
    }
  }
}