using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using DwfTools.W2d.Opcodes;

namespace DwfTools.Renderer
{
    public class HtmlSvgRenderer
    {
        public void Render(IEnumerable<IOpcode> opcodes)
        {
            var rendering = new HtmlSvgRendering();
            rendering.Render(opcodes);
        }

        private class HtmlSvgRendering
        {
            bool currentFillMode = false;

            bool currentIsVisible = true;

            bool absoluteCoordinates = false;

            Point currentPoint = Point.Zero;

            StringBuilder svgString = new StringBuilder();

            int currentLineWeight = 0;

            Color currentColor = Color.Black;

            List<Color> indexedColors = DefaultColorMap.Default.ToList();

            int polyCount = 0;

            public void Render(IEnumerable<IOpcode> opcodes)
            {
                svgString.AppendLine(@"<html>");
                svgString.AppendLine(@"<head>");
                svgString.AppendLine("<script type=\"text/javascript\" src=\"svgpan.js\"></script>");
                svgString.AppendLine("<script type=\"text/javascript\">function msg() { alert(\"Location clicked\") }</script>");
                svgString.AppendLine(@"</head>");
                
                svgString.AppendLine(@"<body>");
                svgString.AppendLine("<svg xmlns=\"http://www.w3.org/2000/svg\" version=\"1.1\">");
                //svgString.AppendLine("<g id=\"viewport\" transform=\"scale(0.005) translate(730,687)\">");
                svgString.AppendLine("<g id=\"viewport\" transform=\"matrix(0.006,0,0,0.006,-214,695)\">");
                
                foreach (var opcode in opcodes)
                {
                    Render(opcode);
                }

                svgString.AppendLine(@"</g>");
                svgString.AppendLine(@"</svg>");
                svgString.AppendLine(@"</body>");
                svgString.AppendLine(@"</html>");
                
                //write the svg file
                using (var file = File.CreateText(@"C:\Development\DWFToolkit-7.7\tests\" + Guid.NewGuid() + ".html"))
                {
                    file.Write(svgString.ToString());
                    file.Flush();
                    file.Close();
                }
            }
            
            void Render(IOpcode opcode)
            {
                //set coordinates type
                if (opcode.CoordinatesType == CoordinatesType.Relative)
                {
                    absoluteCoordinates = false;
                }
                else
                {
                    absoluteCoordinates = true;
                }


                if (opcode is SetVisibillityOn)
                {
                    currentIsVisible = true;
                }
                else if (opcode is SetCurrentPoint)
                {
                    var op = (SetCurrentPoint)opcode;
                    currentPoint = new Point(op.X, op.Y);
                }
                else if (opcode is SetLineWeight)
                {
                    //var op = (SetLineWeight)opcode;
                    //currentLineWeight = op.Weight / 20; //TODO: Just some constant to try and get a sane line weight
                }
                else if (opcode is DrawPolylineShort)
                {
                    polyCount++;
                    var op = (DrawPolylineShort)opcode;
                    DrawPolyline(op.Points);
                }
                else if (opcode is DrawPolylineLong)
                {
                    var op = (DrawPolylineLong)opcode;
                    DrawPolyline(op.Points);
                }
                else if (opcode is DrawPolymarkerShort)
                {
                    var op = (DrawPolymarkerShort)opcode;
                    DrawPolymarker(op.Points);
                }
                else if (opcode is DrawPolymarkerLong)
                {
                    var op = (DrawPolymarkerLong)opcode;
                    DrawPolymarker(op.Points);
                }
                else if (opcode is DrawLineShort)
                {
                    var op = (DrawLineShort)opcode;
                    DrawLine(op.StartingPoint, op.EndPoint);
                }
                else if (opcode is DrawLineLong)
                {
                    var op = (DrawLineLong)opcode;
                    DrawLine(op.StartingPoint, op.EndPoint);
                }
                else if (opcode is SetVisibillityOff)
                {
                    currentIsVisible = false;
                }
                else if (opcode is SetColorIndex)
                {
                    var op = (SetColorIndex)opcode;
                    SetColorIndex(op.Index);
                }
                else if (opcode is SetColorRgba)
                {
                    var op = (SetColorRgba)opcode;
                    SetColorRgba(Color.FromArgb(op.A, op.R, op.G, op.B));
                }
                else if (opcode is DrawTextBasic)
                {
                    var op = (DrawTextBasic)opcode;
                    DrawTextBasic(op.Position, op.Text);
                }
                else
                {
                    var op = (UnrecognizedOpcode)opcode;

                    if (op.OpcodeId == "") return;
                    if (op.OpcodeId == "URL") return;

                    Console.WriteLine("unrecognized opcode:" + op.OpcodeId);

                    return;
                }
            }

            private void DrawTextBasic(Point point, string text)
            {
                var absolutePoint = GetTranslatedPoint(point);
                var textElement = "<text x=\"{0}\" y=\"{1}\" font-family=\"Verdana\" font-size=\"80\" fill=\"blue\">{2}</text>";
                AppendTextToOutput(string.Format(textElement, absolutePoint.X, absolutePoint.Y, text));
            }
            
            private void DrawPolymarker(Point[] points)
            {
                //TODO: actually draw the polymarkers
                foreach (var point in points)
                {
                    var absolutePoint = GetTranslatedPoint(point);
                    var pointText = "<circle cx=\"{0}\" cy=\"{1}\" r=\"10\" {2} />";
                    AppendTextToOutput(string.Format(pointText, absolutePoint.X, absolutePoint.Y, GetCurrentStyle()));
                }
            }

            public void DrawLine(Point point1, Point point2)
            {
                const string line = "<line x1=\"{0}\" y1=\"{1}\" x2=\"{2}\" y2=\"{3}\" {4} />";
                                
                point1 = GetTranslatedPoint(point1);
                point2 = GetTranslatedPoint(point2);
                AppendTextToOutput(string.Format(line, point1.X, point1.Y, point2.X, point2.Y, GetCurrentStyle()));
            }

            public void DrawPolyline(Point[] points)
            {
                const string polyline = "<polyline points=\"{0}\" {1} onclick=\"msg()\" />";

                var pointsString = new StringBuilder();
                foreach (var point in points)
                {
                    var absolutePoint = GetTranslatedPoint(point);
                    pointsString.Append(string.Format(" {0},{1}", absolutePoint.X, absolutePoint.Y));
                }

                AppendTextToOutput(string.Format(polyline, pointsString.ToString().Trim(), GetCurrentStyle()));
            }

            string GetCurrentStyle()
            {
                //TODO: don't know why, but for now its only working with a fixed stroke-width like so:
                string style = "style=\"fill:none;stroke:{0};stroke-width:10\"";

                var colorString = string.Format("rgb({0},{1},{2})", currentColor.R, currentColor.G, currentColor.B);
                return string.Format(style, colorString, currentLineWeight);
            }

            private void SetColorIndex(byte colorIndex)
            {
                if (indexedColors.Count <= colorIndex) return;

                currentColor = indexedColors[colorIndex];
            }

            private void SetColorRgba(Color color)
            {
                currentColor = color;
            }

            Point GetTranslatedPoint(Point point)
            {
                if (!absoluteCoordinates)
                {
                    //translate point to absolute coordinates
                    point = new Point(point.X + currentPoint.X,
                                      point.Y + currentPoint.Y);

                }

                currentPoint = point;

                //Invert Y coordinate
                return new Point(point.X, -point.Y);
            }

            private void AppendTextToOutput(string text)
            {
                //only output visible things
                if (!currentIsVisible) return;

                svgString.AppendLine(text);
            }
        }
    }
}
