// Auto-generated from externLibs/freetype/src\type1\t1objs.h
// DO NOT EDIT — regeneriert via cs2sx addLib
//
// HINWEIS: Diese Datei dient NUR der IDE-Unterstützung (Roslyn-Typen).
// Der Ordner 'FreetypeStubs/' ist in ProjectReader.s_excludedDirNames
// gelistet und wird NICHT von CS2SX transpiliert.
// Die echten C-Funktionen werden direkt aus externLibs/ mitcompiliert.

#pragma warning disable CS0626, CS0649, CS0169, CS8981, CS1591

namespace Freetype;

public unsafe struct T1_SizeRec
{
    public FT_SizeRec root;
}

public unsafe struct T1_GlyphSlotRec
{
    public FT_GlyphSlotRec root;
    public FT_Bool hint;
    public FT_Bool scaled;
    public FT_Fixed x_scale;
    public FT_Fixed y_scale;
    public FT_Int max_points;
    public FT_Int max_contours;
}

