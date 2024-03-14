#region © 2024 Aflac.
//
// All rights reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical, or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
#endregion

using System;

using Microsoft.ML.Data;

namespace chapter02.Extensions
{
    // ----------------------------------------------------
    /// <summary>
    ///     CalibratedBinaryClassificationMetrics_Ext Description
    /// </summary>

    public static class CalibratedBinaryClassificationMetrics_Ext
    {
        public static string Stats(this CalibratedBinaryClassificationMetrics metrics)
        {
            return $"Area Under Curve: {metrics.AreaUnderRocCurve:P2}{Environment.NewLine}" +
                   $"Area Under Precision Recall Curve: {metrics.AreaUnderPrecisionRecallCurve:P2}{Environment.NewLine}" +
                   $"Accuracy: {metrics.Accuracy:P2}{Environment.NewLine}" +
                   $"F1Score: {metrics.F1Score:P2}{Environment.NewLine}" +
                   $"Positive Recall: {metrics.PositiveRecall:#.##}{Environment.NewLine}" +
                   $"Negative Recall: {metrics.NegativeRecall:#.##}{Environment.NewLine}";
        }
    }
}
