using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace IfcViewer.IFC
{
    public class IfcParser
    {
        // Stores entity number and its raw text
        private Dictionary<int, string> _rawEntities = new();

        // Stores parsed entities by type
        public Dictionary<int, object> ParsedEntities { get; private set; } = new();

        public void LoadFromFile(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("IFC file not found.", filePath);

            string[] lines = File.ReadAllLines(filePath);
            foreach (var line in lines)
            {
                ParseRawLine(line);
            }

            ParseEntities(); // After all raw lines are collected
        }

        private void ParseRawLine(string line)
        {
            // Example: #10=IFCCARTESIANPOINT((1.0,2.0,3.0));
            var match = Regex.Match(line, @"#(\d+)\s*=\s*(\w+)\((.+)\);");
            if (match.Success)
            {
                int id = int.Parse(match.Groups[1].Value);
                string rawContent = match.Value;
                _rawEntities[id] = rawContent;
            }
        }

        private void ParseEntities()
        {
            foreach (var kvp in _rawEntities)
            {
                int id = kvp.Key;
                string raw = kvp.Value;

                if (raw.Contains("IFCCARTESIANPOINT"))
                {
                    var point = ParseCartesianPoint(raw);
                    ParsedEntities[id] = point;
                }

                if (raw.Contains("IFCCARTESIANPOINT"))
                {
                    var point = ParseCartesianPoint(raw);
                    ParsedEntities[id] = point;
                }
                else if (raw.Contains("IFCPOLYLINE"))
                {
                    var polyline = ParsePolyline(raw);
                    ParsedEntities[id] = polyline;
                }
                else if (raw.Contains("IFCLOOP"))
                {
                    var loop = ParsePolyLoop(raw);
                    ParsedEntities[id] = loop;
                }
                else if (raw.Contains("IFCFACEOUTERBOUND"))
                {
                    var bound = ParseFaceOuterBound(raw);
                    ParsedEntities[id] = bound;
                }
                else if (raw.Contains("IFCFACE"))
                {
                    var face = ParseFace(raw);
                    ParsedEntities[id] = face;
                }


                // 
            }
        }

        private IfcCartesianPoint ParseCartesianPoint(string raw)
        {
            // Example: #10=IFCCARTESIANPOINT((1.0,2.0,3.0));
            var match = Regex.Match(raw, @"IFCCARTESIANPOINT\(\(\s*([0-9\.\-Ee, ]+)\s*\)\)");
            if (match.Success)
            {
                string[] coords = match.Groups[1].Value.Split(',');
                double x = double.Parse(coords[0], CultureInfo.InvariantCulture);
                double y = double.Parse(coords[1], CultureInfo.InvariantCulture);
                double z = coords.Length > 2 ? double.Parse(coords[2], CultureInfo.InvariantCulture) : 0;
                return new IfcCartesianPoint(x, y, z);
            }

            throw new FormatException("Invalid IFCCARTESIANPOINT format.");
        }
        private IfcPolyline ParsePolyline(string raw)
        {
            var match = Regex.Match(raw, @"IFCPOLYLINE\(\(\s*(#[\d\s,]+)\s*\)\)");
            if (match.Success)
            {
                string[] pointRefs = match.Groups[1].Value.Split(',', StringSplitOptions.TrimEntries);
                var pointIds = new List<int>();
                foreach (var refStr in pointRefs)
                    if (refStr.StartsWith("#"))
                        pointIds.Add(int.Parse(refStr[1..]));

                return new IfcPolyline(pointIds);
            }

            throw new FormatException("Invalid IFCPOLYLINE format.");
        }

        private IfcLoop ParseLoop(string raw)
        {
            var match = Regex.Match(raw, @"IFCLOOP\(\(\s*(#[\d\s,]+)\s*\)\)");
            if (match.Success)
            {
                string[] pointRefs = match.Groups[1].Value.Split(',', StringSplitOptions.TrimEntries);
                var pointIds = new List<int>();
                foreach (var refStr in pointRefs)
                    if (refStr.StartsWith("#"))
                        pointIds.Add(int.Parse(refStr[1..]));

                return new IfcLoop(pointIds);
            }

            throw new FormatException("Invalid IFCLOOP format.");
        }

        private IfcLoop ParsePolyLoop(string raw)
        {
            var match = Regex.Match(raw, @"IFCPOLYLOOP\(\(\s*(#[\d\s,]+)\s*\)\)");
            if (match.Success)
            {
                string[] pointRefs = match.Groups[1].Value.Split(',', StringSplitOptions.TrimEntries);
                var pointIds = new List<int>();
                foreach (var refStr in pointRefs)
                    if (refStr.StartsWith("#"))
                        pointIds.Add(int.Parse(refStr[1..]));

                return new IfcLoop(pointIds);
            }

            throw new FormatException("Invalid IFCPOLYLOOP format.");
        }


        private IfcFaceOuterBound ParseFaceOuterBound(string raw)
        {
            var match = Regex.Match(raw, @"IFCFACEOUTERBOUND\(\s*(#\d+),\s*\.(T|F)\.\)");
            if (match.Success)
            {
                int loopId = int.Parse(match.Groups[1].Value[1..]);
                bool orientation = match.Groups[2].Value == "T";
                return new IfcFaceOuterBound(loopId, orientation);
            }

            throw new FormatException("Invalid IFCFACEOUTERBOUND format.");
        }


        private IfcFace ParseFace(string raw)
        {
            var match = Regex.Match(raw, @"IFCFACE\(\(\s*(#[\d\s,]+)\s*\)\)");
            if (match.Success)
            {
                var boundRefs = match.Groups[1].Value.Split(',', StringSplitOptions.TrimEntries);
                var boundIds = new List<int>();
                foreach (var refStr in boundRefs)
                    if (refStr.StartsWith("#"))
                        boundIds.Add(int.Parse(refStr[1..]));

                return new IfcFace(boundIds);
            }

            throw new FormatException("Invalid IFCFACE format.");
        }


    }
}
