using Crowdfund.Core.Services;
using System;

namespace Crowdfund
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            IProjectService lol = new ProjectService();
        }
    }
}
