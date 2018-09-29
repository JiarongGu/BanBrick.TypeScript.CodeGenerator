using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace BanBrick.TypeScript.CodeGenerator.UnitTest.TestModels
{
    public class TestCollectionModel
    {
        public Collection<string> Value1 { get; set; }
        public ICollection<string> Value2 { get; set; }
        public List<string> Value3 { get; set; }
        public IList<string> Value4 { get; set; }
        public IEnumerable<string> Value5 { get; set; }
        public ConcurrentQueue<string> Value6 { get; set; }
        public Queue<string> Value7 { get; set; }
        public IReadOnlyCollection<string> Value8 { get; set; }
        public string[] Value9 { get; set; }
    }
}
