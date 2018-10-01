using BanBrick.TypeScript.CodeGenerator.Generators;
using BanBrick.TypeScript.CodeGenerator.UnitTest.TestModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace BanBrick.TypeScript.CodeGenerator.UnitTest.Generators
{
    public class ClassCodeGeneratorTest
    {
        private readonly ClassCodeGenerator _generator;

        public ClassCodeGeneratorTest()
        {
            //_generator = new ClassCodeGenerator();
        }

        [Fact]
        public void Generate_PrimitiveModel_ShouldReturnCorrect() {
            //var code = _generator.Generate(typeof(TestPrimitiveModel));
            //var expectedCode = "export class TestPrimitiveModel {\r\n  constructor(\r\n  public value1?: string,\r\n  public value2: number = 0,\r\n  public value3: number = 0,\r\n  public value4: number = 0,\r\n  public value5: number = 0,\r\n  public value6: number = 0,\r\n  public value7?: boolean,\r\n  public value8?: number,\r\n  public value9?: number,\r\n  public value10?: number,\r\n  public value11: string,\r\n  public value12?: string,\r\n  ) { }\r\n}\r\n";
            //Assert.Equal(expectedCode, code);
        }

        [Fact]
        public void Generate_ObjectModelWithDictionary_ShouldReturnCorrect() {
            //var code = _generator.Generate(typeof(TestDictionaryModel));
            //var expectedCode = "export class TestDictionaryModel {\r\n  constructor(\r\n  public value1: { [key: string] : string } = { 'value1': 'test_value1' },\r\n  public value2: { [key: string] : string } = {},\r\n  public value3: { [key: string] : string } = {},\r\n  public value4: { [key: string] : string } = {},\r\n  public value5: { [key: string] : string } = {},\r\n  ) { }\r\n}\r\n";
            //Assert.Equal(expectedCode, code);
        }
    }
}
