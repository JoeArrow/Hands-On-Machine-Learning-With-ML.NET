using System;
using System.IO;

using logistic_regression.Common;
using logistic_regression.ML.Base;

namespace logistic_regression.ML
{
    public class FeatureExtractor : BaseML
    {
        public void Extract(string folderPath)
        {
            var files = Directory.GetFiles(folderPath);
            var path = Path.Combine(AppContext.BaseDirectory, Constants.DATA_PATH,Constants.SAMPLE_DATA);

            using(var streamWriter = new StreamWriter(path))
            {
                foreach (var file in files)
                {
                    var strings = GetStrings(File.ReadAllBytes(file));

                    streamWriter.WriteLine($"{file.ToLower().Contains("malicious")}\t{strings}");
                }
            }

            Console.WriteLine($"Extracted {files.Length} to {Constants.SAMPLE_DATA}");
        }
    }
}