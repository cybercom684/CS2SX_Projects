#include <stdlib.h>
#include "Keyboard.h"
#include "Program.h"

Keyboard* Keyboard_New(void)
{
    Keyboard* self = (Keyboard*)malloc(sizeof(Keyboard));
    if (!self) return NULL;
    memset(self, 0, sizeof(Keyboard));
    self->_rc = 1;
    self->f_KeyW = 88;
    self->f_KeyH = 60;
    self->f_KeyGap = 5;
    self->f_PanelY = 280;
    self->f_PanelH = 440;
    self->f_InputX = 16;
    self->f_InputY = 304;
    self->f_InputH = 52;
    self->f_InputW = 1248;
    self->f_Row1Y = 388;
    self->f_Row2Y = 388 + 66;
    self->f_Row3Y = 388 + 132;
    self->f_Row4Y = 388 + 198;
    self->f_Row5Y = 388 + 264;
    self->f_MaxBuf = 256;
    self->f_ColBg = CS2SX_RGB(8, 78, 74);
    self->f_ColHeader = CS2SX_RGB(5, 52, 50);
    self->f_ColInput = CS2SX_RGB(2, 30, 28);
    self->f_ColInputBorder = CS2SX_RGB(0, 110, 100);
    self->f_ColKey = CS2SX_RGB(12, 88, 82);
    self->f_ColHover = CS2SX_RGB(0, 155, 135);
    self->f_ColSpecial = CS2SX_RGB(0, 98, 88);
    self->f_ColText = CS2SX_RGB(230, 245, 242);
    self->f_ColCursor = CS2SX_RGB(64, 210, 188);
    self->f_ColDanger = CS2SX_RGB(160, 40, 40);
    self->visible = 0;
    self->f_buf = (char*)malloc(Keyboard_MaxBuf * sizeof(char));
    self->f_bufLen = 0;
    self->f_col = 0;
    self->f_row = 0;
    self->f_shifted = 0;
    self->f_tick = 0;
    self->f_confirmed = 0;
    self->f_cancelled = 0;
    self->f_touchOverRow = -1;
    self->f_touchOverCol = -1;
    self->f_lastTouchRow = -1;
    self->f_lastTouchCol = -1;
    self->f_RepeatDelay = 30;
    self->f_RepeatInterval = 4;
    self->f_delHeld = 0;
    self->f_delHoldFrames = 0;
    return self;
}

int Keyboard_WasConfirmed(Keyboard* self)
{
    int v = self->f_confirmed;
    self->f_confirmed = 0;
    return v;
}

int Keyboard_WasCancelled(Keyboard* self)
{
    int v = self->f_cancelled;
    self->f_cancelled = 0;
    return v;
}

const char* Keyboard_GetBuffer(Keyboard* self)
{
    return self->f_buf;
}

void Keyboard_Show(Keyboard* self, const char* initialText)
{
    self->visible = 1;
    self->f_bufLen = 0;
    self->f_col = 0;
    self->f_row = 0;
    self->f_shifted = 0;
    self->f_confirmed = 0;
    self->f_cancelled = 0;
    int len = strlen(initialText);
    if (len > Keyboard_MaxBuf)
    {
        len = Keyboard_MaxBuf;
    }
    for (int i = 0; i < len; i++)
    {
        self->f_buf[i] = initialText[i];
    }
    self->f_bufLen = len;
    self->f_buf[self->f_bufLen] = '\0';
}

void Keyboard_Hide(Keyboard* self)
{
    self->visible = 0;
}

void Keyboard_Update(Keyboard* self)
{
    if (!self->visible)
    {
        return;
    }
    self->f_tick = (self->f_tick + 1) % 60;
    Keyboard_HandleTouch(self);
    u64 kDown = padGetButtonsDown(&g_cs2sx_pad);
    if ((kDown & HidNpadButton_Down) != 0 && self->f_row < 4)
    {
        self->f_row++;
        Keyboard_ClampCol(self);
    }
    if ((kDown & HidNpadButton_Up) != 0 && self->f_row > 0)
    {
        self->f_row--;
        Keyboard_ClampCol(self);
    }
    if ((kDown & HidNpadButton_Right) != 0)
    {
        int max = Keyboard_MaxCol(self, self->f_row);
        self->f_col = (self->f_col + 1) % (max + 1);
    }
    if ((kDown & HidNpadButton_Left) != 0)
    {
        int max = Keyboard_MaxCol(self, self->f_row);
        self->f_col = (self->f_col - 1 + max + 1) % (max + 1);
    }
    if ((kDown & HidNpadButton_A) != 0)
    {
        Keyboard_ExecuteKey(self, self->f_row, self->f_col);
    }
    if ((kDown & HidNpadButton_X) != 0)
    {
        self->f_shifted = !self->f_shifted;
    }
    if ((kDown & HidNpadButton_Y) != 0)
    {
        Keyboard_Confirm(self);
    }
    if ((kDown & HidNpadButton_Plus) != 0)
    {
        Keyboard_Confirm(self);
    }
    if ((kDown & HidNpadButton_Minus) != 0)
    {
        Keyboard_Cancel(self);
    }
    Keyboard_HandleBackspaceRepeat(self);
}

void Keyboard_Draw(Keyboard* self)
{
    if (!self->visible)
    {
        return;
    }
    Graphics_FillRect(0, Keyboard_PanelY, 1280, Keyboard_PanelH, self->f_ColBg);
    Graphics_FillRect(0, Keyboard_PanelY, 1280, 100, self->f_ColHeader);
    Graphics_DrawLine(0, Keyboard_PanelY + 100, 1280, Keyboard_PanelY + 100, CS2SX_RGB(0, 100, 90));
    Keyboard_DrawInputBar(self);
    Keyboard_DrawHints(self);
    Keyboard_DrawRow0(self, Keyboard_Row1Y);
    Keyboard_DrawRow1(self, Keyboard_Row2Y);
    Keyboard_DrawRow2(self, Keyboard_Row3Y);
    Keyboard_DrawRow3(self, Keyboard_Row4Y);
    Keyboard_DrawRow4(self, Keyboard_Row5Y);
}

void Keyboard_HandleBackspaceRepeat(Keyboard* self)
{
    u64 kHeld = padGetButtons(&g_cs2sx_pad);
    int bHeld = (kHeld & HidNpadButton_B) != 0;
    int delTouch = (self->f_touchOverRow == 3 && self->f_touchOverCol == 11);
    int wantDel = bHeld || delTouch;
    if (wantDel)
    {
        if (!self->f_delHeld)
        {
            Keyboard_Backspace(self);
            self->f_delHeld = 1;
            self->f_delHoldFrames = 0;
        }
        else
        {
            self->f_delHoldFrames++;
            if (self->f_delHoldFrames >= Keyboard_RepeatDelay)
            {
                int framesInRepeat = self->f_delHoldFrames - Keyboard_RepeatDelay;
                if (framesInRepeat % Keyboard_RepeatInterval == 0)
                {
                    Keyboard_Backspace(self);
                }
            }
        }
    }
    else
    {
        self->f_delHeld = 0;
        self->f_delHoldFrames = 0;
    }
}

void Keyboard_HandleTouch(Keyboard* self)
{
    CS2SX_TouchState touch = CS2SX_Input_GetTouch();
    if (touch.count == 0)
    {
        self->f_touchOverRow = -1;
        self->f_touchOverCol = -1;
        self->f_lastTouchRow = -1;
        self->f_lastTouchCol = -1;
        return;
    }
    int tx = touch.x[0];
    int ty = touch.y[0];
    int foundRow = -1;
    int foundCol = -1;
    int totalW = 12 * Keyboard_KeyW + 11 * Keyboard_KeyGap;
    int sx = (1280 - totalW) / 2;
    int y = Keyboard_Row1Y;
    if (ty >= y && ty < y + Keyboard_KeyH)
    {
        for (int c = 0; c < 12; c++)
        {
            int x = sx + c * (Keyboard_KeyW + Keyboard_KeyGap);
            if (tx >= x && tx < x + Keyboard_KeyW)
            {
                foundRow = 0;
                foundCol = c;
                break;
            }
        }
    }
    else
    {
        y = Keyboard_Row2Y;
        if (ty >= y && ty < y + Keyboard_KeyH)
        {
            for (int c = 0; c < 12; c++)
            {
                int x = sx + c * (Keyboard_KeyW + Keyboard_KeyGap);
                if (tx >= x && tx < x + Keyboard_KeyW)
                {
                    foundRow = 1;
                    foundCol = c;
                    break;
                }
            }
        }
        else
        {
            y = Keyboard_Row3Y;
            if (ty >= y && ty < y + Keyboard_KeyH)
            {
                for (int c = 0; c < 12; c++)
                {
                    int x = sx + c * (Keyboard_KeyW + Keyboard_KeyGap);
                    if (tx >= x && tx < x + Keyboard_KeyW)
                    {
                        foundRow = 2;
                        foundCol = c;
                        break;
                    }
                }
            }
            else
            {
                int shiftW = Keyboard_KeyW * 2 + Keyboard_KeyGap;
                int delW = Keyboard_KeyW + 20;
                int letW = 10 * Keyboard_KeyW + 9 * Keyboard_KeyGap;
                totalW = shiftW + Keyboard_KeyGap + letW + Keyboard_KeyGap + delW;
                sx = (1280 - totalW) / 2;
                y = Keyboard_Row4Y;
                if (ty >= y && ty < y + Keyboard_KeyH)
                {
                    int xShift = sx;
                    if (tx >= xShift && tx < xShift + shiftW)
                    {
                        foundRow = 3;
                        foundCol = 0;
                    }
                    else
                    {
                        int lx = sx + shiftW + Keyboard_KeyGap;
                        for (int c = 0; c < 10; c++)
                        {
                            int x = lx + c * (Keyboard_KeyW + Keyboard_KeyGap);
                            if (tx >= x && tx < x + Keyboard_KeyW)
                            {
                                foundRow = 3;
                                foundCol = c + 1;
                                break;
                            }
                        }
                        if (foundRow == -1)
                        {
                            int dx = lx + 10 * (Keyboard_KeyW + Keyboard_KeyGap);
                            if (tx >= dx && tx < dx + delW)
                            {
                                foundRow = 3;
                            }
                            foundCol = 11;
                        }
                    }
                }
                else
                {
                    int capsW = 100;
                    int spaceW = 700;
                    int enterW = 140;
                    int closeW = 120;
                    totalW = capsW + Keyboard_KeyGap + spaceW + Keyboard_KeyGap + enterW + Keyboard_KeyGap + closeW;
                    sx = (1280 - totalW) / 2;
                    y = Keyboard_Row5Y;
                    if (ty >= y && ty < y + Keyboard_KeyH)
                    {
                        int x = sx;
                        if (tx >= x && tx < x + capsW)
                        {
                            foundRow = 4;
                            foundCol = 0;
                        }
                        else
                        {
                            x += capsW + Keyboard_KeyGap;
                            if (tx >= x && tx < x + spaceW)
                            {
                                foundRow = 4;
                                foundCol = 1;
                            }
                            else
                            {
                                x += spaceW + Keyboard_KeyGap;
                                if (tx >= x && tx < x + enterW)
                                {
                                    foundRow = 4;
                                    foundCol = 2;
                                }
                                else
                                {
                                    x += enterW + Keyboard_KeyGap;
                                    if (tx >= x && tx < x + closeW)
                                    {
                                        foundRow = 4;
                                        foundCol = 3;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    self->f_touchOverRow = foundRow;
    self->f_touchOverCol = foundCol;
    if (foundRow != -1 && (foundRow != self->f_lastTouchRow || foundCol != self->f_lastTouchCol))
    {
        int isDel = (foundRow == 3 && foundCol == 11);
        if (!isDel)
        {
            Keyboard_ExecuteKey(self, foundRow, foundCol);
        }
        self->f_lastTouchRow = foundRow;
        self->f_lastTouchCol = foundCol;
    }
}

void Keyboard_ExecuteKey(Keyboard* self, int row, int col)
{
    if (row == 0)
    {
        char n[] = { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '-', '=' };
        char s[] = { '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '-', '+' };
        if (col < 12)
        {
            Keyboard_TypeChar(self, (self->f_shifted ? s[col] : n[col]));
        }
    }
    else if (row == 1)
    {
        char n[] = { 'q', 'w', 'e', 'r', 't', 'y', 'u', 'i', 'o', 'p', '[', ']' };
        char s[] = { 'Q', 'W', 'E', 'R', 'T', 'Y', 'U', 'I', 'O', 'P', '{', '}' };
        if (col < 12)
        {
            Keyboard_TypeChar(self, (self->f_shifted ? s[col] : n[col]));
        }
    }
    else if (row == 2)
    {
        char n[] = { 'a', 's', 'd', 'f', 'g', 'h', 'j', 'k', 'l', ';', '\'', '\\' };
        char s[] = { 'A', 'S', 'D', 'F', 'G', 'H', 'J', 'K', 'L', ':', '"', '|' };
        if (col < 12)
        {
            Keyboard_TypeChar(self, (self->f_shifted ? s[col] : n[col]));
        }
    }
    else if (row == 3)
    {
        if (col == 0)
        {
            self->f_shifted = !self->f_shifted;
        }
        else if (col <= 10)
        {
            char n[] = { 'z', 'x', 'c', 'v', 'b', 'n', 'm', ',', '.', '/' };
            char s[] = { 'Z', 'X', 'C', 'V', 'B', 'N', 'M', '<', '>', '?' };
            int idx = col - 1;
            Keyboard_TypeChar(self, (self->f_shifted ? s[idx] : n[idx]));
        }
        else
        {
            Keyboard_Backspace(self);
        }
    }
    else if (row == 4)
    {
        if (col == 0)
        {
            self->f_shifted = !self->f_shifted;
        }
        else if (col == 1)
        {
            Keyboard_TypeChar(self, ' ');
        }
        else if (col == 2)
        {
            Keyboard_Confirm(self);
        }
        else if (col == 3)
        {
            Keyboard_Cancel(self);
        }
    }
}

void Keyboard_DrawInputBar(Keyboard* self)
{
    Graphics_FillRect(Keyboard_InputX - 2, Keyboard_InputY - 2, Keyboard_InputW + 4, Keyboard_InputH + 4, self->f_ColInputBorder);
    Graphics_FillRect(Keyboard_InputX, Keyboard_InputY, Keyboard_InputW, Keyboard_InputH, self->f_ColInput);
    Graphics_DrawLine(Keyboard_InputX + 2, Keyboard_InputY + 1, Keyboard_InputX + Keyboard_InputW - 3, Keyboard_InputY + 1, CS2SX_RGB(0, 55, 52));
    int cx = Keyboard_InputX + 12;
    int scale = 2;
    int charW = 8 * scale + 1;
    int ty = Keyboard_InputY + (Keyboard_InputH - 8 * scale) / 2;
    for (int i = 0; i < self->f_bufLen; i++)
    {
        Graphics_DrawChar(cx, ty, self->f_buf[i], self->f_ColCursor, scale);
        cx += charW;
    }
    if (self->f_tick < 30)
    {
        Graphics_DrawChar(cx, ty, '|', self->f_ColCursor, scale);
    }
}

void Keyboard_DrawHints(Keyboard* self)
{
    int hy = Keyboard_PanelY + 8;
    Graphics_DrawText(Keyboard_InputX, hy, "B=Del", self->f_ColText, 1);
    Graphics_DrawText(Keyboard_InputX + 70, hy, "X=Shift", self->f_ColText, 1);
    Graphics_DrawText(Keyboard_InputX + 155, hy, "Y=OK", self->f_ColCursor, 1);
    Graphics_DrawText(Keyboard_InputX + 210, hy, "-=Esc", self->f_ColText, 1);
    if (self->f_shifted)
    {
        Graphics_DrawText(1170, hy, "[SHIFT]", self->f_ColCursor, 1);
    }
}

void Keyboard_DrawKey(Keyboard* self, int x, int y, int w, const char* lbl, int selected, unsigned int normalBg, int row, int col)
{
    int touchHover = (self->f_touchOverRow == row && self->f_touchOverCol == col);
    unsigned int bg = ((selected || touchHover) ? self->f_ColHover : normalBg);
    Graphics_FillRect(x, y, w, Keyboard_KeyH, bg);
    unsigned int highlight = ((touchHover || selected) ? CS2SX_RGB(80, 220, 200) : CS2SX_RGB(0, 120, 108));
    Graphics_DrawLine(x + 1, y, x + w - 2, y, highlight);
    int scale = 2;
    int textWidth = Graphics_MeasureTextWidth(lbl, scale);
    int textH = 8 * scale;
    int tx = x + (w - textWidth) / 2;
    int ty = y + (Keyboard_KeyH - textH) / 2;
    Graphics_DrawText(tx, ty, lbl, self->f_ColText, scale);
}

void Keyboard_DrawRow0(Keyboard* self, int y)
{
    int totalW = 12 * Keyboard_KeyW + 11 * Keyboard_KeyGap;
    int sx = (1280 - totalW) / 2;
    const char* n[] = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "-", "=" };
    const char* s[] = { "!", "@", "#", "$", "%", "^", "&", "*", "(", ")", "-", "+" };
    for (int c = 0; c < 12; c++)
    {
        Keyboard_DrawKey(self, sx + c * (Keyboard_KeyW + Keyboard_KeyGap), y, Keyboard_KeyW, (self->f_shifted ? s[c] : n[c]), self->f_row == 0 && self->f_col == c, self->f_ColKey, 0, c);
    }
}

void Keyboard_DrawRow1(Keyboard* self, int y)
{
    int totalW = 12 * Keyboard_KeyW + 11 * Keyboard_KeyGap;
    int sx = (1280 - totalW) / 2;
    const char* n[] = { "q", "w", "e", "r", "t", "y", "u", "i", "o", "p", "[", "]" };
    const char* s[] = { "Q", "W", "E", "R", "T", "Y", "U", "I", "O", "P", "{", "}" };
    for (int c = 0; c < 12; c++)
    {
        Keyboard_DrawKey(self, sx + c * (Keyboard_KeyW + Keyboard_KeyGap), y, Keyboard_KeyW, (self->f_shifted ? s[c] : n[c]), self->f_row == 1 && self->f_col == c, self->f_ColKey, 1, c);
    }
}

void Keyboard_DrawRow2(Keyboard* self, int y)
{
    int totalW = 12 * Keyboard_KeyW + 11 * Keyboard_KeyGap;
    int sx = (1280 - totalW) / 2;
    const char* n[] = { "a", "s", "d", "f", "g", "h", "j", "k", "l", ";", "'", "\\" };
    const char* s[] = { "A", "S", "D", "F", "G", "H", "J", "K", "L", ":", "\"", "|" };
    for (int c = 0; c < 12; c++)
    {
        Keyboard_DrawKey(self, sx + c * (Keyboard_KeyW + Keyboard_KeyGap), y, Keyboard_KeyW, (self->f_shifted ? s[c] : n[c]), self->f_row == 2 && self->f_col == c, self->f_ColKey, 2, c);
    }
}

void Keyboard_DrawRow3(Keyboard* self, int y)
{
    int shiftW = Keyboard_KeyW * 2 + Keyboard_KeyGap;
    int delW = Keyboard_KeyW + 20;
    int letW = 10 * Keyboard_KeyW + 9 * Keyboard_KeyGap;
    int totalW = shiftW + Keyboard_KeyGap + letW + Keyboard_KeyGap + delW;
    int sx = (1280 - totalW) / 2;
    Keyboard_DrawKey(self, sx, y, shiftW, (self->f_shifted ? "SHIFT*" : "SHIFT"), self->f_row == 3 && self->f_col == 0, self->f_ColSpecial, 3, 0);
    int lx = sx + shiftW + Keyboard_KeyGap;
    const char* n[] = { "z", "x", "c", "v", "b", "n", "m", ",", ".", "/" };
    const char* s[] = { "Z", "X", "C", "V", "B", "N", "M", "<", ">", "?" };
    for (int c = 0; c < 10; c++)
    {
        Keyboard_DrawKey(self, lx + c * (Keyboard_KeyW + Keyboard_KeyGap), y, Keyboard_KeyW, (self->f_shifted ? s[c] : n[c]), self->f_row == 3 && self->f_col == c + 1, self->f_ColKey, 3, c + 1);
    }
    Keyboard_DrawKey(self, lx + 10 * (Keyboard_KeyW + Keyboard_KeyGap), y, delW, "DEL", self->f_row == 3 && self->f_col == 11, self->f_ColSpecial, 3, 11);
}

void Keyboard_DrawRow4(Keyboard* self, int y)
{
    int capsW = 100;
    int spaceW = 700;
    int enterW = 140;
    int closeW = 120;
    int totalW = capsW + Keyboard_KeyGap + spaceW + Keyboard_KeyGap + enterW + Keyboard_KeyGap + closeW;
    int x = (1280 - totalW) / 2;
    Keyboard_DrawKey(self, x, y, capsW, "CAPS", self->f_row == 4 && self->f_col == 0, self->f_ColSpecial, 4, 0);
    x += capsW + Keyboard_KeyGap;
    Keyboard_DrawKey(self, x, y, spaceW, "SPACE", self->f_row == 4 && self->f_col == 1, self->f_ColKey, 4, 1);
    x += spaceW + Keyboard_KeyGap;
    Keyboard_DrawKey(self, x, y, enterW, "ENTER", self->f_row == 4 && self->f_col == 2, self->f_ColSpecial, 4, 2);
    x += enterW + Keyboard_KeyGap;
    Keyboard_DrawKey(self, x, y, closeW, "CLOSE", self->f_row == 4 && self->f_col == 3, self->f_ColDanger, 4, 3);
}

void Keyboard_TypeChar(Keyboard* self, char ch)
{
    if (self->f_bufLen < Keyboard_MaxBuf)
    {
        self->f_buf[self->f_bufLen] = ch;
        self->f_bufLen++;
        self->f_buf[self->f_bufLen] = '\0';
    }
    self->f_shifted = 0;
}

void Keyboard_Backspace(Keyboard* self)
{
    if (self->f_bufLen > 0)
    {
        self->f_bufLen--;
        self->f_buf[self->f_bufLen] = '\0';
    }
}

void Keyboard_Confirm(Keyboard* self)
{
    self->f_confirmed = 1;
    self->visible = 0;
}

void Keyboard_Cancel(Keyboard* self)
{
    self->f_cancelled = 1;
    self->visible = 0;
}

int Keyboard_MaxCol(Keyboard* self, int row)
{
    if (row == 3)
    {
        return 11;
    }
    if (row == 4)
    {
        return 3;
    }
    return 11;
}

void Keyboard_ClampCol(Keyboard* self)
{
    int max = Keyboard_MaxCol(self, self->f_row);
    if (self->f_col > max)
    {
        self->f_col = max;
    }
}

void Keyboard_Free(Keyboard* self)
{
    if (!self) return;
    if (--self->_rc > 0) return;
    if (self->f_buf) { free(self->f_buf); self->f_buf = NULL; }
    free(self);
}

Keyboard* Keyboard_Retain(Keyboard* self)
{
    if (self) self->_rc++;
    return self;
}

int Keyboard_get_IsVisible(Keyboard* self)
{
    return self->visible;
}

