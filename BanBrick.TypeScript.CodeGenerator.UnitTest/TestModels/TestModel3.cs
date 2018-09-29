using System;
using System.Collections.Generic;
using System.Text;

namespace BanBrick.TypeScript.CodeGenerator.UnitTest.TestModels
{
    public class TestModel3
    {
        public ITestInterface1 Value1 { get; set; }
        public Dictionary<TestEnum2, ITestInterface2> Value2 { get; set; }
    }
}
