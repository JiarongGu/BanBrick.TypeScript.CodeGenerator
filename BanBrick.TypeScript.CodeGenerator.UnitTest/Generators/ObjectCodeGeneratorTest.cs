using BanBrick.TypeScript.CodeGenerator.Generators;
using BanBrick.TypeScript.CodeGenerator.UnitTest.TestModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace BanBrick.TypeScript.CodeGenerator.UnitTest.Generators
{
    public class ObjectCodeGeneratorTest
    {
        private readonly ObjectCodeGenerator _generator;

        public ObjectCodeGeneratorTest()
        {
            _generator = new ObjectCodeGenerator();
        }

        [Fact]
        public void Generate_PrimitiveModel_ShouldReturnCorrect() {
            var code = _generator.Generate(typeof(TestPrimitiveModel));
            var expectedCode = "export class TestPrimitiveModel {\r\n  constructor(\r\n  public value1?: String,\r\n  public value2: Number = 0,\r\n  public value3: Number = 0,\r\n  public value4: Number = 0,\r\n  public value5: Number = 0,\r\n  public value6: Number = 0,\r\n  public value7?: Boolean,\r\n  public value8?: Number,\r\n  public value9?: Number,\r\n  public value10?: Number,\r\n  public value11: String,\r\n  public value12?: String,\r\n  ) { }\r\n}\r\n";
            Assert.Equal(expectedCode, code);
        }
    }
}
