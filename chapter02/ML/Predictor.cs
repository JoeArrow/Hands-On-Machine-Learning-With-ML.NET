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
            var retVal = new RestaurantPrediction();

            if(!File.Exists(ModelPath))
            {
                retVal.Success = false;
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
                    retVal.Success = false;
                }
                else
                {
                    var predictionEngine = MlContext.Model.CreatePredictionEngine<RestaurantFeedback, RestaurantPrediction>(mlModel);

                    retVal = predictionEngine.Predict(new RestaurantFeedback { Text = inputData });
                    retVal.Success = true;
                }
            }

            return retVal;
        }
    }
}