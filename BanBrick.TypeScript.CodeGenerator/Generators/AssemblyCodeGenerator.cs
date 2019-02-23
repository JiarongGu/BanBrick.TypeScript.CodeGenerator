using System;
using System.Reflection;
using System.Text;

namespace BanBrick.TypeScript.CodeGenerator.Generators
{
    internal class AssemblyCodeGenerator
    {
        public string GetSectionSeparator(string sectionName)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine();
            stringBuilder.AppendLine($"// {sectionName} ---------------------------------------------");
            
            return stringBuilder.ToString();
        } 
        
        public string GetAssemblyContent()
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"// Code Generated on {DateTime.Now} using {Assembly.GetEntryAssembly().FullName}");
            stringBuilder.AppendLine("// Do not change this file manually");
            stringBuilder.AppendLine("// ---------------------------------------------");

            stringBuilder.AppendLine();

            return stringBuilder.ToString();
        }
    }
}
