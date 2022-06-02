using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace WebApplication1.DLLs
{
    public class ADDdll
    {
        [DllImport("Dll1.dll", EntryPoint = "myAdd", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int myAdd(int a, int b);

        
    }
}
