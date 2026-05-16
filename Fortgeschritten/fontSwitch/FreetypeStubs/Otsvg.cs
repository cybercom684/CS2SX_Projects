// Auto-generated from externLibs/freetype/include\freetype\otsvg.h
// DO NOT EDIT — regeneriert via cs2sx addLib
//
// HINWEIS: Diese Datei dient NUR der IDE-Unterstützung (Roslyn-Typen).
// Der Ordner 'FreetypeStubs/' ist in ProjectReader.s_excludedDirNames
// gelistet und wird NICHT von CS2SX transpiliert.
// Die echten C-Funktionen werden direkt aus externLibs/ mitcompiliert.

#pragma warning disable CS0626, CS0649, CS0169, CS8981, CS1591

namespace Freetype;

public unsafe struct SVG_RendererHooks
{
    public SVG_Lib_Init_Func init_svg;
    public SVG_Lib_Free_Func free_svg;
    public SVG_Lib_Render_Func render_svg;
    public SVG_Lib_Preset_Slot_Func preset_slot;
}

public unsafe struct FT_SVG_DocumentRec
{
    public IntPtr svg_document;
    public FT_ULong svg_document_length;
    public FT_Size_Metrics metrics;
    public FT_UShort units_per_EM;
    public FT_UShort start_glyph_id;
    public FT_UShort end_glyph_id;
    public FT_Matrix transform;
    public FT_Vector delta;
}

