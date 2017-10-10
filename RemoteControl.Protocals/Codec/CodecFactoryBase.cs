using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace RemoteControl.Protocals.Codec
{
    public class CodecFactoryBase
    {
        // 包长(4个字节) 包类型(1个字节) 数据部分(包长-4-1个字节)

        private Dictionary<ePacketType, Type> _mappings = null;

        public CodecFactoryBase(Dictionary<ePacketType, Type> mappings)
        {
            _mappings = mappings;
        }

        public byte[] EncodeOject(ePacketType packetType, object obj)
        {
            byte[] bodyBytes = null;

            if (obj != null)
            {
                bodyBytes = ToJsonBytes(obj);
            }

            return Encode(packetType, bodyBytes);
        }

        public void DecodeObject(byte[] packetData, out ePacketType packetType, out object obj)
        {
            byte[] bodyData = null;
            Decode(packetData, out packetType, out bodyData);

            obj = null;
            if (_mappings.ContainsKey(packetType))
            {
                obj = FromJsonBytes(bodyData, _mappings[packetType]);
            }
            else
            {
                obj = bodyData;
            }
        }

        private byte[] Encode(ePacketType packetType, byte[] bodyData)
        {
            List<byte> result = new List<byte>();

            if(bodyData == null)
            {
                bodyData = new byte[0];
            }
            int packetLength = bodyData.Length + 1 + 4;
            result.AddRange(BitConverter.GetBytes(packetLength));
            result.Add((byte)packetType);
            result.AddRange(bodyData);

            return result.ToArray();
        }

        private void Decode(byte[] packetData, out ePacketType packetType, out byte[] bodyData)
        {
            int packetLength = BitConverter.ToInt32(packetData, 0);
            packetType = (ePacketType)packetData[4];
            bodyData = new byte[packetLength - 4 - 1];
            for (int i = 0; i < bodyData.Length; i++)
            {
                bodyData[i] = packetData[i + 5];
            }
        }

        private byte[] ToJsonBytes(object obj)
        {
            string json = JsonConvert.SerializeObject(obj);
            return System.Text.Encoding.UTF8.GetBytes(json);
        }

        private object FromJsonBytes(byte[] bodyData, Type type)
        {
            string json = System.Text.Encoding.UTF8.GetString(bodyData);
            return JsonConvert.DeserializeObject(json, type);
        }
    }
}
