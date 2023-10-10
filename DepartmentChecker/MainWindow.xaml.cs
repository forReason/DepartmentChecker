using DepartmentChecker.Helpers_NS;
using DepartmentChecker.Objects_NS;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System;
using System.Windows;
using System.Windows.Threading;
using System.Collections.Generic;
using System.Linq;
using Helpers_NS;
using System.Windows.Media;

namespace DepartmentChecker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<CatalogItem> dataset = new ObservableCollection<CatalogItem>();
        Stopwatch stopwatch = new Stopwatch();
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        DatasetGenerator generator = new DatasetGenerator();

        public MainWindow()
        {
            InitializeComponent();
            InitializeUIElements();

            // Setup stopwatch
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += UpdateStopwatch;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);

        }
        private void Regenerate()
        {
            // Assume you have a List<CatalogItem> called datasetList generated from DatasetGenerator
            generator = new DatasetGenerator(int.Parse(DepartmentCountTextbox.Text));
            var datasetList = generator.GenerateDataset(int.Parse(TrainingCountTextbox.Text)); // Assuming GenerateDataset returns a List

            // Convert it to ObservableCollection<CatalogItem>
            ObservableCollection<CatalogItem> dataset = new ObservableCollection<CatalogItem>(datasetList.Item1);

            // Now, you can use this ObservableCollection with your UI elements.
            DatasetView.ItemsSource = dataset;
            // Populating TagCheckboxes ListView
            TagCheckboxes.ItemsSource = datasetList.Item2;

            // Populating DepartmentDropdown ComboBox
            DepartmentDropdown.ItemsSource = generator.Departments;
        }

        private void InitializeUIElements()
        {
            FilterMethodDropdown.Items.Add("Tibor");
            FilterMethodDropdown.Items.Add("Julian");

            // Initialize other UI elements like TagCheckboxes and DepartmentDropdown
        }

        private void UpdateStopwatch(object sender, EventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                StopwatchLabel.Content = stopwatch.ElapsedMilliseconds.ToString() + " ms";
            }, DispatcherPriority.Background);

        }

        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {

            stopwatch.Reset();
            stopwatch.Start();
            dispatcherTimer.Start();
            
            // Get department from DepartmentDropdown ComboBox and isCadre from CadreCheckbox CheckBox
            string department = DepartmentDropdown.SelectedItem?.ToString() ?? string.Empty;
            string algo = FilterMethodDropdown.SelectedItem?.ToString() ?? string.Empty;
            bool isCadre = CadreCheckbox.IsChecked ?? false;

            CatalogItem[] items = generator.Dataset.ToArray(); // Replace with your actual data fetching method
            List<CatalogItem> filteredItems;
            if (algo == "Julian")
            {
                filteredItems = await JulianAlgorithm.SetupCatalogState(department, isCadre, items.ToList());
            }
            else if (algo == "Tibor")
            {
                filteredItems = await TiborAlgorithm.SetupCatalogState(department, isCadre, items.ToList());
            }
            else
            {
                filteredItems = new List<CatalogItem>();
            }
            SearchButton.Background = Brushes.Cyan;
            DatasetView.ItemsSource = filteredItems;  // Assuming DatasetView is the name of your ListView

            dispatcherTimer.Stop();
            stopwatch.Stop();
            StopwatchLabel.Content = stopwatch.ElapsedMilliseconds.ToString() + " ms";
        }

        private void RegenerateButton_Click(object sender, RoutedEventArgs e)
        {
            Regenerate();
            SearchButton.Background = Brushes.Transparent;
        }
    }
}
