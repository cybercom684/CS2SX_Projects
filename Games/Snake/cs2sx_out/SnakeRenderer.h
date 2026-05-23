#pragma once
#ifndef CS2SX_SNAKERENDERER_H
#define CS2SX_SNAKERENDERER_H

#include "_forward.h"

#define SnakeRenderer_CellSize (20)
#define SnakeRenderer_GridW (64)
#define SnakeRenderer_GridH (36)
#define SnakeRenderer_HudX (SnakeRenderer_GridW * SnakeRenderer_CellSize + 10)
void SnakeRenderer_DrawGrid();
void SnakeRenderer_DrawSnake(int* snakeX, int* snakeY, int snakeLen);
void SnakeRenderer_DrawFood(int foodX, int foodY);
void SnakeRenderer_DrawHUD(int score, int highscore, CS2SX_BatteryInfo battery);
void SnakeRenderer_DrawPauseOverlay();
void SnakeRenderer_DrawGameOver(int score, int highscore, int newBest);



#endif
