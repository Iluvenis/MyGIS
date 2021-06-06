using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;

namespace MyGIS
{
    enum ShapeType
    {
        Point = 1,
        Line = 3,
        Polygon = 5
    }
    enum MapActions
    {
        ZoomIn, ZoomOut,
        MoveUp, MoveDown, MoveLeft, MoveRight
    };

    class Feature
    {
        public Spatial spatial;
        public Attribute attribute;

        public Feature(Spatial spatial, Attribute attribute)
        {
            this.spatial = spatial;
            this.attribute = attribute;
        }

        public void Draw(Graphics graphics, View view, bool shouldDrawAttribute, int index)
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
    abstract class Spatial
    {
        public Vertex centroid;
        public Extent extent;

        public abstract void Draw(Graphics graphics, View view);
    }
    class Attribute
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
        public void Draw(Graphics graphics, View view, Vertex location, int index)
        {
            System.Drawing.Point screenPoint = view.ToScreenPoint(location);
            graphics.DrawString(attributes[index].ToString(),
                new Font("宋体", 20),
                new SolidBrush(Color.Green),
                new PointF(screenPoint.X, screenPoint.Y));
        }
    }

    class Vertex
    {
        public double x;
        public double y;

        public Vertex(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public double Distance(Vertex vertex)
        {
            return Math.Sqrt((x - vertex.x) * (x - vertex.x) + (y - vertex.y) * (y - vertex.y));
        }

        public void CopyFrom(Vertex vertex)
        {
            x = vertex.x;
            y = vertex.y;
        }
    }
    class Point : Spatial
    {
        public Point(Vertex vertex)
        {
            centroid = vertex;
            extent = new Extent(vertex, vertex);
        }

        public override void Draw(Graphics graphics, View view)
        {
            System.Drawing.Point screenPoint = view.ToScreenPoint(centroid);
            graphics.FillEllipse(new SolidBrush(Color.Red), new Rectangle((int)(screenPoint.X) - 3, (int)(screenPoint.Y) - 3, 6, 6));

        }

        public double Distance(Vertex vertex)
        {
            return centroid.Distance(vertex);
        }
    }
    class Line : Spatial
    {
        List<Vertex> vertices;
        public double length;

        public Line(List<Vertex> vertices)
        {
            this.vertices = vertices;
            centroid = Tools.CalculateCentroid(vertices);
            extent = Tools.CalculateExtent(vertices);
            length = Tools.CalculateLength(vertices);
        }

        public override void Draw(Graphics graphics, View view)
        {
            System.Drawing.Point[] points = Tools.GetScreenPoints(vertices, view);
            graphics.DrawLines(new Pen(Color.Red, 2), points);
        }

        public Vertex GetFromNode()
        {
            return vertices[0];
        }

        public Vertex GetToNode()
        {
            return vertices[vertices.Count - 1];
        }
    }
    class Polygon : Spatial
    {
        List<Vertex> vertices;
        public double area;

        public Polygon(List<Vertex> vertices)
        {
            this.vertices = vertices;
            centroid = Tools.CalculateCentroid(vertices);
            extent = Tools.CalculateExtent(vertices);
            area = Tools.CalculateArea(vertices);
        }

        public override void Draw(Graphics graphics, View view)
        {
            System.Drawing.Point[] points = Tools.GetScreenPoints(vertices, view);
            graphics.FillPolygon(new SolidBrush(Color.Yellow), points);
            graphics.DrawPolygon(new Pen(Color.White, 2), points);
        }
    }

    class Field
    {
        public Type dataType;
        public string name;

        public Field(Type dataType, string name)
        {
            this.dataType = dataType;
            this.name = name;
        }
    }

    class Extent
    {
        double zoomingFactor = 2;
        double movingFactor = 0.25;


        public Vertex bottomLeft;
        public Vertex topRight;

        public void ChangeExtent(MapActions action)
        {
            double
            minX = bottomLeft.x,
            minY = bottomLeft.y,
            maxX = topRight.x,
            maxY = topRight.y;
            switch (action)
            {
                case MapActions.ZoomIn:
                    minX = (GetMinX() + GetMaxX() - GetWidth() / zoomingFactor) / 2;
                    minY = (GetMinY() + GetMaxY() - GetHeight() / zoomingFactor) / 2;
                    maxX = (GetMinX() + GetMaxX() + GetWidth() / zoomingFactor) / 2;
                    maxY = (GetMinY() + GetMaxY() + GetHeight() / zoomingFactor) / 2;
                    break;
                case MapActions.ZoomOut:
                    minX = (GetMinX() + GetMaxX() - GetWidth() * zoomingFactor) / 2;
                    minY = (GetMinY() + GetMaxY() - GetHeight() * zoomingFactor) / 2;
                    maxX = (GetMinX() + GetMaxX() + GetWidth() * zoomingFactor) / 2;
                    maxY = (GetMinY() + GetMaxY() + GetHeight() * zoomingFactor) / 2;
                    break;
                case MapActions.MoveUp:
                    minY = GetMinY() - GetHeight() * movingFactor;
                    maxY = GetMaxY() - GetHeight() * movingFactor;
                    break;
                case MapActions.MoveDown:
                    minY = GetMinY() + GetHeight() * movingFactor;
                    maxY = GetMaxY() + GetHeight() * movingFactor;
                    break;
                case MapActions.MoveLeft:
                    minX = GetMinX() + GetWidth() * movingFactor;
                    maxX = GetMaxX() + GetWidth() * movingFactor;
                    break;
                case MapActions.MoveRight:
                    minX = GetMinX() - GetWidth() * movingFactor;
                    maxX = GetMaxX() - GetWidth() * movingFactor;
                    break;
            }
            topRight.x = maxX;
            topRight.y = maxY;
            bottomLeft.x = minX;
            bottomLeft.y = minY;
        }
        public Extent(Vertex bottomLeft, Vertex topRight)
        {
            this.bottomLeft = bottomLeft;
            this.topRight = topRight;
        }
        public Extent(double minX, double minY, double maxX, double maxY)
        {
            //地图的显示范围
            topRight = new Vertex(Math.Max(minX, maxX), Math.Max(maxY, minY));
            bottomLeft = new Vertex(Math.Min(minX, maxX), Math.Min(minY, maxY));
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

        public void CopyFrom(Extent extent)
        {
            topRight.CopyFrom(extent.topRight);
            bottomLeft.CopyFrom(extent.bottomLeft);
        }
    }
    class View
    {
        Extent currentMapExtent;
        Rectangle mapWindowSize;
        double mapMinX, mapMinY;
        int windowWidth, windowHeight;
        double mapWidth, mapHeight;
        double scaleX, scaleY;

        public View(Extent extent, Rectangle rectangle)
        {
            Update(extent, rectangle);
        }
        public void Update(Extent extent, Rectangle rectangle)
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
        public System.Drawing.Point ToScreenPoint(Vertex vertex)
        {
            double ScreenX = (vertex.x - mapMinX) / scaleX;
            double ScreenY = windowHeight - (vertex.y - mapMinY) / scaleY;
            return new System.Drawing.Point((int)ScreenX, (int)ScreenY);

        }
        public Vertex ToMapVertex(System.Drawing.Point point)
        {
            double mapX = scaleX * point.X + mapMinX;
            double mapY = scaleY * (windowHeight - point.Y) + mapMinY;
            return new Vertex(mapX, mapY);
        }
        public void ChangeView(MapActions action)
        {
            currentMapExtent.ChangeExtent(action);
            Update(currentMapExtent, mapWindowSize);
        }
        public void UpdateExtent(Extent extent)
        {
            currentMapExtent.CopyFrom(extent);
            Update(currentMapExtent, mapWindowSize);
        }
    }
    class Layer
    {
        public string name;
        public Extent extent;
        public bool shouldDrawAttribute;
        public int labelIndex;
        public ShapeType shapeType;
        List<Feature> features = new List<Feature>();
        public List<Field> fields;

        public Layer(string name, ShapeType shapeType, Extent extent, List<Field> fields)
        {
            this.name = name;
            this.shapeType = shapeType;
            this.extent = extent;
            this.fields = fields;
        }
        public Layer(string name, ShapeType shapeType, Extent extent)
        {
            this.name = name;
            this.shapeType = shapeType;
            this.extent = extent;
            fields = new List<Field>();
        }

        public void Draw(Graphics graphics, View view)
        {
            for (int i = 0; i < features.Count; i++)
            {
                features[i].Draw(graphics, view, shouldDrawAttribute, labelIndex);
            }
        }
        public void AddFeature(Feature feature)
        {
            features.Add(feature);
        }
        public int FeatureCount()
        {
            return features.Count;
        }
        public Feature GetFeature(int i)
        {
            return features[i];
        }
    }

    class Shapefile
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
        Point ReadPoint(byte[] recordContents)
        {
            double x = BitConverter.ToDouble(recordContents, 0);
            double y = BitConverter.ToDouble(recordContents, 8);
            return new Point(new Vertex(x, y));
        }
        public Layer ReadShapefile(string shapefileName)
        {
            FileStream fileStream = new FileStream(shapefileName, FileMode.Open);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            ShapefileHeader shapefileHeader = ReadFileHeader(binaryReader);
            ShapeType shapeType = (ShapeType)Enum.Parse(typeof(ShapeType), shapefileHeader.ToString());
            Extent extent = new Extent(shapefileHeader.minX, shapefileHeader.minY, shapefileHeader.maxX, shapefileHeader.maxY);
            string dbfFileName = shapefileName.Replace(".shp", ".dbf");
            DataTable table = ReadDBF(dbfFileName);
            Layer layer = new Layer(shapefileName, shapeType, extent, ReadFields(table));
            int rowIndex = 0;

            while (binaryReader.PeekChar() != -1)
            {
                RecordHeader recordHeader = ReadRecordHeader(binaryReader);
                int recordLength = FromBigToLittle(recordHeader.recordLength) * 2 - 4;
                byte[] recordContents = binaryReader.ReadBytes(recordLength);
                if (shapeType == ShapeType.Point)
                {
                    Point point = ReadPoint(recordContents);
                    Feature feature = new Feature(point, new Attribute());
                    layer.AddFeature(feature);
                }

                if (shapeType == ShapeType.Line)
                {
                    List<Line> lines = ReadLines(recordContents);
                    for (int i = 0; i < lines.Count; i++)
                    {
                        Feature feature = new Feature(lines[i], new Attribute());
                        layer.AddFeature(feature);
                    }
                }
                if (shapeType == ShapeType.Polygon)
                {
                    List<Polygon> polygons = ReadPolygons(recordContents);
                    for (int i = 0; i < polygons.Count; i++)
                    {
                        Feature feature = new Feature(polygons[i], new Attribute());
                        layer.AddFeature(feature);
                    }

                }
                rowIndex++;
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
        List<Line> ReadLines(byte[] recordContent)
        {
            int n = BitConverter.ToInt32(recordContent, 32);
            int m = BitConverter.ToInt32(recordContent, 36);
            int[] parts = new int[n + 1];

            for (int i = 0; i < n; i++)
            {
                parts[i] = BitConverter.ToInt32(recordContent, 40 + i * 4);
            }

            parts[n] = m;

            List<Line> lines = new List<Line>();
            for (int i = 0; i < n; i++)
            {
                List<Vertex> vertices = new List<Vertex>();
                for (int j = parts[i]; j < parts[i + 1]; j++)
                {
                    double x = BitConverter.ToDouble(recordContent, 40 + n * 4 + j * 16);
                    double y = BitConverter.ToDouble(recordContent, 40 + n * 4 + j * 16 + 8);
                    vertices.Add(new Vertex(x, y));
                }
                lines.Add(new Line(vertices));
            }

            return lines;
        }
        List<Polygon> ReadPolygons(byte[] recordContent)
        {
            int n = BitConverter.ToInt32(recordContent, 32);
            int m = BitConverter.ToInt32(recordContent, 36);
            int[] parts = new int[n + 1];

            for (int i = 0; i < n; i++)
            {
                parts[i] = BitConverter.ToInt32(recordContent, 40 + i * 4);
            }

            parts[n] = m;

            List<Polygon> polygons = new List<Polygon>();

            for (int i = 0; i < n; i++)
            {
                List<Vertex> vertices = new List<Vertex>();
                for (int j = parts[i]; j < parts[i + 1]; j++)
                {
                    double x = BitConverter.ToDouble(recordContent, 40 + n * 4 + j * 16);
                    double y = BitConverter.ToDouble(recordContent, 40 + n * 4 + j * 16 + 8);
                    vertices.Add(new Vertex(x, y));
                }
                polygons.Add(new Polygon(vertices));
            }

            return polygons;
        }
        static DataTable ReadDBF(string dbfFileName)
        {
            System.IO.FileInfo fileInfo = new FileInfo(dbfFileName);
            DataSet dataSet = null;
            string conectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Sourse=" + fileInfo.DirectoryName + ";Extended Properties=DBASE III";
            using (OleDbConnection connection = new OleDbConnection(conectionString))
            {
                var sql = "select * from " + fileInfo.Name;
                OleDbCommand command = new OleDbCommand(sql, connection);
                connection.Open();
                dataSet = new DataSet();
                OleDbDataAdapter dataAdapter = new OleDbDataAdapter(command);
                dataAdapter.Fill(dataSet);
            }

            return dataSet.Tables[0];
        }
        static List<Field> ReadFields(DataTable table)
        {
            List<Field> fields = new List<Field>();
            foreach (DataColumn column in table.Columns)
            {
                fields.Add(new Field(column.DataType, column.ColumnName));
            }

            return fields;
        }
        static Attribute ReadAttribute(DataTable table, int rowIndex)
        {
            Attribute attribute = new Attribute();
            DataRow row = table.Rows[rowIndex];
            for (int i = 0; i < table.Columns.Count; i++)
            {
                attribute.AddValue(row[i]);
            }

            return attribute;
        }
    }

    class Tools
    {
        public static Vertex CalculateCentroid(List<Vertex> vertices)
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

            return new Vertex(x / vertices.Count, y / vertices.Count);
        }
        public static Extent CalculateExtent(List<Vertex> vertices)
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
            return new Extent(minX, minY, maxX, maxY);

        }
        public static double CalculateLength(List<Vertex> vertices)
        {
            double length = 0;
            for (int i = 0; i < vertices.Count - 1; i++)
            {
                length += vertices[i].Distance(vertices[i + 1]);
            }

            return length;
        }
        public static double CalculateArea(List<Vertex> vertices)
        {
            double area = 0;
            for (int i = 0; i < vertices.Count - 1; i++)
            {
                area += VectorProduct(vertices[i], vertices[i + 1]);
            }
            area += VectorProduct(vertices[vertices.Count - 1], vertices[0]);

            return area / 2;
        }
        public static double VectorProduct(Vertex vertex1, Vertex vertex2)
        {
            return vertex1.x * vertex2.y - vertex1.y * vertex2.x;
        }
        public static System.Drawing.Point[] GetScreenPoints(List<Vertex> vertices, View view)
        {
            System.Drawing.Point[] points = new System.Drawing.Point[vertices.Count];
            for (int i = 0; i < points.Length; i++)
            {
                points[i] = view.ToScreenPoint(vertices[i]);
            }

            return points;
        }
    }
}