using BanBrick.TypeScript.CodeGenerator.Enums;
using BanBrick.TypeScript.CodeGenerator.Resolvers;
using BanBrick.TypeScript.CodeGenerator.UnitTest.TestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace BanBrick.TypeScript.CodeGenerator.UnitTest.Generators
{
    public class TypeDefinitionGeneratorTest
    {
        [Fact]
        public void Get_Should_ReturnAllTypes()
        {
            var resolver = new RelationResolver();
            var types = new List<Type>()
            {
                typeof(TestModel1),
                typeof(TestModel2),
                typeof(ITestInterface1)
            }; 
            var processedTypes = resolver.Resolve(types);
            var duplicatedTypes = processedTypes.Select(x => x.Type).ToList().OrderBy(x => x.Name);
            var disitinctTypes = duplicatedTypes.Distinct();

            Assert.Equal(disitinctTypes.Count(), processedTypes.Count);
            Assert.Equal(2, processedTypes.Where(x => x.ProcessingCategory == ProcessingCategory.Enum).Count());
            Assert.Equal(5, processedTypes.Where(x => x.ProcessingCategory == ProcessingCategory.Object).Count());
            Assert.Equal(2, processedTypes.Where(x => x.ProcessingCategory == ProcessingCategory.Interface).Count());
        }
    }
}
