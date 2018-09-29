using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace BanBrick.TypeScript.CodeGenerator.Helpers
{
    public class AssemblyHelper
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
