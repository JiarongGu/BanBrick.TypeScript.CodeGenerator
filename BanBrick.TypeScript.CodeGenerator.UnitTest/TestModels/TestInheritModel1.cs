using BanBrick.TypeScript.CodeGenerator.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace BanBrick.TypeScript.CodeGenerator.UnitTest.TestModels
{
    [TypeScriptObject(Type = TypeScriptObjectType.Interface, Inherit = true)]
    public class TestInheritModel1
    {
        public TestModel1 TestModel1 { get; set; }
        public TestModel2 TestModel2 { get; set; }
    }
}
