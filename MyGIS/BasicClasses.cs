using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

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
        public Point ToscreenPoint(GISVertex vertex)
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
            Point screenPoint = view.ToscreenPoint(centroid);
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
        List<GISVertex> AllVertexs;
        public override void Draw(Graphics graphics, GISView view)
        {

        }
    }
    class GISPolygon : GISSpatial
    {
        List<GISVertex> AllVertexs;
        public override void Draw(Graphics graphics, GISView view)
        {

        }

    }
}
