using System;
using System.Collections.Generic;
using System.Text;
using BaselineBuilder.PacketReading.Helpers;
using ParameterDataLib;

namespace BaselineBuilder.PacketReading.DataflowBlocks
{
    public class STDCalculatorBlock
    {
        private readonly StandardDeviations _standardDeviations;

        public STDCalculatorBlock(IcdDeserializer icd)
        {
            Dictionary<string, int> indices = [];
            Dictionary<string, double> initialSTDs = [];

            for (int i = 0; i < icd.ParameterCount; i++)
            {
                indices.Add(icd.Parameters[i].Name, i);
                initialSTDs.Add(icd.Parameters[i].Name, 0);
            }

            _standardDeviations = new(icd.ParameterCount)
            {
                ParameterIndices = indices,
                ParameterSTDs = initialSTDs
            };
        }

        public void RecalculateSTDs(List<ParameterData> parameterValues)
        {
            _standardDeviations.Recalculate(parameterValues);
        }
    }
}
