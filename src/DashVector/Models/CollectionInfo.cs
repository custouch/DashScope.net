using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashVector.Models
{
    public class CollectionInfo
    {
        public enum DataType
        {
            FLOAT,
            INT
        }

        public enum Metric
        {

            Euclidean,
            Dotproduct,
            Cosine
        }

    }
}
