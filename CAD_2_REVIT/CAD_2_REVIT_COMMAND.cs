using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAD_2_REVIT
{
    [Transaction(TransactionMode.Manual)]
    public class CAD_2_REVIT_COMMAND : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;
            CAD_2_REVIT_WINDOW myWindow = new CAD_2_REVIT_WINDOW(doc);
            myWindow.Show();
            return Result.Succeeded;
        }
    }
}
