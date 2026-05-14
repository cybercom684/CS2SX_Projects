using System;

public class keyboardApp : SwitchApp
{
    private Keyboard _keyboard;
    private string _enteredText = "Keine Eingabe";

    public override void OnInit()
    {
        Graphics.Init(1280, 720);
        _keyboard = new Keyboard();
        _keyboard.Show("Test");
    }

    public override void OnFrame()
    {
        _keyboard.Update();
        _keyboard.Draw();

        if (_keyboard.WasConfirmed())
        {
            _enteredText = _keyboard.GetBuffer();
        }

        Graphics.DrawText(20, 20, _enteredText, Color.White, 4);
    }
}