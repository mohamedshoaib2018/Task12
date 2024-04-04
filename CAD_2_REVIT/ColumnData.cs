using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAD_2_REVIT
{
    internal class ColumnData
    {
        public XYZ midPoint = null;
        public ColumnData(XYZ columnCoordinate)
        {
            midPoint = columnCoordinate;
        }
    }
}
