using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ILBLI.UCore.TestWeb.T1
{
    public class Son :Person
    {
        public override string Name => "这是子类重写的名字";
        public override void Doing()
        {
            base.Doing();
        }
    }
}
