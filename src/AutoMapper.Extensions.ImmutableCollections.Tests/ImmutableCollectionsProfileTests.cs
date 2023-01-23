using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using AutoFixture;
using FluentAssertions;
using Xunit;

namespace AutoMapper.Extensions.ImmutableCollections.Tests
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
      result.Should().BeEquivalentTo(expectedCollection);
    }

    // ReSharper disable once InconsistentNaming
    public static IEnumerable<object[]> Supports_converting_to_immutable_collection_Data
    {
      get
      {
        var fixture = new Fixture();

        yield return TestCase<List<int>, ImmutableList<int>>(ImmutableList.ToImmutableList);
        yield return TestCase<List<int>, ImmutableQueue<int>>(ImmutableQueue.CreateRange);
        yield return TestCase<List<int>, ImmutableStack<int>>(ImmutableStack.CreateRange);
        yield return TestCase<List<int>, ImmutableHashSet<int>>(ImmutableHashSet.ToImmutableHashSet);
        yield return TestCase<List<int>, ImmutableSortedSet<int>>(ImmutableSortedSet.ToImmutableSortedSet);
        yield return TestCase<List<int>, ImmutableArray<int>>(ImmutableArray.ToImmutableArray);
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
      result.Should().BeEquivalentTo(expectedCollection);
    }
    
    // ReSharper disable once InconsistentNaming
    public static IEnumerable<object[]> Supports_converting_from_immutable_collection_Data
    {
      get
      {
        var fixture = new Fixture();

        yield return TestCase<List<int>, ImmutableList<int>>(ImmutableList.ToImmutableList);
        yield return TestCase<List<int>, ImmutableQueue<int>>(ImmutableQueue.CreateRange);
        yield return TestCase<List<int>, ImmutableStack<int>>(ImmutableStack.CreateRange);
        yield return TestCase<List<int>, ImmutableHashSet<int>>(ImmutableHashSet.ToImmutableHashSet);
        yield return TestCase<List<int>, ImmutableSortedSet<int>>(ImmutableSortedSet.ToImmutableSortedSet);
        yield return TestCase<List<int>, ImmutableArray<int>>(ImmutableArray.ToImmutableArray);
        yield return TestCase<Dictionary<int, int>, ImmutableDictionary<int, int>>(ImmutableDictionary.ToImmutableDictionary);
        yield return TestCase<Dictionary<int, int>, ImmutableSortedDictionary<int, int>>(ImmutableSortedDictionary.ToImmutableSortedDictionary);

        object[] TestCase<TSourceCollection, TCollection>(
          Func<TSourceCollection, TCollection> creator
        )
        {
          var source = fixture.Create<TSourceCollection>();

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