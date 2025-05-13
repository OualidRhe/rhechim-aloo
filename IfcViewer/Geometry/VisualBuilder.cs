using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;
using IfcViewer.IFC;

namespace IfcViewer.Geometry
{
    public static class VisualBuilder
    {
        public static GeometryModel3D BuildGeometryFromFaces(Dictionary<int, object> parsedEntities)
        {
            var builder = new MeshBuilder(false, false);

            // Collect all points
            var points = parsedEntities
                .Where(kv => kv.Value is IfcCartesianPoint)
                .ToDictionary(kv => kv.Key, kv => (IfcCartesianPoint)kv.Value);

            // Process faces
            foreach (var faceEntry in parsedEntities.Values.OfType<IfcFace>())
            {
                foreach (int boundId in faceEntry.BoundIds)
                {
                    if (!parsedEntities.TryGetValue(boundId, out var boundObj)) continue;
                    if (boundObj is not IfcFaceOuterBound bound) continue;

                    if (!parsedEntities.TryGetValue(bound.LoopId, out var loopObj)) continue;
                    if (loopObj is not IfcLoop loop) continue;

                    var loopPoints = new List<Point3D>();

                    foreach (var pid in loop.PointIds)
                    {
                        if (points.TryGetValue(pid, out var p))
                            loopPoints.Add(new Point3D(p.X, p.Y, p.Z));
                    }

                    if (loopPoints.Count < 3) continue;

                    // Simple triangle fan
                    for (int i = 1; i < loopPoints.Count - 1; i++)
                    {
                        builder.AddTriangle(loopPoints[0], loopPoints[i], loopPoints[i + 1]);
                    }
                }
            }

            var mesh = builder.ToMesh();
            var material = Materials.LightGray;

            var model = new GeometryModel3D(mesh, material);
            Console.WriteLine($"Final triangle count: {mesh.TriangleIndices.Count / 3}");

            return model;
        }
    }
}
