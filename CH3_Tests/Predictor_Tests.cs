#region © 2024 Aflac.
//
// All rights reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical, or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
#endregion

using chapter03.ML;
using chapter03.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Predictor_Tests
{
    // ----------------------------------------------------
    /// <summary>
    ///     Summary description for ArrowUnitTestXML1
    /// </summary>

    [TestClass]
    public class Predictor_Tests
    {
        public TestContext? TestContext { get; set; }

        public Predictor_Tests() { }

        // ------------------------------------------------

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
        [DataRow("{'DurationInMonths':245," +
                  "'IsMarried':1," +
                  "'BSDegree':0," +
                  "'MSDegree':0," +
                  "'YearsExperience':30," +
                  "'AgeAtHire':33," +
                  "'HasKids':1," +
                  "'WithinMonthOfVesting':0," +
                  "'DeskDecorations':0," +
                  "'LongCommute':1}")]
        
        [DataRow("{'DurationInMonths':2," +
                  "'IsMarried':1," +
                  "'BSDegree':1," +
                  "'MSDegree':1," +
                  "'YearsExperience':0," +
                  "'AgeAtHire':33," +
                  "'HasKids':0," +
                  "'WithinMonthOfVesting':6," +
                  "'DeskDecorations':0," +
                  "'LongCommute':1}")]
        public void Method_Class(string input)
        {
            // -------
            // Arrange

            var sut = new Predictor();

            // ---
            // Act

            var resp = sut.Predict(input.Replace('\'', '\"'));
            var inputFilePath = $@"{Constants.SOLUTION_FOLDER}\sampledata.csv";

            var content = File.ReadAllText(inputFilePath);
            Console.WriteLine(content);

            // ---
            // Log

            Console.WriteLine(resp.Message);

            // ------
            // Assert

            Assert.IsTrue(resp.Success);
        }
    }
}