#pragma once
#ifndef CS2SX_PROGRAM_H
#define CS2SX_PROGRAM_H

#include "_forward.h"

struct keyboardApp
{
    SwitchApp base;
    Keyboard* f_keyboard;
    const char* f_enteredText;
};

void keyboardApp_Init(keyboardApp* self);
void keyboardApp_OnInit(keyboardApp* self);
void keyboardApp_OnFrame(keyboardApp* self);

void keyboardApp_Free(keyboardApp* self);


#endif
