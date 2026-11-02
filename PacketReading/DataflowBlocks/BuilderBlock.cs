using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using BaselineBuilder.PacketReading.Helpers;
using PacketDotNet;
using SharpPcap;

namespace BaselineBuilder.PacketReading.DataflowBlocks
{
    public class BuilderBlock
    {
        private static readonly int syncBytesCount = IcdConstants.SyncBytes.Length;

        public byte[] Build(RawCapture capture)
        {
            Packet packet = Packet.ParsePacket(capture.LinkLayerType, capture.Data);

            UdpPacket udp = packet.Extract<UdpPacket>();

            if (udp == null || udp.PayloadData == null || udp.PayloadData.Length < syncBytesCount)
            {
                return null!;
            }

            for (int i = 0; i < syncBytesCount; i++)
            {
                if (IcdConstants.SyncBytes[i] != udp.PayloadData[i])
                {
                    return null!;
                }
            }

            ulong timestampMS = GetTimestamp(capture.Timeval);

            return [.. BitConverter.GetBytes(timestampMS), .. udp.PayloadData.Skip(syncBytesCount)];
        }

        private static ulong GetTimestamp(PosixTimeval timeval)
        {
            const int millisInSecond = 1000;
            const int microsInMillis = 1000;

            return (timeval.Seconds * millisInSecond) + (timeval.MicroSeconds / microsInMillis);
        }
    }
}
