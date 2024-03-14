using System;
using System.IO;

using chapter02.ML.Base;
using chapter02.ML.Objects;

using Microsoft.ML;

namespace chapter02.ML
{
    public class Predictor : BaseML
    {
        public RestaurantPrediction Predict(string inputData)
        {
            RestaurantPrediction retVal = null;

            if(!File.Exists(ModelPath))
            {
                Console.WriteLine($"Failed to find model at {ModelPath}");
                return retVal;
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
                    return retVal;
                }
                else
                {
                    var predictionEngine = MlContext.Model.CreatePredictionEngine<RestaurantFeedback, RestaurantPrediction>(mlModel);

                    retVal = predictionEngine.Predict(new RestaurantFeedback { Text = inputData });
                }
            }

            return retVal;
        }
    }
}