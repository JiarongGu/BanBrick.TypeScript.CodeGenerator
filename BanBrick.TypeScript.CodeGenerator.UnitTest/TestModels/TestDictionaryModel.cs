using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace BanBrick.TypeScript.CodeGenerator.UnitTest.TestModels
{
    public class TestDictionaryModel
    {
        public Dictionary<string, string> Value1 => new Dictionary<string, string>()
        {
            { "value1", "test_value1" }
        };

        public IDictionary<string, string> Value2 { get; set; }
        public Dictionary<string, string> Value3 { get; set; }
        public ConcurrentDictionary<string, string> Value4 { get; set; }
        public IReadOnlyDictionary<string, string> Value5 { get; set; }
    }
}
