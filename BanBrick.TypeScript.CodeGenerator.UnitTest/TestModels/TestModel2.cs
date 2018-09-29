using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace BanBrick.TypeScript.CodeGenerator.UnitTest.TestModels
{
    public class TestModel2
    {
        public IEnumerable<TestModel3> Value1 { get; set; }
        public ConcurrentQueue<TestEnum1> Value2 { get; set; }
        public Queue<ITestInterface1> Value3 { get; set; }
        public IReadOnlyCollection<TestModel3> Value4 { get; set; }
    }
}
