#include <stdlib.h>
#include "Keyboard.h"
#include "Program.h"

void keyboardApp_Init(keyboardApp* self)
{
    if (!self) return;
    ((SwitchApp*)self)->OnInit = (void(*)(SwitchApp*))keyboardApp_OnInit;
    ((SwitchApp*)self)->OnFrame = (void(*)(SwitchApp*))keyboardApp_OnFrame;
    self->f_enteredText = "Keine Eingabe";
}

void keyboardApp_OnInit(keyboardApp* self)
{
    Graphics_Init(1280, 720);
    self->f_keyboard = Keyboard_New();
    Keyboard_Show(self->f_keyboard, "Test");
}

void keyboardApp_OnFrame(keyboardApp* self)
{
    Keyboard_Update(self->f_keyboard);
    Keyboard_Draw(self->f_keyboard);
    if (Keyboard_WasConfirmed(self->f_keyboard))
    {
        self->f_enteredText = Keyboard_GetBuffer(self->f_keyboard);
    }
    Graphics_DrawText(20, 20, self->f_enteredText, COLOR_WHITE, 4);
}

void keyboardApp_Free(keyboardApp* self)
{
    if (!self) return;
    if (self->f_keyboard) Keyboard_Free(self->f_keyboard);
    free(self);
}

