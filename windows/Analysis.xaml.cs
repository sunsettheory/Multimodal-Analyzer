
using Microsoft.Samples.Kinect.VisualizadorMultimodal.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Samples.Kinect.VisualizadorMultimodal.utils;
namespace Microsoft.Samples.Kinect.VisualizadorMultimodal.windows
{
    /// <summary>
    /// Interaction logic for Analysis.xaml
    /// </summary>
    public partial class Analysis : Window
    {
        public Analysis()
        {
            InitializeComponent();
            
            DataShow(Scene.Instance.Persons[0]);
            foreach (Person person in Scene.Instance.Persons)
            {
                personComboBox.Items.Add(person);
            }
            personComboBox.SelectedIndex = 0;
        }

        public void DataShow(Person person)
        {
            
            resultDataGrid.ItemsSource = Interaction.Instance.RelationIntervals
                .Where(ri => ri.Person1== person || ri.Person2 == person);
            double valor = person.TotalMinutesAverageInteraction;
            String average = valor.ToString("#.##");
            InteractionAverage.Content = average;            
            //InteractionAverage.Content = person.TotalMinutesAverageInteraction;
            //Console.WriteLine("asdfsda");
        }

        private void person1CheckBox_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void personComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataShow((Person)personComboBox.SelectedItem);
        }

        private void resultDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        //private void showPersonComboBox_Click(object sender, System.EventArgs e)
        //{
        //    int selectedIndex = personComboBox.SelectedIndex;

        // }
    }
}
