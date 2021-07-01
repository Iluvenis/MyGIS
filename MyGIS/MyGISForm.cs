using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MyGIS
{
    public partial class MainForm : Form
    {
        Layer layer = null;
        readonly View view = null;
        readonly List<Feature> features = new();
        public MainForm()
        {
            InitializeComponent();
            view = new View(new Extent(new Vertex(0, 0), new Vertex(100, 100)), ClientRectangle);
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
                MessageBox.Show("没有实体！");

                return;
            }
            Vertex nearestVertex = features[id].spatial.centroid;
            double screendistance = Math.Abs(view.ToScreenPoint(nearestVertex).X - e.X) + Math.Abs(view.ToScreenPoint(nearestVertex).Y - e.Y);
            if (screendistance > 5)
            {
                MessageBox.Show("请靠实体点击！");

                return;
            }
            MessageBox.Show("该实体属性为：" + features[id].GetAttribute(0));
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
            UpdateMap();
        }

        private void ButtonOpenShapeFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new();
            openFileDialog.Filter = "Shapefile 文件|*.shp";
            openFileDialog.RestoreDirectory = false;
            openFileDialog.FilterIndex = 1;
            openFileDialog.Multiselect = false;
            if (openFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            layer = Shapefile.ReadShapefile(openFileDialog.FileName);
            layer.shouldDrawAttribute = false;
            MessageBox.Show("Read" + layer.FeatureCount() + "objects.");
            view.UpdateExtent(layer.extent);
            UpdateMap();
        }

        private void ButtonShowFullMap_Click(object sender, EventArgs e)
        {
            view.UpdateExtent(layer.extent);
            UpdateMap();
        }

        private void UpdateMap()
        {
            if (layer.fields.Count != 0)
            {
                buttonOpenAttribute.Enabled = true;
            }
            Graphics graphics = CreateGraphics();
            graphics.FillRectangle(new SolidBrush(Color.Black), ClientRectangle);
            layer.Draw(graphics, view);
        }

        private void ButtonOpenAttribute_Click(object sender, EventArgs e)
        {
            AttributesForm attributes = new(layer);
            attributes.Show();
        }

        private void ButtonSaveFile_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new();
            saveFileDialog.Filter = "mgdb files (*.mgdb)|*.mgdb|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = true;
            if (saveFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            MyFile.WriteFile(layer, saveFileDialog.FileName);
            MessageBox.Show("Done.");
        }

        private void ButtonOpenFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new();
            openFileDialog.Filter = "mgdb files (*.mgdb)|*.mgdb|All files (*.*)|*.*";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.FilterIndex = 1;
            openFileDialog.Multiselect = false;
            if (openFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            layer = MyFile.ReadFile(openFileDialog.FileName);
            MessageBox.Show("Read" + layer.FeatureCount() + "objects.");
            view.UpdateExtent(layer.extent);
            UpdateMap();
        }
    }
}
