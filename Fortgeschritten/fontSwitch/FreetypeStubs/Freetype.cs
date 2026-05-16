// Auto-generated from externLibs/freetype/include\freetype\freetype.h
// DO NOT EDIT — regeneriert via cs2sx addLib
//
// HINWEIS: Diese Datei dient NUR der IDE-Unterstützung (Roslyn-Typen).
// Der Ordner 'FreetypeStubs/' ist in ProjectReader.s_excludedDirNames
// gelistet und wird NICHT von CS2SX transpiliert.
// Die echten C-Funktionen werden direkt aus externLibs/ mitcompiliert.

#pragma warning disable CS0626, CS0649, CS0169, CS8981, CS1591

namespace Freetype;

public enum FT_Encoding
{
    FT_ENCODING_GB2312 = FT_ENCODING_PRC,
    FT_ENCODING_MS_SJIS = FT_ENCODING_SJIS,
    FT_ENCODING_MS_GB2312 = FT_ENCODING_PRC,
    FT_ENCODING_MS_BIG5 = FT_ENCODING_BIG5,
    FT_ENCODING_MS_WANSUNG = FT_ENCODING_WANSUNG,
    FT_ENCODING_MS_JOHAB = FT_ENCODING_JOHAB,
}

public enum FT_Size_Request_Type
{
    FT_SIZE_REQUEST_TYPE_NOMINAL,
    FT_SIZE_REQUEST_TYPE_REAL_DIM,
    FT_SIZE_REQUEST_TYPE_BBOX,
    FT_SIZE_REQUEST_TYPE_CELL,
    FT_SIZE_REQUEST_TYPE_SCALES,
    FT_SIZE_REQUEST_TYPE_MAX,
}

public enum FT_Render_Mode
{
    FT_RENDER_MODE_NORMAL = 0,
    FT_RENDER_MODE_LIGHT,
    FT_RENDER_MODE_MONO,
    FT_RENDER_MODE_LCD,
    FT_RENDER_MODE_LCD_V,
    FT_RENDER_MODE_SDF,
    FT_RENDER_MODE_MAX,
}

public enum FT_Kerning_Mode
{
    FT_KERNING_DEFAULT = 0,
    FT_KERNING_UNFITTED,
    FT_KERNING_UNSCALED,
}

public unsafe struct FT_Glyph_Metrics
{
    public FT_Pos width;
    public FT_Pos height;
    public FT_Pos horiBearingX;
    public FT_Pos horiBearingY;
    public FT_Pos horiAdvance;
    public FT_Pos vertBearingX;
    public FT_Pos vertBearingY;
    public FT_Pos vertAdvance;
}

public unsafe struct FT_Bitmap_Size
{
    public FT_Short height;
    public FT_Short width;
    public FT_Pos size;
    public FT_Pos x_ppem;
    public FT_Pos y_ppem;
}

public unsafe struct FT_CharMapRec
{
    public FT_Face face;
    public FT_Encoding encoding;
    public FT_UShort platform_id;
    public FT_UShort encoding_id;
}

public unsafe struct FT_FaceRec
{
    public FT_Long num_faces;
    public FT_Long face_index;
    public FT_Long face_flags;
    public FT_Long style_flags;
    public FT_Long num_glyphs;
    public IntPtr family_name;
    public IntPtr style_name;
    public FT_Int num_fixed_sizes;
    public IntPtr available_sizes;
    public FT_Int num_charmaps;
    public IntPtr charmaps;
    public FT_Generic generic;
    public FT_BBox bbox;
    public FT_UShort units_per_EM;
    public FT_Short ascender;
    public FT_Short descender;
    public FT_Short height;
    public FT_Short max_advance_width;
    public FT_Short max_advance_height;
    public FT_Short underline_position;
    public FT_Short underline_thickness;
    public FT_GlyphSlot glyph;
    public FT_Size size;
    public FT_CharMap charmap;
    public FT_Driver driver;
    public FT_Memory memory;
    public FT_Stream stream;
    public FT_ListRec sizes_list;
    public FT_Generic autohint;
    public IntPtr extensions;
    public FT_Face_Internal @internal;
}

public unsafe struct FT_Size_Metrics
{
    public FT_UShort x_ppem;
    public FT_UShort y_ppem;
    public FT_Fixed x_scale;
    public FT_Fixed y_scale;
    public FT_Pos ascender;
    public FT_Pos descender;
    public FT_Pos height;
    public FT_Pos max_advance;
}

public unsafe struct FT_SizeRec
{
    public FT_Face face;
    public FT_Generic generic;
    public FT_Size_Metrics metrics;
    public FT_Size_Internal @internal;
}

public unsafe struct FT_GlyphSlotRec
{
    public FT_Library library;
    public FT_Face face;
    public FT_GlyphSlot next;
    public FT_UInt glyph_index;
    public FT_Generic generic;
    public FT_Glyph_Metrics metrics;
    public FT_Fixed linearHoriAdvance;
    public FT_Fixed linearVertAdvance;
    public FT_Vector advance;
    public FT_Glyph_Format format;
    public FT_Bitmap bitmap;
    public FT_Int bitmap_left;
    public FT_Int bitmap_top;
    public FT_Outline outline;
    public FT_UInt num_subglyphs;
    public FT_SubGlyph subglyphs;
    public IntPtr control_data;
    public long control_len;
    public FT_Pos lsb_delta;
    public FT_Pos rsb_delta;
    public IntPtr other;
    public FT_Slot_Internal @internal;
}

public unsafe struct FT_Parameter
{
    public FT_ULong tag;
    public FT_Pointer data;
}

public unsafe struct FT_Open_Args
{
    public FT_UInt flags;
    public IntPtr memory_base;
    public FT_Long memory_size;
    public IntPtr pathname;
    public FT_Stream stream;
    public FT_Module driver;
    public FT_Int num_params;
    public IntPtr @params;
}

public unsafe struct FT_Size_RequestRec
{
    public FT_Size_Request_Type type;
    public FT_Long width;
    public FT_Long height;
    public FT_UInt horiResolution;
    public FT_UInt vertResolution;
}

