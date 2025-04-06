using System.IO;

using logistic_regression.ML.Base;
using logistic_regression.ML.Objects;

using Microsoft.ML;

namespace logistic_regression.ML
{
    public class Predictor : BaseML
    {
        public FilePrediction Predict(string inputDataFile)
        {
            var retVal = new FilePrediction();

            if (!File.Exists(ModelPath))
            {
                //Console.WriteLine($"Failed to find model at {ModelPath}");
                return retVal;
            }

            if (!File.Exists(inputDataFile))
            {
                //Console.WriteLine($"Failed to find input data at {inputDataFile}");
                return retVal;
            }

            ITransformer mlModel;
            
            using (var stream = new FileStream(ModelPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                mlModel = MlContext.Model.Load(stream, out _);
            }

            if (mlModel == null)
            {
                //Console.WriteLine("Failed to load model");
                return retVal;
            }

            var predictionEngine = MlContext.Model.CreatePredictionEngine<FileInput, FilePrediction>(mlModel);

            retVal = predictionEngine.Predict(new FileInput { Strings = GetStrings(File.ReadAllBytes(inputDataFile)) });

            //Console.WriteLine($"Based on the file ({inputDataFile}) the file is classified as {(retVal.IsMalicious ? "malicious" : "benign")}" + 
            //                  $" at a confidence level of {retVal.Probability:P0}");

            return retVal;
        }
    }
}