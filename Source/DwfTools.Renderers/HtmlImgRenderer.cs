using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DwfTools.W2d.Opcodes;
using System.Drawing;

namespace DwfTools.Renderer
{
    public class HtmlImgRenderer
    {
        public void Render(IEnumerable<IOpcode> opcodes)
        {
            var rendering = new HtmlImgRendering();
            rendering.Render(opcodes);
        }

        private class HtmlImgRendering
        {
            bool currentIsVisible = true;

            Color currentColor = Color.Black;

            bool absoluteCoordinates;

            Point currentPoint = Point.Zero;

            StringBuilder outputString = new StringBuilder();
            
            public void Render(IEnumerable<IOpcode> opcodes)
            {
                outputString.AppendLine(@"<html>");
                outputString.AppendLine(@"<head>");
                outputString.AppendLine(@"</head>");

                outputString.AppendLine(@"<body>");
                outputString.AppendLine("<img src=\"01_Model.png\"/>");
                
                foreach (var opcode in opcodes)
                {
                    Render(opcode);
                }

                Console.WriteLine(minPoint);

                outputString.AppendLine(@"</body>");
                outputString.AppendLine(@"</html>");
                
                //write the svg file
                using (var file = File.CreateText(@"C:\Development\DWFToolkit-7.7\tests\" + Guid.NewGuid() + ".html"))
                {
                    file.Write(outputString.ToString());
                    file.Flush();
                    file.Close();
                }
            }

            Point currentGeometryStartPoint = Point.Zero;
            Point currentGeometryEndPoint = Point.Zero;

            Dictionary<long, Tuple<string, string>> urlsDictionary = new Dictionary<long, Tuple<string, string>>();

            List<Tuple<string, string>> currentUrls = new List<Tuple<string, string>>();

            Point minPoint;
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
                else if (opcode is DrawPolylineShort)
                {
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
                else if (opcode is DrawTextBasic)
                {
                    var op = (DrawTextBasic)opcode;
                    DrawTextBasic(op.Position, op.Text);
                }
                else if (opcode is SetColorIndex)
                {
                    var op = (SetColorIndex)opcode;
                    currentColor = DefaultColorMap.Default[op.Index];
                }
                else if (opcode is SetUrlLink)
                {
                    var op = (SetUrlLink)opcode;

                    if (currentUrls.Count > 0)
                    {
                        string divText = "<div style=\"background-color:pink; position:absolute; top:{0}px; left: {1}px; width: {2}px; height: {3}px;\"></div>";
                        outputString.Append(string.Format(divText, 
                                                          currentGeometryStartPoint.Y,
                                                          currentGeometryStartPoint.X,
                                                          Math.Max(10, currentGeometryEndPoint.X  - currentGeometryStartPoint.X),
                                                          Math.Max(10, currentGeometryEndPoint.Y - currentGeometryStartPoint.Y)));
                    }

                    currentUrls.Clear();
                    currentGeometryStartPoint = Point.Zero;
                    currentGeometryEndPoint = Point.Zero;

                    if (op.Index != 0)
                    {
                        currentUrls = new List<Tuple<string,string>> { urlsDictionary[op.Index] };
                        return;
                    }
                    else if (op.Links != null && op.Links.Length > 0)
                    {
                        foreach (var link in op.Links)
                        {
                            urlsDictionary[link.Index] = Tuple.Create(link.Address, link.Name);

                            currentUrls.Add(urlsDictionary[link.Index]);
                        }
                                              
                        return;
                    }

                }
                else if (opcode is UnrecognizedOpcode)
                {
                    var op = (UnrecognizedOpcode)opcode;

                    Console.WriteLine("unrecognized opcode:" + op.OpcodeId);
                }
                else
                {
                    Console.WriteLine("unimplemented opcode:" + opcode);
                }
            }

            private void DrawTextBasic(Point point, string text)
            {
                GetTranslatedPoint(point);
            }
            
            private void DrawPolymarker(Point[] points)
            {
                foreach (var point in points)
                {
                    GetTranslatedPoint(point);
                }
            }

            public void DrawLine(Point point1, Point point2)
            {           
                point1 = GetTranslatedPoint(point1);
                point2 = GetTranslatedPoint(point2);
            }

            public void DrawPolyline(Point[] points)
            {
                foreach (var point in points)
                {
                    GetTranslatedPoint(point);
                }
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

                if (currentColor != Color.FromArgb(255,0,0,0))
                {
                    var translatedCurrentPoint = new Point(currentPoint.X, -currentPoint.Y);
                    if (minPoint == Point.Zero)
                    {
                        minPoint = translatedCurrentPoint;
                    }
                    else
                    {
                        minPoint = new Point(Math.Min(minPoint.X, translatedCurrentPoint.X),
                                             Math.Min(minPoint.Y, translatedCurrentPoint.Y));
                    }
                }

                //REMARKS: Remember, Y coordinate is inverted
                //scale (x2 - x1) / (x2p - x1p)

                Point a1 = new Point(52853, -53837);
                Point a2 = new Point(217648, -39955);
       
                Point a1p = new Point(98, 844);
                Point a2p = new Point(9865, 2129);

                decimal xScaleFactor = (a2p.X - a1p.X) / (decimal)(a2.X - a1.X);
                decimal yScaleFactor = (a2p.Y - a1p.Y) / (decimal)(a2.Y - a1.Y);

                decimal displacementX = a1p.X - (a1.X * xScaleFactor);
                decimal displacementY = a1p.Y - (a1.Y * yScaleFactor);

                //var translatedPoint = new Point(point.X, -point.Y);
                 var translatedPoint = new Point((long)(displacementX + (point.X * xScaleFactor)), (long)(displacementY + (-point.Y * yScaleFactor)));

                CheckCurrentUrlsPoints(translatedPoint);
                return translatedPoint;
            }

            private void CheckCurrentUrlsPoints(Point point)
            {
                if (currentUrls.Count == 0) return;

                if (currentGeometryStartPoint == Point.Zero)
                {
                    currentGeometryStartPoint = point; 
                }

                if (currentGeometryEndPoint == Point.Zero)
                {
                    currentGeometryEndPoint = point;
                }

                currentGeometryStartPoint = new Point(Math.Min(currentGeometryStartPoint.X, point.X),
                                                      Math.Min(currentGeometryStartPoint.Y, point.Y));

                currentGeometryEndPoint = new Point(Math.Max(currentGeometryStartPoint.X, point.X),
                                                    Math.Max(currentGeometryStartPoint.Y, point.Y));
            }
        }
    }
}
