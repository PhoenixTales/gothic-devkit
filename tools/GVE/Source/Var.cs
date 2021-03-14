using System;
using System.Collections.Generic;
using System.Text;

namespace GVE
{
   
    public class Var :IComparable
    {
        public string name;
        public int value;
        public bool changed;
        public int pos;
        public Var(string name, int value, int pos)
        {
            
            this.name = name;
            this.value = value;
            this.pos = pos;
            this.changed = false;
           
        }
        int IComparable.CompareTo(object obj)
        {
            Var p = obj as Var;

            return string.Compare(name, p.name);
        }
        public override string ToString()
        {
            return this.name;
        }

       
    }
}
