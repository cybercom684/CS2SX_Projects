#include <stdlib.h>
#include "Program.h"
#include "SnakeRenderer.h"

const char* const SnakeApp_SavePath = "/switch/Snake/highscore.txt";
const char* const SnakeApp_SaveDir = "/switch/Snake";
void SnakeApp_Init(SnakeApp* self)
{
    if (!self) return;
    ((SwitchApp*)self)->OnInit = (void(*)(SwitchApp*))SnakeApp_OnInit;
    ((SwitchApp*)self)->OnFrame = (void(*)(SwitchApp*))SnakeApp_OnFrame;
    ((SwitchApp*)self)->OnExit = (void(*)(SwitchApp*))SnakeApp_OnExit;
    self->f_GridW = 64;
    self->f_GridH = 36;
    self->f_FrameDelay = 7;
    self->f_SavePath = "/switch/Snake/highscore.txt";
    self->f_SaveDir = "/switch/Snake";
    self->f_snakeX = (int*)malloc(SnakeApp_GridW * SnakeApp_GridH * sizeof(int));
    self->f_snakeY = (int*)malloc(SnakeApp_GridW * SnakeApp_GridH * sizeof(int));
    self->f_occupied = Dict_int_int_New();
}

void SnakeApp_OnInit(SnakeApp* self)
{
    Graphics_Init(1280, 720);
    CS2SX_Audio_Init(44100);
    if (!CS2SX_Dir_Exists(SnakeApp_SaveDir))
    {
        CS2SX_Dir_Create(SnakeApp_SaveDir);
    }
    if (CS2SX_File_Exists(SnakeApp_SavePath))
    {
        const char* raw = CS2SX_File_ReadAllText(SnakeApp_SavePath);
        if (!CS2SX_Int_TryParse(raw, &self->f_highscore))
        {
            self->f_highscore = 0;
        }
    }
    self->f_battery = CS2SX_GetBattery();
    SnakeApp_ResetGame(self);
}

void SnakeApp_ResetGame(SnakeApp* self)
{
    self->f_snakeLen = 3;
    self->f_snakeX[0] = SnakeApp_GridW / 2;
    self->f_snakeY[0] = SnakeApp_GridH / 2;
    self->f_snakeX[1] = SnakeApp_GridW / 2 - 1;
    self->f_snakeY[1] = SnakeApp_GridH / 2;
    self->f_snakeX[2] = SnakeApp_GridW / 2 - 2;
    self->f_snakeY[2] = SnakeApp_GridH / 2;
    self->f_dirX = 1;
    self->f_dirY = 0;
    self->f_nextDirX = 1;
    self->f_nextDirY = 0;
    self->f_gameOver = 0;
    self->f_paused = 0;
    self->f_newBest = 0;
    self->f_score = 0;
    self->f_frameTimer = 0;
    self->f_foodX = 10;
    self->f_foodY = 10;
    Dict_int_int_Clear(self->f_occupied);
    for (int i = 0; i < self->f_snakeLen; i++)
    {
        Dict_int_int_Add(self->f_occupied, self->f_snakeX[i] + self->f_snakeY[i] * SnakeApp_GridW, 1);
    }
    SnakeApp_SpawnFood(self);
}

void SnakeApp_SpawnFood(SnakeApp* self)
{
    int tries = 0;
    do
    {
        self->f_foodX = CS2SX_Rand_Next(0, SnakeApp_GridW);
        self->f_foodY = CS2SX_Rand_Next(0, SnakeApp_GridH);
        tries++;
    }
    while (Dict_int_int_ContainsKey(self->f_occupied, self->f_foodX + self->f_foodY * SnakeApp_GridW) && tries < 200);
}

void SnakeApp_OnFrame(SnakeApp* self)
{
    self->f_batteryTimer++;
    if (self->f_batteryTimer >= 300)
    {
        self->f_battery = CS2SX_GetBattery();
        self->f_batteryTimer = 0;
    }
    SnakeApp_HandleInput(self);
    SnakeApp_Update(self);
    SnakeApp_Render(self);
}

void SnakeApp_HandleInput(SnakeApp* self)
{
    if ((((SwitchApp*)self)->kDown & HidNpadButton_Minus))
    {
        self->f_paused = !self->f_paused;
    }
    if (self->f_gameOver)
    {
        if ((((SwitchApp*)self)->kDown & HidNpadButton_A))
        {
            SnakeApp_ResetGame(self);
        }
        return;
    }
    if (self->f_paused)
    {
        if ((((SwitchApp*)self)->kDown & HidNpadButton_A))
        {
            self->f_paused = 0;
        }
        return;
    }
    if ((((SwitchApp*)self)->kDown & HidNpadButton_Up) && self->f_dirY == 0)
    {
        self->f_nextDirX = 0;
        self->f_nextDirY = -1;
    }
    if ((((SwitchApp*)self)->kDown & HidNpadButton_Down) && self->f_dirY == 0)
    {
        self->f_nextDirX = 0;
        self->f_nextDirY = 1;
    }
    if ((((SwitchApp*)self)->kDown & HidNpadButton_Left) && self->f_dirX == 0)
    {
        self->f_nextDirX = -1;
        self->f_nextDirY = 0;
    }
    if ((((SwitchApp*)self)->kDown & HidNpadButton_Right) && self->f_dirX == 0)
    {
        self->f_nextDirX = 1;
        self->f_nextDirY = 0;
    }
    CS2SX_StickPos stick = _cs2sx_get_stick_left();
    if (stick.x > 10000 && self->f_dirX == 0)
    {
        self->f_nextDirX = 1;
        self->f_nextDirY = 0;
    }
    if (stick.x < -10000 && self->f_dirX == 0)
    {
        self->f_nextDirX = -1;
        self->f_nextDirY = 0;
    }
    if (stick.y > 10000 && self->f_dirY == 0)
    {
        self->f_nextDirX = 0;
        self->f_nextDirY = -1;
    }
    if (stick.y < -10000 && self->f_dirY == 0)
    {
        self->f_nextDirX = 0;
        self->f_nextDirY = 1;
    }
    CS2SX_TouchState touch = CS2SX_Input_GetTouch();
    if (touch.count > 0 && touch.x[0] > 900 && touch.y[0] < 80)
    {
        self->f_paused = !self->f_paused;
    }
}

void SnakeApp_Update(SnakeApp* self)
{
    if (self->f_gameOver || self->f_paused)
    {
        return;
    }
    self->f_frameTimer++;
    if (self->f_frameTimer < SnakeApp_FrameDelay)
    {
        return;
    }
    self->f_frameTimer = 0;
    self->f_dirX = self->f_nextDirX;
    self->f_dirY = self->f_nextDirY;
    int newX = self->f_snakeX[0] + self->f_dirX;
    int newY = self->f_snakeY[0] + self->f_dirY;
    if (newX < 0 || newX >= SnakeApp_GridW || newY < 0 || newY >= SnakeApp_GridH)
    {
        SnakeApp_TriggerGameOver(self);
        return;
    }
    if (Dict_int_int_ContainsKey(self->f_occupied, newX + newY * SnakeApp_GridW))
    {
        SnakeApp_TriggerGameOver(self);
        return;
    }
    Dict_int_int_Remove(self->f_occupied, self->f_snakeX[self->f_snakeLen - 1] + self->f_snakeY[self->f_snakeLen - 1] * SnakeApp_GridW);
    for (int i = self->f_snakeLen - 1; i > 0; i--)
    {
        self->f_snakeX[i] = self->f_snakeX[i - 1];
        self->f_snakeY[i] = self->f_snakeY[i - 1];
    }
    self->f_snakeX[0] = newX;
    self->f_snakeY[0] = newY;
    Dict_int_int_Add(self->f_occupied, newX + newY * SnakeApp_GridW, 1);
    if (newX == self->f_foodX && newY == self->f_foodY)
    {
        self->f_snakeLen++;
        self->f_score++;
        CS2SX_Audio_PlayTone(880.0f, 0.3f, 80);
        if (self->f_score > self->f_highscore)
        {
            self->f_highscore = self->f_score;
            self->f_newBest = 1;
            CS2SX_File_WriteAllText(SnakeApp_SavePath, Int_ToString((int)self->f_highscore));
        }
        SnakeApp_SpawnFood(self);
    }
}

void SnakeApp_TriggerGameOver(SnakeApp* self)
{
    self->f_gameOver = 1;
    CS2SX_Audio_PlayTone(150.0f, 0.5f, 600);
}

void SnakeApp_Render(SnakeApp* self)
{
    Graphics_FillScreen(COLOR_BLACK);
    SnakeRenderer_DrawGrid();
    SnakeRenderer_DrawFood(self->f_foodX, self->f_foodY);
    SnakeRenderer_DrawSnake(self->f_snakeX, self->f_snakeY, self->f_snakeLen);
    SnakeRenderer_DrawHUD(self->f_score, self->f_highscore, self->f_battery);
    if (self->f_paused)
    {
        SnakeRenderer_DrawPauseOverlay();
    }
    if (self->f_gameOver)
    {
        SnakeRenderer_DrawGameOver(self->f_score, self->f_highscore, self->f_newBest);
    }
}

void SnakeApp_OnExit(SnakeApp* self)
{
    CS2SX_Audio_Exit();
}

void SnakeApp_Free(SnakeApp* self)
{
    if (!self) return;
    if (self->f_snakeX) { free(self->f_snakeX); self->f_snakeX = NULL; }
    if (self->f_snakeY) { free(self->f_snakeY); self->f_snakeY = NULL; }
    free(self);
}

