using System;

public class keyboardApp : SwitchApp
{
    private Keyboard _keyboard;

    public override void OnInit()
    {
        Graphics.Init(1280, 720);
        _keyboard = new Keyboard();
        _keyboard.Show("test");
    }

    public override void OnFrame()
    {
        _keyboard.Update();
        _keyboard.Draw();

        if (_keyboard.WasConfirmed())
        {
            string text = _keyboard.GetBuffer();
            Console.WriteLine("Eingegeben: " + text);
        }
    }
}