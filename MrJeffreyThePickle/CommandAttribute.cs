using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MrJeffreyThePickle
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    internal class CommandAttribute(string commandName) : Attribute
    {
        public string _CommandName { get; } = commandName;
    }
}
