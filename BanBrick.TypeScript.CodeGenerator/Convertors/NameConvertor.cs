using BanBrick.TypeScript.CodeGenerator.Annotations;
using BanBrick.TypeScript.CodeGenerator.Helpers;
using BanBrick.TypeScript.CodeGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BanBrick.TypeScript.CodeGenerator.Convertors
{
    internal interface INameConvertor {
        string GetName(Type type);
    }

    internal sealed class NameConvertor: INameConvertor
    {
        private readonly IDictionary<Type, string> _nameDictionary;

        public NameConvertor(IEnumerable<TypeDefinition> typeDefinitions)
        {
            _nameDictionary = typeDefinitions.ToDictionary(x => x.Type, x => x.Name);
        }

        public string GetName(Type type)
        {
            return _nameDictionary[type];
        }
    }
}
