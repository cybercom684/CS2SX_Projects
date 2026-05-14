public class TetrisApp : SwitchApp
{
    private const int W = 1280;
    private const int H = 720;
    private const int FieldW = 10;
    private const int FieldH = 20;
    private const int BlockSize = 30;
    private const int StartX = FieldW / 2 - 2;
    private const int StartY = 0;

    // Spielfeld als 1D-Array
    private int[] _field = new int[FieldW * FieldH];

    // Tetrominos als Bitmasken (16 Bit = 4x4)
    // 7 Stücke * 4 Rotationen
    private int[] _pieces = new int[28]; // [piece*4 + rot] = mask
    private uint[] _pieceColors;

    private int _currentPiece, _currentRot, _currentX, _currentY;
    private int _nextPiece;
    private int _score, _lines;
    private int _gameOver, _pause;
    private int _fallDelay = 30, _fallCounter;
    private uint _randState;
    private int _lastPause;

    public override void OnInit()
    {
        Graphics.Init(W, H);
        InitPieces();
        InitColors();
        _randState = 123456789;
        ResetGame();
    }

    private void InitPieces()
    {
        // Bitmasken: 4x4, Zeile0 = Bits 0-3, Zeile1 = Bits 4-7, etc.
        // I
        _pieces[0*4+0] = 0x0F00; // 0b0000 1111 0000 0000 (Zeile1)
        _pieces[0*4+1] = 0x2222; // 0b0010 0010 0010 0010
        _pieces[0*4+2] = 0x00F0;
        _pieces[0*4+3] = 0x4444;
        // O
        _pieces[1*4+0] = 0x0660;
        _pieces[1*4+1] = 0x0660;
        _pieces[1*4+2] = 0x0660;
        _pieces[1*4+3] = 0x0660;
        // T
        _pieces[2*4+0] = 0x04E0;
        _pieces[2*4+1] = 0x08C4;
        _pieces[2*4+2] = 0x0E40;
        _pieces[2*4+3] = 0x4C40;
        // L
        _pieces[3*4+0] = 0x08E0;
        _pieces[3*4+1] = 0x0644;
        _pieces[3*4+2] = 0x0E20;
        _pieces[3*4+3] = 0x44C0;
        // J
        _pieces[4*4+0] = 0x02E0;
        _pieces[4*4+1] = 0x044C;
        _pieces[4*4+2] = 0x0E80;
        _pieces[4*4+3] = 0xC440;
        // S
        _pieces[5*4+0] = 0x06C0;
        _pieces[5*4+1] = 0x08C8;
        _pieces[5*4+2] = 0x0C60;
        _pieces[5*4+3] = 0x4C40;
        // Z
        _pieces[6*4+0] = 0x0C60;
        _pieces[6*4+1] = 0x04C8;
        _pieces[6*4+2] = 0x06C0;
        _pieces[6*4+3] = 0x8C40;
    }

    private void InitColors()
    {
        _pieceColors = new uint[7];
        _pieceColors[0] = Color.Cyan;
        _pieceColors[1] = Color.Yellow;
        _pieceColors[2] = Color.Magenta;
        _pieceColors[3] = Color.Orange;
        _pieceColors[4] = Color.Blue;
        _pieceColors[5] = Color.Green;
        _pieceColors[6] = Color.Red;
    }

    private int NextRandom(int min, int max)
    {
        _randState = _randState * 1103515245 + 12345;
        return (int)(min + (_randState % (uint)(max - min)));
    }

    private void ResetGame()
    {
        for (int i = 0; i < FieldW * FieldH; i++) _field[i] = 0;
        _score = 0; _lines = 0; _gameOver = 0; _pause = 0;
        _fallCounter = 0; _fallDelay = 30;
        _nextPiece = NextRandom(0, 7);
        SpawnNewPiece();
    }

    private void SpawnNewPiece()
    {
        _currentPiece = _nextPiece;
        _nextPiece = NextRandom(0, 7);
        _currentRot = 0;
        _currentX = StartX;
        _currentY = StartY;
        if (Collision() != 0) _gameOver = 1;
    }

    private int Collision()
    {
        int mask = _pieces[_currentPiece * 4 + _currentRot];
        for (int row = 0; row < 4; row++)
        {
            for (int col = 0; col < 4; col++)
            {
                if ((mask & (1 << (row * 4 + col))) != 0)
                {
                    int fx = _currentX + col;
                    int fy = _currentY + row;
                    if (fx < 0 || fx >= FieldW || fy >= FieldH || fy < 0) return 1;
                    if (fy >= 0 && _field[fy * FieldW + fx] != 0) return 1;
                }
            }
        }
        return 0;
    }

    private void MergePiece()
    {
        int mask = _pieces[_currentPiece * 4 + _currentRot];
        for (int row = 0; row < 4; row++)
        {
            for (int col = 0; col < 4; col++)
            {
                if ((mask & (1 << (row * 4 + col))) != 0)
                {
                    int fx = _currentX + col;
                    int fy = _currentY + row;
                    if (fy >= 0 && fy < FieldH && fx >= 0 && fx < FieldW)
                        _field[fy * FieldW + fx] = _currentPiece + 1;
                }
            }
        }
        ClearLines();
        SpawnNewPiece();
    }

    private void ClearLines()
    {
        int cleared = 0;
        for (int row = FieldH - 1; row >= 0; row--)
        {
            int full = 1;
            for (int col = 0; col < FieldW; col++)
                if (_field[row * FieldW + col] == 0) { full = 0; break; }
            if (full != 0)
            {
                for (int r = row; r > 0; r--)
                    for (int c = 0; c < FieldW; c++)
                        _field[r * FieldW + c] = _field[(r - 1) * FieldW + c];
                for (int c = 0; c < FieldW; c++) _field[0 * FieldW + c] = 0;
                cleared++;
                row++;
            }
        }
        if (cleared > 0)
        {
            if (cleared == 1) _score += 100;
            else if (cleared == 2) _score += 300;
            else if (cleared == 3) _score += 500;
            else if (cleared >= 4) _score += 800;
            _lines += cleared;
            int nd = 30 - (_lines / 10) * 2;
            if (nd < 5) nd = 5;
            _fallDelay = nd;
        }
    }

    private void TryMove(int dx, int dy)
    {
        if (_gameOver != 0 || _pause != 0) return;
        _currentX += dx;
        _currentY += dy;
        if (Collision() != 0)
        {
            _currentX -= dx;
            _currentY -= dy;
            if (dy == 1) MergePiece();
        }
    }

    private void TryRotate()
    {
        if (_gameOver != 0 || _pause != 0) return;
        int oldRot = _currentRot;
        _currentRot = (_currentRot + 1) % 4;
        if (Collision() != 0) _currentRot = oldRot;
    }

    private void HardDrop()
    {
        if (_gameOver != 0 || _pause != 0) return;
        while (Collision() == 0) _currentY++;
        _currentY--;
        MergePiece();
    }

    private void DrawField()
    {
        int offX = 100, offY = 60;
        Graphics.DrawRect(offX - 2, offY - 2, FieldW * BlockSize + 4, FieldH * BlockSize + 4, Color.White);
        for (int row = 0; row < FieldH; row++)
            for (int col = 0; col < FieldW; col++)
            {
                int v = _field[row * FieldW + col];
                if (v != 0)
                    Graphics.FillRect(offX + col * BlockSize, offY + row * BlockSize, BlockSize - 1, BlockSize - 1, _pieceColors[v - 1]);
            }
        // aktueller Stein
        int mask = _pieces[_currentPiece * 4 + _currentRot];
        for (int row = 0; row < 4; row++)
            for (int col = 0; col < 4; col++)
                if ((mask & (1 << (row * 4 + col))) != 0)
                {
                    int x = _currentX + col, y = _currentY + row;
                    if (y >= 0 && y < FieldH)
                        Graphics.FillRect(offX + x * BlockSize, offY + y * BlockSize, BlockSize - 1, BlockSize - 1, _pieceColors[_currentPiece]);
                }
    }

    private void DrawNextPiece()
    {
        int offX = 100 + FieldW * BlockSize + 40, offY = 60;
        Graphics.DrawText(offX, offY, "Next:", Color.White, 1);
        offY += 30;
        int mask = _pieces[_nextPiece * 4 + 0];
        for (int row = 0; row < 4; row++)
            for (int col = 0; col < 4; col++)
                if ((mask & (1 << (row * 4 + col))) != 0)
                    Graphics.FillRect(offX + col * BlockSize, offY + row * BlockSize, BlockSize - 1, BlockSize - 1, _pieceColors[_nextPiece]);
    }

    private void DrawUI()
    {
        int x = 100 + FieldW * BlockSize + 40, y = 60 + 150;
        Graphics.DrawText(x, y, "Score:", Color.White, 1); y += 25;
        Graphics.DrawText(x, y, _score.ToString(), Color.Yellow, 2); y += 50;
        Graphics.DrawText(x, y, "Lines:", Color.White, 1); y += 25;
        Graphics.DrawText(x, y, _lines.ToString(), Color.Yellow, 2); y += 80;
        Graphics.DrawText(x, y, "Controls:", Color.White, 1); y += 20;
        Graphics.DrawText(x, y, "A/B: Rotate", Color.Gray, 1); y += 20;
        Graphics.DrawText(x, y, "L/R: Move", Color.Gray, 1); y += 20;
        Graphics.DrawText(x, y, "D-Up/Down", Color.Gray, 1); y += 20;
        Graphics.DrawText(x, y, "Plus: Pause", Color.Gray, 1);
    }

    private void DrawPauseOverlay()
    {
        if (_pause != 0)
        {
            Graphics.FillRect(0, 0, W, H, Color.Black);
            Graphics.DrawText(W / 2 - 100, H / 2 - 30, "PAUSED", Color.White, 3);
            Graphics.DrawText(W / 2 - 140, H / 2 + 30, "Press Plus to Resume", Color.White, 1);
        }
    }

    private void DrawGameOver()
    {
        if (_gameOver != 0)
        {
            Graphics.FillRect(0, 0, W, H, Color.Black);
            Graphics.DrawText(W / 2 - 150, H / 2 - 60, "GAME OVER", Color.Red, 4);
            Graphics.DrawText(W / 2 - 120, H / 2, "Score: ", Color.Yellow, 2);
            Graphics.DrawText(W / 2 - 20, H / 2, _score.ToString(), Color.Yellow, 2);
            Graphics.DrawText(W / 2 - 180, H / 2 + 60, "Press A to Restart", Color.White, 2);
        }
    }

    public override void OnFrame()
    {
        int a = (Input.IsDown(NpadButton.A) || Input.IsDown(NpadButton.B)) ? 1 : 0;
        int left = Input.IsDown(NpadButton.Left) ? 1 : 0;
        int right = Input.IsDown(NpadButton.Right) ? 1 : 0;
        int down = Input.IsDown(NpadButton.Down) ? 1 : 0;
        int up = Input.IsDown(NpadButton.Up) ? 1 : 0;
        int plus = Input.IsDown(NpadButton.Plus) ? 1 : 0;

        int pausePressed = (plus != 0 && _lastPause == 0) ? 1 : 0;
        _lastPause = plus;
        if (pausePressed != 0 && _gameOver == 0) _pause = (_pause == 0) ? 1 : 0;

        if (_gameOver != 0)
        {
            if (a != 0) ResetGame();
            DrawField(); DrawNextPiece(); DrawUI(); DrawGameOver();
            return;
        }
        if (_pause != 0)
        {
            DrawField(); DrawNextPiece(); DrawUI(); DrawPauseOverlay();
            return;
        }

        if (left != 0) TryMove(-1, 0);
        if (right != 0) TryMove(1, 0);
        if (down != 0) TryMove(0, 1);
        if (a != 0) TryRotate();
        if (up != 0) HardDrop();

        _fallCounter++;
        if (_fallCounter >= _fallDelay) { _fallCounter = 0; TryMove(0, 1); }

        Graphics.FillScreen(Color.Black);
        DrawField(); DrawNextPiece(); DrawUI();
    }
}