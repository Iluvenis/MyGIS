using System.Windows.Forms;

namespace MyGIS
{
    public partial class AttributesForm : Form
    {
        public AttributesForm(Layer layer)
        {
            InitializeComponent();
            for (int i = 0; i < layer.fields.Count; i++)
            {
                dataGridViewAttributes.Columns.Add(layer.fields[i].name, layer.fields[i].name);
            }
            for (int i = 0; i < layer.FeatureCount(); i++)
            {
                dataGridViewAttributes.Rows.Add();
                for (int j = 0; j < layer.fields.Count; j++)
                {
                    dataGridViewAttributes.Rows[i].Cells[j].Value = layer.GetFeature(i).GetAttribute(j);
                }
            }
        }
    }
}
