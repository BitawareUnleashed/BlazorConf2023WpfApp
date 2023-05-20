using System.IO.Ports;
using System.Text;

namespace BlazorApp.Server.Models;

public class SerialCommunication
{
    SerialPort serial = new SerialPort();
    ICommunicationServer notificationServer;
    public SerialCommunication(ICommunicationServer notificationServer)
    {
        this.notificationServer = notificationServer;
    }

    public void SerialCmdSend(char data)
    {
        if (serial.IsOpen)
        {
            byte[] h = new byte[1 * sizeof(char)];
            char[] arr = { data };
            Buffer.BlockCopy(arr, 0, h, 0, h.Length);

            serial.Write(arr, 0, 1);
            Thread.Sleep(1);
            notificationServer.Send($"SENT {(int)data} char properly on serial port");
        }
        else
        {
            notificationServer.Send($"Failed to SEND  {(int)data} char. Serial port closed");
        }
    }
    public void SerialCmdSend(string data)
    {
        if (serial.IsOpen)
        {
            try
            {
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
                notificationServer.Send($"SENT {data} properly on serial port");
            }
            catch (Exception ex)
            {
                notificationServer.Send($"Failed to SEND {data} {ex}");
            }
        }
        else
        {
            notificationServer.Send($"Failed to SEND  {data}. Serial port closed");
        }
    }

    public void ConnectToSerial(string portName, int baudRate)
    {
        try
        {
            //Sets up serial port
            serial.PortName = portName;
            serial.BaudRate = baudRate;
            serial.Handshake = System.IO.Ports.Handshake.None;
            serial.Parity = Parity.None;
            serial.DataBits = 8;
            serial.StopBits = StopBits.One;
            serial.ReadTimeout = 200;
            serial.WriteTimeout = 50;
            serial.Open();

            serial.DataReceived += Serial_DataReceived;

            notificationServer.Send($"Port open on {portName} and baud rate {baudRate}.");
        }
        catch (Exception ex)
        {
            notificationServer.Send($"Error opening serial port: {ex.Message}");
        }
    }

    public void Disconnect()
    {
        serial.Close();
        notificationServer.Send($"Disconnected");
    }

    private void Serial_DataReceived(object sender, SerialDataReceivedEventArgs e)
    {
        var receivedData = serial.ReadExisting();
        // Send notification to frontend
        notificationServer.Send(receivedData);
    }

    internal void SetName(string requestData)
    {
        serial.PortName = requestData;
    }

    internal void SetBaudRate(int requestData)
    {
        serial.BaudRate = requestData;
    }
}
