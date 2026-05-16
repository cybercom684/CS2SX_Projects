#include "_forward.h"

char         _cs2sx_strbuf[1024];
Framebuffer  g_fb;
u32*         g_fb_addr       = NULL;
int          g_fb_width      = 1280;
int          g_fb_height     = 720;
int          g_gfx_init      = 0;
PadState     g_cs2sx_pad;
unsigned int _cs2sx_rand_state = 12345u;

char _cs2sx_strpool[32][1024];
int  _cs2sx_strpool_idx = 0;

// Audio state (extern in AudioStub.h)
int               _cs2sx_audio_init      = 0;
float             _cs2sx_audio_volume    = 1.0f;
float             _cs2sx_audio_phase     = 0.0f;
CS2SX_AudioBuffer _cs2sx_audio_bufs[4];
int               _cs2sx_audio_buf_idx   = 0;
int               _cs2sx_audio_submitted = 0;
