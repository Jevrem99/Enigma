using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Enigma.Core;

namespace Enigma
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private EnigmaMachine enigma;
        private int firstRotor;
        private int secondRotor;
        private int thirdRotor;

        private String[] rotorValues = ["I", "II", "III", "IV", "V"];
        private List<String> selectedValues = new List<String>();

        public MainWindow()
        {
            InitializeComponent();
            initializeGraphics();
            enigma = new EnigmaMachine();
        }

        private void setRotors(Object sender, SelectionChangedEventArgs e)
        {
            selectedValues.Clear();
            String first_rot = (String)FirstRotorComboBox.SelectedValue;
            String second_rot = (String)SecondRotorComboBox.SelectedValue;
            String third_rot = (String)ThirdRotorComboBox.SelectedValue;

            if (first_rot != "")
                selectedValues.Add(first_rot);
            if (second_rot != "")
                selectedValues.Add(second_rot);
            if(third_rot != "")
                selectedValues.Add(third_rot);

            FirstRotorComboBox.Items.Clear();
            SecondRotorComboBox.Items.Clear();
            ThirdRotorComboBox.Items.Clear();

            FirstRotorComboBox.SelectedValue = first_rot;
            SecondRotorComboBox.SelectedValue = second_rot;
            ThirdRotorComboBox.SelectedValue = third_rot;
            foreach (String rot_val in rotorValues)
            {
                if (!selectedValues.Exists(x => x == rot_val))
                {
                    FirstRotorComboBox.Items.Add(rot_val);
                    SecondRotorComboBox.Items.Add(rot_val);
                    ThirdRotorComboBox.Items.Add(rot_val);
                }
            }
        }

        private void addAllRotors(ComboBox comboBox)
        {
            comboBox.Items.Clear();
            foreach (String rotor in rotorValues) 
            {
                comboBox.Items.Add(rotor);
            }
        }

        private void initializeGraphics()
        {
            foreach (String val in rotorValues)
            {
                FirstRotorComboBox.Items.Add(val);
                SecondRotorComboBox.Items.Add(val);
                ThirdRotorComboBox.Items.Add(val);
            }
        }

    }
}