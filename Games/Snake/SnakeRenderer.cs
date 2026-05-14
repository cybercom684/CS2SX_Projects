public static class SnakeRenderer
{
    private const int CellSize = 20;
    private const int GridW    = 64;
    private const int GridH    = 36;
    private const int HudX     = GridW * CellSize + 10;

    public static void DrawGrid()
    {
        Graphics.FillRect(0, 0, GridW * CellSize, GridH * CellSize, Color.RGB(15, 15, 15));
        Graphics.DrawRect(0, 0, GridW * CellSize, GridH * CellSize, Color.DarkGray);
    }

    public static void DrawSnake(int[] snakeX, int[] snakeY, int snakeLen)
    {
        for (int i = snakeLen - 1; i >= 0; i--)
        {
            int x = snakeX[i] * CellSize + 1;
            int y = snakeY[i] * CellSize + 1;
            int s = CellSize - 2;

            if (i == 0)
            {
                Graphics.FillRoundedRect(x, y, s, s, 4, Color.Lime);
                Graphics.DrawRoundedRect(x, y, s, s, 4, Color.Green);
            }
            else
            {
                int green = 180 - i * 2;
                int blue  = 80  - i;
                if (green < 40) green = 40;
                if (blue  < 0)  blue  = 0;
                Graphics.FillRoundedRect(x, y, s, s, 3, Color.RGB(0, green, blue));
            }
        }
    }

    public static void DrawFood(int foodX, int foodY)
    {
        int cx = foodX * CellSize + CellSize / 2;
        int cy = foodY * CellSize + CellSize / 2;
        int r  = CellSize / 2 - 2;

        Graphics.FillCircle(cx, cy, r, Color.Red);
        Graphics.DrawCircle(cx, cy, r, Color.RGB(255, 100, 100));
        Graphics.FillCircle(cx - 2, cy - 2, 2, Color.White);
    }

    public static void DrawHUD(int score, int highscore, BatteryInfo battery)
    {
        int hx = HudX;

        // Panel
        Graphics.FillRect(hx - 8, 0, 1280 - hx + 8, 720, Color.RGB(10, 10, 10));
        Graphics.DrawLine(hx - 8, 0, hx - 8, 720, Color.DarkGray);

        // Titel
        Graphics.DrawTextShadow(hx, 20, "SNAKE", Color.Lime, Color.RGB(0, 60, 0), 3);

        Graphics.DrawLine(hx, 80, 1275, 80, Color.RGB(40, 40, 40));

        // Score
        Graphics.DrawText(hx, 95,  "SCORE",          Color.Gray,   1);
        Graphics.DrawText(hx, 110, score.ToString(),  Color.Yellow, 3);

        // Highscore
        Graphics.DrawText(hx, 195, "BEST",               Color.Gray,   1);
        Graphics.DrawText(hx, 210, highscore.ToString(),  Color.Orange, 2);

        Graphics.DrawLine(hx, 260, 1275, 260, Color.RGB(40, 40, 40));

        // Controls
        Graphics.DrawText(hx, 275, "STEUERUNG",       Color.Gray,     1);
        Graphics.DrawText(hx, 292, "D-Pad / Stick",   Color.DarkGray, 1);
        Graphics.DrawText(hx, 315, "- = Pause",       Color.DarkGray, 1);
        Graphics.DrawText(hx, 330, "A = Fortfahren",  Color.DarkGray, 1);
        Graphics.DrawText(hx, 345, "+ = Beenden",     Color.DarkGray, 1);
        Graphics.DrawText(hx, 365, "Touch oben",      Color.DarkGray, 1);
        Graphics.DrawText(hx, 380, "rechts = Pause",  Color.DarkGray, 1);

        Graphics.DrawLine(hx, 660, 1275, 660, Color.RGB(40, 40, 40));

        // Akkustand
        uint battCol = Color.Green;
        if (battery.percent <= 50) battCol = Color.Yellow;
        if (battery.percent <= 20) battCol = Color.Red;

        Graphics.DrawText(hx, 668, battery.percent.ToString() + "%", battCol, 1);

        if (battery.charging)
            Graphics.DrawText(hx + 45, 668, "CHG", Color.Cyan, 1);
    }

    public static void DrawPauseOverlay()
    {
        Graphics.FillRectAlpha(0, 0, GridW * CellSize, GridH * CellSize, Color.Black, 160);
        Graphics.FillRoundedRect(220, 275, 840, 170, 14, Color.RGB(20, 20, 20));
        Graphics.DrawRoundedRect(220, 275, 840, 170, 14, Color.Cyan);
        Graphics.DrawTextShadow(375, 300, "PAUSE", Color.Cyan, Color.RGB(0, 80, 80), 3);
        Graphics.DrawLine(220, 370, 1060, 370, Color.RGB(40, 40, 40));
        Graphics.DrawText(295, 385, "A = Fortfahren          - = Pause halten", Color.White, 1);
    }

    public static void DrawGameOver(int score, int highscore, bool newBest)
    {
        Graphics.FillRectAlpha(0, 0, GridW * CellSize, GridH * CellSize, Color.Black, 180);
        Graphics.FillRoundedRect(200, 235, 880, 250, 16, Color.RGB(25, 0, 0));
        Graphics.DrawRoundedRect(200, 235, 880, 250, 16, Color.Red);
        Graphics.DrawTextShadow(310, 260, "GAME OVER", Color.Red, Color.RGB(80, 0, 0), 3);
        Graphics.DrawLine(200, 335, 1080, 335, Color.RGB(80, 0, 0));

        if (newBest)
        {
            Graphics.DrawText(310, 350, "NEUER REKORD!", Color.Yellow, 2);
            Graphics.DrawText(310, 390, "Punkte: " + score.ToString(), Color.White, 2);
        }
        else
        {
            Graphics.DrawText(310, 355, "Punkte:    " + score.ToString(),     Color.White,  2);
            Graphics.DrawText(310, 395, "Rekord:    " + highscore.ToString(), Color.Orange, 2);
        }

        Graphics.DrawLine(200, 440, 1080, 440, Color.RGB(80, 0, 0));
        Graphics.DrawText(310, 452, "A = Neustart", Color.Gray, 2);
    }
}