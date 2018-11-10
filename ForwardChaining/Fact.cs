using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForwardChaining
{
    class Fact
    {
        public string name;
        
        public Fact (string _name)
        {
            name = _name;
        }

        public  override bool Equals(Object obj)
        {
            var fact = obj as Fact;
            return fact.name == name;
        }

        public override int GetHashCode()
        {
            return name.GetHashCode();
        }

    }
}
