#include "_forward.h"

char        _cs2sx_strbuf[512];
Framebuffer g_fb;
u32*        g_fb_addr   = NULL;
int         g_fb_width  = 1280;
int         g_fb_height = 720;
int         g_gfx_init  = 0;
PadState    g_cs2sx_pad;
