using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Networking;

namespace Messages
{
    public class PointsTableMessage : MessageBase
    {
        public List<PointTableRow> Rows;

        public override void Serialize(NetworkWriter writer)
        {
            writer.Write(JsonConvert.SerializeObject(Rows));
        }

        public override void Deserialize(NetworkReader reader)
        {
            var str = reader.ReadString();
            Rows = JsonConvert.DeserializeObject<List<PointTableRow>>(str);
        }

        public override string ToString()
        {
            return string.Join("\n", Rows.Select(x => $"{x.PlayerName}: {x.Point}"));
        }
    }

    public class PointTableRow
    {
        public string PlayerName;
        public int Point;

        public PointTableRow(string name, int point)
        {
            PlayerName = name;
            Point = point;
        }
    }
}
