using System.Collections.Generic;

namespace IfcViewer.IFC
{
    public class IfcPolyline
    {
        public List<int> PointIds { get; }

        public IfcPolyline(List<int> pointIds)
        {
            PointIds = pointIds;
        }
    }
}