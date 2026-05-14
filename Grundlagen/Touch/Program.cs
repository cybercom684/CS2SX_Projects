public class TouchApp : SwitchApp
{
    private int _touchX;
    private int _touchY;
    private bool _touching;

    public override void OnInit()
    {
        Graphics.Init(1280, 720);
    }

    public override void OnFrame()
    {
        var touch = Input.GetTouch();

        _touching = touch.count > 0;

        if (_touching)
        {
            _touchX = touch.x[0];
            _touchY = touch.y[0];
        }

        Graphics.FillScreen(Color.Black);
        Graphics.DrawText(100, 100, "Touch Demo", Color.White, 2);

        if (_touching)
        {
            Graphics.DrawText(100, 200, "Beruehrt!", Color.Green, 2);
            Graphics.FillCircle(_touchX, _touchY, 20, Color.Yellow);
            Graphics.DrawText(100, 260, $"X: {_touchX}  Y: {_touchY}", Color.Gray, 1);
        }
        else
        {
            Graphics.DrawText(100, 200, "Kein Touch", Color.Gray, 2);
        }

        Graphics.DrawText(100, 620, "+ zum Beenden", Color.Gray, 1);
    }
}