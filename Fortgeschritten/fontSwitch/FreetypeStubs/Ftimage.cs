// Auto-generated from externLibs/freetype/include\freetype\ftimage.h
// DO NOT EDIT — regeneriert via cs2sx addLib
//
// HINWEIS: Diese Datei dient NUR der IDE-Unterstützung (Roslyn-Typen).
// Der Ordner 'FreetypeStubs/' ist in ProjectReader.s_excludedDirNames
// gelistet und wird NICHT von CS2SX transpiliert.
// Die echten C-Funktionen werden direkt aus externLibs/ mitcompiliert.

#pragma warning disable CS0626, CS0649, CS0169, CS8981, CS1591

namespace Freetype;

public enum FT_Pixel_Mode
{
    FT_PIXEL_MODE_NONE = 0,
    FT_PIXEL_MODE_MONO,
    FT_PIXEL_MODE_GRAY,
    FT_PIXEL_MODE_GRAY2,
    FT_PIXEL_MODE_GRAY4,
    FT_PIXEL_MODE_LCD,
    FT_PIXEL_MODE_LCD_V,
    FT_PIXEL_MODE_BGRA,
    FT_PIXEL_MODE_MAX,
}

public enum FT_Glyph_Format
{
}

public unsafe struct FT_Vector
{
    public FT_Pos x;
    public FT_Pos y;
}

public unsafe struct FT_BBox
{
    public FT_Pos xMin, yMin;
    public FT_Pos xMax, yMax;
}

public unsafe struct FT_Bitmap
{
    public uint rows;
    public uint width;
    public int pitch;
    public IntPtr buffer;
    public ushort num_grays;
    public byte pixel_mode;
    public byte palette_mode;
    public IntPtr palette;
}

public unsafe struct FT_Outline
{
    public ushort n_contours;
    public ushort n_points;
    public IntPtr points;
    public IntPtr tags;
    public IntPtr contours;
    public int flags;
}

public unsafe struct FT_Outline_Funcs
{
    public FT_Outline_MoveToFunc move_to;
    public FT_Outline_LineToFunc line_to;
    public FT_Outline_ConicToFunc conic_to;
    public FT_Outline_CubicToFunc cubic_to;
    public int shift;
    public FT_Pos delta;
}

public unsafe struct FT_Span
{
    public ushort x;
    public ushort len;
    public byte coverage;
}

public unsafe struct FT_Raster_Params
{
    public IntPtr target;
    public IntPtr source;
    public int flags;
    public FT_SpanFunc gray_spans;
    public FT_SpanFunc black_spans;
    public FT_Raster_BitTest_Func bit_test;
    public FT_Raster_BitSet_Func bit_set;
    public IntPtr user;
    public FT_BBox clip_box;
}

public unsafe struct FT_Raster_Funcs
{
    public FT_Glyph_Format glyph_format;
    public FT_Raster_NewFunc raster_new;
    public FT_Raster_ResetFunc raster_reset;
    public FT_Raster_SetModeFunc raster_set_mode;
    public FT_Raster_RenderFunc raster_render;
    public FT_Raster_DoneFunc raster_done;
}

