using BanBrick.TypeScript.CodeGenerator.Enums;
using BanBrick.TypeScript.CodeGenerator.Helpers;
using BanBrick.TypeScript.CodeGenerator.UnitTest.TestModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xunit;

namespace BanBrick.TypeScript.CodeGenerator.UnitTest.Helpers
{
    public class TypeHelperTest
    {
        private readonly TypeHelper _typeHelper;

        public TypeHelperTest() {
            _typeHelper = new TypeHelper();
        }
        
        [Fact]
        public void IsPrimitiveType_Should_ReturnTrue()
        {
            foreach (var prop in typeof(TestPrimitiveModel).GetProperties())
            {
                Assert.True(_typeHelper.IsPrimitiveType(prop.PropertyType), prop.Name);
            }
        }

        [Fact]
        public void IsPrimitiveType_Should_ReturnFalse()
        {
            foreach (var prop in typeof(TestCollectionModel).GetProperties())
            {
                Assert.False(_typeHelper.IsPrimitiveType(prop.PropertyType), prop.Name);
            }

            foreach (var prop in typeof(TestDictionaryModel).GetProperties())
            {
                Assert.False(_typeHelper.IsPrimitiveType(prop.PropertyType), prop.Name);
            }
        }

        [Fact]
        public void IsDictionaryType_Should_ReturnTrue()
        {
            foreach (var prop in typeof(TestDictionaryModel).GetProperties())
            {
                Assert.True(_typeHelper.IsDictionaryType(prop.PropertyType), prop.Name);
            }
        }

        [Fact]
        public void IsDictionaryType_Should_ReturnFalse()
        {
            foreach (var prop in typeof(TestPrimitiveModel).GetProperties())
            {
                Assert.False(_typeHelper.IsDictionaryType(prop.PropertyType), prop.Name);
            }

            foreach (var prop in typeof(TestCollectionModel).GetProperties())
            {
                Assert.False(_typeHelper.IsDictionaryType(prop.PropertyType), prop.Name);
            }
        }

        [Fact]
        public void IsCollectionType_Should_ReturnTrue()
        {
            foreach (var prop in typeof(TestCollectionModel).GetProperties())
            {
                Assert.True(_typeHelper.IsCollectionType(prop.PropertyType), prop.Name);
            }
        }

        [Fact]
        public void IsCollectionType_Should_ReturnFalse()
        {
            foreach (var prop in typeof(TestPrimitiveModel).GetProperties())
            {
                Assert.False(_typeHelper.IsCollectionType(prop.PropertyType), prop.Name);
            }

            foreach (var prop in typeof(TestDictionaryModel).GetProperties())
            {
                Assert.False(_typeHelper.IsCollectionType(prop.PropertyType), prop.Name);
            }
        }

        [Fact]
        public void GetTypeCategory_Should_ReturnObject()
        {
            Assert.Equal(ProcessingCategory.Object, _typeHelper.GetProcessingCategory(typeof(TestPrimitiveModel)));
            Assert.Equal(ProcessingCategory.Object, _typeHelper.GetProcessingCategory(typeof(TestCollectionModel)));
            Assert.Equal(ProcessingCategory.Object, _typeHelper.GetProcessingCategory(typeof(TestDictionaryModel)));
        }
    }
}
