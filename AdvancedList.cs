using System.Collections.Generic;
using System.Windows.Forms;

namespace SpectrumAnalyzer
{
    class AdvancedList
    {
        private List<PlotEntity> plotEntities;
        private Label currentLabel;
        private Label currentFreq;
        private ListBox currentListBox;
        public int currentIndex { get; set; }
        public AdvancedList(ListBox listbox,Label label,Label freq)
        {
            plotEntities = new List<PlotEntity>();
            currentLabel = label;
            currentFreq = freq;
            currentListBox = listbox;
            currentIndex = -1;
            currentListBox.SelectedIndexChanged += onIndexChanged;
        }
        public PlotEntity getCurrentPlot()
        {
            return plotEntities[currentIndex];
        }
        public void addPlot(PlotEntity entity)
        {
            var newIndex = currentListBox.Items.Count;
            currentListBox.Items.Add(entity.Name);
            plotEntities.Add(entity);
            currentListBox.SelectedIndex = newIndex;
        }
        public void setCurrentPlot(PlotEntity entity)
        {
            plotEntities[currentIndex] = entity;
            currentListBox.Items[currentIndex] = entity.Name;
            onIndexChanged(null, null);
        }
        public void onIndexChanged(object sender, System.EventArgs e)
        {
            currentIndex = currentListBox.SelectedIndex;
            if (currentIndex >= 0)
            {
                var currentEntity = plotEntities[currentIndex];
                currentLabel.Text = currentEntity.Name;
                currentFreq.Text = currentEntity.PeakFrequency.ToString();
            }
            else
            {
                currentLabel.Text = "Current";
                currentFreq.Text = "0";
            }
        }
        public void removePlot()
        {
            plotEntities.RemoveAt(currentIndex);
            currentListBox.Items.RemoveAt(currentIndex);
            currentIndex = currentListBox.Items.Count-1;
            currentListBox.SelectedIndex = currentIndex;
        }
    }
}
