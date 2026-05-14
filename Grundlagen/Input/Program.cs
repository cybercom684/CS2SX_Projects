public class InputApp : SwitchApp
{
    private string _lastPressed = "Keine Taste";

    public override void OnInit()
    {
        Graphics.Init(1280, 720);
    }

    public override void OnFrame()
    {
        if (Input.IsDown(NpadButton.A)) _lastPressed = "A";
        else if (Input.IsDown(NpadButton.B)) _lastPressed = "B";
        else if (Input.IsDown(NpadButton.X)) _lastPressed = "X";
        else if (Input.IsDown(NpadButton.Y)) _lastPressed = "Y";
        else if (Input.IsDown(NpadButton.Up)) _lastPressed = "D-Pad Hoch";
        else if (Input.IsDown(NpadButton.Down)) _lastPressed = "D-Pad Runter";
        else if (Input.IsDown(NpadButton.Left)) _lastPressed = "D-Pad Links";
        else if (Input.IsDown(NpadButton.Right)) _lastPressed = "D-Pad Rechts";
        else if (Input.IsDown(NpadButton.L)) _lastPressed = "L";
        else if (Input.IsDown(NpadButton.R)) _lastPressed = "R";
        else if (Input.IsDown(NpadButton.ZL)) _lastPressed = "ZL";
        else if (Input.IsDown(NpadButton.ZR)) _lastPressed = "ZR";

        Graphics.FillScreen(Color.Black);
        Graphics.DrawText(100, 100, "Input Demo", Color.White, 2);
        Graphics.DrawText(100, 200, "Zuletzt gedrueckt:", Color.Gray, 1);
        Graphics.DrawText(100, 240, _lastPressed, Color.Yellow, 3);
        Graphics.DrawText(100, 620, "+ zum Beenden", Color.Gray, 1);
    }
}