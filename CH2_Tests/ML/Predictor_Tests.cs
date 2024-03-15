#region © 2024 Aflac.
//
// All rights reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical, or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
#endregion

using chapter02.Common;
using chapter02.ML;

namespace Predictor_Tests
{
    // ----------------------------------------------------
    /// <summary>
    ///     Summary description for ArrowUnitTestXML1
    /// </summary>

    [TestClass]
    public class Predictor_Tests
    {
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

        [TestMethod]
        [DataRow("They call that Food?", true)]
        [DataRow("I would eat there again.", false)]
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

            Console.WriteLine(resp.ToString());

            // ------
            // Assert

            Assert.AreEqual(expected, resp.Prediction);
        }
    }
}