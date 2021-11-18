namespace SIM.SolidWorksPlugin.Tests
{
    using BenchmarkDotNet.Running;
    using SIM.SolidWorksPlugin.Tests.Extensions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    static class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<FilePathExtensions_Tests>();
        }
    }
}
