using Enigma.Core;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Media;
using System.Runtime.CompilerServices;
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

namespace Enigma
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window,INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        private EnigmaMachine enigma;
        public ObservableCollection<String> AllRotors { get; set; } = new() { "I", "II", "III", "IV", "V" };
        public ObservableCollection<String> AllReflectors { get; set; } = new() { "A", "B", "C" };

        private Dictionary<char, Border> lamps;
        private bool keyPressed = false;
        private String lastLetter = "";
        private SoundPlayer keyPressedSound = new SoundPlayer("Assets/Sounds/keyPressed2.wav");
        private SoundPlayer keyReleasedSound = new SoundPlayer("Assets/Sounds/keyReleased1.wav");

        public MainWindow()
        {
            InitializeComponent();
            enigma = new EnigmaMachine();
            createLampBoard();
            this.DataContext = this;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void ConfigEnigma(Object sender, RoutedEventArgs e)
        {
            if (areRotorsSet() && isReflectorSet())
            {
                String rotI = (String)FirstRotorComboBox.SelectedValue;
                String rotII = (String)SecondRotorComboBox.SelectedValue;
                String rotIII = (String)ThirdRotorComboBox.SelectedValue;
                String reflector = (String)ReflectorComboBox.SelectedValue;
                int rotIConf = (int)FirstConfIUD.Value;
                int rotIIConf = (int)SecondConfIUD.Value;
                int rotIIIConf = (int)ThirdConfIUD.Value;

                enigma.setAll(rotI, rotII, rotIII, reflector, rotIConf, rotIIConf, rotIIIConf);
            }
        }

        public void KeyTyped(object sender, KeyEventArgs e)
        {
            if(isEnigmaSet() && !keyPressed)
            {
                keyPressed = true;
                char letter = e.Key.ToString()[0];
                char encryptedLetter = enigma.Encrypt(char.ToUpper(letter));
                InputTextBox.Text = InputTextBox.Text + letter;
                OutputTextBox.Text = OutputTextBox.Text + encryptedLetter;
                updateConf();
                lastLetter = encryptedLetter.ToString();
                lightUpLamp(encryptedLetter);
                keyPressedSound.Play();
            }
        }

        public void KeyReleased(object sender, KeyEventArgs e)
        {
            if (!string.IsNullOrEmpty(lastLetter))
            {
                turnOfLamp(lastLetter[0]);
                keyReleasedSound.Play();
            }
            keyPressed = false;
        }

        public void updateConf()
        {
            var conf = enigma.getConf();

            FirstConfIUD.Value = conf.Item1;
            SecondConfIUD.Value = conf.Item2;
            ThirdConfIUD.Value= conf.Item3;
        }

        private bool areRotorsSet()
        {
            if(!String.IsNullOrEmpty((String)FirstRotorComboBox.SelectedValue) && !String.IsNullOrEmpty((String)SecondRotorComboBox.SelectedValue) && !String.IsNullOrEmpty((String)ThirdRotorComboBox.SelectedValue))
                return true;
            return false;
        }

        private bool isEnigmaSet()
        {
            if(areRotorsSet() && isReflectorSet())
                return true;
            return false;
        }

        private bool isReflectorSet()
        {
            if(!String.IsNullOrEmpty((String)ReflectorComboBox.SelectedValue))
                return true;
            return false;
        }

        private void lightUpLamp(char letter)
        {
            lamps.TryGetValue(letter, out var lamp);
            lamp.Background = Brushes.Orange;
        }

        private void turnOfLamp(char letter)
        {

             lamps.TryGetValue(letter, out var lamp);
             lamp.Background = Brushes.LightGray;
        }

        private void createLampBoard()
        {
            lamps = new Dictionary<char, Border>
            {
                {'Q' , Q_Lamp },
                { 'W', W_Lamp },
                { 'E', E_Lamp },
                { 'R', R_Lamp },
                { 'T', T_Lamp },
                { 'Z', Z_Lamp },
                { 'U', U_Lamp },
                { 'I', I_Lamp },
                { 'O', O_Lamp },
                { 'A', A_Lamp },
                { 'S', S_Lamp },
                { 'D', D_Lamp },
                { 'F', F_Lamp },
                { 'G', G_Lamp },
                { 'H', H_Lamp },
                { 'J', J_Lamp },
                { 'K', K_Lamp },
                { 'P', P_Lamp },
                { 'Y', Y_Lamp },
                { 'X', X_Lamp },
                { 'C', C_Lamp },
                { 'V', V_Lamp },
                { 'B', B_Lamp },
                { 'N', N_Lamp },
                { 'M', M_Lamp },
                { 'L', L_Lamp },
            };
        }

    }
}