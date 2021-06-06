using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MyGIS
{
    public partial class Main : Form
    {
        View view = null;
        List<Feature> features = new List<Feature>();
        public Main()
        {
            InitializeComponent();
            view = new View(new Extent(new Vertex(0, 0), new Vertex(100, 100)), ClientRectangle);
        }

        private void ButtonAddPointEntity_Click(object sender, EventArgs e)
        {
            double x = Convert.ToDouble(textBoxX.Text);
            double y = Convert.ToDouble(textBoxY.Text);
            Attribute attribute = new();
            attribute.AddValue(textBoxAttribute.Text);
            Vertex vertex = new(x, y);
            Point point = new(vertex);
            Feature feature = new(point, attribute);
            features.Add(feature);
            Graphics graphics = CreateGraphics();
            feature.Draw(graphics, view, true, 0);
        }

        private void ButtonUpdateMap_Click(object sender, EventArgs e)
        {
            double minX = double.Parse(textBoxMinX.Text);
            double minY = double.Parse(textBoxMinY.Text);
            double maxX = double.Parse(textBoxMaxX.Text);
            double maxY = double.Parse(textBoxMaxY.Text);
            //更新view
            view.Update(new Extent(minX, minY, maxX, maxY), ClientRectangle);
            ButtonUpdateMap_Click();
        }

        private void Main_MouseClick(object sender, MouseEventArgs e)
        {
            Vertex clickCentroid = new(view.ToMapVertex(e.Location).x, view.ToMapVertex(e.Location).y);
            double minDistance = double.MaxValue;
            int id = -1;
            for (int i = 0; i < features.Count; i++)
            {
                double distance = features[i].spatial.centroid.Distance(clickCentroid);
                if (distance < minDistance)
                {
                    id = i;
                    minDistance = distance;
                }
            }
            if (id == -1)
            {
                MessageBox.Show("没有任何空间对象！");

                return;
            }
            Vertex nearestVertex = features[id].spatial.centroid;
            double screendistance = Math.Abs(view.ToScreenPoint(nearestVertex).X - e.X) + Math.Abs(view.ToScreenPoint(nearestVertex).Y - e.Y);
            if (screendistance > 5)
            {
                MessageBox.Show($"请靠近空间对象点击！show n.x{nearestVertex.x},n.y{nearestVertex.y}, e.x{e.X},e.y{e.Y}");

                return;
            }
            MessageBox.Show($"show n.x{nearestVertex.x},n.y{nearestVertex.y}, e.x{e.X},e.y{e.Y},{ClientRectangle} 该空间对象属性是" + features[id].GetAttribute(0));
        }

        private void ButtonMapAction_Click(object sender, EventArgs e)
        {
            MapActions action = MapActions.ZoomIn;
            if ((Button)sender == buttonZoomIn) action = MapActions.ZoomIn;
            else if ((Button)sender == buttonZoomOut) action = MapActions.ZoomOut;
            else if ((Button)sender == buttonMoveUp) action = MapActions.MoveUp;
            else if ((Button)sender == buttonMoveDown) action = MapActions.MoveDown;
            else if ((Button)sender == buttonMoveLeft) action = MapActions.MoveLeft;
            else if ((Button)sender == buttonMoveRight) action = MapActions.MoveRight;
            view.ChangeView(action);
            ButtonUpdateMap_Click();
        }

        private void ButtonUpdateMap_Click()
        {
            Graphics graphics = CreateGraphics();
            graphics.FillRectangle(new SolidBrush(Color.Black), ClientRectangle);
            for (int i = 0; i < features.Count; i++)
            {
                features[i].Draw(graphics, view, true, 0);
            }
        }
    }
}
