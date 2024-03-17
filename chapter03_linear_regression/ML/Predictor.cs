using System;
using System.IO;

using Microsoft.ML;

using chapter03.ML.Base;
using chapter03.ML.Objects;

using jsSer = System.Text.Json.JsonSerializer;

namespace chapter03.ML
{
    public class Predictor : BaseML
    {
        public EmploymentHistoryPrediction Predict(string json)
        {
            var retVal = new EmploymentHistoryPrediction();

            if(!File.Exists(ModelPath))
            {
                retVal.Success = false;
                retVal.Message = $"Failed to find model at {ModelPath}";
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

                    retVal = predictionEngine.Predict(jsSer.Deserialize<EmploymentHistory>(json));

                    if(retVal.DurationInMonths < 0f)
                    {
                        retVal.DurationInMonths = .5f;
                    }

                    retVal.Success = true;
                    retVal.Message = $"Based on input json:{cr}{cr}{json}{cr}{cr}" +
                                        $"The employee is predicted to remain in the current job for {retVal.DurationInMonths:#.##} months.{cr}" +
                                        $"Or {retVal.DurationInMonths / 12:#.##} more years...";
                        
                    if(retVal.DurationInMonths < 6f)
                    {
                        retVal.Message += $"{cr}{cr}We may need to start looking for a replacement, immediately!";
                    }
                }
            }

            return retVal;
        }
    }
}