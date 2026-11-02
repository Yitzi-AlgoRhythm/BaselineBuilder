using System;
using System.Collections.Generic;
using System.Text;
using ParameterDataLib;

namespace BaselineBuilder.PacketReading.Helpers
{
    public class PacketData
    {
        private const int correlatorBytePosition = 8;
        private const int dataStartByte = 9;


        private byte[] _data;

        private ulong _timestampMS;

        public byte CorrelatorByte { get; private set; }

        public PacketData(IEnumerable<byte> data)
        {
            byte[] dataArr = data.ToArray();

            _data = dataArr[dataStartByte..];

            _timestampMS = BitConverter.ToUInt64(dataArr);

            CorrelatorByte = dataArr[correlatorBytePosition];
        }

        public ParameterData GetParameterData(IcdParameter icdParam)
        {
            byte[] valueArr = _data.Skip(icdParam.Offset).Take(icdParam.Size).ToArray();

            double value;

            if (icdParam.Type == ParameterDataType.Float64)
            {
                value = BitMaskService.GetFloat64(valueArr, icdParam.BitMask);
            }
            else
            {
                value = BitMaskService.GetInt32(valueArr, icdParam.BitMask, icdParam.Type);
            }

            return new ParameterData()
            {
                Name = icdParam.Name,
                Units = icdParam.Unit,
                Value = value,
                TimestampMS = _timestampMS
            };
        }
    }
}
