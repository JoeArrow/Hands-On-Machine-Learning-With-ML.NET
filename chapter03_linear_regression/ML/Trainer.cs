using System;
using System.IO;

using chapter03.Common;
using chapter03.ML.Base;
using chapter03.ML.Objects;

using Microsoft.ML;
using Microsoft.ML.Data;

using jsSer = System.Text.Json.JsonSerializer;

namespace chapter03.ML
{
    public class Trainer : BaseML
    {
        public RegressionMetrics Train(string trainingFileName)
        {
            // --------------------------------------
            // RegressionMetrics is defined in ML.NET

            var retVal = null as RegressionMetrics;

            // ------------------------------
            // Verify that we have input data

            if(!File.Exists(trainingFileName))
            {
                Console.WriteLine($"Failed to find training data file ({trainingFileName}");
                retVal = null;
            }
            else
            {
                // ------------------------------
                // IDataView is defined in ML.NET

                var trainingDataView = MlContext.Data.LoadFromTextFile<EmploymentHistory>(trainingFileName, ',');

                var dataSplit = MlContext.Data.TrainTestSplit(trainingDataView, testFraction: 0.4);

                var dataProcessPipeline = MlContext.Transforms.CopyColumns("Label", nameof(EmploymentHistory.DurationInMonths))
                    .Append(MlContext.Transforms.NormalizeMeanVariance(nameof(EmploymentHistory.IsMarried)))
                    .Append(MlContext.Transforms.NormalizeMeanVariance(nameof(EmploymentHistory.BSDegree)))
                    .Append(MlContext.Transforms.NormalizeMeanVariance(nameof(EmploymentHistory.MSDegree)))
                    .Append(MlContext.Transforms.NormalizeMeanVariance(nameof(EmploymentHistory.YearsExperience))
                    .Append(MlContext.Transforms.NormalizeMeanVariance(nameof(EmploymentHistory.AgeAtHire)))
                    .Append(MlContext.Transforms.NormalizeMeanVariance(nameof(EmploymentHistory.HasKids)))
                    .Append(MlContext.Transforms.NormalizeMeanVariance(nameof(EmploymentHistory.WithinMonthOfVesting)))
                    .Append(MlContext.Transforms.NormalizeMeanVariance(nameof(EmploymentHistory.DeskDecorations)))
                    .Append(MlContext.Transforms.NormalizeMeanVariance(nameof(EmploymentHistory.LongCommute)))
                    .Append(MlContext.Transforms.Concatenate("Features",
                        typeof(EmploymentHistory).ToPropertyList<EmploymentHistory>(nameof(EmploymentHistory.DurationInMonths)))));

                var trainer = MlContext.Regression.Trainers.Sdca(labelColumnName: "Label", featureColumnName: "Features");

                var trainingPipeline = dataProcessPipeline.Append(trainer);

                // ---------------
                // Train the model

                ITransformer trainedModel = trainingPipeline.Fit(dataSplit.TrainSet);
                MlContext.Model.Save(trainedModel, dataSplit.TrainSet.Schema, ModelPath);

                var testSetTransform = trainedModel.Transform(dataSplit.TestSet);

                retVal = MlContext.Regression.Evaluate(testSetTransform);

                Console.WriteLine($"{cr}Training Function:{cr}{new string('-', 20)}{crt}" +
                                  $"Loss Function: {retVal.LossFunction:0.##}{crt}" +
                                  $"Mean Absolute Error: {retVal.MeanAbsoluteError:#.##}{crt}" +
                                  $"Mean Squared Error: {retVal.MeanSquaredError:#.##}{crt}" +
                                  $"RSquared: {retVal.RSquared:0.##}{crt}" +
                                  $"Root Mean Squared Error: {retVal.RootMeanSquaredError:#.##}{cr}" +
                                  new string('-', 50));
            }

            return retVal;
        }
    }
}