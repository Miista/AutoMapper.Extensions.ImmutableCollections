using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace AutoMapper.Extensions.ImmutableCollections.Sandbox
{
  class Program
  {
    public static void Main(string[] args)
    {
      var source = new List<int> { 1, 2, 3 };
      var mapper = new MapperConfiguration(configuration =>
      {
        configuration.AddProfile<ImmutableCollectionsProfile>();
      }).CreateMapper();

      var dict = new Dictionary<string, int> { { "1", 1 } }.AsEnumerable();
      var immutableImmutableDictionary = mapper.Map<ImmutableDictionary<string, int>>(dict);
      var immutableArray = ImmutableArray<string>.Empty.Add("Hello, World!");
      var array = mapper.Map<ImmutableArray<string>>(immutableArray);
      var list = mapper.Map<ImmutableList<string>>(array);
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