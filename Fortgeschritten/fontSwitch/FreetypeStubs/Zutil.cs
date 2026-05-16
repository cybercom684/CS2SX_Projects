// Auto-generated from externLibs/freetype/src\gzip\zutil.h
// DO NOT EDIT — regeneriert via cs2sx addLib
//
// HINWEIS: Diese Datei dient NUR der IDE-Unterstützung (Roslyn-Typen).
// Der Ordner 'FreetypeStubs/' ist in ProjectReader.s_excludedDirNames
// gelistet und wird NICHT von CS2SX transpiliert.
// Die echten C-Funktionen werden direkt aus externLibs/ mitcompiliert.

#pragma warning disable CS0626, CS0649, CS0169, CS8981, CS1591

namespace Freetype;

public static class Freetype
{
    public static extern void _Cdecl farfree(IntPtr block);
    public static extern IntPtr farmalloc(ulong nbytes);
    public static extern ZEXTERN uLong ZEXPORT adler32_combine64(uLong p0, uLong p1, z_off_t p2);
    public static extern ZEXTERN uLong ZEXPORT crc32_combine64(uLong p0, uLong p1, z_off_t p2);
    public static extern ZEXTERN uLong ZEXPORT crc32_combine_gen64(z_off_t p0);
    public static extern void ZLIB_INTERNAL zmemcpy(ref Bytef dest, ref Bytef source, uInt len);
    public static extern int ZLIB_INTERNAL zmemcmp(ref Bytef s1, ref Bytef s2, uInt len);
    public static extern void ZLIB_INTERNAL zmemzero(ref Bytef dest, uInt len);
    public static extern extern void ZLIB_INTERNAL z_error(ref byte m);
    public static extern voidpf ZLIB_INTERNAL zcalloc(voidpf opaque, unsigned items, unsigned size);
    public static extern void ZLIB_INTERNAL zcfree(voidpf opaque, voidpf ptr);
}
