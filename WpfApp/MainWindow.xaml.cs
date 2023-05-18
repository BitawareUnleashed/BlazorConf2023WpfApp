using System;
using System.IO.Ports;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Threading;

namespace WpfApp;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    #region variables
    //Richtextbox
    FlowDocument mcFlowDoc = new FlowDocument();
    Paragraph para = new Paragraph();
    //Serial 
    SerialPort serial = new SerialPort();
    string recieved_data;
    #endregion


    public MainWindow()
    {
        //InitializeComponent();
        InitializeComponent();
        //overwite to ensure state
        Connect_btn.Content = "Connect";
    }

    private void Connect_Comms(object sender, RoutedEventArgs e)
    {
        try
        {
            if (Connect_btn.Content == "Connect")
            {
                //Sets up serial port
                serial.PortName = Comm_Port_Names.Text;
                serial.BaudRate = Convert.ToInt32(Baud_Rates.Text);
                serial.Handshake = System.IO.Ports.Handshake.None;
                serial.Parity = Parity.None;
                serial.DataBits = 8;
                serial.StopBits = StopBits.One;
                serial.ReadTimeout = 200;
                serial.WriteTimeout = 50;
                serial.Open();

                //Sets button State and Creates function call on data recieved
                Connect_btn.Content = "Disconnect";
                serial.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(Recieve);

            }
            else
            {
                try // just in case serial port is not open could also be acheved using if(serial.IsOpen)
                {
                    serial.Close();
                    Connect_btn.Content = "Connect";
                }
                catch
                {
                }
            }
        }
        catch (Exception ex)
        {
            WriteData(ex.Message);
        }
    }

    #region Recieving

    private delegate void UpdateUiTextDelegate(string text);
    private void Recieve(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
    {
        // Collecting the characters received to our 'buffer' (string).
        recieved_data = serial.ReadExisting();
        Dispatcher.Invoke(DispatcherPriority.Send, new UpdateUiTextDelegate(WriteData), recieved_data);
    }
    private void WriteData(string text)
    {
        // Assign the value of the recieved_data to the RichTextBox.
        para.Inlines.Add(text);
        mcFlowDoc.Blocks.Add(para);
        Commdata.Document = mcFlowDoc;


    }

    #endregion

    #region Sending

    private void Send_Data(object sender, RoutedEventArgs e)
    {
        SerialCmdSend(SerialData.Text);
        SerialData.Text = "";
    }

    public void SerialCmdSend(char data)
    {
        byte[] h = new byte[1 * sizeof(char)];
        char[] arr = { data };
        System.Buffer.BlockCopy(arr, 0, h, 0, h.Length);

        serial.Write(arr, 0, 1);
        Thread.Sleep(1);

    }

    public void SerialCmdSend(string data)
    {
        if (serial.IsOpen)
        {
            try
            {
                //byte[] h = new byte[data.Length * sizeof(char)];
                //System.Buffer.BlockCopy(data.ToCharArray(), 0, h, 0, h.Length);

                //// Try to send char by char as is
                ////foreach (var item in data)
                ////{
                ////    serial.Write(h, 0, 1);
                ////    Thread.Sleep(1);
                ////}
                //for (int i = 0; i < h.Length; i = i + 2)
                //{
                //    serial.Write(h, i, 1);
                //    Thread.Sleep(1);
                //}

                //return;


                // Send the binary data out the port
                byte[] hexstring = Encoding.ASCII.GetBytes(data);
                //There is a intermitant problem that I came across
                //If I write more than one byte in succesion without a 
                //delay the PIC i'm communicating with will Crash
                //I expect this id due to PC timing issues ad they are
                //not directley connected to the COM port the solution
                //Is a ver small 1 millisecound delay between chracters
                foreach (byte hexval in hexstring)
                {
                    byte[] _hexval = new byte[] { hexval }; // need to convert byte to byte[] to write
                    serial.Write(_hexval, 0, 1);
                    Thread.Sleep(1);
                }
                para.Inlines.Add("SENT " + SerialData.Text + "\n\n");
                mcFlowDoc.Blocks.Add(para);
                Commdata.Document = mcFlowDoc;
            }
            catch (Exception ex)
            {
                para.Inlines.Add("Failed to SEND" + data + "\n" + ex + "\n");
                mcFlowDoc.Blocks.Add(para);
                Commdata.Document = mcFlowDoc;
            }
        }
        else
        {
            para.Inlines.Add("Failed to SEND\n" + data + "\nSerial port closed\n\n");
            mcFlowDoc.Blocks.Add(para);
            Commdata.Document = mcFlowDoc;
        }
    }

    #endregion

    private void Send_Click(object sender, RoutedEventArgs e)
    {
        Button b = sender as Button;

        if (b.Name == "Sender")
        {
            SerialCmdSend(SerialData.Text);
            return;
        }

        var buttonName = b.Name;
        var toTrasmit = '\0';
        switch (buttonName)
        {
            case "Sender1":
                toTrasmit = '\0';
                break;
            case "Sender2":
                toTrasmit = (char)0x01;
                break;
            case "Send3":
                var t = ((char)0x02).ToString() + ((char)2).ToString() + ((char)4).ToString() + ((char)13).ToString() + ((char)1).ToString() + ';' + ((char)64).ToString() + '|';
                SerialCmdSend(t);
                return;
            case "Send4":
                var t1 = ((char)2).ToString() + ((char)2).ToString() + ((char)4).ToString() + ((char)13).ToString() + ((char)1).ToString() + ';' + ((char)2).ToString() + '|';
                SerialCmdSend(t1);
                return;
            case "Sender5":
                toTrasmit = '\0';
                break;
            case "Sender6":
                toTrasmit = '\0';
                break;
            case "Sender7":
                toTrasmit = '\0';
                break;
            case "Sender8":
                toTrasmit = '\0';
                break;
            case "Sender9":
                toTrasmit = '\0';
                break;
            case "Sender10":
                toTrasmit = '\0';
                break;
            case "Sender11":
                toTrasmit = '\0';
                break;
            case "Sender12":
                toTrasmit = '\0';
                break;
            case "Sender13":
                toTrasmit = '\0';
                break;
            case "Sender14":
                toTrasmit = '\0';
                break;
            case "Sender15":
                toTrasmit = '\0';
                break;
            case "Sender16":
                toTrasmit = '\0';
                break;
            case "Sender16b":
                toTrasmit = '\0';
                break;
            case "Sender18":
                toTrasmit = '\0';
                break;
            default:
                toTrasmit = '\0';
                break;
        }
        try
        {
            SerialCmdSend(toTrasmit);
        }
        catch (Exception ex)
        {
            Paragraph p = new Paragraph();
            p.Inlines.Add(ex.Message);
            p.Foreground = new SolidColorBrush(Colors.Red);
            mcFlowDoc.Blocks.Add(p);
            Commdata.Document = mcFlowDoc;
        }
    }

}
