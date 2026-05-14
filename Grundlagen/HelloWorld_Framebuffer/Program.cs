public class HelloWorld_FramebufferApp : SwitchApp
{
    public override void OnInit()
    {
		Graphics.Init(1280,720);
        Console.WriteLine("HelloWorld_Framebuffer started!");
        Console.WriteLine("Press + to exit.");
    }

    public override void OnFrame()
    {
		Graphics.DrawRect(0,0,1280,50,Color.Yellow);
		Graphics.FillRect(1,1,1278,48,Color.Gray);
		Graphics.DrawText(20,20,"Hello World!",Color.Black,3);
		
		
		Graphics.DrawRect(0,670,1280,50,Color.Yellow);
		Graphics.FillRect(1,671,1278,48,Color.Gray);
		Graphics.DrawText(20,681,"Press + to exit",Color.Black,2);
    }
}