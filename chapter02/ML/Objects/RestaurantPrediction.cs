using System;

using Microsoft.ML.Data;

namespace chapter02.ML.Objects
{
    public class RestaurantPrediction
    {
        private readonly string cr = Environment.NewLine;

        public float Score { get; set; }
<<<<<<< HEAD

=======
>>>>>>> 27581bfd44d2484faa862aaa0fcbeb4aa42e7f12
        [ColumnName("PredictedLabel")]
        public bool Prediction { get; set; }
        public float Probability { get; set; }

        // ------------------------------------------------

        public override string ToString()
        {
            var sentiment = Prediction ? "Negative" : "Positive";

<<<<<<< HEAD
            return $"Predicted Feedback: {sentiment}.{cr}" +
                   $"With a confidence level of: {Probability:P0}.";
=======
            return $"Predicted Feedback:         {sentiment}.{cr}" +
                   $"With a confidence level of:      {Probability:P0}.{cr}";
>>>>>>> 27581bfd44d2484faa862aaa0fcbeb4aa42e7f12
        }
    }
}