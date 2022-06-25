using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace WebApplication1.DLLs
{
    public class ExamineCli
    {
        [DllImport("CliDLL.dll", EntryPoint = "ExamineMailCli", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern bool ExamineMailCli(string email);
    }
}
