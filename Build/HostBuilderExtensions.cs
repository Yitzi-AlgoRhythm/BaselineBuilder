using System;
using System.Collections.Generic;
using System.Text;
using BaselineBuilder.Config;
using BaselineBuilder.IO;
using BaselineBuilder.PacketReading;
using BaselineBuilder.PacketReading.DataflowBlocks;
using BaselineBuilder.PacketReading.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BaselineBuilder.Build
{
    public static class HostBuilderExtensions
    {
        public static void AddAppServices(this IHostApplicationBuilder builder)
        {
            builder.Services.Configure<FolderSettings>
                (builder.Configuration.GetSection(nameof(FolderSettings)));
            builder.Services.AddSingleton<FileService>();

            builder.Services.AddSingleton<IcdDeserializer>();

            builder.Services.AddSingleton<StandardDeviations>();

            builder.Services.AddSingleton<BuilderBlock>();
            builder.Services.AddSingleton<DecoderBlock>();
            builder.Services.AddSingleton<STDCalculatorBlock>();
            builder.Services.AddSingleton<STDPipeline>();

            builder.Services.AddSingleton<FileHandler>();
        }
    }
}
