using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;

namespace MyGIS
{
    class GISVertex
    {
        public double x;
        public double y;
        public GISVertex(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
        public double Distance(GISVertex vertex)
        {
            return Math.Sqrt((x - vertex.x) * (x - vertex.x) + (y - vertex.y) * (y - vertex.y));
        }
        public void CopyFrom(GISVertex vertex)
        {
            x = vertex.x;
            y = vertex.y;
        }
    }
    class GISFeature
    {
        public GISSpatial spatial;
        public GISAttribute attribute;
        public GISFeature(GISSpatial spatial, GISAttribute attribute)
        {
            this.spatial = spatial;
            this.attribute = attribute;
        }
        public void Draw(Graphics graphics, GISView view, bool shouldDrawAttribute, int index)
        {
            spatial.Draw(graphics, view);
            if (shouldDrawAttribute)
                attribute.Draw(graphics, view, this.spatial.centroid, index);

        }
        public object GetAttribute(int index)
        {
            return this.attribute.GetValue(index);
        }

    }
    class GISAttribute
    {
        ArrayList attributes = new ArrayList();
        public void AddValue(object o)
        {
            attributes.Add(o);
        }
        public object GetValue(int index)
        {
            return attributes[index];
        }
        public void Draw(Graphics graphics, GISView view, GISVertex location, int index)
        {
            Point screenPoint = new Point((int)location.x, (int)location.y);
            graphics.DrawString(attributes[index].ToString(),
                new Font("宋体", 20),
                new SolidBrush(Color.Green),
                new PointF(screenPoint.X, screenPoint.Y));
        }
    }
    abstract class GISSpatial
    {
        public GISVertex centroid;
        public GISExtent extent;
        public abstract void Draw(Graphics graphics, GISView view);
    }
    class GISExtent
    {
        double zoomingFactor = 2;
        double movingFactor = 0.25;
        public void ChangeExtent(GISMapActions action)
        {
            double
            minX = bottomLeft.x,
            minY = bottomLeft.y,
            maxX = topRight.x,
            maxY = topRight.y;
            switch (action)
            {
                case GISMapActions.ZoomIn:
                    minX = (GetMinX() + GetMaxX() - GetWidth() / zoomingFactor) / 2;
                    minY = (GetMinY() + GetMaxY() - GetHeight() / zoomingFactor) / 2;
                    maxX = (GetMinX() + GetMaxX() + GetWidth() / zoomingFactor) / 2;
                    maxY = (GetMinY() + GetMaxY() + GetHeight() / zoomingFactor) / 2;
                    break;
                case GISMapActions.ZoomOut:
                    minX = (GetMinX() + GetMaxX() - GetWidth() * zoomingFactor) / 2;
                    minY = (GetMinY() + GetMaxY() - GetHeight() * zoomingFactor) / 2;
                    maxX = (GetMinX() + GetMaxX() + GetWidth() * zoomingFactor) / 2;
                    maxY = (GetMinY() + GetMaxY() + GetHeight() * zoomingFactor) / 2;
                    break;
                case GISMapActions.MoveUp:
                    minY = GetMinY() - GetHeight() * movingFactor;
                    maxY = GetMaxY() - GetHeight() * movingFactor;
                    break;
                case GISMapActions.MoveDown:
                    minY = GetMinY() + GetHeight() * movingFactor;
                    maxY = GetMaxY() + GetHeight() * movingFactor;
                    break;
                case GISMapActions.MoveLeft:
                    minX = GetMinX() + GetWidth() * movingFactor;
                    maxX = GetMaxX() + GetWidth() * movingFactor;
                    break;
                case GISMapActions.MoveRight:
                    minX = GetMinX() - GetWidth() * movingFactor;
                    maxX = GetMaxX() - GetWidth() * movingFactor;
                    break;
            }
            topRight.x = maxX;
            topRight.y = maxY;
            bottomLeft.x = minX;
            bottomLeft.y = minY;
        }
        public GISVertex bottomLeft;
        public GISVertex topRight;
        public GISExtent(GISVertex bottomLeft, GISVertex topRight)
        {
            this.bottomLeft = bottomLeft;
            this.topRight = topRight;
        }
        public GISExtent(double minX, double minY, double maxX, double maxY)
        {
            //地图的显示范围
            topRight = new GISVertex(Math.Max(minX, maxX), Math.Max(maxY, minY));
            bottomLeft = new GISVertex(Math.Min(minX, maxX), Math.Min(minY, maxY));
        }
        public double GetMinX()
        {
            return bottomLeft.x;
        }
        public double GetMinY()
        {
            return bottomLeft.y;
        }
        public double GetMaxX()
        {
            return topRight.x;
        }
        public double GetMaxY()
        {
            return topRight.y;
        }
        public double GetWidth()
        {
            return topRight.x - bottomLeft.x;
        }
        public double GetHeight()
        {
            return topRight.y - bottomLeft.y;
        }

        public void CopyFrom(GISExtent extent)
        {
            topRight.CopyFrom(extent.topRight);
            bottomLeft.CopyFrom(extent.bottomLeft);
        }
    }
    class GISView
    {
        GISExtent currentMapExtent;
        Rectangle mapWindowSize;
        double mapMinX, mapMinY;
        int windowWidth, windowHeight;
        double mapWidth, mapHeight;
        double scaleX, scaleY;
        public GISView(GISExtent extent, Rectangle rectangle)
        {
            Update(extent, rectangle);
        }
        public void Update(GISExtent extent, Rectangle rectangle)
        {
            currentMapExtent = extent;
            mapWindowSize = rectangle;
            mapMinX = currentMapExtent.GetMinX();
            mapMinY = currentMapExtent.GetMinY();
            windowWidth = mapWindowSize.Width;
            windowHeight = mapWindowSize.Height;
            mapWidth = currentMapExtent.GetWidth();
            mapHeight = currentMapExtent.GetHeight();
            scaleX = mapWidth / windowWidth;
            scaleY = mapHeight / windowHeight;
        }
        public Point ToScreenPoint(GISVertex vertex)
        {
            double ScreenX = (vertex.x - mapMinX) / scaleX;
            double ScreenY = windowHeight - (vertex.y - mapMinY) / scaleY;
            return new Point((int)ScreenX, (int)ScreenY);

        }
        public GISVertex ToMapVertex(Point point)
        {
            double mapX = scaleX * point.X + mapMinX;
            double mapY = scaleY * (windowHeight - point.Y) + mapMinY;
            return new GISVertex(mapX, mapY);
        }
        public void ChangeView(GISMapActions action)
        {
            currentMapExtent.ChangeExtent(action);
            Update(currentMapExtent, mapWindowSize);
        }

        public void UpdateExtent(GISExtent extent)
        {
            currentMapExtent.CopyFrom(extent);
            Update(currentMapExtent, mapWindowSize);
        }
    }
    enum GISMapActions
    {
        ZoomIn, ZoomOut,
        MoveUp, MoveDown, MoveLeft, MoveRight
    };
    class GISPoint : GISSpatial
    {
        public GISPoint(GISVertex vertex)
        {
            centroid = vertex;
            extent = new GISExtent(vertex, vertex);
        }
        public override void Draw(Graphics graphics, GISView view)
        {
            Point screenPoint = view.ToScreenPoint(centroid);
            graphics.FillEllipse(new SolidBrush(Color.Red),
                new Rectangle((int)(centroid.x) - 3, (int)(centroid.y) - 3, 6, 6));

        }
        public double Distance(GISVertex vertex)
        {
            return centroid.Distance(vertex);
        }
    }
    class GISLine : GISSpatial
    {
        List<GISVertex> vertices;
        public double length;
        public GISLine(List<GISVertex> vertices)
        {
            this.vertices = vertices;
            centroid = GISTools.CalculateCentroid(vertices);
            extent = GISTools.CalculateExtent(vertices);
            length = GISTools.CalculateLength(vertices);
        }

        public override void Draw(Graphics graphics, GISView view)
        {
            Point[] points = GISTools.GetScreenPoints(vertices, view);
            graphics.DrawLines(new Pen(Color.Red, 2), points);
        }

        public GISVertex FromNode()
        {
            return vertices[0];
        }

        public GISVertex ToNode()
        {
            return vertices[vertices.Count - 1];
        }
    }
    class GISPolygon : GISSpatial
    {
        List<GISVertex> vertices;
        public double area;
        public GISPolygon(List<GISVertex> vertices)
        {
            this.vertices = vertices;
            centroid = GISTools.CalculateCentroid(vertices);
            extent = GISTools.CalculateExtent(vertices);
            area = GISTools.CalculateArea(vertices);
        }
        public override void Draw(Graphics graphics, GISView view)
        {
            Point[] points = GISTools.GetScreenPoints(vertices, view);
            graphics.FillPolygon(new SolidBrush(Color.Yellow), points);
            graphics.DrawPolygon(new Pen(Color.White, 2), points);
        }
    }
    class GISShapefile
    {
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct ShapefileHeader
        {
            public int unused1, unused2, unused3, unused4;
            public int unused5, unused6, unused7, unused8;
            public int shapeType;
            public double minX;
            public double minY;
            public double maxX;
            public double maxY;
            public double unused9, unused10, unused11, unused12;
        }

        ShapefileHeader ReadFileHeader(BinaryReader binaryReader)
        {
            byte[] buff = binaryReader.ReadBytes(Marshal.SizeOf(typeof(ShapefileHeader)));
            GCHandle handle = GCHandle.Alloc(buff, GCHandleType.Pinned);
            ShapefileHeader header = (ShapefileHeader)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(ShapefileHeader));
            handle.Free();

            return header;
        }

        GISPoint ReadPoint(byte[] recordContents)
        {
            double x = BitConverter.ToDouble(recordContents, 0);
            double y = BitConverter.ToDouble(recordContents, 8);
            return new GISPoint(new GISVertex(x, y));
        }

        public GISLayer ReadShapefile(string shapefileName)
        {
            FileStream fileStream = new FileStream(shapefileName, FileMode.Open);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            ShapefileHeader shapefileHeader = ReadFileHeader(binaryReader);
            ShapeType shapeType = (ShapeType)Enum.Parse(typeof(ShapeType), shapefileHeader.ToString());
            GISExtent extent = new GISExtent(shapefileHeader.minX, shapefileHeader.minY, shapefileHeader.maxX, shapefileHeader.maxY);
            GISLayer layer = new GISLayer(shapefileName, shapeType, extent);

            while (binaryReader.PeekChar() != -1)
            {
                RecordHeader recordHeader = ReadRecordHeader(binaryReader);
                int recordLength = FromBigToLittle(recordHeader.recordLength) * 2 - 4;
                byte[] recordContents = binaryReader.ReadBytes(recordLength);
                if (shapeType == ShapeType.Point)
                {
                    GISPoint point = ReadPoint(recordContents);
                    GISFeature feature = new GISFeature(point, new GISAttribute());
                    layer.AddFeature(feature);
                }

                if (shapeType == ShapeType.Line)
                {
                    List<GISLine> lines = ReadLines(recordContents);
                    for (int i = 0; i < lines.Count; i++)
                    {
                        GISFeature feature = new GISFeature(lines[i], new GISAttribute());
                        layer.AddFeature(feature);
                    }
                }
                if (shapeType == ShapeType.Polygon)
                {
                    List<GISPolygon> polygons = ReadPolygons(recordContents);
                    for (int i = 0; i < polygons.Count; i++)
                    {
                        GISFeature feature = new GISFeature(polygons[i], new GISAttribute());
                        layer.AddFeature(feature);
                    }

                }
            }

            binaryReader.Close();
            fileStream.Close();
            return layer;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        struct RecordHeader
        {
            public int recordNumber;
            public int recordLength;
            public int shapeType;
        }

        RecordHeader ReadRecordHeader(BinaryReader binaryReader)
        {
            byte[] buff = binaryReader.ReadBytes(Marshal.SizeOf(typeof(RecordHeader)));
            GCHandle handle = GCHandle.Alloc(buff, GCHandleType.Pinned);
            RecordHeader header = (RecordHeader)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(ShapefileHeader));
            handle.Free();

            return header;
        }

        int FromBigToLittle(int value)
        {
            byte[] bytes = new byte[4];

            GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            Marshal.StructureToPtr(value, handle.AddrOfPinnedObject(), false);
            handle.Free();
            byte tmp1 = bytes[2];
            byte tmp2 = bytes[3];
            bytes[3] = bytes[0];
            bytes[2] = bytes[1];
            bytes[1] = tmp1;
            bytes[0] = tmp2;

            return BitConverter.ToInt32(bytes, 0);
        }

        List<GISLine> ReadLines(byte[] recordContent)
        {
            int n = BitConverter.ToInt32(recordContent, 32);
            int m = BitConverter.ToInt32(recordContent, 36);
            int[] parts = new int[n + 1];

            for (int i = 0; i < n; i++)
            {
                parts[i] = BitConverter.ToInt32(recordContent, 40 + i * 4);
            }

            parts[n] = m;

            List<GISLine> lines = new List<GISLine>();
            for (int i = 0; i < n; i++)
            {
                List<GISVertex> vertices = new List<GISVertex>();
                for (int j = parts[i]; j < parts[i + 1]; j++)
                {
                    double x = BitConverter.ToDouble(recordContent, 40 + n * 4 + j * 16);
                    double y = BitConverter.ToDouble(recordContent, 40 + n * 4 + j * 16 + 8);
                    vertices.Add(new GISVertex(x, y));
                }
                lines.Add(new GISLine(vertices));
            }

            return lines;
        }

        List<GISPolygon> ReadPolygons(byte[] recordContent)
        {
            int n = BitConverter.ToInt32(recordContent, 32);
            int m = BitConverter.ToInt32(recordContent, 36);
            int[] parts = new int[n + 1];

            for (int i = 0; i < n; i++)
            {
                parts[i] = BitConverter.ToInt32(recordContent, 40 + i * 4);
            }

            parts[n] = m;

            List<GISPolygon> polygons = new List<GISPolygon>();

            for (int i = 0; i < n; i++)
            {
                List<GISVertex> vertices = new List<GISVertex>();
                for (int j = parts[i]; j < parts[i  + 1]; j++)
                {
                    double x = BitConverter.ToDouble(recordContent, 40 + n * 4 + j * 16);
                    double y = BitConverter.ToDouble(recordContent, 40 + n * 4 + j * 16 + 8);
                    vertices.Add(new GISVertex(x, y));
                }
                polygons.Add(new GISPolygon(vertices));
            }

            return polygons;
        }
    }

    enum ShapeType
    {
        Point = 1,
        Line = 3,
        Polygon = 5
    }

    class GISLayer
    {
        public string name;
        public GISExtent extent;
        public bool shouldDrawAttribute;
        public int labelIndex;
        public ShapeType shapeType;
        List<GISFeature> features = new List<GISFeature>();

        public GISLayer(string name, ShapeType shapeType, GISExtent extent)
        {
            this.name = name;
            this.shapeType = shapeType;
            this.extent = extent;
        }

        public void Draw(Graphics graphics, GISView view)
        {
            for (int i = 0; i < features.Count; i++)
            {
                features[i].Draw(graphics, view, shouldDrawAttribute, labelIndex);
            }
        }

        public void AddFeature(GISFeature feature)
        {
            features.Add(feature);
        }

        public int FeatureCount()
        {
            return features.Count;
        }
    }

    class GISTools
    {
        public static GISVertex CalculateCentroid(List<GISVertex> vertices)
        {
            if (vertices.Count == 0)
            {
                return null;
            }
            double x = 0;
            double y = 0;
            for (int i = 0; i < vertices.Count; i++)
            {
                x += vertices[i].x;
                y += vertices[i].y;
            }

            return new GISVertex(x / vertices.Count, y / vertices.Count);
        }

        public static GISExtent CalculateExtent(List<GISVertex> vertices)
        {
            if (vertices.Count == 0)
            {
                return null;
            }

            double minX = double.MaxValue;
            double minY = double.MaxValue;
            double maxX = double.MinValue;
            double maxY = double.MinValue;

            for (int i = 0; i < vertices.Count; i++)
            {
                if (vertices[i].x < minX)
                {
                    minX = vertices[i].x;
                }
                if (vertices[i].y < minY)
                {
                    minY = vertices[i].y;
                }
                if (vertices[i].x > maxX)
                {
                    maxX = vertices[i].x;
                }
                if (vertices[i].y > maxY)
                {
                    maxY = vertices[i].y;
                }
            }
            return new GISExtent(minX, minY, maxX, maxY);
            
        }

        public static double CalculateLength(List<GISVertex> vertices)
        {
            double length = 0;
            for (int i = 0; i < vertices.Count - 1; i++)
            {
                length += vertices[i].Distance(vertices[i + 1]);
            }

            return length;
        }

        public static double CalculateArea(List<GISVertex> vertices)
        {
            double area = 0;
            for (int i = 0; i < vertices.Count - 1; i++)
            {
                area += VectorProduct(vertices[i], vertices[i + 1]);
            }
            area += VectorProduct(vertices[vertices.Count - 1], vertices[0]);

            return area / 2;
        }

        public static double VectorProduct(GISVertex vertex1, GISVertex vertex2)
        {
            return vertex1.x * vertex2.y - vertex1.y * vertex2.x;
        }

        public static Point[] GetScreenPoints(List<GISVertex> vertices, GISView view)
        {
            Point[] points = new Point[vertices.Count];
            for (int i = 0; i < points.Length; i++)
            {
                points[i] = view.ToScreenPoint(vertices[i]);
            }

            return points;
        }
    }
}
