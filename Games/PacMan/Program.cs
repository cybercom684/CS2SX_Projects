public class PacmanApp : SwitchApp
{
    private const int W = 1280;
    private const int H = 720;
    private const int CELL = 24;
    private const int MAZE_W = 19;
    private const int MAZE_H = 21;
    private const int OFFSET_X = (W - MAZE_W * CELL) / 2;
    private const int OFFSET_Y = (H - MAZE_H * CELL) / 2;

    private const int WALL = 1;
    private const int DOT = 0;
    private const int POWER = 2;
    private const int EMPTY = 3;

    private int[] _maze = new int[MAZE_W * MAZE_H];
    private int[] _mazeBase = new int[MAZE_W * MAZE_H];

    private int _score, _highscore, _dotsLeft;
    private int _pacX, _pacY, _pacDir, _pacNextDir;
    private int _mouthOpen, _animCounter;
    private int _moveDelay = 12, _ghostDelay = 14;
    private int _moveCounter, _ghostCounter;

    private int[] _gx = new int[4];
    private int[] _gy = new int[4];
    private int[] _gdir = new int[4];
    private int[] _gmode = new int[4];   // 0=chase,1=scatter,2=frightened,3=eaten
    private int[] _gmodeTimer = new int[4];
    private uint[] _gcolor = new uint[4];

    private int _frightenedTimer;
    private int _gameState;   // 0=start,1=play,2=gameover,3=win
    private int _lives;
    private int _deathTimer;
    private int _pelletBlink;
    private uint _randState = 123456789;
    private int _lastA, _lastPlus;

    public override void OnInit()
    {
        Graphics.Init(W, H);
        BuildMaze();
        for (int i = 0; i < MAZE_W * MAZE_H; i++) _mazeBase[i] = _maze[i];
        _gcolor[0] = Color.Red;
        _gcolor[1] = 0xFF69B4; // Pink
        _gcolor[2] = Color.Cyan;
        _gcolor[3] = Color.Orange;
        ResetGame();
    }

    private void BuildMaze()
    {
        // 19x21 Labyrinth (wie im Original, aber als flaches Array)
        int[] raw = new int[399];
        // Zeile 0
        for (int i = 0; i < 19; i++) raw[0 * 19 + i] = WALL;
        // Zeile 1
        raw[1 * 19 + 0] = WALL; raw[1 * 19 + 1] = DOT; raw[1 * 19 + 2] = DOT; raw[1 * 19 + 3] = DOT; raw[1 * 19 + 4] = DOT; raw[1 * 19 + 5] = DOT; raw[1 * 19 + 6] = DOT; raw[1 * 19 + 7] = DOT; raw[1 * 19 + 8] = WALL; raw[1 * 19 + 9] = DOT; raw[1 * 19 + 10] = DOT; raw[1 * 19 + 11] = DOT; raw[1 * 19 + 12] = DOT; raw[1 * 19 + 13] = DOT; raw[1 * 19 + 14] = DOT; raw[1 * 19 + 15] = DOT; raw[1 * 19 + 16] = DOT; raw[1 * 19 + 17] = DOT; raw[1 * 19 + 18] = WALL;
        // Zeile 2
        raw[2 * 19 + 0] = WALL; raw[2 * 19 + 1] = DOT; raw[2 * 19 + 2] = WALL; raw[2 * 19 + 3] = WALL; raw[2 * 19 + 4] = WALL; raw[2 * 19 + 5] = DOT; raw[2 * 19 + 6] = WALL; raw[2 * 19 + 7] = DOT; raw[2 * 19 + 8] = WALL; raw[2 * 19 + 9] = DOT; raw[2 * 19 + 10] = WALL; raw[2 * 19 + 11] = DOT; raw[2 * 19 + 12] = WALL; raw[2 * 19 + 13] = WALL; raw[2 * 19 + 14] = WALL; raw[2 * 19 + 15] = DOT; raw[2 * 19 + 16] = WALL; raw[2 * 19 + 17] = DOT; raw[2 * 19 + 18] = WALL;
        // Zeile 3
        raw[3 * 19 + 0] = WALL; raw[3 * 19 + 1] = POWER; raw[3 * 19 + 2] = WALL; raw[3 * 19 + 3] = WALL; raw[3 * 19 + 4] = WALL; raw[3 * 19 + 5] = DOT; raw[3 * 19 + 6] = WALL; raw[3 * 19 + 7] = DOT; raw[3 * 19 + 8] = WALL; raw[3 * 19 + 9] = DOT; raw[3 * 19 + 10] = WALL; raw[3 * 19 + 11] = DOT; raw[3 * 19 + 12] = WALL; raw[3 * 19 + 13] = WALL; raw[3 * 19 + 14] = WALL; raw[3 * 19 + 15] = DOT; raw[3 * 19 + 16] = WALL; raw[3 * 19 + 17] = POWER; raw[3 * 19 + 18] = WALL;
        // Zeile 4
        raw[4 * 19 + 0] = WALL; raw[4 * 19 + 1] = DOT; raw[4 * 19 + 2] = WALL; raw[4 * 19 + 3] = WALL; raw[4 * 19 + 4] = WALL; raw[4 * 19 + 5] = DOT; raw[4 * 19 + 6] = WALL; raw[4 * 19 + 7] = DOT; raw[4 * 19 + 8] = WALL; raw[4 * 19 + 9] = DOT; raw[4 * 19 + 10] = WALL; raw[4 * 19 + 11] = DOT; raw[4 * 19 + 12] = WALL; raw[4 * 19 + 13] = WALL; raw[4 * 19 + 14] = WALL; raw[4 * 19 + 15] = DOT; raw[4 * 19 + 16] = WALL; raw[4 * 19 + 17] = DOT; raw[4 * 19 + 18] = WALL;
        // Zeile 5
        raw[5 * 19 + 0] = WALL; raw[5 * 19 + 1] = DOT; raw[5 * 19 + 2] = DOT; raw[5 * 19 + 3] = DOT; raw[5 * 19 + 4] = DOT; raw[5 * 19 + 5] = DOT; raw[5 * 19 + 6] = DOT; raw[5 * 19 + 7] = DOT; raw[5 * 19 + 8] = DOT; raw[5 * 19 + 9] = DOT; raw[5 * 19 + 10] = DOT; raw[5 * 19 + 11] = DOT; raw[5 * 19 + 12] = DOT; raw[5 * 19 + 13] = DOT; raw[5 * 19 + 14] = DOT; raw[5 * 19 + 15] = DOT; raw[5 * 19 + 16] = DOT; raw[5 * 19 + 17] = DOT; raw[5 * 19 + 18] = WALL;
        // Zeile 6
        raw[6 * 19 + 0] = WALL; raw[6 * 19 + 1] = WALL; raw[6 * 19 + 2] = WALL; raw[6 * 19 + 3] = WALL; raw[6 * 19 + 4] = WALL; raw[6 * 19 + 5] = DOT; raw[6 * 19 + 6] = WALL; raw[6 * 19 + 7] = WALL; raw[6 * 19 + 8] = WALL; raw[6 * 19 + 9] = WALL; raw[6 * 19 + 10] = WALL; raw[6 * 19 + 11] = DOT; raw[6 * 19 + 12] = WALL; raw[6 * 19 + 13] = WALL; raw[6 * 19 + 14] = WALL; raw[6 * 19 + 15] = WALL; raw[6 * 19 + 16] = WALL; raw[6 * 19 + 17] = WALL; raw[6 * 19 + 18] = WALL;
        // Zeile 7
        raw[7 * 19 + 0] = WALL; raw[7 * 19 + 1] = DOT; raw[7 * 19 + 2] = DOT; raw[7 * 19 + 3] = DOT; raw[7 * 19 + 4] = DOT; raw[7 * 19 + 5] = DOT; raw[7 * 19 + 6] = WALL; raw[7 * 19 + 7] = DOT; raw[7 * 19 + 8] = DOT; raw[7 * 19 + 9] = DOT; raw[7 * 19 + 10] = DOT; raw[7 * 19 + 11] = DOT; raw[7 * 19 + 12] = WALL; raw[7 * 19 + 13] = DOT; raw[7 * 19 + 14] = DOT; raw[7 * 19 + 15] = DOT; raw[7 * 19 + 16] = DOT; raw[7 * 19 + 17] = DOT; raw[7 * 19 + 18] = WALL;
        // Zeile 8
        raw[8 * 19 + 0] = WALL; raw[8 * 19 + 1] = DOT; raw[8 * 19 + 2] = WALL; raw[8 * 19 + 3] = WALL; raw[8 * 19 + 4] = WALL; raw[8 * 19 + 5] = DOT; raw[8 * 19 + 6] = WALL; raw[8 * 19 + 7] = WALL; raw[8 * 19 + 8] = WALL; raw[8 * 19 + 9] = DOT; raw[8 * 19 + 10] = WALL; raw[8 * 19 + 11] = WALL; raw[8 * 19 + 12] = WALL; raw[8 * 19 + 13] = DOT; raw[8 * 19 + 14] = WALL; raw[8 * 19 + 15] = WALL; raw[8 * 19 + 16] = WALL; raw[8 * 19 + 17] = DOT; raw[8 * 19 + 18] = WALL;
        // Zeile 9
        raw[9 * 19 + 0] = WALL; raw[9 * 19 + 1] = DOT; raw[9 * 19 + 2] = WALL; raw[9 * 19 + 3] = WALL; raw[9 * 19 + 4] = WALL; raw[9 * 19 + 5] = DOT; raw[9 * 19 + 6] = WALL; raw[9 * 19 + 7] = WALL; raw[9 * 19 + 8] = WALL; raw[9 * 19 + 9] = DOT; raw[9 * 19 + 10] = WALL; raw[9 * 19 + 11] = WALL; raw[9 * 19 + 12] = WALL; raw[9 * 19 + 13] = DOT; raw[9 * 19 + 14] = WALL; raw[9 * 19 + 15] = WALL; raw[9 * 19 + 16] = WALL; raw[9 * 19 + 17] = DOT; raw[9 * 19 + 18] = WALL;
        // Zeile 10
        raw[10 * 19 + 0] = WALL; raw[10 * 19 + 1] = DOT; raw[10 * 19 + 2] = DOT; raw[10 * 19 + 3] = DOT; raw[10 * 19 + 4] = DOT; raw[10 * 19 + 5] = DOT; raw[10 * 19 + 6] = DOT; raw[10 * 19 + 7] = DOT; raw[10 * 19 + 8] = DOT; raw[10 * 19 + 9] = DOT; raw[10 * 19 + 10] = DOT; raw[10 * 19 + 11] = DOT; raw[10 * 19 + 12] = DOT; raw[10 * 19 + 13] = DOT; raw[10 * 19 + 14] = DOT; raw[10 * 19 + 15] = DOT; raw[10 * 19 + 16] = DOT; raw[10 * 19 + 17] = DOT; raw[10 * 19 + 18] = WALL;
        // Zeile 11
        raw[11 * 19 + 0] = WALL; raw[11 * 19 + 1] = DOT; raw[11 * 19 + 2] = WALL; raw[11 * 19 + 3] = WALL; raw[11 * 19 + 4] = WALL; raw[11 * 19 + 5] = DOT; raw[11 * 19 + 6] = WALL; raw[11 * 19 + 7] = WALL; raw[11 * 19 + 8] = WALL; raw[11 * 19 + 9] = WALL; raw[11 * 19 + 10] = WALL; raw[11 * 19 + 11] = WALL; raw[11 * 19 + 12] = WALL; raw[11 * 19 + 13] = DOT; raw[11 * 19 + 14] = WALL; raw[11 * 19 + 15] = WALL; raw[11 * 19 + 16] = WALL; raw[11 * 19 + 17] = DOT; raw[11 * 19 + 18] = WALL;
        // Zeile 12
        raw[12 * 19 + 0] = WALL; raw[12 * 19 + 1] = DOT; raw[12 * 19 + 2] = WALL; raw[12 * 19 + 3] = WALL; raw[12 * 19 + 4] = WALL; raw[12 * 19 + 5] = DOT; raw[12 * 19 + 6] = WALL; raw[12 * 19 + 7] = WALL; raw[12 * 19 + 8] = WALL; raw[12 * 19 + 9] = WALL; raw[12 * 19 + 10] = WALL; raw[12 * 19 + 11] = WALL; raw[12 * 19 + 12] = WALL; raw[12 * 19 + 13] = DOT; raw[12 * 19 + 14] = WALL; raw[12 * 19 + 15] = WALL; raw[12 * 19 + 16] = WALL; raw[12 * 19 + 17] = DOT; raw[12 * 19 + 18] = WALL;
        // Zeile 13
        raw[13 * 19 + 0] = WALL; raw[13 * 19 + 1] = DOT; raw[13 * 19 + 2] = DOT; raw[13 * 19 + 3] = DOT; raw[13 * 19 + 4] = DOT; raw[13 * 19 + 5] = DOT; raw[13 * 19 + 6] = WALL; raw[13 * 19 + 7] = DOT; raw[13 * 19 + 8] = DOT; raw[13 * 19 + 9] = DOT; raw[13 * 19 + 10] = DOT; raw[13 * 19 + 11] = DOT; raw[13 * 19 + 12] = WALL; raw[13 * 19 + 13] = DOT; raw[13 * 19 + 14] = DOT; raw[13 * 19 + 15] = DOT; raw[13 * 19 + 16] = DOT; raw[13 * 19 + 17] = DOT; raw[13 * 19 + 18] = WALL;
        // Zeile 14
        raw[14 * 19 + 0] = WALL; raw[14 * 19 + 1] = WALL; raw[14 * 19 + 2] = WALL; raw[14 * 19 + 3] = WALL; raw[14 * 19 + 4] = WALL; raw[14 * 19 + 5] = DOT; raw[14 * 19 + 6] = WALL; raw[14 * 19 + 7] = WALL; raw[14 * 19 + 8] = WALL; raw[14 * 19 + 9] = WALL; raw[14 * 19 + 10] = WALL; raw[14 * 19 + 11] = DOT; raw[14 * 19 + 12] = WALL; raw[14 * 19 + 13] = WALL; raw[14 * 19 + 14] = WALL; raw[14 * 19 + 15] = WALL; raw[14 * 19 + 16] = WALL; raw[14 * 19 + 17] = WALL; raw[14 * 19 + 18] = WALL;
        // Zeile 15
        raw[15 * 19 + 0] = WALL; raw[15 * 19 + 1] = DOT; raw[15 * 19 + 2] = DOT; raw[15 * 19 + 3] = DOT; raw[15 * 19 + 4] = DOT; raw[15 * 19 + 5] = DOT; raw[15 * 19 + 6] = DOT; raw[15 * 19 + 7] = DOT; raw[15 * 19 + 8] = WALL; raw[15 * 19 + 9] = DOT; raw[15 * 19 + 10] = DOT; raw[15 * 19 + 11] = DOT; raw[15 * 19 + 12] = DOT; raw[15 * 19 + 13] = DOT; raw[15 * 19 + 14] = DOT; raw[15 * 19 + 15] = DOT; raw[15 * 19 + 16] = DOT; raw[15 * 19 + 17] = DOT; raw[15 * 19 + 18] = WALL;
        // Zeile 16
        raw[16 * 19 + 0] = WALL; raw[16 * 19 + 1] = DOT; raw[16 * 19 + 2] = WALL; raw[16 * 19 + 3] = WALL; raw[16 * 19 + 4] = WALL; raw[16 * 19 + 5] = DOT; raw[16 * 19 + 6] = WALL; raw[16 * 19 + 7] = DOT; raw[16 * 19 + 8] = WALL; raw[16 * 19 + 9] = DOT; raw[16 * 19 + 10] = WALL; raw[16 * 19 + 11] = DOT; raw[16 * 19 + 12] = WALL; raw[16 * 19 + 13] = WALL; raw[16 * 19 + 14] = WALL; raw[16 * 19 + 15] = DOT; raw[16 * 19 + 16] = WALL; raw[16 * 19 + 17] = DOT; raw[16 * 19 + 18] = WALL;
        // Zeile 17
        raw[17 * 19 + 0] = WALL; raw[17 * 19 + 1] = DOT; raw[17 * 19 + 2] = WALL; raw[17 * 19 + 3] = WALL; raw[17 * 19 + 4] = WALL; raw[17 * 19 + 5] = DOT; raw[17 * 19 + 6] = WALL; raw[17 * 19 + 7] = DOT; raw[17 * 19 + 8] = WALL; raw[17 * 19 + 9] = DOT; raw[17 * 19 + 10] = WALL; raw[17 * 19 + 11] = DOT; raw[17 * 19 + 12] = WALL; raw[17 * 19 + 13] = WALL; raw[17 * 19 + 14] = WALL; raw[17 * 19 + 15] = DOT; raw[17 * 19 + 16] = WALL; raw[17 * 19 + 17] = DOT; raw[17 * 19 + 18] = WALL;
        // Zeile 18
        raw[18 * 19 + 0] = WALL; raw[18 * 19 + 1] = DOT; raw[18 * 19 + 2] = DOT; raw[18 * 19 + 3] = DOT; raw[18 * 19 + 4] = DOT; raw[18 * 19 + 5] = DOT; raw[18 * 19 + 6] = DOT; raw[18 * 19 + 7] = DOT; raw[18 * 19 + 8] = DOT; raw[18 * 19 + 9] = DOT; raw[18 * 19 + 10] = DOT; raw[18 * 19 + 11] = DOT; raw[18 * 19 + 12] = DOT; raw[18 * 19 + 13] = DOT; raw[18 * 19 + 14] = DOT; raw[18 * 19 + 15] = DOT; raw[18 * 19 + 16] = DOT; raw[18 * 19 + 17] = DOT; raw[18 * 19 + 18] = WALL;
        // Zeile 19
        for (int i = 0; i < 19; i++) raw[19 * 19 + i] = WALL;
        // Zeile 20
        for (int i = 0; i < 19; i++) raw[20 * 19 + i] = WALL;

        for (int i = 0; i < MAZE_W * MAZE_H; i++) _maze[i] = raw[i];
    }

    private void ResetGame()
    {
        _score = 0; _lives = 3; _gameState = 0;
        _deathTimer = 0; _frightenedTimer = 0;
        _moveCounter = 0; _ghostCounter = 0;
        _pelletBlink = 0;
        for (int i = 0; i < MAZE_W * MAZE_H; i++) _maze[i] = _mazeBase[i];
        _dotsLeft = CountDots();
        _pacX = 14; _pacY = 15; _pacDir = 2; _pacNextDir = -1;
        _mouthOpen = 0; _animCounter = 0;
        // Geister starten im Haus (leicht versetzt)
        _gx[0] = 13; _gy[0] = 14; _gdir[0] = 2; _gmode[0] = 1; _gmodeTimer[0] = 0;
        _gx[1] = 13; _gy[1] = 14; _gdir[1] = 2; _gmode[1] = 1; _gmodeTimer[1] = 60;
        _gx[2] = 12; _gy[2] = 14; _gdir[2] = 2; _gmode[2] = 1; _gmodeTimer[2] = 120;
        _gx[3] = 15; _gy[3] = 14; _gdir[3] = 2; _gmode[3] = 1; _gmodeTimer[3] = 180;
    }

    private int CountDots()
    {
        int c = 0;
        for (int i = 0; i < MAZE_W * MAZE_H; i++) if (_maze[i] == DOT || _maze[i] == POWER) c++;
        return c;
    }

    private int Rand4()
    {
        _randState = _randState * 1103515245 + 12345;
        return (int)((_randState >> 16) & 3);
    }

    private int CanMove(int x, int y, int dir, int isGhost)
    {
        int nx = x, ny = y;
        switch (dir)
        {
            case 0: nx++; break;
            case 1: ny++; break;
            case 2: nx--; break;
            case 3: ny--; break;
        }
        if (ny == 14 && (nx < 0 || nx >= MAZE_W)) return 1;
        if (nx < 0 || nx >= MAZE_W || ny < 0 || ny >= MAZE_H) return 0;
        int cell = _maze[ny * MAZE_W + nx];
        if (cell == WALL) return 0;
        return 1;
    }

    private void MovePacman()
    {
        if (_pacNextDir != -1 && CanMove(_pacX, _pacY, _pacNextDir, 0))
        {
            _pacDir = _pacNextDir;
            _pacNextDir = -1;
        }
        if (CanMove(_pacX, _pacY, _pacDir, 0))
        {
            switch (_pacDir)
            {
                case 0: _pacX++; break;
                case 1: _pacY++; break;
                case 2: _pacX--; break;
                case 3: _pacY--; break;
            }
            if (_pacX < 0) _pacX = MAZE_W - 1;
            if (_pacX >= MAZE_W) _pacX = 0;
        }
        int idx = _pacY * MAZE_W + _pacX;
        if (_maze[idx] == DOT)
        {
            _maze[idx] = EMPTY;
            _score += 10;
            _dotsLeft--;
            if (_dotsLeft == 0) _gameState = 3;
        }
        else if (_maze[idx] == POWER)
        {
            _maze[idx] = EMPTY;
            _score += 50;
            _dotsLeft--;
            _frightenedTimer = 360;
            for (int g = 0; g < 4; g++) if (_gmode[g] != 3) _gmode[g] = 2;
            if (_dotsLeft == 0) _gameState = 3;
        }
    }

    private void MoveGhosts()
    {
        for (int g = 0; g < 4; g++)
        {
            if (_gmodeTimer[g] > 0) { _gmodeTimer[g]--; continue; }

            int tx = _pacX, ty = _pacY;
            // Scatter-Ziele (Ecken)
            if (_gmode[g] == 1)
            {
                if (g == 0) { tx = MAZE_W - 1; ty = 0; }
                else if (g == 1) { tx = MAZE_W - 1; ty = MAZE_H - 1; }
                else if (g == 2) { tx = 0; ty = MAZE_H - 1; }
                else { tx = 0; ty = 0; }
            }
            else if (_gmode[g] == 2) // frightened: zufällige Richtung
            {
                int r = Rand4();
                for (int t = 0; t < 4; t++)
                {
                    int d = (r + t) % 4;
                    if (d != (_gdir[g] + 2) % 4 && CanMove(_gx[g], _gy[g], d, 1))
                    {
                        _gdir[g] = d;
                        break;
                    }
                }
            }
            else if (_gmode[g] == 3) // gefressen: zurück zum Haus
            {
                tx = 14; ty = 11;
            }
            else // chase
            {
                if (g == 1) { tx = _pacX + (_pacDir == 0 ? 4 : (_pacDir == 2 ? -4 : 0)); ty = _pacY + (_pacDir == 1 ? 4 : (_pacDir == 3 ? -4 : 0)); }
                else if (g == 2)
                {
                    int dx = _gx[0] - _pacX;
                    int dy = _gy[0] - _pacY;
                    tx = _pacX + dx * 2 + (_pacDir == 0 ? 2 : (_pacDir == 2 ? -2 : 0));
                    ty = _pacY + dy * 2 + (_pacDir == 1 ? 2 : (_pacDir == 3 ? -2 : 0));
                }
                else if (g == 3)
                {
                    int dist = (_gx[g] - _pacX) * (_gx[g] - _pacX) + (_gy[g] - _pacY) * (_gy[g] - _pacY);
                    if (dist < 64) { tx = 0; ty = MAZE_H - 1; }
                }
            }

            if (_gmode[g] != 2)
            {
                int bestDir = _gdir[g];
                int bestDist = 999999;
                for (int d = 0; d < 4; d++)
                {
                    if (d == (_gdir[g] + 2) % 4 && _gmode[g] != 3) continue;
                    if (!CanMove(_gx[g], _gy[g], d, 1)) continue;
                    int nx = _gx[g], ny = _gy[g];
                    switch (d) { case 0: nx++; break; case 1: ny++; break; case 2: nx--; break; case 3: ny--; break; }
                    int dist = (nx - tx) * (nx - tx) + (ny - ty) * (ny - ty);
                    if (dist < bestDist) { bestDist = dist; bestDir = d; }
                }
                _gdir[g] = bestDir;
            }

            switch (_gdir[g])
            {
                case 0: _gx[g]++; break;
                case 1: _gy[g]++; break;
                case 2: _gx[g]--; break;
                case 3: _gy[g]--; break;
            }
            if (_gx[g] < 0) _gx[g] = MAZE_W - 1;
            if (_gx[g] >= MAZE_W) _gx[g] = 0;

            if (_gmode[g] == 3 && _gx[g] == 14 && _gy[g] == 11)
            {
                _gmode[g] = 0;
                _gmodeTimer[g] = 60;
            }
        }
    }

    private void CheckCollisions()
    {
        for (int g = 0; g < 4; g++)
        {
            if (_gx[g] != _pacX || _gy[g] != _pacY) continue;
            if (_gmode[g] == 2)
            {
                _gmode[g] = 3;
                _score += 200;
                if (_score > _highscore) _highscore = _score;
            }
            else if (_gmode[g] != 3)
            {
                if (_score > _highscore) _highscore = _score;
                _lives--;
                if (_lives <= 0) _gameState = 2;
                else
                {
                    _deathTimer = 90;
                    _pacX = 14; _pacY = 15; _pacDir = 2; _pacNextDir = -1;
                    _gx[0] = 13; _gy[0] = 14; _gx[1] = 13; _gy[1] = 14;
                    _gx[2] = 12; _gy[2] = 14; _gx[3] = 15; _gy[3] = 14;
                    _frightenedTimer = 0;
                    for (int i = 0; i < 4; i++) _gmode[i] = 1;
                }
                return;
            }
        }
    }

    private void UpdateTimers()
    {
        if (_frightenedTimer > 0)
        {
            _frightenedTimer--;
            if (_frightenedTimer == 0)
                for (int g = 0; g < 4; g++) if (_gmode[g] == 2) _gmode[g] = 1;
        }
        _pelletBlink = (_pelletBlink + 1) % 60;
        _animCounter++;
        if (_animCounter >= 6) { _animCounter = 0; _mouthOpen = 1 - _mouthOpen; }
    }

    // Zeichnen
    private void DrawMaze()
    {
        for (int row = 0; row < MAZE_H; row++)
            for (int col = 0; col < MAZE_W; col++)
            {
                int cell = _maze[row * MAZE_W + col];
                int x = OFFSET_X + col * CELL;
                int y = OFFSET_Y + row * CELL;
                if (cell == WALL)
                    Graphics.FillRect(x + 1, y + 1, CELL - 2, CELL - 2, Color.Blue);
                else if (cell == DOT)
                    Graphics.FillCircle(x + CELL / 2, y + CELL / 2, 3, Color.White);
                else if (cell == POWER && (_pelletBlink < 30))
                    Graphics.FillCircle(x + CELL / 2, y + CELL / 2, 8, Color.White);
            }
    }

    private void DrawPacman()
    {
        int cx = OFFSET_X + _pacX * CELL + CELL / 2;
        int cy = OFFSET_Y + _pacY * CELL + CELL / 2;
        int r = CELL / 2 - 2;
        Graphics.FillCircle(cx, cy, r, Color.Yellow);
        if (_mouthOpen == 1)
        {
            int mw = r, mh = r / 2, mx = cx, my = cy;
            switch (_pacDir)
            {
                case 0: mx = cx + r / 2; my = cy - mh / 2; break;
                case 1: mx = cx - mw / 2; my = cy + r / 2; break;
                case 2: mx = cx - r - mw / 2; my = cy - mh / 2; break;
                case 3: mx = cx - mw / 2; my = cy - r - mh / 2; break;
            }
            Graphics.FillRect(mx, my, mw, mh, Color.Black);
        }
    }

    private void DrawGhost(int g)
    {
        if (_gmodeTimer[g] > 0) return;
        int x = OFFSET_X + _gx[g] * CELL;
        int y = OFFSET_Y + _gy[g] * CELL;
        int r = CELL / 2 - 2;
        int cx = x + CELL / 2, cy = y + r + 1;
        uint col = _gcolor[g];
        if (_gmode[g] == 2)
            col = (_frightenedTimer < 120 && (_pelletBlink / 10 % 2 == 0)) ? Color.White : Color.Blue;
        else if (_gmode[g] == 3) col = Color.White;
        Graphics.FillCircle(cx, cy, r, col);
        Graphics.FillRect(x, cy, CELL, r, col);
        int bw = CELL / 3;
        for (int b = 0; b < 3; b++)
            Graphics.FillCircle(x + bw / 2 + b * bw, cy + r, bw / 2, col);
        if (_gmode[g] != 3)
        {
            Graphics.FillCircle(cx - r / 2, cy - r / 3, 4, Color.White);
            Graphics.FillCircle(cx + r / 2, cy - r / 3, 4, Color.White);
            int edx = 0, edy = 0;
            switch (_gdir[g])
            {
                case 0: edx = 2; break;
                case 1: edy = 2; break;
                case 2: edx = -2; break;
                case 3: edy = -2; break;
            }
            Graphics.FillCircle(cx - r / 2 + edx, cy - r / 3 + edy, 2, Color.Black);
            Graphics.FillCircle(cx + r / 2 + edx, cy - r / 3 + edy, 2, Color.Black);
        }
    }

    private void DrawUI()
    {
        Graphics.DrawText(20, 20, "Score:", Color.White, 1);
        Graphics.DrawText(90, 20, _score.ToString(), Color.Yellow, 2);
        Graphics.DrawText(W - 200, 20, "Highscore:", Color.White, 1);
        Graphics.DrawText(W - 100, 20, _highscore.ToString(), Color.Yellow, 2);
        Graphics.DrawText(20, 50, "Lives:", Color.White, 1);
        for (int i = 0; i < _lives; i++) Graphics.FillCircle(90 + i * 25, 55, 8, Color.Yellow);
        if (_gameState == 0)
        {
            Graphics.DrawText(W / 2 - 180, H / 2 - 60, "PACMAN", Color.Red, 5);
            Graphics.DrawText(W / 2 - 180, H / 2, "Press A to Start", Color.White, 2);
        }
        else if (_gameState == 2)
        {
            Graphics.FillRect(0, 0, W, H, Color.Black);
            Graphics.DrawText(W / 2 - 150, H / 2 - 60, "GAME OVER", Color.Red, 4);
            Graphics.DrawText(W / 2 - 120, H / 2, "Score: ", Color.Yellow, 2);
            Graphics.DrawText(W / 2 - 40, H / 2, _score.ToString(), Color.Yellow, 2);
            Graphics.DrawText(W / 2 - 180, H / 2 + 60, "Press Plus to Restart", Color.White, 2);
        }
        else if (_gameState == 3)
        {
            Graphics.DrawText(W / 2 - 150, H / 2 - 60, "YOU WIN!", Color.Green, 4);
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

        if (_gameState == 0) { if (aEdge != 0 || plusEdge != 0) { ResetGame(); _gameState = 1; } }
        else if (_gameState == 2) { if (plusEdge != 0) ResetGame(); }
        else if (_gameState == 3) { if (plusEdge != 0) ResetGame(); }
        else if (_gameState == 1)
        {
            if (Input.IsDown(NpadButton.Left)) _pacNextDir = 2;
            if (Input.IsDown(NpadButton.Right)) _pacNextDir = 0;
            if (Input.IsDown(NpadButton.Up)) _pacNextDir = 3;
            if (Input.IsDown(NpadButton.Down)) _pacNextDir = 1;

            if (_deathTimer > 0) _deathTimer--;
            else
            {
                _moveCounter++;
                if (_moveCounter >= _moveDelay) { _moveCounter = 0; MovePacman(); CheckCollisions(); }
                _ghostCounter++;
                if (_ghostCounter >= _ghostDelay) { _ghostCounter = 0; MoveGhosts(); CheckCollisions(); }
            }
            UpdateTimers();
        }

        Graphics.FillScreen(Color.Black);
        DrawMaze();
        for (int i = 0; i < 4; i++) DrawGhost(i);
        if (_deathTimer == 0 || (_deathTimer / 10) % 2 == 0) DrawPacman();
        DrawUI();
    }
}