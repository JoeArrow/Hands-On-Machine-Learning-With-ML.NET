using System;
using System.IO;

using chapter03.Common;

using Microsoft.ML;

namespace chapter03.ML.Base
{
    public class BaseML
    {
        protected readonly MLContext MlContext;
        protected readonly string cr = Environment.NewLine;
        protected readonly string crt = Environment.NewLine + "\t";

        protected static string ModelPath => Path.Combine(AppContext.BaseDirectory, Constants.MODEL_FILENAME);

        protected BaseML()
        {
            MlContext = new MLContext(2020);
        }
    }
}