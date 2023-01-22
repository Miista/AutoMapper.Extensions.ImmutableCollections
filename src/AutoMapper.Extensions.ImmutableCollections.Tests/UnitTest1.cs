using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using AutoFixture;
using FluentAssertions;
using Xunit;

namespace AutoMapper.Extensions.ImmutableCollections.Tests
{
  public class UnitTest1
  {
    [Theory]
    [MemberData(nameof(Test1_Data))]
    public void Test1<TSourceCollection, TExpectedCollection>(
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
    public static IEnumerable<object[]> Test1_Data
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
  }
}