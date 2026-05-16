// Auto-generated from externLibs/freetype/src\gzip\gzguts.h
// DO NOT EDIT — regeneriert via cs2sx addLib
//
// HINWEIS: Diese Datei dient NUR der IDE-Unterstützung (Roslyn-Typen).
// Der Ordner 'FreetypeStubs/' ist in ProjectReader.s_excludedDirNames
// gelistet und wird NICHT von CS2SX transpiliert.
// Die echten C-Funktionen werden direkt aus externLibs/ mitcompiliert.

#pragma warning disable CS0626, CS0649, CS0169, CS8981, CS1591

namespace Freetype;

public unsafe struct gz_state
{
    public int mode;
    public int fd;
    public IntPtr path;
    public unsigned size;
    public unsigned want;
    public IntPtr @in;
    public IntPtr @out;
    public int direct;
    public int how;
    public z_off64_t start;
    public int eof;
    public int past;
    public int level;
    public int strategy;
    public int reset;
    public z_off64_t skip;
    public int seek;
    public int err;
    public IntPtr msg;
    public z_stream strm;
}

public static class Freetype
{
    public static extern extern voidp malloc(uInt size);
    public static extern extern void free(voidpf ptr);
    public static extern ZEXTERN gzFile ZEXPORT gzopen64(ref byte p0, ref byte p1);
    public static extern ZEXTERN z_off64_t ZEXPORT gzseek64(gzFile p0, z_off64_t p1, int p2);
    public static extern ZEXTERN z_off64_t ZEXPORT gztell64(gzFile p0);
    public static extern ZEXTERN z_off64_t ZEXPORT gzoffset64(gzFile p0);
    public static extern void ZLIB_INTERNAL gz_error(gz_statep p0, int p1, ref byte p2);
    public static extern unsigned ZLIB_INTERNAL gz_intmax();
}
