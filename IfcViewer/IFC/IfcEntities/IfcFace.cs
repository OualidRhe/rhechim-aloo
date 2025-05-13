using System.Collections.Generic;

namespace IfcViewer.IFC
{
    public class IfcFace
    {
        public List<int> BoundIds { get; }

        public IfcFace(List<int> boundIds)
        {
            BoundIds = boundIds;
        }
    }
}
