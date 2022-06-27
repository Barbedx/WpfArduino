using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

using System.Timers;
using System.IO.Ports;
using System.Windows.Controls;
using System.Windows.Media;
using System.Text.RegularExpressions;
using System.Threading;

namespace WpfArduino
{

    public class EffectItem
    {
        public EffectItem(int id, string name, string description)
        {
            Id = id;
            Name = name.Trim();
            Description = description.Trim();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SerialPort currentPort;
        private bool effectChanged = true;

        private delegate void updateDelegate(string txt);

        public MainWindow()
        {
            InitializeComponent();

            cbbeffects.Items.Add(new EffectItem(2, " STRIP RAINBOW FADE", "                     плавная смена цветов всей ленты"));
            cbbeffects.Items.Add(new EffectItem(3, " RAINBOW LOOP", "    крутящаяся радуга"));
            cbbeffects.Items.Add(new EffectItem(4, " RANDOM BURST", "    случайная смена цветов"));
            cbbeffects.Items.Add(new EffectItem(5, " CYLON v1", "    бегающий светодиод"));
            cbbeffects.Items.Add(new EffectItem(6, " CYLON v2", "    бегающий паровозик светодиодов"));
            cbbeffects.Items.Add(new EffectItem(7, " POLICE LIGHTS SINGLE", "    aвращаются красный и синий"));
            cbbeffects.Items.Add(new EffectItem(8, " POLICE LIGHTS SOLID", "     вращается половина красных и половина синих"));
            cbbeffects.Items.Add(new EffectItem(9, " STRIP FLICKER", "       случайный стробоскоп"));
            cbbeffects.Items.Add(new EffectItem(10, "PULSE COLOR BRIGHTNESS", "      пульсация одним цветом"));
            cbbeffects.Items.Add(new EffectItem(11, "PULSE COLOR SATURATION", "      пульсация со сменой цветов"));
            cbbeffects.Items.Add(new EffectItem(12, "VERTICAL SOMETHING", "      плавная смена яркости по вертикали (для кольца)"));
            cbbeffects.Items.Add(new EffectItem(13, "CELL AUTO -RULE 30(RED)", "     безумие красных светодиодов"));
            cbbeffects.Items.Add(new EffectItem(14, "MARCH RANDOM COLORS", "     безумие случайных цветов"));
            cbbeffects.Items.Add(new EffectItem(15, "MARCH RWB COLORS", "    белый синий красный бегут по кругу (ПАТРИОТИЗМ!)"));
            cbbeffects.Items.Add(new EffectItem(16, "RADIATION SYMBOL", "    пульсирует значок радиации"));
            cbbeffects.Items.Add(new EffectItem(17, "color_loop_vardelay", "      красный светодиод бегает по кругу"));
            cbbeffects.Items.Add(new EffectItem(18, "white_temps", "      бело синий градиент (?)"));
            cbbeffects.Items.Add(new EffectItem(19, "SIN WAVE BRIGHTNESS", "     тоже хрень какая то"));
            cbbeffects.Items.Add(new EffectItem(20, "POP LEFT/ RIGHT", "     красные вспышки спускаются вниз"));
            cbbeffects.Items.Add(new EffectItem(21, "QUADRATIC BRIGHTNESS CURVE", "      полумесяц"));
            cbbeffects.Items.Add(new EffectItem(22, "flame", "    эффект пламени"));
            cbbeffects.Items.Add(new EffectItem(23, "VERITCAL RAINBOW", "    радуга в вертикаьной плоскости (кольцо)"));
            cbbeffects.Items.Add(new EffectItem(24, "PACMAN", "      пакман"));
            cbbeffects.Items.Add(new EffectItem(25, "RANDOM COLOR POP", "    безумие случайных вспышек"));
            cbbeffects.Items.Add(new EffectItem(26, "EMERGECNY STROBE", "    полицейская мигалка"));
            cbbeffects.Items.Add(new EffectItem(27, "RGB PROPELLER", "       RGB пропеллер"));
            cbbeffects.Items.Add(new EffectItem(28, "KITT", "    случайные вспышки красного в вертикаьной плоскости"));
            cbbeffects.Items.Add(new EffectItem(29, "MATRIX RAIN", "     зелёненькие бегают по кругу случайно"));
            cbbeffects.Items.Add(new EffectItem(30, "NEW RAINBOW LOOP", "    крутая плавная вращающаяся радуга"));
            cbbeffects.Items.Add(new EffectItem(31, "MARCH STRIP NOW CCW", "     чёт сломалось"));
            cbbeffects.Items.Add(new EffectItem(32, "MARCH STRIP NOW CCW", "     чёт сломалось"));
            cbbeffects.Items.Add(new EffectItem(33, "colorWipe", "       плавное заполнение цветом"));
            cbbeffects.Items.Add(new EffectItem(34, "CylonBounce", "     бегающие светодиоды"));
            cbbeffects.Items.Add(new EffectItem(35, "Fire", "    линейный огонь"));
            cbbeffects.Items.Add(new EffectItem(36, "NewKITT", "     беготня секторов круга (не работает)"));
            cbbeffects.Items.Add(new EffectItem(37, "rainbowCycle", "    очень плавная вращающаяся радуга"));
            cbbeffects.Items.Add(new EffectItem(38, "rainbowTwinkle", "      случайные разноцветные включения (1 - танцуют все, \"0 - случайный 1 диод)"));
            cbbeffects.Items.Add(new EffectItem(39, "RunningLights", "       бегущие огни"));
            cbbeffects.Items.Add(new EffectItem(40, "Sparkle", "     случайные вспышки белого цвета"));
            cbbeffects.Items.Add(new EffectItem(41, "SnowSparkle", "     случайные вспышки белого цвета на белом фоне"));
            cbbeffects.Items.Add(new EffectItem(42, "theaterChase", "    бегущие каждые 3 (ЧИСЛО СВЕТОДИОДОВ ДОЛЖНО БЫТЬ КРАТНО 3)"));
            cbbeffects.Items.Add(new EffectItem(43, "theaterChaseRainbow", "     бегущие каждые 3 радуга (ЧИСЛО СВЕТОДИОДОВ ДОЛЖНО БЫТЬ КРАТНО 3)"));
            cbbeffects.Items.Add(new EffectItem(44, "Strobe", "      стробоскоп"));
            cbbeffects.Items.Add(new EffectItem(45, "BouncingBalls", "       прыгающие мячики"));
            cbbeffects.Items.Add(new EffectItem(46, "BouncingColoredBalls", "      прыгающие мячики цветные"));


        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            bool ArduinoPortFound = false;

            try
            {
                var ports = SerialPort.GetPortNames().OrderByDescending(x => int.TryParse(x.Replace("COM", string.Empty), out int result) ? result : 0);
                foreach (string port in ports)
                {
                    currentPort = new SerialPort(port, 9600);
                    currentPort.DtrEnable = false;
                    if (ArduinoDetected())
                    {
                        ArduinoPortFound = true;

                        break;
                    }
                    else
                    {
                        ArduinoPortFound = false;
                    }
                }
            }
            catch { }

            if (ArduinoPortFound == false) return;
            System.Threading.Thread.Sleep(500); // немного подождем
            lblPortData.Content = currentPort.PortName;
            currentPort.BaudRate = 9600;
            currentPort.DtrEnable = false;
            //currentPort.DtrEnable = true;
            currentPort.ReadTimeout = 1000;
            try
            {
                currentPort.Open();
            }
            catch { }
            currentPort.DataReceived += CurrentPort_DataReceived;


        }

        private void CurrentPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (!currentPort.IsOpen) return;
            try // так как после закрытия окна таймер еще может выполнится или предел ожидания может быть превышен
            {
                // удалим накопившееся в буфере
                //currentPort.DiscardInBuffer();
                // считаем последнее значение 

                string strFromPort = currentPort.ReadLine();

                _ = lblPortData.Dispatcher.BeginInvoke(new updateDelegate(updateTextBox), strFromPort);
            }
            catch { }
        }

        private bool ArduinoDetected()
        {
            try
            {
                currentPort.Open();
                System.Threading.Thread.Sleep(1000);
                // небольшая пауза, ведь SerialPort не терпит суеты

                string returnMessage = String.Empty;//currentPort.ReadExisting();//; .ReadLine();

                for (int i = 0; i < 10; i++)
                {
                    returnMessage = currentPort.ReadTo(Environment.NewLine);//; .ReadLine();
                }
                currentPort.Close();

                // необходимо чтобы void loop() в скетче содержал код Serial.println("Info from Arduino");
                if (returnMessage.Contains("Info from Arduino"))
                {
                    setDefaults(returnMessage);
                    return true;
                };
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                currentPort.Close();
            }
        }

        private void updateTextBox(string txt)
        {
            //setDefaults(txt);
            if (txt.Contains("Info from Arduino"))
            {
                var regex = Regex.Match(txt, "mode:([0-9].*?),");
                if (regex.Success)
                    if (effectChanged == false)
                    {
                        if (regex.Success && regex.Groups[1].Value == cbbeffects.SelectedValue?.ToString())
                            effectChanged = true;
                    }
                    else
                    {
                        cbbeffects.SelectedValue = int.TryParse(regex.Groups[1].Value, out var b) ? b : 0;
                    }

                //if (skipdebug.IsChecked == true && txt.Contains("Info from Arduino"))
                //{
                //    return;
                //}

            }
            if (listbox.Items.Count == 0 || (listbox.Items.Count >0 && (string)listbox.Items[^1] != txt))
            {

                listbox.Items.Add(txt);
                if (autoscroll.IsChecked ?? false)
                {
                    var border = (Decorator)VisualTreeHelper.GetChild(listbox, 0);
                    var scrollViewer = (ScrollViewer)border.Child;
                    scrollViewer.ScrollToEnd();
                }
            }
        }

        private void setDefaults(string txt)
        {
            if (txt.Contains("Info from Arduino"))
            {
                var regex = Regex.Match(txt, "b:([0-9].*?),");
                if (regex.Success)
                    slider.Value = int.TryParse(regex.Groups[1].Value, out var b) ? b : 0;

                regex = Regex.Match(txt, "mode:([0-9].*?),");
                if (regex.Success)
                    cbbeffects.SelectedValue = int.TryParse(regex.Groups[1].Value, out var b) ? b : 0;

                var rgbMatches = Regex.Matches(txt + ",", "RGB:[RGB]([0-9].*?),");
                if (rgbMatches.All(x => x.Success) && rgbMatches.Count == 3)
                {

                    byte R = GetByte(rgbMatches[0].Groups[1].Value);
                    byte G = GetByte(rgbMatches[1].Groups[1].Value);
                    byte B = GetByte(rgbMatches[2].Groups[1].Value);

                    color.SelectedColor = Color.FromRgb(R, G, B);
                }


            }
        }

        private static byte GetByte(string r)
        {
            return byte.Parse(r.Substring(0, r.Length>= 3? 3:r.Length));
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            currentPort.Close();
        }

        private void btnOne_Click(object sender, RoutedEventArgs e)
        {
            if (!currentPort.IsOpen) return;
            currentPort.Write("1");
        }

        private void btnZero_Click(object sender, RoutedEventArgs e)
        {
            if (!currentPort.IsOpen)
            {
                return;
            }
            currentPort.Write("0");
        }

        private void btnSendText_Click(object sender, RoutedEventArgs e)
        {

            if (currentPort.IsOpen)
            {
                currentPort.Write(text.Text);
            }
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (currentPort.IsOpen)
            {
                currentPort.WriteLine($"b{Math.Round(slider.Value)}");
                Thread.Sleep(10);

            }

        }

        private void color_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (currentPort.IsOpen)
            {
                var color = e.NewValue.Value;
                var s = string.Format("c{0:D3}{1:D3}{2:D3}", color.R, color.G, color.B);
                currentPort.WriteLine(s);
                Thread.Sleep(10);

            }
        }


        private void cbbeffects_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (currentPort.IsOpen)
            {
                effectChanged = false;
                var message = cbbeffects.SelectedValue?.ToString();
                if (!string.IsNullOrEmpty(message))
                {
                    currentPort.WriteLine(message);

                }
            }
        }
    }
}
