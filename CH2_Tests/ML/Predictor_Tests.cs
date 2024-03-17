#region © 2024 Aflac.
//
// All rights reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical, or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
#endregion

using chapter02.ML;
using chapter02.Common;

namespace Predictor_Tests
{
    // ----------------------------------------------------
    /// <summary>
    ///     Summary description for ArrowUnitTestXML1
    /// </summary>

    [TestClass]
    public class Predictor_Tests
    {
        private readonly string cr = Environment.NewLine;

        public Predictor_Tests() { }

        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext) 
        {
            var input = @".\Data\sampledata.csv";
            var modelFile = Constants.MODEL_FILENAME;

            if(!File.Exists(modelFile))
            {
                new Trainer().Train(input);
            }
        }

        // ------------------------------------------------
        // true = Negative
        // false = Positive

        [TestMethod]
        [DataRow("My fork was dirty", true)]
        [DataRow("They call that Food?", true)]
        [DataRow("The wait was too long.", true)]
        [DataRow("I would eat there again.", false)]
        [DataRow("It smelled so good in there.", false)]
        [DataRow("I can't believe how long we had to wait.", true)]
        public void Predict_Predictor(string input, bool expected)
        {
            // -------
            // Arrange

            var sut = new Predictor();

            // ---
            // Act

            var resp = sut.Predict(input);

            // ---
            // Log

            Console.WriteLine($"Input String: {input}{cr}{cr}{resp}");

            // ------
            // Assert

            Assert.AreEqual(expected, resp.Prediction);
        }

        // ------------------------------------------------

        private void Retrain(string input, bool sentiment)
        {
            var sent = sentiment ? 0 : 1;
            var modelFile = Constants.MODEL_FILENAME;
            var stream = File.AppendText(INPUT_FILE);

            // ------------------------------------
            // Add the failed test to the test data

            stream.WriteLine($"{cr}{sent},\"{input}\"");
            stream.Close();

            // --------------------
            // Remove the old model

            if(File.Exists(modelFile))
            {
                File.Delete(modelFile);
            }

            Console.WriteLine("Retraining...");

            // -----------------
            // Retrain the model

            new Trainer().Train(input);
        }
    }
}