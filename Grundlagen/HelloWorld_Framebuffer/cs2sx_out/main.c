// Auto-generiert von CS2SX -- nicht manuell bearbeiten
#include <stdlib.h>
#include <string.h>
#include "_forward.h"
#include "Program.h"

int main(int argc, char* argv[])
{
    (void)argc;
    (void)argv;

    // Heap-Allokation statt Stack — sicherer für große App-Structs
    HelloWorld_FramebufferApp* app = (HelloWorld_FramebufferApp*)calloc(1, sizeof(HelloWorld_FramebufferApp));
    if (!app) return 1;

    HelloWorld_FramebufferApp_Init(app);
    SwitchApp_Run((SwitchApp*)app);

    free(app);
    return 0;
}
