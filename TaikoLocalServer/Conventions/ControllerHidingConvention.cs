using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace TaikoLocalServer.Conventions;

public class ControllerHidingConvention : IActionModelConvention
{
    public void Apply(ActionModel action)
    {
        if (action.Controller.ControllerType.Namespace != "TaikoLocalServer.Controllers.Api")
        {
            action.ApiExplorer.IsVisible = false;
        }
    }
}