using BanBrick.TypeScript.CodeGenerator.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BanBrick.TypeScript.CodeGenerator.Models
{
    public class ProcessConfig
    {
        public OutputType OutputType { get; set; }

        public string Name { get; set; }

        public bool Inherit { get; set; }
    }
}
