using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using BaselineBuilder.IO;
using Microsoft.Extensions.Hosting;

namespace BaselineBuilder.Build
{
    public class AppHostedService : BackgroundService
    {
        private readonly FileService _fileService;
        private readonly FileHandler _fileHandler;

        public AppHostedService(FileService fileService, FileHandler fileHandler)
        {
            _fileService = fileService;
            _fileHandler = fileHandler;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            List<string> pcapFilePaths = _fileService.GetFiles();

            await _fileHandler.CalculateSTDsFromFiles(pcapFilePaths);
        }
    }
}
