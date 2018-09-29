using BanBrick.TypeScript.CodeGenerator.Generators;
using BanBrick.TypeScript.CodeGenerator.Helpers;
using BanBrick.TypeScript.CodeGenerator.UnitTest.TestModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace BanBrick.TypeScript.CodeGenerator.UnitTest.Generators
{
    public class ValueCodeGeneratorTest
    {
        private readonly ValueCodeGenerator _generator;

        public ValueCodeGeneratorTest()
        {
            _generator = new ValueCodeGenerator();
        }

        [Fact]
        public void GetPrimitiveValueCode_ShouldReturnCorrect() {
            var instance = new TestPrimitiveModel()
            {
                Value1  = "Test value",
                Value2  = 10,
                Value3  = 20,
                Value4  = 30,
                Value5  = 40,
                Value6  = 50,
                Value7  = true,
                Value8  = null,
                Value9  = 70,
                Value10  = null
            };
            
            var value1 = _generator.GetPrimitiveValueCode(instance.Value1);
            Assert.Equal("'Test value'", value1);

            var value2 = _generator.GetPrimitiveValueCode(instance.Value2);
            Assert.Equal("10", value2);

            var value3 = _generator.GetPrimitiveValueCode(instance.Value3);
            Assert.Equal("20", value3);

            var value4 = _generator.GetPrimitiveValueCode(instance.Value4);
            Assert.Equal("30", value4);

            var value5 = _generator.GetPrimitiveValueCode(instance.Value5);
            Assert.Equal("40", value5);

            var value6 = _generator.GetPrimitiveValueCode(instance.Value6);
            Assert.Equal("50", value6);

            var value7 = _generator.GetPrimitiveValueCode(instance.Value7);
            Assert.Equal("true", value7);

            var value8 = _generator.GetPrimitiveValueCode(instance.Value8);
            Assert.Equal("null", value8);

            var value9 = _generator.GetPrimitiveValueCode(instance.Value9);
            Assert.Equal("70", value9);

            var value10 = _generator.GetPrimitiveValueCode(instance.Value10);
            Assert.Equal("null", value10);
        }
        

        [Fact]
        public void GenerateValueCode_ShouldReturnNumberArrayCode()
        {
            var numberList = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var numberListCode = _generator.GenerateValueCode(numberList.GetType(), numberList);

            Assert.Equal("[ 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 ]", numberListCode);
        }

        [Fact]
        public void GenerateValueCode_ShouldReturnNumberDictionaryCode()
        {
            var numberDisctionary = new Dictionary<int, string>() {
                { 1, "1" },
                { 2, "2" },
                { 3, "3" },
                { 4, "4" },
                { 5, "5" },
            };
            var numberListCode = _generator.GenerateValueCode(numberDisctionary.GetType(), numberDisctionary);
            Assert.Equal("{ 1: '1', 2: '2', 3: '3', 4: '4', 5: '5' }", numberListCode);
        }
    }
}
