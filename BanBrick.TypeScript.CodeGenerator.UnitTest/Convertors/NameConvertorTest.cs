using BanBrick.TypeScript.CodeGenerator.Convertors;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace BanBrick.TypeScript.CodeGenerator.UnitTest.Convertors
{
    public class NameConvertorTest
    {
        private readonly NameConvertor _convertor;

        public NameConvertorTest()
        {
            _convertor = new NameConvertor();
        }

        [Fact]
        public void GetObjectTypeScriptName_ShouldReturnCorrect()
        {

        }

        [Fact]
        public void GetPrimitiveTypeScriptName_ShouldReturnCorrect()
        {

        }

        [Fact]
        public void GetDictionaryTypeScriptName_ShouldReturnCorrect()
        {

        }

        [Fact]
        public void GetCollectionTypeScriptName_ShouldReturnCorrect()
        {

        }
    }
}
