using System;

public class Keyboard
{
    private const int KeyW = 80;
    private const int KeyH = 58;
    private const int KeyGap = 5;
    private const int PanelY = 310;
    private const int PanelH = 410;
    private const int InputY = 335;
    private const int InputH = 36;
    private const int Row1Y = 395;
    private const int Row2Y = 395 + 63;
    private const int Row3Y = 395 + 126;
    private const int Row4Y = 395 + 189;
    private const int Row5Y = 395 + 252;
    private const int MaxBuf = 256;

    private uint ColBg = Color.RGB(7, 92, 90);
    private uint ColHeader = Color.RGB(9, 68, 64);
    private uint ColKey = Color.RGB(3, 63, 58);
    private uint ColHover = Color.RGB(0, 130, 114);
    private uint ColSpecial = Color.RGB(0, 90, 80);
    private uint ColText = Color.RGB(220, 235, 235);
    private uint ColCursor = Color.RGB(59, 200, 180);
    private uint ColInput = Color.RGB(5, 50, 48);
    private uint ColDanger = Color.RGB(120, 30, 30);

    // Zustand
    private bool _visible = false;
    private char[] _buf = new char[MaxBuf];
    private int _bufLen = 0;
    private int _col = 0;
    private int _row = 0;
    private bool _shifted = false;
    private int _tick = 0;
    private bool _confirmed = false;
    private bool _cancelled = false;

    // Touch-Entprellung
    private int _lastTouchRow = -1;
    private int _lastTouchCol = -1;

    public bool IsVisible => _visible;

    public bool WasConfirmed()
    {
        bool v = _confirmed;
        _confirmed = false;
        return v;
    }

    public bool WasCancelled()
    {
        bool v = _cancelled;
        _cancelled = false;
        return v;
    }

    public string GetBuffer()
    {
        return _buf;
    }

    public void Show(string initialText)
    {
        _visible = true;
        _bufLen = 0;
        _col = 0;
        _row = 0;
        _shifted = false;
        _confirmed = false;
        _cancelled = false;
        int len = initialText.Length;
        if (len > MaxBuf) len = MaxBuf;
        for (int i = 0; i < len; i++)
            _buf[i] = initialText[i];
        _bufLen = len;
        _buf[_bufLen] = '\0';
    }

    public void Hide()
    {
        _visible = false;
    }

    public void Update()
    {
        if (!_visible) return;
        _tick = (_tick + 1) % 60;

        // ----- 1. Touch-Bedienung (mit Entprellung) -----
        HandleTouch();

        // ----- 2. Physische Tasten (Navigation + Aktionen) -----
        u64 kDown = padGetButtonsDown(&g_cs2sx_pad);

        if ((kDown & NpadButton.Down) != 0 && _row < 4) { _row++; ClampCol(); }
        if ((kDown & NpadButton.Up) != 0 && _row > 0) { _row--; ClampCol(); }
        if ((kDown & NpadButton.Right) != 0)
        {
            int max = MaxCol(_row);
            _col = (_col + 1) % (max + 1);
        }
        if ((kDown & NpadButton.Left) != 0)
        {
            int max = MaxCol(_row);
            _col = (_col - 1 + max + 1) % (max + 1);
        }

        if ((kDown & NpadButton.A) != 0) ExecuteKey(_row, _col);
        if ((kDown & NpadButton.B) != 0) Backspace();
        if ((kDown & NpadButton.X) != 0) _shifted = !_shifted;
        if ((kDown & NpadButton.Y) != 0) Confirm();
        if ((kDown & NpadButton.Plus) != 0) Confirm();
        if ((kDown & NpadButton.Minus) != 0) Cancel();
    }

    public void Draw()
    {
        if (!_visible) return;

        Graphics.FillRect(0, PanelY, 1280, PanelH, ColBg);
        Graphics.FillRect(0, PanelY, 1280, 70, ColHeader);

        DrawInputBar();
        DrawHints();
        DrawRow0(Row1Y);
        DrawRow1(Row2Y);
        DrawRow2(Row3Y);
        DrawRow3(Row4Y);
        DrawRow4(Row5Y);
    }

    // ========== Touch-Erkennung mit Entprellung ==========

    private void HandleTouch()
    {
        TouchState touch = Input.GetTouch();
        if (touch.count == 0)
        {
            _lastTouchRow = -1;
            _lastTouchCol = -1;
            return;
        }

        int tx = touch.x[0];
        int ty = touch.y[0];
        int foundRow = -1, foundCol = -1;

        // Reihe 0
        int totalW = 12 * KeyW + 11 * KeyGap;
        int sx = (1280 - totalW) / 2;
        int y = Row1Y;
        if (ty >= y && ty < y + KeyH)
        {
            for (int c = 0; c < 12; c++)
            {
                int x = sx + c * (KeyW + KeyGap);
                if (tx >= x && tx < x + KeyW)
                {
                    foundRow = 0; foundCol = c;
                    break;
                }
            }
        }
        // Reihe 1
        else
        {
            y = Row2Y;
            if (ty >= y && ty < y + KeyH)
            {
                for (int c = 0; c < 12; c++)
                {
                    int x = sx + c * (KeyW + KeyGap);
                    if (tx >= x && tx < x + KeyW)
                    {
                        foundRow = 1; foundCol = c;
                        break;
                    }
                }
            }
            // Reihe 2
            else
            {
                y = Row3Y;
                if (ty >= y && ty < y + KeyH)
                {
                    for (int c = 0; c < 12; c++)
                    {
                        int x = sx + c * (KeyW + KeyGap);
                        if (tx >= x && tx < x + KeyW)
                        {
                            foundRow = 2; foundCol = c;
                            break;
                        }
                    }
                }
                // Reihe 3
                else
                {
                    int shiftW = KeyW * 2 + KeyGap;
                    int delW = KeyW + 20;
                    int letW = 10 * KeyW + 9 * KeyGap;
                    totalW = shiftW + KeyGap + letW + KeyGap + delW;
                    sx = (1280 - totalW) / 2;
                    y = Row4Y;
                    if (ty >= y && ty < y + KeyH)
                    {
                        int xShift = sx;
                        if (tx >= xShift && tx < xShift + shiftW)
                        {
                            foundRow = 3; foundCol = 0;
                        }
                        else
                        {
                            int lx = sx + shiftW + KeyGap;
                            for (int c = 0; c < 10; c++)
                            {
                                int x = lx + c * (KeyW + KeyGap);
                                if (tx >= x && tx < x + KeyW)
                                {
                                    foundRow = 3; foundCol = c + 1;
                                    break;
                                }
                            }
                            if (foundRow == -1)
                            {
                                int dx = lx + 10 * (KeyW + KeyGap);
                                if (tx >= dx && tx < dx + delW)
                                    foundRow = 3; foundCol = 11;
                            }
                        }
                    }
                    // Reihe 4
                    else
                    {
                        int capsW = 100;
                        int spaceW = 700;
                        int enterW = 140;
                        int closeW = 120;
                        totalW = capsW + KeyGap + spaceW + KeyGap + enterW + KeyGap + closeW;
                        sx = (1280 - totalW) / 2;
                        y = Row5Y;
                        if (ty >= y && ty < y + KeyH)
                        {
                            int x = sx;
                            if (tx >= x && tx < x + capsW) { foundRow = 4; foundCol = 0; }
                            else
                            {
                                x += capsW + KeyGap;
                                if (tx >= x && tx < x + spaceW) { foundRow = 4; foundCol = 1; }
                                else
                                {
                                    x += spaceW + KeyGap;
                                    if (tx >= x && tx < x + enterW) { foundRow = 4; foundCol = 2; }
                                    else
                                    {
                                        x += enterW + KeyGap;
                                        if (tx >= x && tx < x + closeW) { foundRow = 4; foundCol = 3; }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        if (foundRow != -1 && (foundRow != _lastTouchRow || foundCol != _lastTouchCol))
        {
            ExecuteKey(foundRow, foundCol);
            _lastTouchRow = foundRow;
            _lastTouchCol = foundCol;
        }
    }

    // ========== Zentrale Ausführung einer Taste ==========

    private void ExecuteKey(int row, int col)
    {
        if (row == 0)
        {
            char[] n = new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '-', '=' };
            char[] s = new char[] { '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '-', '+' };
            if (col < 12) TypeChar(_shifted ? s[col] : n[col]);
        }
        else if (row == 1)
        {
            char[] n = new char[] { 'q', 'w', 'e', 'r', 't', 'y', 'u', 'i', 'o', 'p', '[', ']' };
            char[] s = new char[] { 'Q', 'W', 'E', 'R', 'T', 'Y', 'U', 'I', 'O', 'P', '{', '}' };
            if (col < 12) TypeChar(_shifted ? s[col] : n[col]);
        }
        else if (row == 2)
        {
            char[] n = new char[] { 'a', 's', 'd', 'f', 'g', 'h', 'j', 'k', 'l', ';', '\'', '\\' };
            char[] s = new char[] { 'A', 'S', 'D', 'F', 'G', 'H', 'J', 'K', 'L', ':', '"', '|' };
            if (col < 12) TypeChar(_shifted ? s[col] : n[col]);
        }
        else if (row == 3)
        {
            if (col == 0) _shifted = !_shifted;
            else if (col <= 10)
            {
                char[] n = new char[] { 'z', 'x', 'c', 'v', 'b', 'n', 'm', ',', '.', '/' };
                char[] s = new char[] { 'Z', 'X', 'C', 'V', 'B', 'N', 'M', '<', '>', '?' };
                int idx = col - 1;
                TypeChar(_shifted ? s[idx] : n[idx]);
            }
            else Backspace();
        }
        else if (row == 4)
        {
            if (col == 0) _shifted = !_shifted;
            else if (col == 1) TypeChar(' ');
            else if (col == 2) Confirm();
            else if (col == 3) Cancel();
        }
    }

    // ========== Zeichenmethoden (mittiger Text) ==========

    private void DrawInputBar()
    {
        Graphics.FillRect(16, InputY, 1248, InputH, ColInput);
        int cx = 24;
        int scale = 2;
        int charW = 8 * scale + 1;
        for (int i = 0; i < _bufLen; i++)
        {
            Graphics.DrawChar(cx, InputY + 10, _buf[i], ColCursor, scale);
            cx += charW;
        }
        if (_tick < 30)
            Graphics.DrawChar(cx, InputY + 10, '|', ColCursor, scale);
    }

    private void DrawHints()
    {
        int hy = PanelY + 10;
        Graphics.DrawText(20, hy, "Touch = Type", ColText, 1);
        Graphics.DrawText(130, hy, "B=Del", ColText, 1);
        Graphics.DrawText(200, hy, "X=Shift", ColText, 1);
        Graphics.DrawText(280, hy, "Y/+=OK", ColCursor, 1);
        Graphics.DrawText(370, hy, "-=Esc", ColText, 1);
        if (_shifted)
            Graphics.DrawText(1150, hy, "[SHIFT]", ColCursor, 1);
    }

    private void DrawKey(int x, int y, int w, string lbl, bool selected, uint normalBg)
    {
        uint bg = selected ? ColHover : normalBg;
        Graphics.FillRect(x, y, w, KeyH, bg);

        int scale = 2;
        int textWidth = Graphics.MeasureTextWidth(lbl, scale);
        int tx = x + (w - textWidth) / 2;
        int textHeight = 8 * scale;
        int ty = y + (KeyH - textHeight) / 2;

        Graphics.DrawText(tx, ty, lbl, ColText, scale);
    }

    private void DrawRow0(int y)
    {
        int totalW = 12 * KeyW + 11 * KeyGap;
        int sx = (1280 - totalW) / 2;
        string[] n = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "-", "=" };
        string[] s = new string[] { "!", "@", "#", "$", "%", "^", "&", "*", "(", ")", "_", "+" };
        for (int c = 0; c < 12; c++)
        {
            string lbl = _shifted ? s[c] : n[c];
            DrawKey(sx + c * (KeyW + KeyGap), y, KeyW, lbl, _row == 0 && _col == c, ColKey);
        }
    }

    private void DrawRow1(int y)
    {
        int totalW = 12 * KeyW + 11 * KeyGap;
        int sx = (1280 - totalW) / 2;
        string[] n = new string[] { "q", "w", "e", "r", "t", "y", "u", "i", "o", "p", "[", "]" };
        string[] s = new string[] { "Q", "W", "E", "R", "T", "Y", "U", "I", "O", "P", "{", "}" };
        for (int c = 0; c < 12; c++)
        {
            string lbl = _shifted ? s[c] : n[c];
            DrawKey(sx + c * (KeyW + KeyGap), y, KeyW, lbl, _row == 1 && _col == c, ColKey);
        }
    }

    private void DrawRow2(int y)
    {
        int totalW = 12 * KeyW + 11 * KeyGap;
        int sx = (1280 - totalW) / 2;
        string[] n = new string[] { "a", "s", "d", "f", "g", "h", "j", "k", "l", ";", "'", "\\" };
        string[] s = new string[] { "A", "S", "D", "F", "G", "H", "J", "K", "L", ":", "\"", "|" };
        for (int c = 0; c < 12; c++)
        {
            string lbl = _shifted ? s[c] : n[c];
            DrawKey(sx + c * (KeyW + KeyGap), y, KeyW, lbl, _row == 2 && _col == c, ColKey);
        }
    }

    private void DrawRow3(int y)
    {
        int shiftW = KeyW * 2 + KeyGap;
        int delW = KeyW + 20;
        int letW = 10 * KeyW + 9 * KeyGap;
        int totalW = shiftW + KeyGap + letW + KeyGap + delW;
        int sx = (1280 - totalW) / 2;

        string shiftLbl = _shifted ? "SHIFT*" : "SHIFT";
        DrawKey(sx, y, shiftW, shiftLbl, _row == 3 && _col == 0, ColSpecial);

        int lx = sx + shiftW + KeyGap;
        string[] n = new string[] { "z", "x", "c", "v", "b", "n", "m", ",", ".", "/" };
        string[] s = new string[] { "Z", "X", "C", "V", "B", "N", "M", "<", ">", "?" };
        for (int c = 0; c < 10; c++)
        {
            string lbl = _shifted ? s[c] : n[c];
            DrawKey(lx + c * (KeyW + KeyGap), y, KeyW, lbl, _row == 3 && _col == c + 1, ColKey);
        }

        int dx = lx + 10 * (KeyW + KeyGap);
        DrawKey(dx, y, delW, "DEL", _row == 3 && _col == 11, ColSpecial);
    }

    private void DrawRow4(int y)
    {
        int capsW = 100;
        int spaceW = 700;
        int enterW = 140;
        int closeW = 120;
        int totalW = capsW + KeyGap + spaceW + KeyGap + enterW + KeyGap + closeW;
        int x = (1280 - totalW) / 2;

        DrawKey(x, y, capsW, "CAPS", _row == 4 && _col == 0, ColSpecial);
        x += capsW + KeyGap;
        DrawKey(x, y, spaceW, "SPACE", _row == 4 && _col == 1, ColKey);
        x += spaceW + KeyGap;
        DrawKey(x, y, enterW, "ENTER", _row == 4 && _col == 2, ColSpecial);
        x += enterW + KeyGap;
        DrawKey(x, y, closeW, "CLOSE", _row == 4 && _col == 3, ColDanger);
    }

    // ========== Aktionen ==========

    private void TypeChar(char ch)
    {
        if (_bufLen < MaxBuf)
        {
            _buf[_bufLen] = ch;
            _bufLen++;
            _buf[_bufLen] = '\0';
        }
        _shifted = false;
    }

    private void Backspace()
    {
        if (_bufLen > 0)
        {
            _bufLen--;
            _buf[_bufLen] = '\0';
        }
    }

    private void Confirm()
    {
        _confirmed = true;
        _visible = false;
    }

    private void Cancel()
    {
        _cancelled = true;
        _visible = false;
    }

    private int MaxCol(int row)
    {
        if (row == 3) return 11;
        if (row == 4) return 3;
        return 11;
    }

    private void ClampCol()
    {
        int max = MaxCol(_row);
        if (_col > max) _col = max;
    }
}