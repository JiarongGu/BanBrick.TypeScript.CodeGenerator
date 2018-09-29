using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace BanBrick.TypeScript.CodeGenerator.UnitTest.TestModels
{
    public class TestModel1
    {
        public TestModel2 Value1 { get; set; }
        public ICollection<TestEnum2> Value2 { get; set; }
        public List<TestModel2> Value3 { get; set; }
        public IList<TestModel3> Value4 { get; set; }
        public IDictionary<string, TestModel4> Value5 { get; set; }
        public Dictionary<TestEnum1, TestModel5> Value6 { get; set; }
        public ITestInterface2 value7 { get; set; }
    }
}
