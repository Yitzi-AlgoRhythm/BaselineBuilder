using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks.Dataflow;
using BaselineBuilder.PacketReading.DataflowBlocks;
using ParameterDataLib;
using SharpPcap;

namespace BaselineBuilder.PacketReading
{
    public class STDPipeline
    {
        public Task Completion { get; init; }

        private readonly TransformBlock<RawCapture, byte[]> _builderBlock;
        private readonly TransformManyBlock<byte[], List<ParameterData>> _decoderBlock;
        private readonly ActionBlock<List<ParameterData>> _stdBlock;

        public STDPipeline(BuilderBlock builder, DecoderBlock decoder, STDCalculatorBlock stdCalculator)
        {
            ExecutionDataflowBlockOptions blockOptions = new()
            {
                MaxDegreeOfParallelism = Environment.ProcessorCount
            };

            DataflowLinkOptions linkOptions = new()
            {
                PropagateCompletion = true
            };

            _builderBlock = new
            (
                builder.Build,
                blockOptions
            );

            _decoderBlock = new
            (
                decoder.Decode,
                blockOptions
            );

            _stdBlock = new
            (
                stdCalculator.RecalculateSTDs,
                blockOptions
            );

            _builderBlock.LinkTo(_decoderBlock, linkOptions);
            _decoderBlock.LinkTo(_stdBlock, linkOptions);

            Completion = _stdBlock.Completion;
        }

        public async Task HandlePacket(RawCapture packet)
        {
            await _builderBlock.SendAsync(packet);
        }
    }
}
