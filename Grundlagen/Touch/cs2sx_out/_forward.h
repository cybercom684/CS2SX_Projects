#pragma once
#include <switch.h>
#include <stdlib.h>
#include <string.h>
#include <stdio.h>
#include <stdbool.h>

typedef void (*Action)(void*);

typedef struct Control    Control;
typedef struct Form       Form;
typedef struct Label      Label;
typedef struct Button     Button;
typedef struct ProgressBar ProgressBar;
typedef struct SwitchApp  SwitchApp;

#include "switchapp.h"

extern char _cs2sx_strbuf[512];

typedef struct Input Input;
typedef struct Graphics Graphics;
typedef struct Directory Directory;
typedef struct Path Path;
typedef struct System System;
typedef struct SwitchAppEx SwitchAppEx;
typedef struct Color Color;
typedef struct Texture Texture;
typedef struct TouchApp TouchApp;
