// Auto-generated from externLibs/freetype/src\psaux\psglue.h
// DO NOT EDIT — regeneriert via cs2sx addLib
//
// HINWEIS: Diese Datei dient NUR der IDE-Unterstützung (Roslyn-Typen).
// Der Ordner 'FreetypeStubs/' ist in ProjectReader.s_excludedDirNames
// gelistet und wird NICHT von CS2SX transpiliert.
// Die echten C-Funktionen werden direkt aus externLibs/ mitcompiliert.

#pragma warning disable CS0626, CS0649, CS0169, CS8981, CS1591

namespace Freetype;

public enum CF2_PathOp
{
    CF2_PathOpMoveTo = 1,
    CF2_PathOpLineTo = 2,
    CF2_PathOpQuadTo = 3,
    CF2_PathOpCubeTo = 4,
}

public unsafe struct CF2_Matrix
{
    public CF2_F16Dot16 a;
    public CF2_F16Dot16 b;
    public CF2_F16Dot16 c;
    public CF2_F16Dot16 d;
    public CF2_F16Dot16 tx;
    public CF2_F16Dot16 ty;
}

