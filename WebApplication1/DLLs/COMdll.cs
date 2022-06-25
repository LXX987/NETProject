using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;


namespace WebApplication1.DLLs
{
    public class COMdll
    {
        public void comdll()
        {
            // 7a92c0b6-615a-489e-ad23-192b14a099c5
            Guid clsid = new Guid("b5145993-2d74-452e-b1a5-8b460952b4e2");
            Type comType = Type.GetTypeFromCLSID(clsid);
            Object comObject = Activator.CreateInstance(comType);
            Object[] paras = { 4 };
            Object result = comType.InvokeMember("Number", System.Reflection.BindingFlags.InvokeMethod, null, comObject, paras);
            Console.WriteLine(result);
        }

        public void comOrderdll(int order_id)
        {
            // 7a92c0b6-615a-489e-ad23-192b14a099c5
            Guid clsid = new Guid("b5145993-2d74-452e-b1a5-8b460952b4e2");
            Type comType = Type.GetTypeFromCLSID(clsid);
            Object comObject = Activator.CreateInstance(comType);
            Object[] paras = { "购买成功，订单完成！订单号：" + order_id.ToString() };
            Object result = comType.InvokeMember("Order", System.Reflection.BindingFlags.InvokeMethod, null, comObject, paras);
            Console.WriteLine(result);
        }

    }
}
