// Auto-generated from externLibs/freetype/include\freetype\ftcolor.h
// DO NOT EDIT — regeneriert via cs2sx addLib
//
// HINWEIS: Diese Datei dient NUR der IDE-Unterstützung (Roslyn-Typen).
// Der Ordner 'FreetypeStubs/' ist in ProjectReader.s_excludedDirNames
// gelistet und wird NICHT von CS2SX transpiliert.
// Die echten C-Funktionen werden direkt aus externLibs/ mitcompiliert.

#pragma warning disable CS0626, CS0649, CS0169, CS8981, CS1591

namespace Freetype;

public enum FT_PaintFormat
{
    FT_COLR_PAINTFORMAT_COLR_LAYERS = 1,
    FT_COLR_PAINTFORMAT_SOLID = 2,
    FT_COLR_PAINTFORMAT_LINEAR_GRADIENT = 4,
    FT_COLR_PAINTFORMAT_RADIAL_GRADIENT = 6,
    FT_COLR_PAINTFORMAT_SWEEP_GRADIENT = 8,
    FT_COLR_PAINTFORMAT_GLYPH = 10,
    FT_COLR_PAINTFORMAT_COLR_GLYPH = 11,
    FT_COLR_PAINTFORMAT_TRANSFORM = 12,
    FT_COLR_PAINTFORMAT_TRANSLATE = 14,
    FT_COLR_PAINTFORMAT_SCALE = 16,
    FT_COLR_PAINTFORMAT_ROTATE = 24,
    FT_COLR_PAINTFORMAT_SKEW = 28,
    FT_COLR_PAINTFORMAT_COMPOSITE = 32,
    FT_COLR_PAINT_FORMAT_MAX = 33,
    FT_COLR_PAINTFORMAT_UNSUPPORTED = 255,
}

public enum FT_PaintExtend
{
    FT_COLR_PAINT_EXTEND_PAD = 0,
    FT_COLR_PAINT_EXTEND_REPEAT = 1,
    FT_COLR_PAINT_EXTEND_REFLECT = 2,
}

public enum FT_Composite_Mode
{
    FT_COLR_COMPOSITE_CLEAR = 0,
    FT_COLR_COMPOSITE_SRC = 1,
    FT_COLR_COMPOSITE_DEST = 2,
    FT_COLR_COMPOSITE_SRC_OVER = 3,
    FT_COLR_COMPOSITE_DEST_OVER = 4,
    FT_COLR_COMPOSITE_SRC_IN = 5,
    FT_COLR_COMPOSITE_DEST_IN = 6,
    FT_COLR_COMPOSITE_SRC_OUT = 7,
    FT_COLR_COMPOSITE_DEST_OUT = 8,
    FT_COLR_COMPOSITE_SRC_ATOP = 9,
    FT_COLR_COMPOSITE_DEST_ATOP = 10,
    FT_COLR_COMPOSITE_XOR = 11,
    FT_COLR_COMPOSITE_PLUS = 12,
    FT_COLR_COMPOSITE_SCREEN = 13,
    FT_COLR_COMPOSITE_OVERLAY = 14,
    FT_COLR_COMPOSITE_DARKEN = 15,
    FT_COLR_COMPOSITE_LIGHTEN = 16,
    FT_COLR_COMPOSITE_COLOR_DODGE = 17,
    FT_COLR_COMPOSITE_COLOR_BURN = 18,
    FT_COLR_COMPOSITE_HARD_LIGHT = 19,
    FT_COLR_COMPOSITE_SOFT_LIGHT = 20,
    FT_COLR_COMPOSITE_DIFFERENCE = 21,
    FT_COLR_COMPOSITE_EXCLUSION = 22,
    FT_COLR_COMPOSITE_MULTIPLY = 23,
    FT_COLR_COMPOSITE_HSL_HUE = 24,
    FT_COLR_COMPOSITE_HSL_SATURATION = 25,
    FT_COLR_COMPOSITE_HSL_COLOR = 26,
    FT_COLR_COMPOSITE_HSL_LUMINOSITY = 27,
    FT_COLR_COMPOSITE_MAX = 28,
}

public enum FT_Color_Root_Transform
{
    FT_COLOR_INCLUDE_ROOT_TRANSFORM,
    FT_COLOR_NO_ROOT_TRANSFORM,
    FT_COLOR_ROOT_TRANSFORM_MAX,
}

public unsafe struct FT_Color
{
    public FT_Byte blue;
    public FT_Byte green;
    public FT_Byte red;
    public FT_Byte alpha;
}

public unsafe struct FT_Palette_Data
{
    public FT_UShort num_palettes;
    public IntPtr palette_name_ids;
    public IntPtr palette_flags;
    public FT_UShort num_palette_entries;
    public IntPtr palette_entry_name_ids;
}

public unsafe struct FT_LayerIterator
{
    public FT_UInt num_layers;
    public FT_UInt layer;
    public IntPtr p;
}

public unsafe struct FT_ColorStopIterator
{
    public FT_UInt num_color_stops;
    public FT_UInt current_color_stop;
    public IntPtr p;
    public FT_Bool read_variable;
}

public unsafe struct FT_ColorIndex
{
    public FT_UInt16 palette_index;
    public FT_F2Dot14 alpha;
}

public unsafe struct FT_ColorStop
{
    public FT_Fixed stop_offset;
    public FT_ColorIndex color;
}

public unsafe struct FT_ColorLine
{
    public FT_PaintExtend extend;
    public FT_ColorStopIterator color_stop_iterator;
}

public unsafe struct FT_Affine23
{
    public FT_Fixed xx, xy, dx;
    public FT_Fixed yx, yy, dy;
}

public unsafe struct FT_OpaquePaint
{
    public IntPtr p;
    public FT_Bool insert_root_transform;
}

public unsafe struct FT_PaintColrLayers
{
    public FT_LayerIterator layer_iterator;
}

public unsafe struct FT_PaintSolid
{
    public FT_ColorIndex color;
}

public unsafe struct FT_PaintLinearGradient
{
    public FT_ColorLine colorline;
    public FT_Vector p0;
    public FT_Vector p1;
    public FT_Vector p2;
}

public unsafe struct FT_PaintRadialGradient
{
    public FT_ColorLine colorline;
    public FT_Vector c0;
    public FT_Pos r0;
    public FT_Vector c1;
    public FT_Pos r1;
}

public unsafe struct FT_PaintSweepGradient
{
    public FT_ColorLine colorline;
    public FT_Vector center;
    public FT_Fixed start_angle;
    public FT_Fixed end_angle;
}

public unsafe struct FT_PaintGlyph
{
    public FT_OpaquePaint paint;
    public FT_UInt glyphID;
}

public unsafe struct FT_PaintColrGlyph
{
    public FT_UInt glyphID;
}

public unsafe struct FT_PaintTransform
{
    public FT_OpaquePaint paint;
    public FT_Affine23 affine;
}

public unsafe struct FT_PaintTranslate
{
    public FT_OpaquePaint paint;
    public FT_Fixed dx;
    public FT_Fixed dy;
}

public unsafe struct FT_PaintScale
{
    public FT_OpaquePaint paint;
    public FT_Fixed scale_x;
    public FT_Fixed scale_y;
    public FT_Fixed center_x;
    public FT_Fixed center_y;
}

public unsafe struct FT_PaintRotate
{
    public FT_OpaquePaint paint;
    public FT_Fixed angle;
    public FT_Fixed center_x;
    public FT_Fixed center_y;
}

public unsafe struct FT_PaintSkew
{
    public FT_OpaquePaint paint;
    public FT_Fixed x_skew_angle;
    public FT_Fixed y_skew_angle;
    public FT_Fixed center_x;
    public FT_Fixed center_y;
}

public unsafe struct FT_PaintComposite
{
    public FT_OpaquePaint source_paint;
    public FT_Composite_Mode composite_mode;
    public FT_OpaquePaint backdrop_paint;
}

public unsafe struct FT_ClipBox
{
    public FT_Vector bottom_left;
    public FT_Vector top_left;
    public FT_Vector top_right;
    public FT_Vector bottom_right;
}

