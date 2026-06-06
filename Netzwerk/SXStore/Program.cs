using SXStore.Core;
using SXStore.Core.Controller;
using SXStore.Core.Repository;
using SXStore.Core.Screens;

public class SXStoreApp : SwitchApp
{
    private readonly InputHandler _input = new InputHandler();
    private readonly IAppRepository _repository = new MockAppRepository();

    public override void OnInit()
    {
        Graphics.Init(GlobalDefines.WindowWidth, GlobalDefines.WindowHeight);
        Navigator.SetShell(new SidebarScreen(_input, _repository));
    }

    public override void OnFrame()
    {
        _input.Poll();
        Navigator.Update();

        Graphics.BeginFrame();
        Navigator.Draw();
        Graphics.EndFrame();
    }
}