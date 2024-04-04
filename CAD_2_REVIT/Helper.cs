using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace CAD_2_REVIT
{
    internal class Helper
    {
        public static Document documentH = null;
        public static List<ColumnData> columnsH = new List<ColumnData>();
        public static double topLevelElevationH = 0;
        public static double botLevelElevationH = 0;
        public static FamilySymbol ColumnTypeH = null;
        public static Level bottomLevelH = null;

    }
}
