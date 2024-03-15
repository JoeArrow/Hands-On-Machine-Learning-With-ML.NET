using System;
using System.IO;

using chapter03.Common;
using chapter03.ML.Base;
using chapter03.ML.Objects;

using Microsoft.ML;
using Microsoft.ML.Data;

namespace chapter03.ML
{
    public class Trainer : BaseML
    {
        public RegressionMetrics Train(string trainingFileName)
        {
            var retVal = null as RegressionMetrics;

            if(!File.Exists(trainingFileName))
            {
                Console.WriteLine($"Failed to find training data file ({trainingFileName}");
                retVal = null;
            }
            else
            {
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

                ITransformer trainedModel = trainingPipeline.Fit(dataSplit.TrainSet);
                MlContext.Model.Save(trainedModel, dataSplit.TrainSet.Schema, ModelPath);

                var testSetTransform = trainedModel.Transform(dataSplit.TestSet);

                retVal = MlContext.Regression.Evaluate(testSetTransform);

                Console.WriteLine($"Loss Function: {retVal.LossFunction:0.##}{Environment.NewLine}" +
                                  $"Mean Absolute Error: {retVal.MeanAbsoluteError:#.##}{Environment.NewLine}" +
                                  $"Mean Squared Error: {retVal.MeanSquaredError:#.##}{Environment.NewLine}" +
                                  $"RSquared: {retVal.RSquared:0.##}{Environment.NewLine}" +
                                  $"Root Mean Squared Error: {retVal.RootMeanSquaredError:#.##}");
            }

            return retVal;
        }
    }
}