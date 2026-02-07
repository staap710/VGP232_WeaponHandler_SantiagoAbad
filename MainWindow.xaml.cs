using Microsoft.Win32;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Assignment1
{
    public partial class MainWindow : Window
    {
        private WeaponCollection mWeaponCollection;
        private string currentSortColumn = "Name";

        public MainWindow()
        {
            InitializeComponent();
            mWeaponCollection = new WeaponCollection();
            WeaponListBox.ItemsSource = mWeaponCollection;

            // Populate Filter Combo
            TypeFilterComboBox.ItemsSource = Enum.GetValues(typeof(WeaponType));
        }

        private void RefreshWeaponList()
        {
            // Guard against initialization race conditions where events fire before objects are created
            if (mWeaponCollection == null || TypeFilterComboBox == null || NameFilterTextBox == null) return;

            // Re-apply sort
            mWeaponCollection.SortBy(currentSortColumn);
            
            
            WeaponType selectedType = (WeaponType)(TypeFilterComboBox.SelectedItem ?? WeaponType.None);
            string nameFilter = NameFilterTextBox?.Text ?? string.Empty;

            IEnumerable<Weapon> filtered = mWeaponCollection;

            if (selectedType != WeaponType.None)
            {
                filtered = filtered.Where(w => w.Type == selectedType);
            }

            if (!string.IsNullOrEmpty(nameFilter))
            {
                filtered = filtered.Where(w => w.Name.IndexOf(nameFilter, StringComparison.OrdinalIgnoreCase) >= 0);
            }
            WeaponListBox.ItemsSource = filtered.ToList();
           
        }

        private void LoadClicked(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV Files|*.csv|JSON Files|*.json|XML Files|*.xml|All Files|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                if (mWeaponCollection.Load(openFileDialog.FileName))
                {
                    RefreshWeaponList();
                }
                else
                {
                    MessageBox.Show("Failed to load file.");
                }
            }
        }

        private void SaveClicked(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV Files|*.csv|JSON Files|*.json|XML Files|*.xml|All Files|*.*";
            if (saveFileDialog.ShowDialog() == true)
            {
                if (!mWeaponCollection.Save(saveFileDialog.FileName))
                {
                    MessageBox.Show("Failed to save file.");
                }
            }
        }

        private void AddClicked(object sender, RoutedEventArgs e)
        {
            EditWeaponWindow editWindow = new EditWeaponWindow();
            if (editWindow.ShowDialog() == true)
            {
                mWeaponCollection.Add(editWindow.Weapon);
                RefreshWeaponList();
                WeaponListBox.Items.Refresh(); // Explicit requirement check
            }
        }

        private void EditClicked(object sender, RoutedEventArgs e)
        {
            Weapon selected = WeaponListBox.SelectedItem as Weapon;
            if (selected == null) return;

            EditWeaponWindow editWindow = new EditWeaponWindow();
            editWindow.Weapon = selected; // Pass reference or clone? Usually reference for editing.

            
            if (editWindow.ShowDialog() == true)
            {
                RefreshWeaponList();
                WeaponListBox.Items.Refresh(); // Explicit requirement check
            }
        }

        private void RemoveClicked(object sender, RoutedEventArgs e)
        {
            Weapon selected = WeaponListBox.SelectedItem as Weapon;
            if (selected != null)
            {
                mWeaponCollection.Remove(selected);
                RefreshWeaponList();
                WeaponListBox.Items.Refresh(); // Explicit requirement check
            }
        }

        private void SortRadioSelected(object sender, RoutedEventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb != null && rb.Tag != null)
            {
                currentSortColumn = rb.Tag.ToString();
                RefreshWeaponList();
                // WeaponListBox.Items.Refresh(); // Requirement? Maybe.
            }
        }

        private void FilterTypeOnlySelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshWeaponList();
        }

        private void FilterNameTextChanged(object sender, TextChangedEventArgs e)
        {
            RefreshWeaponList();
        }

        private void ApplyLightTheme()
        {
            Application.Current.Resources["PrimaryBackgroundBrush"] = new SolidColorBrush(Colors.White);
            Application.Current.Resources["PrimaryForegroundBrush"] = new SolidColorBrush(Colors.Black);
            Application.Current.Resources["ButtonBackgroundBrush"] = new SolidColorBrush(Color.FromRgb(230, 230, 230));
            Application.Current.Resources["ButtonForegroundBrush"] = new SolidColorBrush(Colors.Black);
            Application.Current.Resources["ListBackgroundBrush"] = new SolidColorBrush(Colors.White);
            Application.Current.Resources["ListForegroundBrush"] = new SolidColorBrush(Colors.Black);
        }
    }
}
