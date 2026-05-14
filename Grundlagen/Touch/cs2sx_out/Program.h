#pragma once
#ifndef CS2SX_PROGRAM_H
#define CS2SX_PROGRAM_H

#include "_forward.h"

struct TouchApp
{
    SwitchApp base;
    int f_touchX;
    int f_touchY;
    bool f_touching;
};

void TouchApp_Init(TouchApp* self);
void TouchApp_OnInit(TouchApp* self);
void TouchApp_OnFrame(TouchApp* self);



#endif
