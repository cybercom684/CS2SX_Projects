// Auto-generated from externLibs/freetype/src\base\md5.h
// DO NOT EDIT — regeneriert via cs2sx addLib
//
// HINWEIS: Diese Datei dient NUR der IDE-Unterstützung (Roslyn-Typen).
// Der Ordner 'FreetypeStubs/' ist in ProjectReader.s_excludedDirNames
// gelistet und wird NICHT von CS2SX transpiliert.
// Die echten C-Funktionen werden direkt aus externLibs/ mitcompiliert.

#pragma warning disable CS0626, CS0649, CS0169, CS8981, CS1591

namespace Freetype;

public unsafe struct MD5_CTX
{
    public MD5_u32plus lo, hi;
    public MD5_u32plus a, b, c, d;
    public fixed byte buffer[64];
    // skipped array field: MD5_u32plus block[16]
}

public static class Freetype
{
    public static extern extern void MD5_Init(ref MD5_CTX ctx);
    public static extern extern void MD5_Update(ref MD5_CTX ctx, IntPtr data, ulong size);
    public static extern extern void MD5_Final(ref byte result, ref MD5_CTX ctx);
}
