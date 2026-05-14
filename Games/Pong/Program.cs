public class PongApp : SwitchApp
{
    private const int W = 1280;
    private const int H = 720;
    private const int PaddleW = 15;
    private const int PaddleH = 100;
    private const int BallSize = 12;
    private const int PaddleSpeed = 6;
    private const int WinScore = 5;

    // Paddles
    private int _p1Y;
    private int _p2Y;

    // Ball
    private int _ballX;
    private int _ballY;
    private int _ballDX;
    private int _ballDY;

    // Score
    private int _score1;
    private int _score2;

    private bool _gameOver;
    private int _winner;

    public override void OnInit()
    {
        Graphics.Init(W, H);
        ResetGame();
    }

    private void ResetGame()
    {
        _p1Y = H / 2 - PaddleH / 2;
        _p2Y = H / 2 - PaddleH / 2;
        _score1 = 0;
        _score2 = 0;
        _gameOver = false;
        _winner = 0;
        ResetBall(1);
    }

    private void ResetBall(int dir)
    {
        _ballX = W / 2;
        _ballY = H / 2;
        _ballDX = dir * 5;
        _ballDY = 3;
    }

    public override void OnFrame()
    {
        if (_gameOver)
        {
            if (Input.IsDown(NpadButton.A))
                ResetGame();
            DrawGameOver();
            return;
        }

        // Input Spieler 1 (links) — D-Pad
        if (Input.IsHeld(NpadButton.Up) && _p1Y > 0)
            _p1Y -= PaddleSpeed;
        if (Input.IsHeld(NpadButton.Down) && _p1Y < H - PaddleH)
            _p1Y += PaddleSpeed;

        // Input Spieler 2 (rechts) — Sticks simuliert via L/R Tasten
        if (Input.IsHeld(NpadButton.L) && _p2Y > 0)
            _p2Y -= PaddleSpeed;
        if (Input.IsHeld(NpadButton.R) && _p2Y < H - PaddleH)
            _p2Y += PaddleSpeed;

        // Ball bewegen
        _ballX += _ballDX;
        _ballY += _ballDY;

        // Oben/Unten
        if (_ballY <= 0) { _ballY = 0; _ballDY = -_ballDY; }
        if (_ballY >= H - BallSize) { _ballY = H - BallSize; _ballDY = -_ballDY; }

        // Paddle 1 Kollision (links)
        if (_ballX <= 30 + PaddleW &&
            _ballX >= 30 &&
            _ballY + BallSize >= _p1Y &&
            _ballY <= _p1Y + PaddleH)
        {
            _ballDX = -_ballDX;
            _ballX = 30 + PaddleW;
            int rel = (_ballY + BallSize / 2) - (_p1Y + PaddleH / 2);
            _ballDY = rel / 10;
        }

        // Paddle 2 Kollision (rechts)
        if (_ballX + BallSize >= W - 30 - PaddleW &&
            _ballX + BallSize <= W - 30 &&
            _ballY + BallSize >= _p2Y &&
            _ballY <= _p2Y + PaddleH)
        {
            _ballDX = -_ballDX;
            _ballX = W - 30 - PaddleW - BallSize;
            int rel = (_ballY + BallSize / 2) - (_p2Y + PaddleH / 2);
            _ballDY = rel / 10;
        }

        // Punkte
        if (_ballX < 0)
        {
            _score2++;
            if (_score2 >= WinScore) { _gameOver = true; _winner = 2; }
            else ResetBall(1);
        }
        if (_ballX > W)
        {
            _score1++;
            if (_score1 >= WinScore) { _gameOver = true; _winner = 1; }
            else ResetBall(-1);
        }

        // Draw
        Graphics.FillScreen(Color.Black);
        DrawCenterLine();
        DrawPaddle(30, _p1Y, Color.White);
        DrawPaddle(W - 30 - PaddleW, _p2Y, Color.White);
        Graphics.FillRect(_ballX, _ballY, BallSize, BallSize, Color.White);
        DrawHUD();
    }

    private void DrawPaddle(int x, int y, uint color)
    {
        Graphics.FillRect(x, y, PaddleW, PaddleH, color);
    }

    private void DrawCenterLine()
{
    for (int i = 0; i < H; i += 30)
        Graphics.FillRect(W / 2 - 2, i, 4, 15, Color.Gray);
}

    private void DrawHUD()
    {
        Graphics.DrawText(W / 2 - 80, 30, _score1.ToString(), Color.White, 3);
        Graphics.DrawText(W / 2 + 40, 30, _score2.ToString(), Color.White, 3);
        Graphics.DrawText(20, H - 30, "Hoch/Runter", Color.Gray, 1);
        Graphics.DrawText(W - 180, H - 30, "L / R", Color.Gray, 1);
        Graphics.DrawText(W / 2 - 40, H - 30, "+ Beenden", Color.Gray, 1);
    }

    private void DrawGameOver()
    {
        Graphics.FillScreen(Color.Black);
        Graphics.DrawText(400, 280, "GAME OVER", Color.Red, 3);
        Graphics.DrawText(400, 360, _winner == 1 ? "Spieler 1 gewinnt!" : "Spieler 2 gewinnt!", Color.Yellow, 2);
        Graphics.DrawText(400, 440, "A = Neustart", Color.White, 2);
        Graphics.DrawText(400, 500, "+ Beenden", Color.Gray, 1);
    }
}