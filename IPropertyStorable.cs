using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tomflection
{
    public interface IPropertyStorable
    {
        Dictionary<string, object> PropertyStorage
        {
            get;
        }
    }
}
