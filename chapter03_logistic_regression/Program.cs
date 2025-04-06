using System;

using logistic_regression.ML;

namespace logistic_regression
{
    class Program
    {
        static void Main(string[] args)
        {
            var cr = Environment.NewLine;

            if (args.Length != 2)
            {
                Console.WriteLine($"Invalid arguments, Exiting App.{cr}{cr}Usage:{cr}" +
                                  $"predict <path to input file>{cr}or {cr}" +
                                  $"train <path to training data file>{cr}or {cr}" +
                                  $"extract <path to folder>{cr}");
                return;
            }

            switch (args[0].ToLower())
            {
                case "extract":
                    new FeatureExtractor().Extract(args[1]);
                    break;

                case "predict":
                    new Predictor().Predict(args[1]);
                    break;

                case "train":
                    new Trainer().Train(args[1]);
                    break;

                default:
                    Console.WriteLine($"{args[0]} is an invalid option");
                    break;
            }
        }
    }
}