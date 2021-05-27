using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MyGIS
{
    class BasicClasses
    {
        class GISVertex
        {
            public double x, y;

            public GISVertex(double _x, double _y)
            {
                x = _x;
                y = _y;
            }

            public double Distance(GISVertex vertex)
            {
                return Math.Sqrt(Math.Pow(vertex.x - x, 2) + Math.Pow(vertex.y - y, 2));
            }
        }

        class GISPoint
        {
            public GISVertex Location;
            public string Attribute;

            public GISPoint(GISVertex vertex, string attribute)
            {
                Location = vertex;
                Attribute = attribute;
            }

            public void DrawPoint(Graphics graphics)
            {
                graphics.FillEllipse(new SolidBrush(Color.Red), new Rectangle((int)(Location.x) - 3, (int)(Location.y) - 3, 6, 6));
            }

            public void DrawAttribute(Graphics graphics)
            {
                graphics.DrawString(Attribute, new Font("黑体", 20), new SolidBrush(Color.Green), new PointF((int)(Location.x), (int)(Location.y)));
            }

            public double Distance(GISVertex vertex)
            {
                return Location.Distance(vertex);
            }
        }

        class GISLine
        {
            List<GISVertex> AllVertices;
        }

        class GISPolygon
        {
            List<GISVertex> AllVertices;
        }
    }
}
