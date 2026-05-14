#include <stdlib.h>
#include "Program.h"

void InputApp_Init(InputApp* self)
{
    if (!self) return;
    ((SwitchApp*)self)->OnInit = (void(*)(SwitchApp*))InputApp_OnInit;
    ((SwitchApp*)self)->OnFrame = (void(*)(SwitchApp*))InputApp_OnFrame;
    self->f_lastPressed = "Keine Taste";
}

void InputApp_OnInit(InputApp* self)
{
    Graphics_Init(1280, 720);
}

void InputApp_OnFrame(InputApp* self)
{
    if ((((SwitchApp*)self)->kDown & HidNpadButton_A))
    {
        self->f_lastPressed = "A";
    }
    else if ((((SwitchApp*)self)->kDown & HidNpadButton_B))
    {
        self->f_lastPressed = "B";
    }
    else if ((((SwitchApp*)self)->kDown & HidNpadButton_X))
    {
        self->f_lastPressed = "X";
    }
    else if ((((SwitchApp*)self)->kDown & HidNpadButton_Y))
    {
        self->f_lastPressed = "Y";
    }
    else if ((((SwitchApp*)self)->kDown & HidNpadButton_Up))
    {
        self->f_lastPressed = "D-Pad Hoch";
    }
    else if ((((SwitchApp*)self)->kDown & HidNpadButton_Down))
    {
        self->f_lastPressed = "D-Pad Runter";
    }
    else if ((((SwitchApp*)self)->kDown & HidNpadButton_Left))
    {
        self->f_lastPressed = "D-Pad Links";
    }
    else if ((((SwitchApp*)self)->kDown & HidNpadButton_Right))
    {
        self->f_lastPressed = "D-Pad Rechts";
    }
    else if ((((SwitchApp*)self)->kDown & HidNpadButton_L))
    {
        self->f_lastPressed = "L";
    }
    else if ((((SwitchApp*)self)->kDown & HidNpadButton_R))
    {
        self->f_lastPressed = "R";
    }
    else if ((((SwitchApp*)self)->kDown & HidNpadButton_ZL))
    {
        self->f_lastPressed = "ZL";
    }
    else if ((((SwitchApp*)self)->kDown & HidNpadButton_ZR))
    {
        self->f_lastPressed = "ZR";
    }
    Graphics_FillScreen(COLOR_BLACK);
    Graphics_DrawText(100, 100, "Input Demo", COLOR_WHITE, 2);
    Graphics_DrawText(100, 200, "Zuletzt gedrueckt:", COLOR_GRAY, 1);
    Graphics_DrawText(100, 240, self->f_lastPressed, COLOR_YELLOW, 3);
    Graphics_DrawText(100, 620, "+ zum Beenden", COLOR_GRAY, 1);
}

