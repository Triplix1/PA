using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_4
{
    public class Member
    {
        public bool[] Things;
        public Member(bool[] th)
        {
            Things = th;
        }
        public bool this[int index]
        {
            get { return Things[index]; }
            set { Things[index] = value; }
        }
    }
}
