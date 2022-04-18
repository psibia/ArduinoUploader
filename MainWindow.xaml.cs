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
using System.IO.Ports;
using ArduinoUploader;
using ArduinoUploader.Hardware;

namespace ConnectToArduino
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SerialPort sp = new SerialPort();
        string portName;
        public MainWindow()
        {
            InitializeComponent();
            sp.DataReceived += new SerialDataReceivedEventHandler(DataIn);
        }

        private void COM_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedcomboitem = sender as ComboBox;
            string name = selectedcomboitem.SelectedItem as string;

            portName = COM.SelectedItem as string;
            sp.PortName = portName;
            sp.BaudRate = 9600;
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            sp.Open();
            MessageBox.Show("Порт открыт");
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            sp.Close();
            MessageBox.Show("Порт закрыт");
        }

        private void Write_Click(object sender, RoutedEventArgs e)
        {
            sp.Write(TextOut.Text);
            MessageBox.Show("Данные отправлены");
        }

        public void DataIn (object sender, SerialDataReceivedEventArgs e)
        {
            Dispatcher.Invoke(() => TextIn.Text += sp.ReadExisting());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            sp.Close();
            try
            {

                var uploader = new ArduinoSketchUploader(
                    new ArduinoSketchUploaderOptions()
                    {
                        FileName = AppDomain.CurrentDomain.BaseDirectory + @"sketch_apr18a.ino.hex",
                        PortName = portName,
                        ArduinoModel = ArduinoModel.NanoR3
                    });
                uploader.UploadSketch();
                this.Show();
                MessageBox.Show("Прошивка успешно загружена");
                sp.Open();
            }
            catch
            {
                MessageBox.Show("Произошел сбой при загрузке скетча");
            }
        }
    }
}
