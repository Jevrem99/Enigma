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
using System.Windows.Ink;
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
        private Dictionary<Button, char> plugs;
        private Dictionary<Line, (char, char)> lines = new Dictionary<Line, (char, char)>();

        private bool keyPressed = false;
        private String lastLetter = "";
        private bool isUpdatingConf = false;
        private SoundPlayer keyPressedSound = new SoundPlayer("Assets/Sounds/keyPressed2.wav");
        private SoundPlayer keyReleasedSound = new SoundPlayer("Assets/Sounds/keyReleased1.wav");

        public MainWindow()
        {
            InitializeComponent();
            enigma = new EnigmaMachine();
            createLampBoard();
            createPlugboard();
            addConfChanged();
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

                enigma.setAll(rotI, rotII, rotIII, reflector, rotIConf - 1, rotIIConf - 1, rotIIIConf - 1);
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

        private Button firstPlug = null;
        private Button secondPlug = null;
        private Line plugLine = null;

        public void DrawLine(object sender, RoutedEventArgs e)
        {
            if (firstPlug == null)
            {

                firstPlug = sender as Button;

                Point start = firstPlug.TranslatePoint(
                    new Point(firstPlug.ActualWidth / 2, firstPlug.ActualHeight / 2), Plugboard);

                plugLine = new Line
                {
                    X1 = start.X,
                    Y1 = start.Y,
                    X2 = start.X,
                    Y2 = start.Y,
                    Stroke = Brushes.Green,
                    StrokeThickness = 2,
                    StrokeDashArray = new DoubleCollection { 2, 2 }
                };

                Plugboard.Children.Add(plugLine);

                Plugboard.MouseMove += Plugboard_plug_half_conected;
            }
            else
            {

                secondPlug = sender as Button;

                if (secondPlug != null && secondPlug != firstPlug)
                {
                    Point end = secondPlug.TranslatePoint(
                        new Point(secondPlug.ActualWidth / 2, secondPlug.ActualHeight / 2), Plugboard);

                    plugLine.X2 = end.X;
                    plugLine.Y2 = end.Y;
                    plugLine.StrokeDashArray = null;

                    plugLine.MouseLeftButtonDown += BreakPlug;

                    plugs.TryGetValue(firstPlug, out char letter1);
                    plugs.TryGetValue(secondPlug, out char letter2);

                    enigma.setPlug(letter1,letter2);
                    lines.Add(plugLine,(letter1,letter2));

                    firstPlug = null;
                    secondPlug = null;
                    plugLine = null;

                    Plugboard.MouseMove -= Plugboard_plug_half_conected;                 
                }
            }
        }

        private void Plugboard_plug_half_conected(object sender, MouseEventArgs e)
        {
            if (firstPlug != null && secondPlug == null){
                Point currentPosition = e.GetPosition(Plugboard);
                plugLine.X2 = currentPosition.X;
                plugLine.Y2 = currentPosition.Y;
            }
        }

        private void BreakPlug(Object sender,MouseEventArgs e)
        {
            Plugboard.Children.Remove(sender as Line);
            lines.TryGetValue(sender as Line, out var letters);
            lines.Remove(sender as Line);
            enigma.removePlug(letters.Item1,letters.Item2);
        }

        public void ConfChanged(object sender, EventArgs e)
        {
            if (isUpdatingConf)
                return;

            if (FirstConfIUD?.Value is int val1 &&
                SecondConfIUD?.Value is int val2 &&
                ThirdConfIUD?.Value is int val3)
            {
                enigma.setConf(val1 - 1, val2 - 1, val3 - 1);
            }
        }

        private void addConfChanged()
        {
            FirstConfIUD.ValueChanged += ConfChanged;
            SecondConfIUD.ValueChanged += ConfChanged;
            ThirdConfIUD.ValueChanged += ConfChanged;
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
            isUpdatingConf = true;
            var conf = enigma.getConf();

            FirstConfIUD.Value = conf.Item1 + 1;
            SecondConfIUD.Value = conf.Item2 + 1;
            ThirdConfIUD.Value = conf.Item3 + 1;

            isUpdatingConf = false;
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
            if (!String.IsNullOrEmpty((String)ReflectorComboBox.SelectedValue))
                return true;
            return false;
        }

        private void lightUpLamp(char letter)
        {
            lamps.TryGetValue(letter, out var lamp);
            lamp!.Background = Brushes.Orange;
        }

        private void turnOfLamp(char letter)
        {

             lamps.TryGetValue(letter, out var lamp);
             lamp!.Background = Brushes.LightGray;
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

        private void createPlugboard()
        {
            plugs = new Dictionary<Button, char>
            {
                { Q_Plug, 'Q' },
                { W_Plug, 'W' },
                { E_Plug, 'E' },
                { R_Plug, 'R' },
                { T_Plug, 'T' },
                { Z_Plug, 'Z' },
                { U_Plug, 'U' },
                { I_Plug, 'I' },
                { O_Plug, 'O' },
                { A_Plug, 'A' },
                { S_Plug, 'S' },
                { D_Plug, 'D' },
                { F_Plug, 'F' },
                { G_Plug, 'G' },
                { H_Plug, 'H' },
                { J_Plug, 'J' },
                { K_Plug, 'K' },
                { P_Plug, 'P' },
                { Y_Plug, 'Y' },
                { X_Plug, 'X' },
                { C_Plug, 'C' },
                { V_Plug, 'V' },
                { B_Plug, 'B' },
                { N_Plug, 'N' },
                { M_Plug, 'M' },
                { L_Plug, 'L' }
            };

            foreach (var plug in plugs)
            {
                plug.Key.Click += DrawLine;
            }
        }

    }
}