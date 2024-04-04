using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAD_2_REVIT
{
    internal class DWGAnalysis
    {
       
        Element DWG = null;
        Document Document = null;
        List<Line> DWG_Lines = new List<Line>();
        List<Solid> DWG_Solids = new List<Solid>();
        List<Arc> DWG_ARCS = new List<Arc>();
        public List<PolyLine> DWG_PloyLines = new List<PolyLine>();


        public DWGAnalysis(Document doc, Element dwg)
        {
            //00-DWG Import Instance
            Document = doc;
            DWG = dwg;
            //01- Get Geometry Element using get geometry 
            GeometryElement DWG_GEO_ELE = DWG.get_Geometry(new Options());
            //02- Get Main Geometry Opjects Lists using get Geometry Instance
            List<GeometryObject> DWG_GEO_MAIN_OBJECTS = new List<GeometryObject>();

            foreach (GeometryObject M_GEO_OBJ in DWG_GEO_ELE)
            {
                DWG_GEO_MAIN_OBJECTS.Add(M_GEO_OBJ);
            }
            //03- Get Geometry Instance From Geometry Opjects and Sebrate them to lines , Arc , solid , Polyline
            foreach (GeometryObject GEO_OBJ in DWG_GEO_MAIN_OBJECTS)
            {
                if (GEO_OBJ is GeometryInstance)
                {
                    GeometryInstance inst = (GeometryInstance)GEO_OBJ;
                    foreach (GeometryObject geo_obj in inst.GetInstanceGeometry())
                    {
                        if (geo_obj is Line) { Line l = geo_obj as Line; DWG_Lines.Add(l); }
                        else if (geo_obj is Arc) { Arc arc = geo_obj as Arc; DWG_ARCS.Add(arc); }
                        else if (geo_obj is PolyLine) { PolyLine poly = geo_obj as PolyLine; DWG_PloyLines.Add(poly); }
                        else if (geo_obj is Solid) { Solid solid = geo_obj as Solid; DWG_Solids.Add(solid); }
                    }

                }
            }


        }

    }

}