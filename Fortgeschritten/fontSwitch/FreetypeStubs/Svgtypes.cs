// Auto-generated from externLibs/freetype/src\svg\svgtypes.h
// DO NOT EDIT — regeneriert via cs2sx addLib
//
// HINWEIS: Diese Datei dient NUR der IDE-Unterstützung (Roslyn-Typen).
// Der Ordner 'FreetypeStubs/' ist in ProjectReader.s_excludedDirNames
// gelistet und wird NICHT von CS2SX transpiliert.
// Die echten C-Funktionen werden direkt aus externLibs/ mitcompiliert.

#pragma warning disable CS0626, CS0649, CS0169, CS8981, CS1591

namespace Freetype;

public unsafe struct SVG_RendererRec
{
    public FT_RendererRec root;
    public FT_Bool loaded;
    public FT_Bool hooks_set;
    public SVG_RendererHooks hooks;
    public FT_Pointer state;
}

