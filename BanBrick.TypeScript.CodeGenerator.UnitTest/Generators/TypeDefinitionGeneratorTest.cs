﻿using BanBrick.TypeScript.CodeGenerator.Enums;
using BanBrick.TypeScript.CodeGenerator.Generators;
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
            var generator = new TypeDefinitionGenerator();
            var types = new List<Type>()
            {
                typeof(TestModel1),
                typeof(TestModel2),
                typeof(ITestInterface1)
            }; 
            var processedTypes = generator.Generate(types);

            Assert.Equal(9, processedTypes.Count);
            Assert.Equal(2, processedTypes.Where(x => x.Category == ProcessingCategory.Enum).Count());
            Assert.Equal(7, processedTypes.Where(x => x.Category == ProcessingCategory.Object).Count());
        }
    }
}