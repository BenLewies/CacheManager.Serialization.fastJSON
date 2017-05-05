# CacheManager
CacheManager is an open source caching abstraction layer for .NET written in C#. It supports various cache providers and implements many advanced features.

The main goal of the CacheManager package is to make developer's life easier to handle even very complex caching scenarios.  
With CacheManager it is possible to implement multiple layers of caching, e.g. in-process caching in front of a distributed cache, in just a few lines of code.

CacheManager is not just an interface to unify the programming model for various cache providers, which will make it very easy to change the caching strategy later on in a project. It also offers additional features, like cache synchronization, concurrent updates, serialization, events, performance counters... 
The developer can opt-in to those features only if needed.

# CacheManager.Serialization.fastJSON
[CacheManager ](https://github.com/MichaCo/CacheManager)serializer for [fastJSON](https://github.com/mgholam/fastJSON)
### Install Package
```


Install-Package CacheManager.Serialization.fastJSON
```
### Code Examples
``` csharp
using System;
using System.Collections.Generic;
using CacheManager.Core;
using CacheManager.Serialization.fastJSON;

namespace fastJSON.Samples
{

    public class MyClass
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            var cfg = SetUp();
            var simpleCache = CacheFactory.FromConfiguration<int>("cache1", cfg);
            simpleCache.Add("test", 8999);
            Console.WriteLine(simpleCache.Get("test"));

            var objCache = CacheFactory.FromConfiguration<List<MyClass>>("cache2", cfg);
            objCache.Add("test1", new List<MyClass> {
                new MyClass {
                Id=1,
                Name="vigoss",
                CreatedDate=DateTime.Now
                },
                new MyClass {
                Id=2,
                Name="loda",
                CreatedDate=DateTime.Now
                }  ,
                new MyClass {
                Id=3,
                Name="xxxtes",
                CreatedDate=DateTime.Now
                }
            });
            var arr = objCache.Get("test1");
            arr.ForEach(p => Console.WriteLine($"{p.Id}-{p.Name}"));

            Console.ReadKey();
        }
        static CacheManagerConfiguration SetUp()
        {
            var cfg = ConfigurationBuilder.BuildConfiguration(settings =>
            {
                settings.WithUpdateMode(CacheUpdateMode.Up)
                        .WithRedisConfiguration("redis", config =>
                        {
                            config.WithAllowAdmin()
                            .WithDatabase(0)
                            .WithEndpoint("localhost", 6379);
                        })
                        .WithMaxRetries(100)
                        .WithRetryTimeout(50)
                        //.WithfastJSONSerializer(new JSONParameters
                        //{
                        //  UseEscapedUnicode = false,
                        //  UseExtensions = false,
                        //  EnableAnonymousTypes = true
                        //})
                        .WithGzJsonSerializer(new JSONParameters
                        {
                            UseEscapedUnicode = false,
                            UseExtensions = false,
                            EnableAnonymousTypes = true
                        })
                        .WithRedisBackplane("redis")
                        .WithRedisCacheHandle("redis", true);
            });
            return cfg;
        }
    }

}

```
