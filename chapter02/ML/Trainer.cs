using System;
using System.IO;

using chapter02.ML.Base;
using chapter02.ML.Objects;

using Microsoft.ML;
using Microsoft.ML.Data;

namespace chapter02.ML
{
    public class Trainer : BaseML
    {
        public CalibratedBinaryClassificationMetrics Train(string trainingFileName)
        {
            var retVal = null as CalibratedBinaryClassificationMetrics;

            if (!File.Exists(trainingFileName))
            {
                Console.WriteLine($"Failed to find training data file ({trainingFileName}");
                return retVal;
            }

            var loaderOptions = new TextLoader.Options()
            {
                HasHeader = false,    // Specify whether the data file has a header row
                Separators = [','],   // Specify the column separator (comma in this case)
                AllowQuoting = true   // Specify whether quoting is allowed in the data file
            };

            var trainingDataView = MlContext.Data.LoadFromTextFile<RestaurantFeedback>(trainingFileName, loaderOptions);

            var dataSplit = MlContext.Data.TrainTestSplit(trainingDataView, testFraction: 0.2);

            var dataProcessPipeline = MlContext.Transforms.Text.FeaturizeText(outputColumnName: "Features", 
                                                                              inputColumnName: nameof(RestaurantFeedback.Text));

            var sdcaRegressionTrainer = 
                MlContext.BinaryClassification.Trainers.SdcaLogisticRegression(labelColumnName: nameof(RestaurantFeedback.Label), 
                                                                               featureColumnName: "Features");

            var trainingPipeline = dataProcessPipeline.Append(sdcaRegressionTrainer);

            var trainedModel = trainingPipeline.Fit(dataSplit.TrainSet);

            MlContext.Model.Save(trainedModel, dataSplit.TrainSet.Schema, ModelPath);

            var testSetTransform = trainedModel.Transform(dataSplit.TestSet);

            retVal = MlContext.BinaryClassification.Evaluate(data: testSetTransform, 
                                                             labelColumnName: nameof(RestaurantFeedback.Label), 
                                                             scoreColumnName: nameof(RestaurantPrediction.Score));
            return retVal;
        }
    }
}