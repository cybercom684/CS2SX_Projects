#pragma once
#ifndef CS2SX_PROGRAM_H
#define CS2SX_PROGRAM_H

#include "_forward.h"

struct HelloWorld_KonsoleApp
{
    SwitchApp base;
    Label* f_label;
    Label* f_label_exit;
};

void HelloWorld_KonsoleApp_Init(HelloWorld_KonsoleApp* self);
void HelloWorld_KonsoleApp_OnInit(HelloWorld_KonsoleApp* self);
void HelloWorld_KonsoleApp_OnFrame(HelloWorld_KonsoleApp* self);



#endif
