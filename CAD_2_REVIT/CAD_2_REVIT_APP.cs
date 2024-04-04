using Autodesk.Revit.DB.Visual;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace CAD_2_REVIT
{
    internal class CAD_2_REVIT_APP : IExternalApplication
    {
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication application)
        {
            string tabName = "CTR20";
            application.CreateRibbonTab(tabName);
            string panelName = "Columns";
            RibbonPanel myPanel = application.CreateRibbonPanel(tabName,panelName);
            string internalname = "Column Push Button";
            string buttonName = "Create";
            string assemplyName = Assembly.GetExecutingAssembly().Location;
            string solutionName = "CAD_2_REVIT";
            string calssName = "CAD_2_REVIT_COMMAND";
            PushButtonData myButtonData = new PushButtonData(internalname, buttonName, assemplyName, $"{solutionName}.{calssName}");
            PushButton myButton = myPanel.AddItem(myButtonData) as PushButton;
            string imagename = "Square44x44Logo.targetsize-32.png";
            Uri myuri = new Uri($"pack://application:,,,/{solutionName};component/Images/{imagename}");
            BitmapImage myimage = new BitmapImage(myuri);
            myButton.LargeImage = myimage;
            return Result.Succeeded;
        }
    }
}
