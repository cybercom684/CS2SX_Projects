// Auto-generated from externLibs/freetype/src\bdf\bdf.h
// DO NOT EDIT — regeneriert via cs2sx addLib
//
// HINWEIS: Diese Datei dient NUR der IDE-Unterstützung (Roslyn-Typen).
// Der Ordner 'FreetypeStubs/' ist in ProjectReader.s_excludedDirNames
// gelistet und wird NICHT von CS2SX transpiliert.
// Die echten C-Funktionen werden direkt aus externLibs/ mitcompiliert.

#pragma warning disable CS0626, CS0649, CS0169, CS8981, CS1591

namespace Freetype;

public unsafe struct bdf_bbx_t
{
    public ushort width;
    public ushort height;
    public short x_offset;
    public short y_offset;
    public short ascent;
    public short descent;
}

public unsafe struct bdf_glyph_t
{
    public IntPtr name;
    public ulong encoding;
    public ushort swidth;
    public ushort dwidth;
    public bdf_bbx_t bbx;
    public IntPtr bitmap;
    public ulong bpr;
    public ushort bytes;
}

public unsafe struct bdf_font_t
{
    public IntPtr name;
    public bdf_bbx_t bbx;
    public ulong point_size;
    public ulong resolution_x;
    public ulong resolution_y;
    public int spacing;
    public ulong default_char;
    public long font_ascent;
    public long font_descent;
    public ulong glyphs_size;
    public ulong glyphs_used;
    public IntPtr glyphs;
    public ulong unencoded_size;
    public ulong unencoded_used;
    public IntPtr unencoded;
    public ulong props_size;
    public ulong props_used;
    public IntPtr props;
    public IntPtr comments;
    public ulong comments_len;
    public FT_Hash @internal;
    public ushort bpp;
    public FT_Memory memory;
    public IntPtr user_props;
    public ulong nuser_props;
    public FT_HashRec proptbl;
}

