#region © 2025 Joe Arrowood (JoeWare)
//
// All rights reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical, or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
#endregion

using logistic_regression.ML;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FeatureExtractor_Tests
{
    // ----------------------------------------------------
    /// <summary>
    ///     Summary description for ArrowUnitTestXML1
    /// </summary>

    [TestClass]
    public class FeatureExtractor_Tests
    {
        public FeatureExtractor_Tests() { }

        // ------------------------------------------------

        [TestMethod]
        [DataRow(@"..\..\..\Data", @"Data\sampledata.csv")]
        public void Extract_FeatureExtractor(string folder, string dataFile)
        {
            // -------
            // Arrange

            var sut = new FeatureExtractor();

            // ---
            // Act

            sut.Extract(Path.Combine(AppContext.BaseDirectory, folder));

            // ------
            // Assert

            Assert.IsTrue(File.Exists(Path.Combine(AppContext.BaseDirectory, dataFile)));

            // -------
            // Cleanup

            File.Delete(Path.Combine(AppContext.BaseDirectory, dataFile));
        }
    }
}