using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Networking;

namespace Messages
{
    public class UserNameMessage : MessageBase
    {
        public string UserName;

        public override void Serialize(NetworkWriter writer)
        {
            writer.Write(UserName);
        }

        public override void Deserialize(NetworkReader reader)
        {
            UserName = reader.ReadString();
        }
    }
}
