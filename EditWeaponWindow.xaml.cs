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

namespace Assignment1
{
    /// <summary>
    /// Interaction logic for EditWeaponWindow.xaml
    /// </summary>
    public partial class EditWeaponWindow : Window
    {
        public Weapon Weapon { get; set; }

        public EditWeaponWindow()
        {
            InitializeComponent();
            TypeComboBox.ItemsSource = Enum.GetValues(typeof(WeaponType));
            Loaded += EditWeaponWindow_Loaded;
        }

        private void EditWeaponWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (Weapon != null)
            {
                NameTextBox.Text = Weapon.Name;
                TypeComboBox.SelectedItem = Weapon.Type;
                RarityTextBox.Text = Weapon.Rarity.ToString();
                BaseAttackTextBox.Text = Weapon.BaseAttack.ToString();
                ImageTextBox.Text = Weapon.Image;
                SecondaryStatTextBox.Text = Weapon.SecondaryStat;
                PassiveTextBox.Text = Weapon.Passive;
            }
            else
            {
                // New Weapon
                Weapon = new Weapon();
                TypeComboBox.SelectedIndex = 0; // Default
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameTextBox.Text))
            {
                MessageBox.Show("Name is required.");
                return;
            }

            if (!int.TryParse(RarityTextBox.Text, out int rarity))
            {
                MessageBox.Show("Rarity must be a number.");
                return;
            }

            if (!int.TryParse(BaseAttackTextBox.Text, out int attack))
            {
                MessageBox.Show("Base Attack must be a number.");
                return;
            }

            if (Weapon == null) Weapon = new Weapon();

            Weapon.Name = NameTextBox.Text;
            Weapon.Type = (WeaponType)(TypeComboBox.SelectedItem ?? WeaponType.None);
            Weapon.Rarity = rarity;
            Weapon.BaseAttack = attack;
            Weapon.Image = ImageTextBox.Text;
            Weapon.SecondaryStat = SecondaryStatTextBox.Text;
            Weapon.Passive = PassiveTextBox.Text;

            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
