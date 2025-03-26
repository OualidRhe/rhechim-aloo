using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;

namespace IfcViewer
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Build a simple triangle manually
            var builder = new MeshBuilder();
            builder.AddTriangle(
                new Point3D(0, 0, 0),
                new Point3D(1, 0, 0),
                new Point3D(0, 1, 0)
            );

            var mesh = builder.ToMesh();

            // Create a red material
            var material = new DiffuseMaterial(new SolidColorBrush(Colors.Red));

            // Build model and assign back material
            var model = new GeometryModel3D(mesh, material)
            {
                BackMaterial = material
            };

            // Wrap into ModelVisual3D
            var visual = new ModelVisual3D { Content = model };
            viewport.Children.Add(visual);

            // Adjust camera to show the mesh
            viewport.ZoomExtents();
        }
    }
}
