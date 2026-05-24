#pragma once
#ifndef CS2SX_PROGRAM_H
#define CS2SX_PROGRAM_H

#include "_forward.h"

struct SnakeApp
{
    SwitchApp base;
    int f_GridW;
    int f_GridH;
    int f_FrameDelay;
    const char* f_SavePath;
    const char* f_SaveDir;
    int* f_snakeX;
    int* f_snakeY;
    int f_snakeLen;
    int f_dirX;
    int f_dirY;
    int f_nextDirX;
    int f_nextDirY;
    Dict_int_int* f_occupied;
    int f_foodX;
    int f_foodY;
    int f_gameOver;
    int f_paused;
    int f_newBest;
    int f_score;
    int f_highscore;
    int f_frameTimer;
    int f_batteryTimer;
    CS2SX_BatteryInfo f_battery;
};

#define SnakeApp_GridW (64)
#define SnakeApp_GridH (36)
#define SnakeApp_FrameDelay (7)
extern const char* const SnakeApp_SavePath;
extern const char* const SnakeApp_SaveDir;
void SnakeApp_Init(SnakeApp* self);
void SnakeApp_OnInit(SnakeApp* self);
void SnakeApp_ResetGame(SnakeApp* self);
void SnakeApp_SpawnFood(SnakeApp* self);
void SnakeApp_OnFrame(SnakeApp* self);
void SnakeApp_HandleInput(SnakeApp* self);
void SnakeApp_Update(SnakeApp* self);
void SnakeApp_TriggerGameOver(SnakeApp* self);
void SnakeApp_Render(SnakeApp* self);
void SnakeApp_OnExit(SnakeApp* self);

void SnakeApp_Free(SnakeApp* self);


#endif
