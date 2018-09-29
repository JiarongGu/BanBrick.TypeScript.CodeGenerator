using BanBrick.TypeScript.CodeGenerator.Generators;
using BanBrick.TypeScript.CodeGenerator.UnitTest.TestModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace BanBrick.TypeScript.CodeGenerator.UnitTest.Generators
{
    public class EnumCodeGeneratorTest
    {
        private readonly EnumCodeGenerator _generator;

        public EnumCodeGeneratorTest() {
            _generator = new EnumCodeGenerator();
        }

        [Fact]
        public void GeneratorEnumCode_ReturnCorrectly() {
            var generatedCode = _generator.Generate(typeof(TestEnum1));
            var sampleCode = "export enum TestEnum1 {\r\n  NSW = 0,\r\n  ACT = 1,\r\n  VIC = 2\r\n}\r\nexport const TestEnum1Array = [\r\n  'NSW',\r\n  'ACT',\r\n  'VIC',\r\n];\r\n";

            Assert.Equal(sampleCode, generatedCode);
        }

        [Fact]
        public void GeneratorEnumCode_ReturnCorrectlyWithTypeScriptName()
        {
            var generatedCode = _generator.Generate(typeof(TestEnum2));
            var sampleCode = "export enum TestObject {\r\n  NSW = 0,\r\n  ACT = 1,\r\n  VIC = 2\r\n}\r\nexport const TestObjectArray = [\r\n  'NSW',\r\n  'ACT',\r\n  'VIC',\r\n];\r\n";

            Assert.Equal(sampleCode, generatedCode);
        }
    }
}
