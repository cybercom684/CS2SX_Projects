#include <stdlib.h>
#include "Program.h"

void HelloWorld_KonsoleApp_Init(HelloWorld_KonsoleApp* self)
{
    if (!self) return;
    ((SwitchApp*)self)->OnInit = (void(*)(SwitchApp*))HelloWorld_KonsoleApp_OnInit;
    ((SwitchApp*)self)->OnFrame = (void(*)(SwitchApp*))HelloWorld_KonsoleApp_OnFrame;
}

void HelloWorld_KonsoleApp_OnInit(HelloWorld_KonsoleApp* self)
{
    self->f_label = Label_New("Hello World from C#!");
    self->f_label->base.x = 5;
    self->f_label->base.y = 5;
    SwitchApp_Add((SwitchApp*)self, (Control*)self->f_label);
    self->f_label_exit = Label_New("Press + to exit");
    self->f_label_exit->base.x = 5;
    self->f_label_exit->base.y = 620;
    SwitchApp_Add((SwitchApp*)self, (Control*)self->f_label_exit);
}

void HelloWorld_KonsoleApp_OnFrame(HelloWorld_KonsoleApp* self)
{
}

