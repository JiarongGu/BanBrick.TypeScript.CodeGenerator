using BanBrick.TypeScript.CodeGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BanBrick.TypeScript.CodeGenerator.Convertors
{
    public interface INameConvertor {
        string GetName(Type type);
    }

    internal sealed class NameConvertor: INameConvertor
    {
        private readonly IDictionary<Type, string> _nameDictionary;

        public NameConvertor(IEnumerable<TypeDefinition> typeDefinitions)
        {
            _nameDictionary = typeDefinitions.ToDictionary(x => x.Type, x => x.ProcessConfig.Name);
        }

        public string GetName(Type type)
        {
            return _nameDictionary[type];
        }
    }
}
