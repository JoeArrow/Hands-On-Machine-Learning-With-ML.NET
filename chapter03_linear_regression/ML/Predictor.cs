using System;
using System.IO;

using chapter03.ML.Base;
using chapter03.ML.Objects;

using Microsoft.ML;

using Newtonsoft.Json;

namespace chapter03.ML
{
    public class Predictor : BaseML
    {
        public EmploymentHistoryPrediction Predict(string inputDataFile)
        {
            var retVal = null as EmploymentHistoryPrediction;

            if(!File.Exists(ModelPath))
            {
                Console.WriteLine($"Failed to find model at {ModelPath}");
            }
            else
            {
                if(!File.Exists(inputDataFile))
                {
                    Console.WriteLine($"Failed to find input data at {inputDataFile}");
                }
                else
                {
                    ITransformer mlModel;

                    using(var stream = new FileStream(ModelPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        mlModel = MlContext.Model.Load(stream, out _);
                    }

                    if(mlModel == null)
                    {
                        Console.WriteLine("Failed to load model");
                    }
                    else
                    {
                        var predictionEngine = MlContext.Model.CreatePredictionEngine<EmploymentHistory, EmploymentHistoryPrediction>(mlModel);

                        var json = File.ReadAllText(inputDataFile);

                        retVal = predictionEngine.Predict(JsonConvert.DeserializeObject<EmploymentHistory>(json));

                        //Console.WriteLine(
                        //                    $"Based on input json:{Environment.NewLine}" +
                        //                    $"{json}{Environment.NewLine}" +
                        //                    $"The employee is predicted to work {retVal.DurationInMonths:#.##} months");
                    }
                }
            }

            return retVal;
        }
    }
}