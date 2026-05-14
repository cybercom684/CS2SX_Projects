#pragma once
#ifndef CS2SX_PROGRAM_H
#define CS2SX_PROGRAM_H

#include "_forward.h"

struct InputApp
{
    SwitchApp base;
    const char* f_lastPressed;
};

void InputApp_Init(InputApp* self);
void InputApp_OnInit(InputApp* self);
void InputApp_OnFrame(InputApp* self);



#endif
