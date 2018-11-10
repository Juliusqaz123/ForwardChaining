using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForwardChaining
{
    class Rule
    {
        public readonly string name;
        public readonly List<Fact> requirements;
        public readonly Fact result;
        
        static int nameCounter = 1;
        public enum Flag { flag0, flag1, flag2}
        public Flag flag;

        public Rule(List<Fact> _requirements, Fact _result)
        {
            name = "R" + nameCounter;
            nameCounter++;
            requirements = _requirements;
            result = _result;
            flag = Flag.flag0;
        }
    }
}
