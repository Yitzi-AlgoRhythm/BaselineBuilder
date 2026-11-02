using System;
using System.Collections.Generic;
using System.Text;
using BaselineBuilder.PacketReading.Helpers;
using ParameterDataLib;

namespace BaselineBuilder.PacketReading.DataflowBlocks
{
    public class DecoderBlock
    {
        private readonly IcdDeserializer _deserializer;

        public DecoderBlock(IcdDeserializer deserializer)
        {
            _deserializer = deserializer;
        }

        public IEnumerable<List<ParameterData>> Decode(byte[] data)
        {
            if (data is null)
            {
                return [];
            }

            PacketData packetData = new(data);

            List<ParameterData> result = [];

            foreach (IcdParameter icdParam in _deserializer.CorrelatorGroups[packetData.CorrelatorByte])
            {
                result.Add(packetData.GetParameterData(icdParam));
            }

            return [result];
        }
    }
}
