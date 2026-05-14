#pragma once
#ifndef CS2SX_PROGRAM_H
#define CS2SX_PROGRAM_H

#include "_forward.h"

struct HelloWorld_FramebufferApp
{
    SwitchApp base;
};

void HelloWorld_FramebufferApp_Init(HelloWorld_FramebufferApp* self);
void HelloWorld_FramebufferApp_OnInit(HelloWorld_FramebufferApp* self);
void HelloWorld_FramebufferApp_OnFrame(HelloWorld_FramebufferApp* self);



#endif
