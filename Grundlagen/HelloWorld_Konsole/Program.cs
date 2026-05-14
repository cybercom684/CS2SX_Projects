public class HelloWorld_KonsoleApp : SwitchApp
{
    private Label _label;
	private Label _label_exit;

    public override void OnInit()
    {
        _label = new Label("Hello World from C#!");
        _label.X = 5;
        _label.Y = 5;
        Form.Add(_label);
		
		_label_exit = new Label("Press + to exit");
        _label_exit.X = 5;
        _label_exit.Y = 620;
        Form.Add(_label_exit);
    }

    public override void OnFrame() { }
}