public class SpaceInvadersApp : SwitchApp
{
    private const int W = 1280;
    private const int H = 720;
    private const int ALIEN_COLS = 11;
    private const int ALIEN_ROWS = 5;
    private const int ALIEN_W = 24;
    private const int ALIEN_H = 20;
    private const int ALIEN_START_X = 100;
    private const int ALIEN_START_Y = 60;
    private const int PLAYER_W = 30;
    private const int PLAYER_H = 20;
    private const int PLAYER_Y = H - 70;
    private const int PLAYER_BULLET_SPEED = 8;
    private const int BARRIER_COUNT = 4;
    private const int BARRIER_SIZE = 8;

    private int[] _aliens = new int[ALIEN_COLS * ALIEN_ROWS]; // 1=lebt
    private int _alienOffsetX, _alienOffsetY;
    private int _alienMoveDir = 1;
    private int _alienMoveStep = 4;
    private int _alienMoveDelay = 20;
    private int _alienMoveCounter;
    private int _alienStepDown; // 0/1
    private int _aliensLeft;

    private int _playerX;
    private int[] _playerBulletX = new int[3];
    private int[] _playerBulletY = new int[3];
    private int[] _playerBulletActive = new int[3];
    private int[] _alienBulletX = new int[6];
    private int[] _alienBulletY = new int[6];
    private int[] _alienBulletActive = new int[6];
    private int _alienShootDelay;

    private int[] _barriers = new int[BARRIER_COUNT * BARRIER_SIZE * BARRIER_SIZE]; // 1=solid

    private int _score, _highscore, _lives, _level;
    private int _gameState; // 0=Start,1=Spielen,2=GameOver
    private int _invincibleFrames;
    private uint _randState = 123456789;
    private int _lastA, _lastPlus;

    public override void OnInit()
    {
        Graphics.Init(W, H);
        ResetGame();
    }

    private int NextRandom(int min, int max)
    {
        _randState = _randState * 1103515245 + 12345;
        return (int)(min + (_randState % (uint)(max - min)));
    }

    private void ResetGame()
    {
        _score = 0;
        _lives = 3;
        _level = 1;
        _gameState = 0;
        _invincibleFrames = 0;
        InitAliens();
        InitBarriers();
        _playerX = (W - PLAYER_W) / 2;
        for (int i = 0; i < 3; i++) _playerBulletActive[i] = 0;
        for (int i = 0; i < 6; i++) _alienBulletActive[i] = 0;
        _alienShootDelay = 0;
        _alienMoveDelay = 20;
        _alienMoveStep = 4;
        _alienMoveDir = 1;
        _alienStepDown = 0;
        _alienOffsetX = 0;
        _alienOffsetY = 0;
    }

    private void InitAliens()
    {
        for (int row = 0; row < ALIEN_ROWS; row++)
            for (int col = 0; col < ALIEN_COLS; col++)
                _aliens[row * ALIEN_COLS + col] = 1;
        _aliensLeft = ALIEN_COLS * ALIEN_ROWS;
    }

    private void InitBarriers()
    {
        for (int b = 0; b < BARRIER_COUNT; b++)
            for (int row = 0; row < BARRIER_SIZE; row++)
                for (int col = 0; col < BARRIER_SIZE; col++)
                {
                    int idx = b * BARRIER_SIZE * BARRIER_SIZE + row * BARRIER_SIZE + col;
                    if (row >= BARRIER_SIZE - 2 && (col < 2 || col > BARRIER_SIZE - 3))
                        _barriers[idx] = 0;
                    else
                        _barriers[idx] = 1;
                }
    }

    private void MoveAliens()
    {
        if (_alienStepDown == 1)
        {
            _alienOffsetY += ALIEN_H / 2;
            _alienStepDown = 0;
            _alienMoveDir = -_alienMoveDir;
            if (_alienOffsetY + ALIEN_ROWS * ALIEN_H >= PLAYER_Y - 20)
                _gameState = 2;
            return;
        }

        int edge = 0;
        for (int row = 0; row < ALIEN_ROWS; row++)
            for (int col = 0; col < ALIEN_COLS; col++)
                if (_aliens[row * ALIEN_COLS + col] == 1)
                {
                    int x = ALIEN_START_X + _alienOffsetX + col * ALIEN_W + (_alienMoveDir > 0 ? ALIEN_W : 0);
                    if (x >= W - 40 || x <= 20) edge = 1;
                }
        if (edge != 0)
        {
            _alienStepDown = 1;
            if (_alienMoveDelay > 5) _alienMoveDelay -= 2;
            return;
        }

        _alienOffsetX += _alienMoveDir * _alienMoveStep;
    }

    private void AlienShoot()
    {
        int col = NextRandom(0, ALIEN_COLS);
        int row = ALIEN_ROWS - 1;
        while (row >= 0 && _aliens[row * ALIEN_COLS + col] == 0) row--;
        if (row < 0) return;

        for (int i = 0; i < 6; i++)
            if (_alienBulletActive[i] == 0)
            {
                _alienBulletActive[i] = 1;
                _alienBulletX[i] = ALIEN_START_X + _alienOffsetX + col * ALIEN_W + ALIEN_W / 2 - 3;
                _alienBulletY[i] = ALIEN_START_Y + _alienOffsetY + row * ALIEN_H + ALIEN_H;
                break;
            }
    }

    private void UpdatePlayerBullets()
    {
        for (int i = 0; i < 3; i++)
        {
            if (_playerBulletActive[i] == 0) continue;
            int x = _playerBulletX[i];
            int y = _playerBulletY[i];
            y -= PLAYER_BULLET_SPEED;
            if (y < 0)
            {
                _playerBulletActive[i] = 0;
                continue;
            }
            _playerBulletY[i] = y;

            int hit = 0;
            for (int row = 0; row < ALIEN_ROWS; row++)
                for (int col = 0; col < ALIEN_COLS; col++)
                    if (_aliens[row * ALIEN_COLS + col] == 1)
                    {
                        int ax = ALIEN_START_X + _alienOffsetX + col * ALIEN_W;
                        int ay = ALIEN_START_Y + _alienOffsetY + row * ALIEN_H;
                        if (x + 3 > ax && x < ax + ALIEN_W && y + 3 > ay && y < ay + ALIEN_H)
                        {
                            _aliens[row * ALIEN_COLS + col] = 0;
                            _playerBulletActive[i] = 0;
                            hit = 1;
                            _aliensLeft--;
                            if (row == 0) _score += 30;
                            else if (row < 3) _score += 20;
                            else _score += 10;
                            if (_score > _highscore) _highscore = _score;
                            if (_aliensLeft == 0) NextLevel();
                            break;
                        }
                    }
            if (hit == 1) continue;

            for (int b = 0; b < BARRIER_COUNT; b++)
                for (int row = 0; row < BARRIER_SIZE; row++)
                    for (int col = 0; col < BARRIER_SIZE; col++)
                    {
                        int idx = b * BARRIER_SIZE * BARRIER_SIZE + row * BARRIER_SIZE + col;
                        if (_barriers[idx] == 1)
                        {
                            int bx = (W / (BARRIER_COUNT + 1)) * (b + 1) - 40 + col * 5;
                            int by = H - 120 + row * 5;
                            if (x + 3 > bx && x < bx + 5 && y + 3 > by && y < by + 5)
                            {
                                _barriers[idx] = 0;
                                _playerBulletActive[i] = 0;
                                hit = 1;
                                break;
                            }
                        }
                    }
        }
    }

    private void NextLevel()
    {
        _level++;
        InitAliens();
        InitBarriers();
        _alienOffsetX = 0;
        _alienOffsetY = 0;
        int nd = _alienMoveDelay - 2;
        if (nd < 5) nd = 5;
        _alienMoveDelay = nd;
        int ns = _alienMoveStep + 1;
        if (ns > 8) ns = 8;
        _alienMoveStep = ns;
        _score += 1000;
    }

    private void UpdateAlienBullets()
    {
        for (int i = 0; i < 6; i++)
        {
            if (_alienBulletActive[i] == 0) continue;
            int x = _alienBulletX[i];
            int y = _alienBulletY[i];
            y += 5;
            if (y > H)
            {
                _alienBulletActive[i] = 0;
                continue;
            }
            _alienBulletY[i] = y;

            if (_invincibleFrames == 0 && x + 3 > _playerX && x < _playerX + PLAYER_W && y + 3 > PLAYER_Y && y < PLAYER_Y + PLAYER_H)
            {
                _alienBulletActive[i] = 0;
                _lives--;
                if (_lives <= 0) _gameState = 2;
                else
                {
                    _invincibleFrames = 60;
                    _playerX = (W - PLAYER_W) / 2;
                    for (int j = 0; j < 3; j++) _playerBulletActive[j] = 0;
                }
                continue;
            }

            for (int b = 0; b < BARRIER_COUNT; b++)
                for (int row = 0; row < BARRIER_SIZE; row++)
                    for (int col = 0; col < BARRIER_SIZE; col++)
                    {
                        int idx = b * BARRIER_SIZE * BARRIER_SIZE + row * BARRIER_SIZE + col;
                        if (_barriers[idx] == 1)
                        {
                            int bx = (W / (BARRIER_COUNT + 1)) * (b + 1) - 40 + col * 5;
                            int by = H - 120 + row * 5;
                            if (x + 3 > bx && x < bx + 5 && y + 3 > by && y < by + 5)
                            {
                                _barriers[idx] = 0;
                                _alienBulletActive[i] = 0;
                                break;
                            }
                        }
                    }
        }
    }

    private void UpdateGame()
    {
        if (_gameState != 1) return;

        int step = 8;
        if (Input.IsHeld(NpadButton.Left) && _playerX > 0) _playerX -= step;
        if (Input.IsHeld(NpadButton.Right) && _playerX < W - PLAYER_W) _playerX += step;

        if (Input.IsDown(NpadButton.A))
        {
            for (int i = 0; i < 3; i++)
                if (_playerBulletActive[i] == 0)
                {
                    _playerBulletActive[i] = 1;
                    _playerBulletX[i] = _playerX + PLAYER_W / 2 - 3;
                    _playerBulletY[i] = PLAYER_Y - 5;
                    break;
                }
        }

        _alienMoveCounter++;
        if (_alienMoveCounter >= _alienMoveDelay)
        {
            _alienMoveCounter = 0;
            MoveAliens();
        }

        _alienShootDelay--;
        if (_alienShootDelay <= 0)
        {
            AlienShoot();
            _alienShootDelay = NextRandom(20, 50);
        }

        UpdatePlayerBullets();
        UpdateAlienBullets();

        if (_invincibleFrames > 0) _invincibleFrames--;
    }

    private void DrawAliens()
    {
        for (int row = 0; row < ALIEN_ROWS; row++)
            for (int col = 0; col < ALIEN_COLS; col++)
                if (_aliens[row * ALIEN_COLS + col] == 1)
                {
                    int x = ALIEN_START_X + _alienOffsetX + col * ALIEN_W;
                    int y = ALIEN_START_Y + _alienOffsetY + row * ALIEN_H;
                    uint color;
                    if (row == 0) color = Color.Magenta;
                    else if (row < 3) color = Color.Cyan;
                    else color = Color.Green;
                    Graphics.FillRect(x, y, ALIEN_W, ALIEN_H, color);
                    Graphics.FillCircle(x + ALIEN_W / 3, y + ALIEN_H / 3, 4, Color.White);
                    Graphics.FillCircle(x + 2 * ALIEN_W / 3, y + ALIEN_H / 3, 4, Color.White);
                    Graphics.FillCircle(x + ALIEN_W / 3, y + ALIEN_H / 3, 2, Color.Black);
                    Graphics.FillCircle(x + 2 * ALIEN_W / 3, y + ALIEN_H / 3, 2, Color.Black);
                    Graphics.FillRect(x + 4, y + ALIEN_H - 4, 4, 4, color);
                    Graphics.FillRect(x + ALIEN_W - 8, y + ALIEN_H - 4, 4, 4, color);
                }
    }

    private void DrawPlayer()
    {
        if (_invincibleFrames > 0 && (_invincibleFrames / 5) % 2 == 0) return;
        Graphics.FillRect(_playerX, PLAYER_Y, PLAYER_W, PLAYER_H, Color.White);
        Graphics.FillRect(_playerX + PLAYER_W / 2 - 5, PLAYER_Y - 8, 10, 8, Color.White);
    }

    private void DrawBullets()
    {
        for (int i = 0; i < 3; i++)
            if (_playerBulletActive[i] == 1)
                Graphics.FillRect(_playerBulletX[i], _playerBulletY[i], 4, 8, Color.Red);
        for (int i = 0; i < 6; i++)
            if (_alienBulletActive[i] == 1)
                Graphics.FillRect(_alienBulletX[i], _alienBulletY[i], 4, 8, Color.Yellow);
    }

    private void DrawBarriers()
    {
        for (int b = 0; b < BARRIER_COUNT; b++)
        {
            int barrierX = (W / (BARRIER_COUNT + 1)) * (b + 1) - 40;
            for (int row = 0; row < BARRIER_SIZE; row++)
                for (int col = 0; col < BARRIER_SIZE; col++)
                {
                    int idx = b * BARRIER_SIZE * BARRIER_SIZE + row * BARRIER_SIZE + col;
                    if (_barriers[idx] == 1)
                        Graphics.FillRect(barrierX + col * 5, H - 120 + row * 5, 4, 4, Color.Green);
                }
        }
    }

    private void DrawUI()
    {
        Graphics.DrawText(20, 20, "Score:", Color.White, 1);
        Graphics.DrawText(80, 20, _score.ToString(), Color.Yellow, 2);
        Graphics.DrawText(20, 50, "Highscore:", Color.White, 1);
        Graphics.DrawText(110, 50, _highscore.ToString(), Color.Yellow, 2);
        Graphics.DrawText(W - 200, 20, "Lives:", Color.White, 1);
        Graphics.DrawText(W - 130, 20, _lives.ToString(), Color.Yellow, 2);
        Graphics.DrawText(W / 2 - 40, 20, "Level:", Color.White, 1);
        Graphics.DrawText(W / 2 + 10, 20, _level.ToString(), Color.Yellow, 2);

        if (_gameState == 0)
        {
            Graphics.DrawText(W / 2 - 180, H / 2 - 60, "SPACE INVADERS", Color.Red, 4);
            Graphics.DrawText(W / 2 - 180, H / 2, "Press A to Start", Color.White, 2);
            Graphics.DrawText(W / 2 - 200, H / 2 + 50, "L/R to move, A to shoot", Color.Gray, 1);
        }
        else if (_gameState == 2)
        {
            Graphics.FillRect(0, 0, W, H, Color.Black);
            Graphics.DrawText(W / 2 - 150, H / 2 - 60, "GAME OVER", Color.Red, 4);
            Graphics.DrawText(W / 2 - 120, H / 2, "Score:", Color.Yellow, 2);
            Graphics.DrawText(W / 2 - 50, H / 2, _score.ToString(), Color.Yellow, 2);
            Graphics.DrawText(W / 2 - 180, H / 2 + 60, "Press Plus to Restart", Color.White, 2);
        }
    }

    public override void OnFrame()
    {
        int a = Input.IsDown(NpadButton.A) ? 1 : 0;
        int plus = Input.IsDown(NpadButton.Plus) ? 1 : 0;
        int aEdge = (a != 0 && _lastA == 0) ? 1 : 0;
        int plusEdge = (plus != 0 && _lastPlus == 0) ? 1 : 0;
        _lastA = a; _lastPlus = plus;

        if (_gameState == 0 && aEdge != 0) { ResetGame(); _gameState = 1; }
        if (_gameState == 2 && plusEdge != 0) ResetGame();
        if (_gameState == 1) UpdateGame();

        Graphics.FillScreen(Color.Black);
        DrawAliens();
        DrawBarriers();
        DrawPlayer();
        DrawBullets();
        DrawUI();
    }
}