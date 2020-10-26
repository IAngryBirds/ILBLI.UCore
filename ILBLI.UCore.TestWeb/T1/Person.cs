using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ILBLI.UCore.TestWeb.T1
{
    public class Person
    {
        public virtual string Name => "我是父类";

        public virtual void Doing()
        {
            Console.WriteLine(Name);
        }
    }
}
