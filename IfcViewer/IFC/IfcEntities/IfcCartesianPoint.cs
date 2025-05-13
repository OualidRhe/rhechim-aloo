namespace IfcViewer.IFC
{
    public class IfcCartesianPoint
    {
        public double X { get; }
        public double Y { get; }
        public double Z { get; }

        public IfcCartesianPoint(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}
