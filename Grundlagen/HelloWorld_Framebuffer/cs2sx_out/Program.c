#include <stdlib.h>
#include "Program.h"

void HelloWorld_FramebufferApp_Init(HelloWorld_FramebufferApp* self)
{
    if (!self) return;
    ((SwitchApp*)self)->OnInit = (void(*)(SwitchApp*))HelloWorld_FramebufferApp_OnInit;
    ((SwitchApp*)self)->OnFrame = (void(*)(SwitchApp*))HelloWorld_FramebufferApp_OnFrame;
}

void HelloWorld_FramebufferApp_OnInit(HelloWorld_FramebufferApp* self)
{
    Graphics_Init(1280, 720);
    printf("HelloWorld_Framebuffer started!\n");
    printf("Press + to exit.\n");
}

void HelloWorld_FramebufferApp_OnFrame(HelloWorld_FramebufferApp* self)
{
    Graphics_DrawRect(0, 0, 1280, 50, COLOR_YELLOW);
    Graphics_FillRect(1, 1, 1278, 48, COLOR_GRAY);
    Graphics_DrawText(20, 20, "Hello World!", COLOR_BLACK, 3);
    Graphics_DrawRect(0, 670, 1280, 50, COLOR_YELLOW);
    Graphics_FillRect(1, 671, 1278, 48, COLOR_GRAY);
    Graphics_DrawText(20, 681, "Press + to exit", COLOR_BLACK, 2);
}

