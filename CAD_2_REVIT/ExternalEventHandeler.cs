using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Runtime.Remoting.Contexts;

namespace CAD_2_REVIT
{
    internal class ExternalEventHandeler : IExternalEventHandler
    {
        public void Execute(UIApplication app)
        {
            try
            {
               
                using (Transaction trans = new Transaction(Helper.documentH, "Create Columns"))
                {
                    trans.Start();
                    foreach (var col in Helper.columnsH)
                    {
                        XYZ botPoint = new XYZ(col.midPoint.X, col.midPoint.Y,Helper.botLevelElevationH);
                        XYZ topPoint = new XYZ(col.midPoint.X, col.midPoint.Y, Helper.topLevelElevationH);
                        Curve ColLine = Line.CreateBound( botPoint, topPoint);
                        Helper.documentH.Create.NewFamilyInstance(ColLine,Helper.ColumnTypeH,Helper.bottomLevelH,Autodesk.Revit.DB.Structure.StructuralType.Column);
                    }
                    trans.Commit();
                    
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK);
            }

        }

        public string GetName()
        {
            return "ExternalEventHandeler";
        }
    }
}
