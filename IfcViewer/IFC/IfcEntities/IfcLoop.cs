using System.Collections.Generic;

namespace IfcViewer.IFC
{
    public class IfcLoop
    {
        public List<int> PointIds { get; }

        public IfcLoop(List<int> pointIds)
        {
            PointIds = pointIds;
        }
    }
}
