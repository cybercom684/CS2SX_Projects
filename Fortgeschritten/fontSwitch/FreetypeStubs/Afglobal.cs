// Auto-generated from externLibs/freetype/src\autofit\afglobal.h
// DO NOT EDIT — regeneriert via cs2sx addLib
//
// HINWEIS: Diese Datei dient NUR der IDE-Unterstützung (Roslyn-Typen).
// Der Ordner 'FreetypeStubs/' ist in ProjectReader.s_excludedDirNames
// gelistet und wird NICHT von CS2SX transpiliert.
// Die echten C-Funktionen werden direkt aus externLibs/ mitcompiliert.

#pragma warning disable CS0626, CS0649, CS0169, CS8981, CS1591

namespace Freetype;

public unsafe struct AF_FaceGlobalsRec
{
    public FT_Face face;
    public FT_UInt glyph_count;
    public IntPtr glyph_styles;
    public IntPtr hb_font;
    public IntPtr hb_buf;
    public IntPtr gsub;
    public FT_UShort gsub_lookup_count;
    public IntPtr gsub_lookups_single_alternate;
    public FT_UInt increase_x_height;
    public AF_StyleMetrics metrics;
    public FT_UShort stem_darkening_for_ppem;
    public FT_Pos standard_vertical_width;
    public FT_Pos standard_horizontal_width;
    public FT_Pos darken_x;
    public FT_Pos darken_y;
    public FT_Fixed scale_down_factor;
    public AF_Module module;
}

