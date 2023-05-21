using BlazorApp.Client.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System.Text;

namespace BlazorApp.Client.Pages;

public partial class Index
{
    private List<string> comList = new List<string>();
    private List<string> baudList = new List<string>();

    private string sendBoxValue = string.Empty;
    private string buttonText = "Connect";
    private string sendText = "Send";
    private string logAreaNotifications = string.Empty;
    private string comName = "COM1";
    private string baudRate = "2400";

    private void OnComSelected(IEnumerable<string> com)
    {
        comName = com.FirstOrDefault();
    }

    private void OnBaudRateSelected(IEnumerable<string> baudRate)
    {
        this.baudRate =baudRate.FirstOrDefault();
    }


    protected override Task OnInitializedAsync()
    {
        comList.Add("COM1");
        comList.Add("COM2");
        comList.Add("COM3");
        comList.Add("COM4");
        comList.Add("COM5");
        comList.Add("COM6");
        comList.Add("COM7");

        baudList.Add("2400");
        baudList.Add("4800");
        baudList.Add("9600");
        baudList.Add("19200");
        baudList.Add("38400");
        baudList.Add("57600");
        baudList.Add("115200");
        baudList.Add("230400");
        baudList.Add("460800");
        baudList.Add("921600");

        SerialPortService.NotificationReceived += SerialPortService_NotificationReceived;
        SerialPortService.SerialPortStateChanged += SerialPortService_SerialPortStateChanged;
        return base.OnInitializedAsync();
    }

    private void SerialPortService_SerialPortStateChanged(object? sender, bool e)
    {
        if(e)
        {
            buttonText = "Disconnect";
        }
        else
        {
            buttonText = "Connect";
        }
        StateHasChanged();
    }

    private void SerialPortService_NotificationReceived(object? sender, string e)
    {
        logAreaNotifications += $"{e}\n";
        StateHasChanged();
    }

    private void Connect()
    {
        if (buttonText == "Connect")
        {
            _=SerialPortService.Connect(comName, int.Parse(baudRate));
        }
        else
        {
            _ = SerialPortService.Disconnect();
        }
    }

    private void Send()
    {
        if(sendBoxValue.Length>0)
        {
            _ = SerialPortService.SendDataToSerialPort(sendBoxValue);
        }
    }


    /// <summary>
    /// Use of functions button.
    /// </summary>
    /// <param name="button">The button.</param>
    public void FunctionButton(int button)
    {
        char toTrasmit;

        switch (button)
        {
            case 0x01:
                toTrasmit = (char)0x01;
                break;
            case 0x02:
                toTrasmit = (char)0x02;
                break;
            case 0x03:
                toTrasmit = (char)0x03;
                return;
            case 0x04:
                toTrasmit = (char)0x04;
                return;
            case 0x05:
                toTrasmit = (char)0x05;
                break;
            case 0x06:
                toTrasmit = (char)0x06;
                break;
            case 0x07:
                toTrasmit = (char)0x07;
                break;
            case 0x08:
                toTrasmit = (char)0x07;
                break;
            case 0x09:
                toTrasmit = (char)0x09;
                break;
            case 0x0A:
                toTrasmit = (char)0x0A;
                break;
            case 0x0B:
                toTrasmit = (char)0x0B;
                break;
            case 0x0C:
                toTrasmit = (char)0x0C;
                break;
            case 0x0D:
                toTrasmit = (char)0x0D;
                break;
            case 0x0E:
                toTrasmit = (char)0x0E;
                break;
            case 0x0F:
                toTrasmit = (char)0x0F;
                break;
            case 0x10:
                toTrasmit = (char)0x10;
                break;
            case 0x11:
                var t = ((char)0x02).ToString() + ((char)2).ToString() + ((char)4).ToString() + ((char)13).ToString() + ((char)1).ToString() + ';' + ((char)64).ToString() + '|';
                _ = SerialPortService.SendDataToSerialPort(t);
                break;
            case 0x12:
                var t1 = ((char)2).ToString() + ((char)2).ToString() + ((char)4).ToString() + ((char)13).ToString() + ((char)1).ToString() + ';' + ((char)2).ToString() + '|';
                _ = SerialPortService.SendDataToSerialPort(t1);
                break;
            default:
                toTrasmit = '\0';
                break;
        }
    }

}
