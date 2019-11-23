using Brix.SergeRoz.AnagramTest.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Brix.SergeRoz.AnagramTest.Dao
{
    public class FileDao : IDao
    {

        #region Properties

        public string DirectoryPath { get; private set; }
        public string FileName { get; private set; }
        public string FullFilePath { get; private set; }
        public string LinesSource { get { return FullFilePath; } }

        #endregion

        #region Constructor

        public FileDao()
        {
            Initialize();
        }

        #endregion

        #region Public Methods       

        public async Task<string[]> GetLinesAsync()
        {
            return await ReadFileLinesAsync();
        }

        public async Task SetLinesAsync(string[] lines, bool append)
        {
            await WriteLinesToFileAsync(lines, append);
        }

        #endregion

        #region Private Methods

        internal void Initialize()
        {
            DirectoryPath = Consts.DirectoryPath;
            FileName = Consts.FileName;

            FullFilePath = Path.Combine(DirectoryPath, FileName);
        }

        private async Task<string[]> ReadFileLinesAsync()
        {
            try
            {
                using (var reader = File.OpenText(FullFilePath))
                {
                    var fileText = await reader.ReadToEndAsync();

                    return fileText.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                }
            }
            catch (FileNotFoundException e)
            {
                throw e;
            }
        }

        private async Task WriteLinesToFileAsync(string[] lines, bool append)
        {
            if (!append && File.Exists(FullFilePath))
            {
                File.Delete(FullFilePath);
            }

            using (var writer = File.OpenWrite(FullFilePath))
            {
                using (var streamWriter = new StreamWriter(writer))
                {
                    foreach (var line in lines)
                    {
                        await streamWriter.WriteLineAsync(line);
                    }
                }
            }
        }

        #endregion

    }
}

