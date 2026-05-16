// Auto-generated from externLibs/freetype/src\raster\ftmisc.h
// DO NOT EDIT — regeneriert via cs2sx addLib
//
// HINWEIS: Diese Datei dient NUR der IDE-Unterstützung (Roslyn-Typen).
// Der Ordner 'FreetypeStubs/' ist in ProjectReader.s_excludedDirNames
// gelistet und wird NICHT von CS2SX transpiliert.
// Die echten C-Funktionen werden direkt aus externLibs/ mitcompiliert.

#pragma warning disable CS0626, CS0649, CS0169, CS8981, CS1591

namespace Freetype;

public unsafe struct FT_MemoryRec
{
    public IntPtr user;
    public FT_Alloc_Func alloc;
    public FT_Free_Func free;
    public FT_Realloc_Func realloc;
}

