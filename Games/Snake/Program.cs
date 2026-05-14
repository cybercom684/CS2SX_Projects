public class SnakeApp : SwitchApp
{
    private const int GridW      = 64;
    private const int GridH      = 36;
    private const int FrameDelay = 7;
    private const string SavePath = "/switch/Snake/highscore.txt";
    private const string SaveDir  = "/switch/Snake";

    // Snake-Body
    private int[] _snakeX = new int[GridW * GridH];
    private int[] _snakeY = new int[GridW * GridH];
    private int _snakeLen;
    private int _dirX;
    private int _dirY;
    private int _nextDirX;
    private int _nextDirY;

    // Kollisions-Map: gridIndex → 1 (belegt)
    private Dictionary<int, int> _occupied = new Dictionary<int, int>();

    // Food
    private int _foodX;
    private int _foodY;

    // State
    private bool _gameOver;
    private bool _paused;
    private bool _newBest;
    private int  _score;
    private int  _highscore;
    private int  _frameTimer;
    private int  _batteryTimer;
    private BatteryInfo _battery;

    public override void OnInit()
    {
        Graphics.Init(1280, 720);
        Audio.Init(44100);

        if (!Directory.Exists(SaveDir))
            Directory.CreateDirectory(SaveDir);

        if (File.Exists(SavePath))
        {
            string raw = File.ReadAllText(SavePath);
            if (!int.TryParse(raw, out _highscore))
                _highscore = 0;
        }

        _battery = System.GetBattery();
        ResetGame();
    }

    private void ResetGame()
    {
        _snakeLen  = 3;
        _snakeX[0] = GridW / 2;
        _snakeY[0] = GridH / 2;
        _snakeX[1] = GridW / 2 - 1;
        _snakeY[1] = GridH / 2;
        _snakeX[2] = GridW / 2 - 2;
        _snakeY[2] = GridH / 2;

        _dirX     = 1;
        _dirY     = 0;
        _nextDirX = 1;
        _nextDirY = 0;

        _gameOver   = false;
        _paused     = false;
        _newBest    = false;
        _score      = 0;
        _frameTimer = 0;
        _foodX      = 10;
        _foodY      = 10;

        _occupied.Clear();
        for (int i = 0; i < _snakeLen; i++)
            _occupied.Add(_snakeX[i] + _snakeY[i] * GridW, 1);

        SpawnFood();
    }

    private void SpawnFood()
    {
        int tries = 0;
        do
        {
            _foodX = Random.Shared.Next(0, GridW);
            _foodY = Random.Shared.Next(0, GridH);
            tries++;
        }
        while (_occupied.ContainsKey(_foodX + _foodY * GridW) && tries < 200);
    }

    public override void OnFrame()
    {
        // Akku alle ~5s abfragen
        _batteryTimer++;
        if (_batteryTimer >= 300)
        {
            _battery = System.GetBattery();
            _batteryTimer = 0;
        }

        HandleInput();
        Update();
        Render();
    }

    private void HandleInput()
    {
        if (Input.IsDown(NpadButton.Minus))
            _paused = !_paused;

        if (_gameOver)
        {
            if (Input.IsDown(NpadButton.A))
                ResetGame();
            return;
        }

        if (_paused)
        {
            if (Input.IsDown(NpadButton.A))
                _paused = false;
            return;
        }

        // D-Pad
        if (Input.IsDown(NpadButton.Up)    && _dirY == 0) { _nextDirX = 0;  _nextDirY = -1; }
        if (Input.IsDown(NpadButton.Down)  && _dirY == 0) { _nextDirX = 0;  _nextDirY = 1;  }
        if (Input.IsDown(NpadButton.Left)  && _dirX == 0) { _nextDirX = -1; _nextDirY = 0;  }
        if (Input.IsDown(NpadButton.Right) && _dirX == 0) { _nextDirX = 1;  _nextDirY = 0;  }

        // Analog-Stick
        StickPos stick = Input.GetStickLeft();
        if (stick.x > 10000  && _dirX == 0) { _nextDirX = 1;  _nextDirY = 0;  }
        if (stick.x < -10000 && _dirX == 0) { _nextDirX = -1; _nextDirY = 0;  }
        if (stick.y > 10000  && _dirY == 0) { _nextDirX = 0;  _nextDirY = -1; }
        if (stick.y < -10000 && _dirY == 0) { _nextDirX = 0;  _nextDirY = 1;  }

        // Touch: rechts oben → Pause
        TouchState touch = Input.GetTouch();
        if (touch.count > 0 && touch.x[0] > 900 && touch.y[0] < 80)
            _paused = !_paused;
    }

    private void Update()
    {
        if (_gameOver || _paused) return;

        _frameTimer++;
        if (_frameTimer < FrameDelay) return;
        _frameTimer = 0;

        _dirX = _nextDirX;
        _dirY = _nextDirY;

        int newX = _snakeX[0] + _dirX;
        int newY = _snakeY[0] + _dirY;

        // Wand-Kollision
        if (newX < 0 || newX >= GridW || newY < 0 || newY >= GridH)
        {
            TriggerGameOver();
            return;
        }

        // Selbst-Kollision via Dictionary (O(1))
        if (_occupied.ContainsKey(newX + newY * GridW))
        {
            TriggerGameOver();
            return;
        }

        // Schwanz aus Map entfernen
        _occupied.Remove(_snakeX[_snakeLen - 1] + _snakeY[_snakeLen - 1] * GridW);

        // Body verschieben
        for (int i = _snakeLen - 1; i > 0; i--)
        {
            _snakeX[i] = _snakeX[i - 1];
            _snakeY[i] = _snakeY[i - 1];
        }

        _snakeX[0] = newX;
        _snakeY[0] = newY;
        _occupied.Add(newX + newY * GridW, 1);

        // Food gefressen?
        if (newX == _foodX && newY == _foodY)
        {
            _snakeLen++;
            _score++;
            Audio.PlayTone(880.0f, 0.3f, 80);

            if (_score > _highscore)
            {
                _highscore = _score;
                _newBest   = true;
                File.WriteAllText(SavePath, _highscore.ToString());
            }

            SpawnFood();
        }
    }

    private void TriggerGameOver()
    {
        _gameOver = true;
        Audio.PlayTone(150.0f, 0.5f, 600);
    }

    private void Render()
    {
        Graphics.FillScreen(Color.Black);
        SnakeRenderer.DrawGrid();
        SnakeRenderer.DrawFood(_foodX, _foodY);
        SnakeRenderer.DrawSnake(_snakeX, _snakeY, _snakeLen);
        SnakeRenderer.DrawHUD(_score, _highscore, _battery);

        if (_paused)
            SnakeRenderer.DrawPauseOverlay();

        if (_gameOver)
            SnakeRenderer.DrawGameOver(_score, _highscore, _newBest);
    }

    public override void OnExit()
    {
        Audio.Exit();
    }
}