using System;
using System.Collections.Generic;
using System.Text;
using ParameterDataLib;

namespace BaselineBuilder.PacketReading.Helpers
{
    public class StandardDeviations
    {
        public required Dictionary<string, int> ParameterIndices { get; init; }

        public required Dictionary<string, double> ParameterSTDs { get; init; }

        private int[] _amountSeen;
        private double[] _runningMean;
        private double[] _M2;

        public StandardDeviations(int parameterCount) 
        {
            _amountSeen = new int[parameterCount];
            _runningMean = new double[parameterCount];
            _M2 = new double[parameterCount];
        }

        public void Recalculate(List<ParameterData> parameterValues)
        {
            foreach (ParameterData data in parameterValues)
            {
                int i = ParameterIndices[data.Name];
                double x = data.Value;

                _amountSeen[i]++;
                double delta = x - _runningMean[i];
                _runningMean[i] += delta / _amountSeen[i];
                double delta2 = x - _runningMean[i];
                _M2[i] += delta * delta2;

                ParameterSTDs[data.Name] = _amountSeen[i] > 1
                    ? Math.Sqrt(_M2[i] / (_amountSeen[i] - 1))
                    : 0.0;
            }
        }
    }
}
