using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisDemo
{
    static class Program
    {
        static void Main(string[] args)
        {
            var redisHelper = new RedisHelper("127.0.0.1:6379");
            //redisHelper.SetStringValue("A", "123");
            var result = redisHelper.GetStringValue("A");
            Console.WriteLine(result);

            Console.ReadKey();
        }
    }
}
