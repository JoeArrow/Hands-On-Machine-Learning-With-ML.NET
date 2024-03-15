using System;

using Microsoft.ML.Data;

namespace chapter02.ML.Objects
{
    public class RestaurantPrediction
    {
        private readonly string cr = Environment.NewLine;

        public float Score { get; set; }

        [ColumnName("PredictedLabel")]
        public bool Prediction { get; set; }
        public float Probability { get; set; }

        // ------------------------------------------------

        public override string ToString()
        {
            var sentiment = Prediction ? "Negative" : "Positive";

            return $"Predicted Feedback:         {sentiment}.{cr}" +
                   $"With a confidence level of:      {Probability:P0}.{cr}";
        }
    }
}