using System;
using System.Collections.Generic;
using System.Text;
using BaselineBuilder.Config;
using Microsoft.Extensions.Options;

namespace BaselineBuilder.IO
{
    public class FileService
    {
        private readonly string folderPath;

        public FileService(IOptions<FolderSettings> options)
        {
            folderPath = options.Value.FolderPath;
        }

        public List<string> GetFiles()
        {
            List<string> pcapFiles = Directory.GetFiles(folderPath, "*.pcap*").ToList();

            if (pcapFiles.Count == 0)
            {
                throw new FileLoadException("No PCAP files found in current folder.");
            }

            return pcapFiles;
        }
    }
}
