using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp;
public partial class MudButtonWrap
{
    private string mudImage = string.Empty;
    private string style = "width:148px; height:48px";

    [Parameter] public string ButtonText { get; set; } = "-";

    [Parameter] public string ButtonId { get; set; } = "-";



    private void ButtonClicked()
    {
        switch (ButtonId)
        {
            case "cancel-button":
                
                break;
            case "confirm-button":
                
                break;
        }

    }
}
