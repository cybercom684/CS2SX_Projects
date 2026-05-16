// freetype_main.h
// Projektordner ablegen. Wird via _forward.h in jede Translation Unit includiert.
//
// DESIGN: Alle Shim-Funktionen nehmen "unsigned long long" statt "FtHandle"
// als Parameter-Typ. Das vermeidet den "incomplete type"-Fehler komplett —
// FtHandle muss zur Zeit der Shim-Definition nicht bekannt sein.
//
// In C# schreibt man: Freetype.FT_Init_FreeType(ref _library)
// Der Transpiler emittiert: Freetype_FT_Init_FreeType(&self->f_library)
// self->f_library ist vom Typ FtHandle → &self->f_library ist FtHandle*
// Wir casten intern zu unsigned long long* — das ist auf AArch64 sicher
// weil FtHandle { unsigned long long ptr } genau 8 Byte ist.

#pragma once

#include <ft2build.h>
#include FT_FREETYPE_H
#include "switchapp.h"

// FtHandle ist { unsigned long long ptr } — 8 Byte auf AArch64
// Wir lesen/schreiben ptr direkt über einen ull-Pointer
#define _FT_READ_LIB(p)  ((FT_Library)(uintptr_t)(*(unsigned long long*)(p)))
#define _FT_READ_FACE(p) ((FT_Face)(uintptr_t)(*(unsigned long long*)(p)))
#define _FT_READ_SLOT(p) ((FT_GlyphSlot)(uintptr_t)(*(unsigned long long*)(p)))
#define _FT_WRITE(p, v)  (*(unsigned long long*)(p) = (unsigned long long)(uintptr_t)(v))

// ── Init / Done ───────────────────────────────────────────────────────────────
// Parameter: Pointer auf FtHandle (= Pointer auf unsigned long long)

static inline int Freetype_FT_Init_FreeType(void* out_lib)
{
    FT_Library lib = NULL;
    int err = (int)FT_Init_FreeType(&lib);
    _FT_WRITE(out_lib, lib);
    return err;
}

static inline int Freetype_FT_New_Face(unsigned long long lib_ptr,
                                        const char* path,
                                        int index,
                                        void* out_face)
{
    FT_Library lib = (FT_Library)(uintptr_t)lib_ptr;
    FT_Face face = NULL;
    int err = (int)FT_New_Face(lib, path, index, &face);
    _FT_WRITE(out_face, face);
    return err;
}

static inline int Freetype_FT_Set_Char_Size(unsigned long long face_ptr,
                                              int w, int h,
                                              unsigned int hdpi,
                                              unsigned int vdpi)
{
    FT_Face face = (FT_Face)(uintptr_t)face_ptr;
    return (int)FT_Set_Char_Size(face, (FT_F26Dot6)w, (FT_F26Dot6)h, hdpi, vdpi);
}

static inline int Freetype_FT_Done_Face(unsigned long long face_ptr)
{
    return (int)FT_Done_Face((FT_Face)(uintptr_t)face_ptr);
}

static inline int Freetype_FT_Done_FreeType(unsigned long long lib_ptr)
{
    return (int)FT_Done_FreeType((FT_Library)(uintptr_t)lib_ptr);
}

// ── Glyph laden ───────────────────────────────────────────────────────────────

static inline unsigned int Freetype_FT_Get_Char_Index(unsigned long long face_ptr,
                                                        unsigned int c)
{
    return FT_Get_Char_Index((FT_Face)(uintptr_t)face_ptr, (FT_ULong)c);
}

static inline int Freetype_FT_Load_Glyph(unsigned long long face_ptr,
                                           unsigned int idx,
                                           int flags)
{
    return (int)FT_Load_Glyph((FT_Face)(uintptr_t)face_ptr, idx, flags);
}

static inline unsigned long long Freetype_FT_Face_GetGlyphSlot(unsigned long long face_ptr)
{
    FT_Face f = (FT_Face)(uintptr_t)face_ptr;
    return f ? (unsigned long long)(uintptr_t)f->glyph : 0ULL;
}

static inline int Freetype_FT_Render_Glyph(unsigned long long slot_ptr, int mode)
{
    return (int)FT_Render_Glyph(
        (FT_GlyphSlot)(uintptr_t)slot_ptr, (FT_Render_Mode)mode);
}

// ── Bitmap-Accessor ───────────────────────────────────────────────────────────

static inline int Freetype_FT_GlyphSlot_GetBitmapWidth(unsigned long long slot_ptr)
{
    return (int)((FT_GlyphSlot)(uintptr_t)slot_ptr)->bitmap.width;
}

static inline int Freetype_FT_GlyphSlot_GetBitmapRows(unsigned long long slot_ptr)
{
    return (int)((FT_GlyphSlot)(uintptr_t)slot_ptr)->bitmap.rows;
}

static inline int Freetype_FT_GlyphSlot_GetBitmapLeft(unsigned long long slot_ptr)
{
    return ((FT_GlyphSlot)(uintptr_t)slot_ptr)->bitmap_left;
}

static inline int Freetype_FT_GlyphSlot_GetBitmapTop(unsigned long long slot_ptr)
{
    return ((FT_GlyphSlot)(uintptr_t)slot_ptr)->bitmap_top;
}

static inline int Freetype_FT_GlyphSlot_GetAdvanceX(unsigned long long slot_ptr)
{
    return (int)((FT_GlyphSlot)(uintptr_t)slot_ptr)->advance.x;
}

// ── Pixel-Loop in C ───────────────────────────────────────────────────────────

static inline void Freetype_DrawGlyphToFramebuffer(
    unsigned long long slot_ptr, int width, int rows,
    int destX, int destY, unsigned int color)
{
    FT_GlyphSlot s = (FT_GlyphSlot)(uintptr_t)slot_ptr;
    if (!s || !s->bitmap.buffer || width <= 0 || rows <= 0) return;

    unsigned char* pixels = s->bitmap.buffer;
    for (int row = 0; row < rows; row++)
    {
        for (int col = 0; col < width; col++)
        {
            unsigned char alpha = pixels[row * width + col];
            if (!alpha) continue;
            int px = destX + col;
            int py = destY + row;
            if (alpha == 255)
                Graphics_SetPixel(px, py, color);
            else
                Graphics_SetPixelAlpha(px, py, color, alpha);
        }
    }
}