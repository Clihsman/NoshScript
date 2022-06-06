using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoshScript.Nosh
{
    public class NoshException : Exception
    {
        private int line;
        private string msg;

        public NoshException(string msg,int line) : base(msg)
        {
            this.msg = msg;
            this.line = line;
        }

        public string GetMsg() {
            return msg;
        }

        public int GetLine() {
            return line;
        }
    }
}
