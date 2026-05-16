// Auto-generated from externLibs/freetype/src\gzip\inftrees.h
// DO NOT EDIT — regeneriert via cs2sx addLib
//
// HINWEIS: Diese Datei dient NUR der IDE-Unterstützung (Roslyn-Typen).
// Der Ordner 'FreetypeStubs/' ist in ProjectReader.s_excludedDirNames
// gelistet und wird NICHT von CS2SX transpiliert.
// Die echten C-Funktionen werden direkt aus externLibs/ mitcompiliert.

#pragma warning disable CS0626, CS0649, CS0169, CS8981, CS1591

namespace Freetype;

public enum codetype
{
    CODES,
    LENS,
    DISTS,
}

public unsafe struct code
{
    public byte op;
    public byte bits;
    public ushort val;
}

public static class Freetype
{
    public static extern static int ZLIB_INTERNAL inflate_table(codetype type, ref unsigned short FAR lens, unsigned codes, ref code FAR FAR table, ref unsigned FAR bits, ref unsigned short FAR work);
}
