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
        public GISVertex(double _x, double _y)
        {
            x = _x;
            y = _y;
        }
        public double Distance(GISVertex anothervertex)
        {
            return Math.Sqrt((x - anothervertex.x) * (x - anothervertex.x) + (y - anothervertex.y) * (y - anothervertex.y));
        }

    }
    class GISFeature
    {
        public GISSpatial spatialpart;
        public GISAttribute attributepart;
        public GISFeature(GISSpatial spatial, GISAttribute attribute)
        {
            spatialpart = spatial;
            attributepart = attribute;
        }
        public void draw(Graphics graphics, GISView view, bool DrawAttributeOrNot, int index)
        {
            spatialpart.draw(graphics, view);
            if (DrawAttributeOrNot)
                attributepart.draw(graphics, view, spatialpart.centroid, index);

        }
        public object getAttribute(int index)
        {
            return attributepart.GetValue(index);
        }

    }
    class GISAttribute
    {
        ArrayList values = new ArrayList();
        public void AddValue(object o)
        {
            values.Add(o);
        }
        public object GetValue(int index)
        {
            return values[index];
        }
        public void draw(Graphics graphics, GISView view, GISVertex location, int index)
        {
            Point screenpoint = new Point((int)location.x, (int)location.y);
            graphics.DrawString(values[index].ToString(),
                new Font("宋体", 20),
                new SolidBrush(Color.Green),
                new PointF(screenpoint.X, screenpoint.Y));
        }
    }
    abstract class GISSpatial
    {
        public GISVertex centroid;
        public GISExtent extent;
        public abstract void draw(Graphics graphics, GISView view);
    }
    class GISExtent
    {
        double ZoomingFactor = 2;
        double MovingFactor = 0.25;
        public void ChangeExtent(GISMapActions action)
        {
            double newminx = bottomleft.x, newminy = bottomleft.y,
            newmaxx = upright.x, newmaxy = upright.y;
            switch (action)
            {
                case GISMapActions.zoomin:
                    newminx = (getMinX() + getMaxX() - getWidth() / ZoomingFactor) / 2;
                    newminy = (getMinY() + getMaxY() - getHeight() / ZoomingFactor) / 2;
                    newmaxx = (getMinX() + getMaxX() + getWidth() / ZoomingFactor) / 2;
                    newmaxy = (getMinY() + getMaxY() + getHeight() / ZoomingFactor) / 2;
                    break;
                case GISMapActions.zoomout:
                    newminx = (getMinX() + getMaxX() - getWidth() * ZoomingFactor) / 2;
                    newminy = (getMinY() + getMaxY() - getHeight() * ZoomingFactor) / 2;
                    newmaxx = (getMinX() + getMaxX() + getWidth() * ZoomingFactor) / 2;
                    newmaxy = (getMinY() + getMaxY() + getHeight() * ZoomingFactor) / 2;
                    break;
                case GISMapActions.moveup:
                    newminy = getMinY() - getHeight() * MovingFactor;
                    newmaxy = getMaxY() - getHeight() * MovingFactor;
                    break;
                case GISMapActions.movedown:
                    newminy = getMinY() + getHeight() * MovingFactor;
                    newmaxy = getMaxY() + getHeight() * MovingFactor;
                    break;
                case GISMapActions.moveleft:
                    newminx = getMinX() + getWidth() * MovingFactor;
                    newmaxx = getMaxX() + getWidth() * MovingFactor;
                    break;
                case GISMapActions.moveright:
                    newminx = getMinX() - getWidth() * MovingFactor;
                    newmaxx = getMaxX() - getWidth() * MovingFactor;
                    break;
            }
            upright.x = newmaxx;
            upright.y = newmaxy;
            bottomleft.x = newminx;
            bottomleft.y = newminy;
        }
        public GISVertex bottomleft;
        public GISVertex upright;
        public GISExtent(GISVertex _bottomleft, GISVertex _upright)
        {
            bottomleft = _bottomleft;
            upright = _upright;
        }
        public GISExtent(double minx, double miny, double maxx, double maxy)
        {
            //地图的显示范围
            upright = new GISVertex(Math.Max(minx, maxx), Math.Max(maxy, miny));
            bottomleft = new GISVertex(Math.Min(minx, maxx), Math.Min(miny, maxy));
        }
        public double getMinX()
        {
            return bottomleft.x;
        }
        public double getMinY()
        {
            return bottomleft.y;
        }
        public double getMaxX()
        {
            return upright.x;
        }
        public double getMaxY()
        {
            return upright.y;
        }
        public double getWidth()
        {
            return upright.x - bottomleft.x;
        }
        public double getHeight()
        {
            return upright.y - bottomleft.y;
        }
    }
    class GISView
    {
        GISExtent CurrentMapExtent;
        Rectangle MapWindowSize;
        double MapMinX, MapMinY;
        int WinW, WinH;
        double MapW, MapH;
        double ScaleX, ScaleY;
        public GISView(GISExtent _extent, Rectangle _rectangle)
        {
            Update(_extent, _rectangle);
        }
        public void Update(GISExtent _extent, Rectangle _rectangle)
        {
            CurrentMapExtent = _extent;
            MapWindowSize = _rectangle;
            MapMinX = CurrentMapExtent.getMinX();
            MapMinY = CurrentMapExtent.getMinY();
            WinW = MapWindowSize.Width;
            WinH = MapWindowSize.Height;
            MapW = CurrentMapExtent.getWidth();
            MapH = CurrentMapExtent.getHeight();
            ScaleX = MapW / WinW;
            ScaleY = MapH / WinH;
        }
        public Point ToScreenPoint(GISVertex onevertex)
        {
            double ScreenX = (onevertex.x - MapMinX) / ScaleX;
            double ScreenY = WinH - (onevertex.y - MapMinY) / ScaleY;
            return new Point((int)ScreenX, (int)ScreenY);

        }
        public GISVertex ToMapVertex(Point point)
        {
            double MapX = ScaleX * point.X + MapMinX;
            double MapY = ScaleY * (WinH - point.Y) + MapMinY;
            return new GISVertex(MapX, MapY);
        }
        public void ChangeView(GISMapActions action)
        {
            CurrentMapExtent.ChangeExtent(action);
            Update(CurrentMapExtent, MapWindowSize);
        }
    }
    enum GISMapActions
    {
        zoomin, zoomout,
        moveup, movedown, moveleft, moveright
    };
    class GISPoint : GISSpatial
    {
        public GISPoint(GISVertex onevertex)
        {
            centroid = onevertex;
            extent = new GISExtent(onevertex, onevertex);
        }
        public override void draw(Graphics graphics, GISView view)
        {
            Point screenpoint = view.ToScreenPoint(centroid);
            graphics.FillEllipse(new SolidBrush(Color.Red),
                new Rectangle((int)(centroid.x) - 3, (int)(centroid.y) - 3, 6, 6));

        }
        public double Distance(GISVertex anothervertex)
        {
            return centroid.Distance(anothervertex);
        }
    }
    class GISLine : GISSpatial
    {
        List<GISVertex> AllVertexs;
        public override void draw(Graphics graphics, GISView view)
        {

        }
    }
    class GISPolygon : GISSpatial
    {
        List<GISVertex> AllVertexs;
        public override void draw(Graphics graphics, GISView view)
        {

        }

    }
}
