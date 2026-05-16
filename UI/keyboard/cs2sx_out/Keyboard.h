#pragma once
#ifndef CS2SX_KEYBOARD_H
#define CS2SX_KEYBOARD_H

#include "_forward.h"

struct Keyboard
{
    int f_KeyW;
    int f_KeyH;
    int f_KeyGap;
    int f_PanelY;
    int f_PanelH;
    int f_InputX;
    int f_InputY;
    int f_InputH;
    int f_InputW;
    int f_Row1Y;
    int f_Row2Y;
    int f_Row3Y;
    int f_Row4Y;
    int f_Row5Y;
    int f_MaxBuf;
    unsigned int f_ColBg;
    unsigned int f_ColHeader;
    unsigned int f_ColInput;
    unsigned int f_ColInputBorder;
    unsigned int f_ColKey;
    unsigned int f_ColHover;
    unsigned int f_ColSpecial;
    unsigned int f_ColText;
    unsigned int f_ColCursor;
    unsigned int f_ColDanger;
    int visible;
    char* f_buf;
    int f_bufLen;
    int f_col;
    int f_row;
    int f_shifted;
    int f_tick;
    int f_confirmed;
    int f_cancelled;
    int f_touchOverRow;
    int f_touchOverCol;
    int f_lastTouchRow;
    int f_lastTouchCol;
    int f_RepeatDelay;
    int f_RepeatInterval;
    int f_delHeld;
    int f_delHoldFrames;
};

#define Keyboard_KeyW (88)
#define Keyboard_KeyH (60)
#define Keyboard_KeyGap (5)
#define Keyboard_PanelY (280)
#define Keyboard_PanelH (440)
#define Keyboard_InputX (16)
#define Keyboard_InputY (304)
#define Keyboard_InputH (52)
#define Keyboard_InputW (1248)
#define Keyboard_Row1Y (388)
#define Keyboard_Row2Y (388 + 66)
#define Keyboard_Row3Y (388 + 132)
#define Keyboard_Row4Y (388 + 198)
#define Keyboard_Row5Y (388 + 264)
#define Keyboard_MaxBuf (256)
#define Keyboard_RepeatDelay (30)
#define Keyboard_RepeatInterval (4)
Keyboard* Keyboard_New();
int Keyboard_WasConfirmed(Keyboard* self);
int Keyboard_WasCancelled(Keyboard* self);
const char* Keyboard_GetBuffer(Keyboard* self);
void Keyboard_Show(Keyboard* self, const char* initialText);
void Keyboard_Hide(Keyboard* self);
void Keyboard_Update(Keyboard* self);
void Keyboard_Draw(Keyboard* self);
void Keyboard_HandleBackspaceRepeat(Keyboard* self);
void Keyboard_HandleTouch(Keyboard* self);
void Keyboard_ExecuteKey(Keyboard* self, int row, int col);
void Keyboard_DrawInputBar(Keyboard* self);
void Keyboard_DrawHints(Keyboard* self);
void Keyboard_DrawKey(Keyboard* self, int x, int y, int w, const char* lbl, int selected, unsigned int normalBg, int row, int col);
void Keyboard_DrawRow0(Keyboard* self, int y);
void Keyboard_DrawRow1(Keyboard* self, int y);
void Keyboard_DrawRow2(Keyboard* self, int y);
void Keyboard_DrawRow3(Keyboard* self, int y);
void Keyboard_DrawRow4(Keyboard* self, int y);
void Keyboard_TypeChar(Keyboard* self, char ch);
void Keyboard_Backspace(Keyboard* self);
void Keyboard_Confirm(Keyboard* self);
void Keyboard_Cancel(Keyboard* self);
int Keyboard_MaxCol(Keyboard* self, int row);
void Keyboard_ClampCol(Keyboard* self);

void Keyboard_Free(Keyboard* self);
int Keyboard_get_IsVisible(Keyboard* self);


#endif
