using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RemoteControl.Protocals;

namespace RemoteControl.Client.Handlers
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    class PacketTypeAttribute : Attribute
    {
        public ePacketType Type { get; set; }
        public PacketTypeAttribute(ePacketType type)
        {
            this.Type = type;
        }
    }
}
