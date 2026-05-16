// Auto-generated from externLibs/freetype/include\freetype\ftglyph.h
// DO NOT EDIT — regeneriert via cs2sx addLib
//
// HINWEIS: Diese Datei dient NUR der IDE-Unterstützung (Roslyn-Typen).
// Der Ordner 'FreetypeStubs/' ist in ProjectReader.s_excludedDirNames
// gelistet und wird NICHT von CS2SX transpiliert.
// Die echten C-Funktionen werden direkt aus externLibs/ mitcompiliert.

#pragma warning disable CS0626, CS0649, CS0169, CS8981, CS1591

namespace Freetype;

public enum FT_Glyph_BBox_Mode
{
    FT_GLYPH_BBOX_UNSCALED = 0,
    FT_GLYPH_BBOX_SUBPIXELS = 0,
    FT_GLYPH_BBOX_GRIDFIT = 1,
    FT_GLYPH_BBOX_TRUNCATE = 2,
    FT_GLYPH_BBOX_PIXELS = 3,
}

public unsafe struct FT_GlyphRec
{
    public FT_Library library;
    public IntPtr clazz;
    public FT_Glyph_Format format;
    public FT_Vector advance;
}

public unsafe struct FT_BitmapGlyphRec
{
    public FT_GlyphRec root;
    public FT_Int left;
    public FT_Int top;
    public FT_Bitmap bitmap;
}

public unsafe struct FT_OutlineGlyphRec
{
    public FT_GlyphRec root;
    public FT_Outline outline;
}

public unsafe struct FT_SvgGlyphRec
{
    public FT_GlyphRec root;
    public IntPtr svg_document;
    public FT_ULong svg_document_length;
    public FT_UInt glyph_index;
    public FT_Size_Metrics metrics;
    public FT_UShort units_per_EM;
    public FT_UShort start_glyph_id;
    public FT_UShort end_glyph_id;
    public FT_Matrix transform;
    public FT_Vector delta;
}

