using System;
using System.Collections.Generic;
using System.Text;
using BaselineBuilder.PacketReading;
using SharpPcap;
using SharpPcap.LibPcap;

namespace BaselineBuilder.IO
{
    public class FileHandler
    {
        private readonly STDPipeline _stdPipeline;

        public FileHandler(STDPipeline stdPipeline)
        {
            _stdPipeline = stdPipeline;
        }

        public async Task CalculateSTDsFromFiles(List<string> pcapFiles)
        {
            foreach (string filePath in pcapFiles)
            {
                using CaptureFileReaderDevice reader = new CaptureFileReaderDevice(filePath);
                reader.Open();

                PacketCapture packetCapture;

                while (reader.GetNextPacket(out packetCapture) == GetPacketStatus.PacketRead)
                {
                    RawCapture packet = packetCapture.GetPacket();

                    await _stdPipeline.HandlePacket(packet);
                }
            }

            await _stdPipeline.Completion;
        }
    }
}
