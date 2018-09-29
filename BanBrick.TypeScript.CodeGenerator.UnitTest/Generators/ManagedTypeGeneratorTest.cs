using BanBrick.TypeScript.CodeGenerator.Enums;
using BanBrick.TypeScript.CodeGenerator.Generators;
using BanBrick.TypeScript.CodeGenerator.UnitTest.TestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace BanBrick.TypeScript.CodeGenerator.UnitTest.Generators
{
    public class ManagedTypeGeneratorTest
    {
        [Fact]
        public void Get_Should_ReturnAllTypes()
        {
            var convertor = new ManagedTypeGenerator();
            var types = new List<Type>()
            {
                typeof(TestModel1),
                typeof(TestModel2),
                typeof(ITestInterface1)
            }; 
            var processedTypes = convertor.Generate(types);

            Assert.Equal(9, processedTypes.Count);
            Assert.Equal(2, processedTypes.Where(x => x.Category == TypeCategory.Enum).Count());
            Assert.Equal(7, processedTypes.Where(x => x.Category == TypeCategory.Object).Count());
        }
    }
}
