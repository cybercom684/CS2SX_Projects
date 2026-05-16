// Auto-generated from externLibs/freetype/src\autofit\ft-hb-types.h
// DO NOT EDIT — regeneriert via cs2sx addLib
//
// HINWEIS: Diese Datei dient NUR der IDE-Unterstützung (Roslyn-Typen).
// Der Ordner 'FreetypeStubs/' ist in ProjectReader.s_excludedDirNames
// gelistet und wird NICHT von CS2SX transpiliert.
// Die echten C-Funktionen werden direkt aus externLibs/ mitcompiliert.

#pragma warning disable CS0626, CS0649, CS0169, CS8981, CS1591

namespace Freetype;

public enum hb_direction_t
{
    HB_DIRECTION_INVALID = 0,
    HB_DIRECTION_LTR = 4,
    HB_DIRECTION_RTL,
    HB_DIRECTION_TTB,
    HB_DIRECTION_BTT,
}

public enum hb_memory_mode_t
{
    HB_MEMORY_MODE_DUPLICATE,
    HB_MEMORY_MODE_READONLY,
    HB_MEMORY_MODE_WRITABLE,
    HB_MEMORY_MODE_READONLY_MAY_MAKE_WRITABLE,
}

public unsafe struct hb_feature_t
{
    public hb_tag_t tag;
    public uint value;
    public uint start;
    public uint end;
}

public unsafe struct hb_glyph_info_t
{
    public hb_codepoint_t codepoint;
    public hb_mask_t mask;
    public uint cluster;
    public uint var1;
    public uint var2;
}

public unsafe struct hb_glyph_position_t
{
    public hb_position_t x_advance;
    public hb_position_t y_advance;
    public hb_position_t x_offset;
    public hb_position_t y_offset;
    public uint var;
}

