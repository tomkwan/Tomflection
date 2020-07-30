using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tomflection
{
    public class TypeN_Attr<AT>
    {
        public AT attr;
        public Type t;
        public TypeN_Attr(Type t, AT attr)
        {
            this.attr = attr;
            this.t = t;
        }
    }
}
