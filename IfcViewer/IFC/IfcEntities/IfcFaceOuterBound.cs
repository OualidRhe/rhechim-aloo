namespace IfcViewer.IFC
{
    public class IfcFaceOuterBound
    {
        public int LoopId { get; }
        public bool Orientation { get; }

        public IfcFaceOuterBound(int loopId, bool orientation)
        {
            LoopId = loopId;
            Orientation = orientation;
        }
    }
}
