using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace MyGIS
{
    public enum ShapeType
    {
        Point = 1,
        Line = 3,
        Polygon = 5
    }
    public enum MapActions
    {
        ZoomIn, ZoomOut,
        MoveUp, MoveDown, MoveLeft, MoveRight
    };

    public enum MyTypes
    {
        System_Boolean,
        System_SByte,
        System_Byte,
        System_Char,
        System_Single,
        System_Double,
        System_Decimal,
        System_Int16,
        System_Int32,
        System_Int64,
        System_UInt16,
        System_UInt32,
        System_UInt64,
        System_String
    }

    public class Feature
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
    public abstract class Spatial
    {
        public Vertex centroid;
        public Extent extent;

        public abstract void Draw(Graphics graphics, View view);
    }
    public class Attribute
    {
        ArrayList attributes = new();

        public void AddValue(object o)
        {
            attributes.Add(o);
        }
        public object GetValue(int index)
        {
            if (attributes.Count != 0)
            {
                return attributes[index];
            }
            return null;
        }
        public void Draw(Graphics graphics, View view, Vertex centriod, int index)
        {
            System.Drawing.Point screenPoint = view.ToScreenPoint(centriod);
            graphics.DrawString(attributes[index].ToString(),
                new Font("宋体", 20),
                new SolidBrush(Color.Green),
                new PointF(screenPoint.X, screenPoint.Y));
        }

        public int ValueCount()
        {
            return attributes.Count;
        }
    }

    public class Vertex
    {
        public double x;
        public double y;

        public Vertex(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public Vertex(BinaryReader binaryReader)
        {
            x = binaryReader.ReadDouble();
            y = binaryReader.ReadDouble();
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

        public void WriteVertex(BinaryWriter binaryWriter)
        {
            binaryWriter.Write(x);
            binaryWriter.Write(y);
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
        public List<Vertex> vertices;
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
            return vertices[^1];
        }
    }
    class Polygon : Spatial
    {
        public List<Vertex> vertices;
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

    public class Field
    {
        public Type dataType;
        public string name;

        public Field(Type dataType, string name)
        {
            this.dataType = dataType;
            this.name = name;
        }
    }

    public class Extent
    {
        //地图的显示范围

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
    public class View
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
    public class Layer
    {
        public string name;
        public Extent extent;
        public bool shouldDrawAttribute = false;
        public int labelIndex;
        public ShapeType shapeType;
        List<Feature> features = new();
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
            return (ShapefileHeader)Tools.FromBytes(binaryReader, typeof(ShapefileHeader));
        }
        Point ReadPoint(byte[] recordContents)
        {
            double x = BitConverter.ToDouble(recordContents, 0);
            double y = BitConverter.ToDouble(recordContents, 8);
            return new Point(new Vertex(x, y));
        }
        public Layer ReadShapefile(string shapefileName)
        {
            FileStream fileStream = new(shapefileName, FileMode.Open);
            BinaryReader binaryReader = new(fileStream);
            ShapefileHeader shapefileHeader = ReadFileHeader(binaryReader);
            ShapeType shapeType = (ShapeType)Enum.Parse(typeof(ShapeType), shapefileHeader.shapeType.ToString());
            Extent extent = new(shapefileHeader.minX, shapefileHeader.minY, shapefileHeader.maxX, shapefileHeader.maxY);
            string dbfFileName = shapefileName.Replace(".shp", ".dbf");
            DataTable table = ReadDBF(dbfFileName);
            Layer layer = new(shapefileName, shapeType, extent, ReadFields(table));
            int rowIndex = 0;

            while (binaryReader.PeekChar() != -1)
            {
                RecordHeader recordHeader = ReadRecordHeader(binaryReader);
                int recordLength = FromBigToLittle(recordHeader.recordLength) * 2 - 4;
                byte[] recordContents = binaryReader.ReadBytes(recordLength);
                if (shapeType == ShapeType.Point)
                {
                    Point point = ReadPoint(recordContents);
                    Feature feature = new(point, ReadAttribute(table, rowIndex));
                    layer.AddFeature(feature);
                }

                if (shapeType == ShapeType.Line)
                {
                    List<Line> lines = ReadLines(recordContents);
                    for (int i = 0; i < lines.Count; i++)
                    {
                        Feature feature = new(lines[i], ReadAttribute(table, rowIndex));
                        layer.AddFeature(feature);
                    }
                }
                if (shapeType == ShapeType.Polygon)
                {
                    List<Polygon> polygons = ReadPolygons(recordContents);
                    for (int i = 0; i < polygons.Count; i++)
                    {
                        Feature feature = new(polygons[i], ReadAttribute(table, rowIndex));
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
            return (RecordHeader)Tools.FromBytes(binaryReader, typeof(RecordHeader));
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

            List<Line> lines = new();
            for (int i = 0; i < n; i++)
            {
                List<Vertex> vertices = new();
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

            List<Polygon> polygons = new();

            for (int i = 0; i < n; i++)
            {
                List<Vertex> vertices = new();
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
            FileInfo fileInfo = new(dbfFileName);
            DataSet dataSet = null;
            string conectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileInfo.DirectoryName + ";Extended Properties=dBASE IV;User ID=Admin;";
            using (OleDbConnection connection = new(conectionString))
            {
                string sql = "select  *  from " + fileInfo.Name;
                OleDbCommand command = new(sql, connection);
                connection.Open();
                dataSet = new DataSet();
                OleDbDataAdapter dataAdapter = new(command);
                dataAdapter.Fill(dataSet);
            }

            return dataSet.Tables[0];
        }
        static List<Field> ReadFields(DataTable table)
        {
            List<Field> fields = new();
            foreach (DataColumn column in table.Columns)
            {
                fields.Add(new Field(column.DataType, column.ColumnName));
            }

            return fields;
        }
        static Attribute ReadAttribute(DataTable table, int rowIndex)
        {
            Attribute attribute = new();
            DataRow row = table.Rows[rowIndex];
            for (int i = 0; i < table.Columns.Count; i++)
            {
                attribute.AddValue(row[i]);
            }

            return attribute;
        }
    }

    class MyFile
    {
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        struct MyFileHeader
        {
            public double minX, minY, maxX, maxY;
            public int featureCount, shapeType, fieldCount;
        };

        static void WriteFileHeader(Layer layer, BinaryWriter binaryWriter)
        {
            MyFileHeader myFileHeader = new();
            myFileHeader.minX = layer.extent.GetMinX();
            myFileHeader.minY = layer.extent.GetMinY();
            myFileHeader.maxX = layer.extent.GetMaxX();
            myFileHeader.maxY = layer.extent.GetMaxY();
            myFileHeader.featureCount = layer.FeatureCount();
            myFileHeader.shapeType = (int)(layer.shapeType);
            myFileHeader.fieldCount = layer.fields.Count;
            binaryWriter.Write(Tools.ToBytes(myFileHeader));
        }

        public static void WriteFile(Layer layer, string fileName)
        {
            FileStream fileStream = new(fileName, FileMode.Create);
            BinaryWriter binaryWriter = new(fileStream);
            WriteFileHeader(layer, binaryWriter);
            Tools.WriteName(layer.name, binaryWriter);
            WriteFields(layer.fields, binaryWriter);
            WriteFeatures(layer, binaryWriter);

            binaryWriter.Close();
            fileStream.Close();
        }

        static void WriteFields(List<Field> fields, BinaryWriter binaryWriter)
        {
            foreach (Field field in fields)
            {
                binaryWriter.Write(Tools.TypeToInt(field.dataType));
                Tools.WriteName(field.name, binaryWriter);
            }
        }

        static void WriteVertices(List<Vertex> vertices, BinaryWriter binaryWriter)
        {
            binaryWriter.Write(vertices.Count);
            foreach (Vertex vertex in vertices)
            {
                vertex.WriteVertex(binaryWriter);
            }
        }

        static void WriteAttribute(Attribute attribute, BinaryWriter binaryWriter)
        {
            for (int i = 0; i < attribute.ValueCount(); i++)
            {
                Type type = attribute.GetValue(i).GetType();
                switch (type.ToString())
                {
                    case("System.Boolean"):
                        binaryWriter.Write((bool)attribute.GetValue(i));
                        break;
                    case ("System.Byte"):
                        binaryWriter.Write((byte)attribute.GetValue(i));
                        break;
                    case ("System.Char"):
                        binaryWriter.Write((char)attribute.GetValue(i));
                        break;
                    case ("System.Single"):
                        binaryWriter.Write((float)attribute.GetValue(i));
                        break;
                    case ("System.Double"):
                        binaryWriter.Write((double)attribute.GetValue(i));
                        break;
                    case ("System.Decimal"):
                        binaryWriter.Write((decimal)attribute.GetValue(i));
                        break;
                    case ("System.Int16"):
                        binaryWriter.Write((short)attribute.GetValue(i));
                        break;
                    case ("System.Int32"):
                        binaryWriter.Write((int)attribute.GetValue(i));
                        break;
                    case ("System.Int64"):
                        binaryWriter.Write((long)attribute.GetValue(i));
                        break;
                    case ("System.UInt16"):
                        binaryWriter.Write((ushort)attribute.GetValue(i));
                        break;
                    case ("System.UInt32"):
                        binaryWriter.Write((uint)attribute.GetValue(i));
                        break;
                    case ("System.UInt64"):
                        binaryWriter.Write((ulong)attribute.GetValue(i));
                        break;
                    case ("System.SByte"):
                        binaryWriter.Write((sbyte)attribute.GetValue(i));
                        break;
                    case ("System.String"):
                        Tools.WriteName((string)attribute.GetValue(i), binaryWriter);
                        break;
                    default:
                        break;
                }
            }
        }

        static void WriteFeatures(Layer layer, BinaryWriter binaryWriter)
        {
            for (int i = 0; i < layer.FeatureCount(); i++)
            {
                Feature feature = layer.GetFeature(i);
                switch (layer.shapeType)
                {
                    case ShapeType.Point:
                        ((Point)feature.spatial).centroid.WriteVertex(binaryWriter);
                        break;
                    case ShapeType.Line:
                        Line line = (Line)(feature.spatial);
                        WriteVertices(line.vertices, binaryWriter);
                        break;
                    case ShapeType.Polygon:
                        Polygon polygon = (Polygon)(feature.spatial);
                        WriteVertices(polygon.vertices, binaryWriter);
                        break;
                    default:
                        break;
                }
                WriteAttribute(feature.attribute, binaryWriter);
            }
        }

        static List<Field> ReadFields(BinaryReader binaryReader, int fieldCount)
        {
            List<Field> fields = new();
            for (int i = 0; i < fieldCount; i++)
            {
                Type type = Tools.IntToType(binaryReader.ReadInt32());
                string name = Tools.ReadString(binaryReader);
                fields.Add(new Field(type, name));
            }

            return fields;
        }

        static List<Vertex> ReadVertices(BinaryReader binaryReader)
        {
            List<Vertex> vertices = new();
            int verticesCount = binaryReader.ReadInt32();
            for (int i = 0; i < verticesCount; i++)
            {
                vertices.Add(new Vertex(binaryReader));
            }

            return vertices;
        }

        static Attribute ReadAttributes(List<Field> fields, BinaryReader binaryReader)
        {
            Attribute attribute = new();
            for (int i = 0; i < fields.Count; i++)
            {
                Type type = fields[i].dataType;
                switch (type.ToString())
                {
                    case ("System.Boolean"):
                        attribute.AddValue(binaryReader.ReadBoolean());
                        break;
                    case ("System.Byte"):
                        attribute.AddValue(binaryReader.ReadByte());
                        break;
                    case ("System.Char"):
                        attribute.AddValue(binaryReader.ReadChar());
                        break;
                    case ("System.Single"):
                        attribute.AddValue(binaryReader.ReadSingle());
                        break;
                    case ("System.Double"):
                        attribute.AddValue(binaryReader.ReadDouble());
                        break;
                    case ("System.Decimal"):
                        attribute.AddValue(binaryReader.ReadDecimal()); 
                        break;
                    case ("System.Int16"):
                        attribute.AddValue(binaryReader.ReadInt16()); 
                        break;
                    case ("System.Int32"):
                        attribute.AddValue(binaryReader.ReadInt32()); 
                        break;
                    case ("System.Int64"):
                        attribute.AddValue(binaryReader.ReadInt64()); 
                        break;
                    case ("System.UInt16"):
                        attribute.AddValue(binaryReader.ReadUInt16()); 
                        break;
                    case ("System.UInt32"):
                        attribute.AddValue(binaryReader.ReadUInt32()); 
                        break;
                    case ("System.UInt64"):
                        attribute.AddValue(binaryReader.ReadUInt64()); 
                        break;
                    case ("System.SByte"):
                        attribute.AddValue(binaryReader.ReadSByte()); 
                        break;
                    case ("System.String"):
                        attribute.AddValue(Tools.ReadString(binaryReader));
                        break;
                    default:
                        break;
                }
            }

            return attribute;
        }

        static void ReadFeatures(Layer layer, BinaryReader binaryReader, int FeatureCount)
        {
            for (int i = 0; i < FeatureCount; i++)
            {
                Feature feature = new(null, null);
                switch (layer.shapeType)
                {
                    case ShapeType.Point:
                        feature.spatial = new Point(new Vertex(binaryReader));
                        break;
                    case ShapeType.Line:
                        feature.spatial = new Line(ReadVertices(binaryReader));
                        break;
                    case ShapeType.Polygon:
                        feature.spatial = new Polygon(ReadVertices(binaryReader));
                        break;
                    default:
                        break;
                }
                feature.attribute = ReadAttributes(layer.fields, binaryReader);
                layer.AddFeature(feature);
            }
        }

        public static Layer ReadFile(string fileName)
        {
            FileStream fileStream = new(fileName, FileMode.Open);
            BinaryReader binaryReader = new(fileStream);
            MyFileHeader myFileHeader = (MyFileHeader)(Tools.FromBytes(binaryReader, typeof(MyFileHeader)));
            ShapeType shapeType = (ShapeType)Enum.Parse(typeof(ShapeType), myFileHeader.shapeType.ToString());
            Extent extent = new(myFileHeader.minX, myFileHeader.minY, myFileHeader.maxX, myFileHeader.maxY);
            string layerName = Tools.ReadString(binaryReader);
            List<Field> fields = ReadFields(binaryReader, myFileHeader.fieldCount);
            Layer layer = new(layerName, shapeType, extent, fields);
            ReadFeatures(layer, binaryReader, myFileHeader.featureCount);

            binaryReader.Close();
            fileStream.Close();

            return layer;
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
            area += VectorProduct(vertices[^1], vertices[0]);

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
        public static byte[] ToBytes(object o)
        {
            byte[] bytes = new byte[Marshal.SizeOf(o.GetType())];
            GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            Marshal.StructureToPtr(o, handle.AddrOfPinnedObject(), false);
            handle.Free();
            return bytes;
        }
        public static void WriteName(string name, BinaryWriter binaryWriter)
        {
            binaryWriter.Write(StringLength(name));
            byte[] bytes = Encoding.Default.GetBytes(name);
            binaryWriter.Write(bytes);
        }
        public static int StringLength(string str)
        {
            int ChineseCount = 0;
            byte[] bytes = new ASCIIEncoding().GetBytes(str);
            foreach (byte b in bytes)
            {
                if (b == 0X3F)
                {
                    ChineseCount++;
                }
            }

            return ChineseCount + bytes.Length;
        }
        public static int TypeToInt(Type type)
        {
            MyTypes myType = (MyTypes)Enum.Parse(typeof(MyTypes), type.ToString().Replace(".", "_"));
            return (int)myType;
        }
        public static object FromBytes(BinaryReader binaryReader, Type type)
        {
            byte[] bytes = binaryReader.ReadBytes(Marshal.SizeOf(type));
            GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            object result = Marshal.PtrToStructure(handle.AddrOfPinnedObject(), type);
            handle.Free();
            return result;
        }
        public static string ReadString(BinaryReader binaryReader)
        {
            int length = binaryReader.ReadInt32();
            byte[] bytes = binaryReader.ReadBytes(length);
            return Encoding.Default.GetString(bytes);
        }
        public static Type IntToType(int index)
        {
            string typeString = Enum.GetName(typeof(MyTypes), index);
            typeString = typeString.Replace("_", ".");
            return Type.GetType(typeString);
        }
    }
}