using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace StructureGo
{
    /// <summary>
    /// 程序选项
    /// </summary>
    public class ProgramOption
    {
        /// <summary>
        /// 要分析的程序集路径
        /// </summary>
        public string AssemblyPath { get; set; } = Path.Combine(Directory.GetCurrentDirectory(), $"{nameof(StructureGo)}.dll");

        /// <summary>
        /// 要分析的命名空间名称
        /// </summary>
        public string Namespace { get; set; } = nameof(StructureGo);

        /// <summary>
        /// 图片输出格式
        /// </summary>
        public string OutputFormat { get; set; } = "svg";

        public ProgramOption(string[] args)
        {
            foreach (var arg in args)
            {
                if (arg.StartsWith("-P"))
                {
                    AssemblyPath = arg.Substring(2);
                }

                if (arg.StartsWith("-N"))
                {
                    Namespace = arg.Substring(2);
                }

                if (arg.StartsWith("-T"))
                {
                    OutputFormat = arg.Substring(2);
                }
            }
        }
    }
}
