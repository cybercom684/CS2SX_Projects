#include <stdlib.h>
#include "Program.h"

void TouchApp_Init(TouchApp* self)
{
    if (!self) return;
    ((SwitchApp*)self)->OnInit = (void(*)(SwitchApp*))TouchApp_OnInit;
    ((SwitchApp*)self)->OnFrame = (void(*)(SwitchApp*))TouchApp_OnFrame;
}

void TouchApp_OnInit(TouchApp* self)
{
    Graphics_Init(1280, 720);
}

void TouchApp_OnFrame(TouchApp* self)
{
    CS2SX_TouchState touch = CS2SX_Input_GetTouch();
    self->f_touching = touch.count > 0;
    if (self->f_touching)
    {
        self->f_touchX = touch.x[0];
        self->f_touchY = touch.y[0];
    }
    Graphics_FillScreen(COLOR_BLACK);
    Graphics_DrawText(100, 100, "Touch Demo", COLOR_WHITE, 2);
    if (self->f_touching)
    {
        Graphics_DrawText(100, 200, "Beruehrt!", COLOR_GREEN, 2);
        Graphics_FillCircle(self->f_touchX, self->f_touchY, 20, COLOR_YELLOW);
        char _sb0[512];
        snprintf(_sb0, sizeof(_sb0), "X: %d  Y: %d", self->f_touchX, self->f_touchY);
        Graphics_DrawText(100, 260, _sb0, COLOR_GRAY, 1);
    }
    else
    {
        Graphics_DrawText(100, 200, "Kein Touch", COLOR_GRAY, 2);
    }
    Graphics_DrawText(100, 620, "+ zum Beenden", COLOR_GRAY, 1);
}

void TouchApp_Free(TouchApp* self)
{
    if (!self) return;
    free(self);
}

