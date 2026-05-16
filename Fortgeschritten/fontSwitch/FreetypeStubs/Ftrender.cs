// Auto-generated from externLibs/freetype/include\freetype\ftrender.h
// DO NOT EDIT — regeneriert via cs2sx addLib
//
// HINWEIS: Diese Datei dient NUR der IDE-Unterstützung (Roslyn-Typen).
// Der Ordner 'FreetypeStubs/' ist in ProjectReader.s_excludedDirNames
// gelistet und wird NICHT von CS2SX transpiliert.
// Die echten C-Funktionen werden direkt aus externLibs/ mitcompiliert.

#pragma warning disable CS0626, CS0649, CS0169, CS8981, CS1591

namespace Freetype;

public unsafe struct FT_Renderer_Class
{
    public FT_Module_Class root;
    public FT_Glyph_Format glyph_format;
    public FT_Renderer_RenderFunc render_glyph;
    public FT_Renderer_TransformFunc transform_glyph;
    public FT_Renderer_GetCBoxFunc get_glyph_cbox;
    public FT_Renderer_SetModeFunc set_mode;
    public IntPtr raster_class;
}

