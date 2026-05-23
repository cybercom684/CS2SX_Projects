#include <stdlib.h>
#include "Program.h"
#include "SnakeRenderer.h"

void SnakeRenderer_DrawGrid()
{
    Graphics_FillRect(0, 0, SnakeRenderer_GridW * SnakeRenderer_CellSize, SnakeRenderer_GridH * SnakeRenderer_CellSize, CS2SX_RGB(15, 15, 15));
    Graphics_DrawRect(0, 0, SnakeRenderer_GridW * SnakeRenderer_CellSize, SnakeRenderer_GridH * SnakeRenderer_CellSize, COLOR_DGRAY);
}

void SnakeRenderer_DrawSnake(int* snakeX, int* snakeY, int snakeLen)
{
    for (int i = snakeLen - 1; i >= 0; i--)
    {
        int x = snakeX[i] * SnakeRenderer_CellSize + 1;
        int y = snakeY[i] * SnakeRenderer_CellSize + 1;
        int s = SnakeRenderer_CellSize - 2;
        if (i == 0)
        {
            Graphics_FillRoundedRect(x, y, s, s, 4, COLOR_LIME);
            Graphics_DrawRoundedRect(x, y, s, s, 4, COLOR_GREEN);
        }
        else
        {
            int green = 180 - i * 2;
            int blue = 80 - i;
            if (green < 40)
            {
                green = 40;
            }
            if (blue < 0)
            {
                blue = 0;
            }
            Graphics_FillRoundedRect(x, y, s, s, 3, CS2SX_RGB(0, green, blue));
        }
    }
}

void SnakeRenderer_DrawFood(int foodX, int foodY)
{
    int cx = foodX * SnakeRenderer_CellSize + SnakeRenderer_CellSize / 2;
    int cy = foodY * SnakeRenderer_CellSize + SnakeRenderer_CellSize / 2;
    int r = SnakeRenderer_CellSize / 2 - 2;
    Graphics_FillCircle(cx, cy, r, COLOR_RED);
    Graphics_DrawCircle(cx, cy, r, CS2SX_RGB(255, 100, 100));
    Graphics_FillCircle(cx - 2, cy - 2, 2, COLOR_WHITE);
}

void SnakeRenderer_DrawHUD(int score, int highscore, CS2SX_BatteryInfo battery)
{
    int hx = SnakeRenderer_HudX;
    Graphics_FillRect(hx - 8, 0, 1280 - hx + 8, 720, CS2SX_RGB(10, 10, 10));
    Graphics_DrawLine(hx - 8, 0, hx - 8, 720, COLOR_DGRAY);
    Graphics_DrawTextShadow(hx, 20, "SNAKE", COLOR_LIME, CS2SX_RGB(0, 60, 0), 3);
    Graphics_DrawLine(hx, 80, 1275, 80, CS2SX_RGB(40, 40, 40));
    Graphics_DrawText(hx, 95, "SCORE", COLOR_GRAY, 1);
    Graphics_DrawText(hx, 110, Int_ToString((int)score), COLOR_YELLOW, 3);
    Graphics_DrawText(hx, 195, "BEST", COLOR_GRAY, 1);
    Graphics_DrawText(hx, 210, Int_ToString((int)highscore), COLOR_ORANGE, 2);
    Graphics_DrawLine(hx, 260, 1275, 260, CS2SX_RGB(40, 40, 40));
    Graphics_DrawText(hx, 275, "STEUERUNG", COLOR_GRAY, 1);
    Graphics_DrawText(hx, 292, "D-Pad / Stick", COLOR_DGRAY, 1);
    Graphics_DrawText(hx, 315, "- = Pause", COLOR_DGRAY, 1);
    Graphics_DrawText(hx, 330, "A = Fortfahren", COLOR_DGRAY, 1);
    Graphics_DrawText(hx, 345, "+ = Beenden", COLOR_DGRAY, 1);
    Graphics_DrawText(hx, 365, "Touch oben", COLOR_DGRAY, 1);
    Graphics_DrawText(hx, 380, "rechts = Pause", COLOR_DGRAY, 1);
    Graphics_DrawLine(hx, 660, 1275, 660, CS2SX_RGB(40, 40, 40));
    unsigned int battCol = COLOR_GREEN;
    if (battery.percent <= 50)
    {
        battCol = COLOR_YELLOW;
    }
    if (battery.percent <= 20)
    {
        battCol = COLOR_RED;
    }
    char _sb0[512];
    snprintf(_sb0, sizeof(_sb0), "%s%%", Int_ToString((int)battery.percent));
    Graphics_DrawText(hx, 668, _sb0, battCol, 1);
    if (battery.charging)
    {
        Graphics_DrawText(hx + 45, 668, "CHG", COLOR_CYAN, 1);
    }
}

void SnakeRenderer_DrawPauseOverlay()
{
    Graphics_FillRectAlpha(0, 0, SnakeRenderer_GridW * SnakeRenderer_CellSize, SnakeRenderer_GridH * SnakeRenderer_CellSize, COLOR_BLACK, 160);
    Graphics_FillRoundedRect(220, 275, 840, 170, 14, CS2SX_RGB(20, 20, 20));
    Graphics_DrawRoundedRect(220, 275, 840, 170, 14, COLOR_CYAN);
    Graphics_DrawTextShadow(375, 300, "PAUSE", COLOR_CYAN, CS2SX_RGB(0, 80, 80), 3);
    Graphics_DrawLine(220, 370, 1060, 370, CS2SX_RGB(40, 40, 40));
    Graphics_DrawText(295, 385, "A = Fortfahren          - = Pause halten", COLOR_WHITE, 1);
}

void SnakeRenderer_DrawGameOver(int score, int highscore, int newBest)
{
    Graphics_FillRectAlpha(0, 0, SnakeRenderer_GridW * SnakeRenderer_CellSize, SnakeRenderer_GridH * SnakeRenderer_CellSize, COLOR_BLACK, 180);
    Graphics_FillRoundedRect(200, 235, 880, 250, 16, CS2SX_RGB(25, 0, 0));
    Graphics_DrawRoundedRect(200, 235, 880, 250, 16, COLOR_RED);
    Graphics_DrawTextShadow(310, 260, "GAME OVER", COLOR_RED, CS2SX_RGB(80, 0, 0), 3);
    Graphics_DrawLine(200, 335, 1080, 335, CS2SX_RGB(80, 0, 0));
    if (newBest)
    {
        Graphics_DrawText(310, 350, "NEUER REKORD!", COLOR_YELLOW, 2);
        char _sb1[512];
        snprintf(_sb1, sizeof(_sb1), "Punkte: %s", Int_ToString((int)score));
        Graphics_DrawText(310, 390, _sb1, COLOR_WHITE, 2);
    }
    else
    {
        char _sb2[512];
        snprintf(_sb2, sizeof(_sb2), "Punkte:    %s", Int_ToString((int)score));
        Graphics_DrawText(310, 355, _sb2, COLOR_WHITE, 2);
        char _sb3[512];
        snprintf(_sb3, sizeof(_sb3), "Rekord:    %s", Int_ToString((int)highscore));
        Graphics_DrawText(310, 395, _sb3, COLOR_ORANGE, 2);
    }
    Graphics_DrawLine(200, 440, 1080, 440, CS2SX_RGB(80, 0, 0));
    Graphics_DrawText(310, 452, "A = Neustart", COLOR_GRAY, 2);
}

