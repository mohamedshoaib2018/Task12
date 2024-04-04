using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;

namespace CAD_2_REVIT
{
    [Transaction(TransactionMode.Manual)]
    /// <summary>
    /// Interaction logic for CAD_2_REVIT_WINDOW.xaml
    /// </summary>
    public partial class CAD_2_REVIT_WINDOW : Window
    {
        Document Document = null;
        List<ColumnData> Columns = new List<ColumnData>();
        ExternalEventHandeler myHandeler = null;
        ExternalEvent externalEvent = null;
        public CAD_2_REVIT_WINDOW(Document document)
        {
            
            Document = document;
            InitializeComponent();
            myHandeler = new ExternalEventHandeler();
            externalEvent = ExternalEvent.Create(myHandeler);
           
        }

        #region Collect DWG,ColumnTypes,Levels
        private void Collect_Button_Click(object sender, RoutedEventArgs e)
        {
            //01-Collect DwG family instance in revit
            try
            {
                dwgBox.Items.Clear();
                var dwgs = new FilteredElementCollector(Document).OfClass(typeof(ImportInstance)).
               WhereElementIsNotElementType().ToElementIds().ToList();

                foreach (var dwgId in dwgs)
                {
                    Element dwgElement = Document.GetElement(dwgId);
                    string dwgName = dwgElement.Category.Name;
                    dwgBox.Items.Add(dwgName.ToString());
                }
                logText.Text += "DWG Successfully Loaded\n";

            }
            catch (Exception)
            {
                logText.Text += "Error While Loading DWG \n";
            }

            //02-Collect Column types
            try
            {
                ColumnTypeBox.Items.Clear();
                var columnTypesIds = new FilteredElementCollector(Document).OfCategory(BuiltInCategory.OST_StructuralColumns)
                .WhereElementIsElementType().ToElementIds().ToList();

                foreach (var columnTypeId in columnTypesIds)
                {
                    Element ColumnTypeElement = Document.GetElement(columnTypeId);
                    string ColumnTypeName = ColumnTypeElement.Name;
                    ColumnTypeBox.Items.Add(ColumnTypeName.ToString());
                }
                logText.Text += "Column Types Successfully Loaded \n";
            }
            catch (Exception)
            {

                logText.Text += "Error While Loading Column Types \n";
            }

            //03- Collect Levels
            try
            {
                var levelsIds = new FilteredElementCollector(Document).OfClass(typeof(Level))
               .WhereElementIsNotElementType().ToElementIds().ToList();

                foreach (var levelId in levelsIds)
                {
                    Element levelElement = Document.GetElement(levelId);
                    string levelElementName = levelElement.Name;
                    bottomLevelBox.Items.Add(levelElementName.ToString());
                    topLevelBox.Items.Add(levelElementName.ToString());
                }
                logText.Text += "Levels Successfully Loaded \n";
            }
            catch (Exception)
            {

                logText.Text += "Error While Loading Levels \n";
            }


        }
        #endregion

        #region Collect Layers from target dwg
        private void GetLayers_Button_Click(object sender, RoutedEventArgs e)
        {
            var selectedDwgName = dwgBox.SelectedItem.ToString();

            try
            {
                var targetDwgs = new FilteredElementCollector(Document).OfClass(typeof(ImportInstance)).
                WhereElementIsNotElementType().FirstOrDefault(x => x.Category.Name == selectedDwgName);
                DWGAnalysis myDWGassis = new DWGAnalysis(Document, targetDwgs);
                List<PolyLine> polyLines = myDWGassis.DWG_PloyLines;
                List<string> LayersName = new List<string>();
                foreach (PolyLine polyLine in polyLines)
                {
                    var layerId = polyLine.GraphicsStyleId;
                    var layer = Document.GetElement(layerId) as GraphicsStyle;
                    var layerName = layer.GraphicsStyleCategory.Name;
                    if (!LayersName.Contains(layerName)) { LayersName.Add(layerName); }
                    else { continue; }
                    
                }
                layerBox.ItemsSource = LayersName;
                logText.Text += "Layers Successfully Loaded\n";

            }
            catch (Exception)
            {

                logText.Text += "Error While Loading Layers\n";
            }


        } 
        #endregion

        private void GetPolylines_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<PolyLine> targetPolys = new List<PolyLine>();
                string targetLayerName = layerBox.SelectedItem.ToString();

                var selectedDwgName = dwgBox.SelectedItem.ToString();

                var targetDwgs = new FilteredElementCollector(Document).OfClass(typeof(ImportInstance)).
                    WhereElementIsNotElementType().FirstOrDefault(x => x.Category.Name == selectedDwgName);

                DWGAnalysis myDWGassis = new DWGAnalysis(Document, targetDwgs);

                List<PolyLine> PolyLines = myDWGassis.DWG_PloyLines.ToList();


                foreach (PolyLine polyLine in PolyLines)
                {
                    var layerId = polyLine.GraphicsStyleId;
                    var layer = Document.GetElement(layerId) as GraphicsStyle;
                    var layerName = layer.GraphicsStyleCategory.Name;
                    if (targetLayerName == layerName)
                    {
                        XYZ maxPo = polyLine.GetOutline().MaximumPoint;
                        XYZ minPo = polyLine.GetOutline().MinimumPoint;
                        XYZ midPo = GetMidPoint(maxPo, minPo);
                        ColumnData ColumnsData = new ColumnData(midPo);
                        Columns.Add(ColumnsData);
                    }

                }
                logText.Text += "target poly lines is ready\n";
            }
            catch (Exception)
            {

                logText.Text += "Error While Loading polylines\n";
            }
           
        }

        private XYZ GetMidPoint(XYZ maxPoint, XYZ minPoint)
        {
            var MidPointX = (maxPoint.X + minPoint.X) / 2;
            var MidPointY = (maxPoint.Y + minPoint.Y) / 2;
            var MidPointZ = (maxPoint.Z + minPoint.Z) / 2;

            return new XYZ(MidPointX, MidPointY, MidPointZ);
        }

        private void Generate_Button_Click(object sender, RoutedEventArgs e)
        {
            //Generate Elements
            string selected_LayerName = layerBox.SelectedItem.ToString();
            string selected_columnType = ColumnTypeBox.SelectedItem.ToString();
            string selected_TopLevel = topLevelBox.SelectedItem.ToString();
            string selected_bottomlevel = bottomLevelBox.SelectedItem.ToString();
            var topLevel = new FilteredElementCollector(Document)
                .OfClass(typeof(Level))
                .WhereElementIsNotElementType().FirstOrDefault(x => x.Name == selected_TopLevel) as Level;
            double topLevelEleveation = topLevel.Elevation;
            var baseLevel = new FilteredElementCollector(Document)
               .OfClass(typeof(Level))
               .WhereElementIsNotElementType().FirstOrDefault(x => x.Name == selected_bottomlevel) as Level;
            double bottomLevelEleveation = baseLevel.Elevation;
            var columnFamilysympol = new FilteredElementCollector(Document)
               .OfCategory(BuiltInCategory.OST_StructuralColumns)
               .WhereElementIsElementType().Cast<FamilySymbol>().FirstOrDefault(x => x.Name == selected_columnType);
            
            Helper.ColumnTypeH = columnFamilysympol;
            Helper.bottomLevelH = baseLevel;
            Helper.topLevelElevationH = topLevelEleveation;
            Helper.botLevelElevationH = bottomLevelEleveation;
            Helper.documentH = Document;
            Helper.columnsH = Columns;
            externalEvent.Raise();




        }
    }
}
