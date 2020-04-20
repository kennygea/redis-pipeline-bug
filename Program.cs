using StackExchange.Redis;
using System;
using System.Linq;
using System.Threading.Tasks;
namespace RedisTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var multiplexor = ConnectionMultiplexer.Connect("localhost");
            var db = multiplexor.GetDatabase();
            var random = new Random();
            long i = 0;
            int previousVal = -1;
            while (true)
            {
                int val = random.Next();
                var a = db.HashSetAsync("foo", new[] { new HashEntry("a", val) });
                var b = db.HashGetAllAsync("foo");
                await Task.WhenAll(a, b);
                var foo = await b;
                var test = foo.ToDictionary(i => i.Name, i => i.Value);
                if (test["a"] != val)
                    throw new Exception("wrong val " + val);
                if (++i % 100_000 == 0)
                    Console.WriteLine($"{i:N0} successful iterations");
                previousVal = val;
            }
        }
    }
}