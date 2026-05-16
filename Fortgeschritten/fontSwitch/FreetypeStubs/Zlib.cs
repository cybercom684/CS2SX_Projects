// Auto-generated from externLibs/freetype/src\gzip\zlib.h
// DO NOT EDIT — regeneriert via cs2sx addLib
//
// HINWEIS: Diese Datei dient NUR der IDE-Unterstützung (Roslyn-Typen).
// Der Ordner 'FreetypeStubs/' ist in ProjectReader.s_excludedDirNames
// gelistet und wird NICHT von CS2SX transpiliert.
// Die echten C-Funktionen werden direkt aus externLibs/ mitcompiliert.

#pragma warning disable CS0626, CS0649, CS0169, CS8981, CS1591

namespace Freetype;

public unsafe struct z_stream
{
    public IntPtr next_in;
    public uInt avail_in;
    public uLong total_in;
    public IntPtr next_out;
    public uInt avail_out;
    public uLong total_out;
    public IntPtr msg;
    public alloc_func zalloc;
    public free_func zfree;
    public voidpf opaque;
    public int data_type;
    public uLong adler;
    public uLong reserved;
}

public unsafe struct gz_header
{
    public int text;
    public uLong time;
    public int xflags;
    public int os;
    public IntPtr extra;
    public uInt extra_len;
    public uInt extra_max;
    public IntPtr name;
    public uInt name_max;
    public IntPtr comment;
    public uInt comm_max;
    public int hcrc;
    public int done;
}

public static class Freetype
{
    public static extern IntPtr zlibVersion();
    public static extern ZEXTERN int ZEXPORT deflate(z_streamp strm, int flush);
    public static extern ZEXTERN int ZEXPORT deflateEnd(z_streamp strm);
    public static extern ZEXTERN int ZEXPORT inflate(z_streamp strm, int flush);
    public static extern ZEXTERN int ZEXPORT inflateEnd(z_streamp strm);
    public static extern ZEXTERN int ZEXPORT deflateSetDictionary(z_streamp strm, ref Bytef dictionary, uInt dictLength);
    public static extern ZEXTERN int ZEXPORT deflateGetDictionary(z_streamp strm, ref Bytef dictionary, ref uInt dictLength);
    public static extern ZEXTERN int ZEXPORT deflateCopy(z_streamp dest, z_streamp source);
    public static extern ZEXTERN int ZEXPORT deflateReset(z_streamp strm);
    public static extern ZEXTERN int ZEXPORT deflateParams(z_streamp strm, int level, int strategy);
    public static extern ZEXTERN int ZEXPORT deflateTune(z_streamp strm, int good_length, int max_lazy, int nice_length, int max_chain);
    public static extern ZEXTERN uLong ZEXPORT deflateBound(z_streamp strm, uLong sourceLen);
    public static extern ZEXTERN int ZEXPORT deflatePending(z_streamp strm, ref unsigned pending, ref int bits);
    public static extern ZEXTERN int ZEXPORT deflatePrime(z_streamp strm, int bits, int value);
    public static extern ZEXTERN int ZEXPORT deflateSetHeader(z_streamp strm, gz_headerp head);
    public static extern ZEXTERN int ZEXPORT inflateSetDictionary(z_streamp strm, ref Bytef dictionary, uInt dictLength);
    public static extern ZEXTERN int ZEXPORT inflateGetDictionary(z_streamp strm, ref Bytef dictionary, ref uInt dictLength);
    public static extern ZEXTERN int ZEXPORT inflateSync(z_streamp strm);
    public static extern ZEXTERN int ZEXPORT inflateCopy(z_streamp dest, z_streamp source);
    public static extern ZEXTERN int ZEXPORT inflateReset(z_streamp strm);
    public static extern ZEXTERN int ZEXPORT inflateReset2(z_streamp strm, int windowBits);
    public static extern ZEXTERN int ZEXPORT inflatePrime(z_streamp strm, int bits, int value);
    public static extern ZEXTERN long ZEXPORT inflateMark(z_streamp strm);
    public static extern ZEXTERN int ZEXPORT inflateGetHeader(z_streamp strm, gz_headerp head);
    public static extern ZEXTERN int ZEXPORT inflateBack(z_streamp strm, in_func @in, ref void FAR in_desc, out_func @out, ref void FAR out_desc);
    public static extern ZEXTERN int ZEXPORT inflateBackEnd(z_streamp strm);
    public static extern ZEXTERN uLong ZEXPORT zlibCompileFlags();
    public static extern ZEXTERN int ZEXPORT compress(ref Bytef dest, ref uLongf destLen, ref Bytef source, uLong sourceLen);
    public static extern ZEXTERN int ZEXPORT compress2(ref Bytef dest, ref uLongf destLen, ref Bytef source, uLong sourceLen, int level);
    public static extern ZEXTERN uLong ZEXPORT compressBound(uLong sourceLen);
    public static extern ZEXTERN int ZEXPORT uncompress(ref Bytef dest, ref uLongf destLen, ref Bytef source, uLong sourceLen);
    public static extern ZEXTERN int ZEXPORT uncompress2(ref Bytef dest, ref uLongf destLen, ref Bytef source, ref uLong sourceLen);
    public static extern ZEXTERN gzFile ZEXPORT gzdopen(int fd, ref byte mode);
    public static extern ZEXTERN int ZEXPORT gzbuffer(gzFile file, unsigned size);
    public static extern ZEXTERN int ZEXPORT gzsetparams(gzFile file, int level, int strategy);
    public static extern ZEXTERN int ZEXPORT gzread(gzFile file, voidp buf, unsigned len);
    public static extern ZEXTERN z_size_t ZEXPORT gzfread(voidp buf, z_size_t size, z_size_t nitems, gzFile file);
    public static extern ZEXTERN int ZEXPORT gzwrite(gzFile file, voidpc buf, unsigned len);
    public static extern ZEXTERN z_size_t ZEXPORT gzfwrite(voidpc buf, z_size_t size, z_size_t nitems, gzFile file);
    public static extern ZEXTERN int ZEXPORTVA gzprintf(gzFile file, ref byte format, ... p0);
    public static extern ZEXTERN int ZEXPORT gzputs(gzFile file, ref byte s);
    public static extern IntPtr gzgets(gzFile file, ref byte buf, int len);
    public static extern ZEXTERN int ZEXPORT gzputc(gzFile file, int c);
    public static extern ZEXTERN int ZEXPORT gzgetc(gzFile file);
    public static extern ZEXTERN int ZEXPORT gzungetc(int c, gzFile file);
    public static extern ZEXTERN int ZEXPORT gzflush(gzFile file, int flush);
    public static extern ZEXTERN int ZEXPORT gzrewind(gzFile file);
    public static extern ZEXTERN int ZEXPORT gzeof(gzFile file);
    public static extern ZEXTERN int ZEXPORT gzdirect(gzFile file);
    public static extern ZEXTERN int ZEXPORT gzclose(gzFile file);
    public static extern ZEXTERN int ZEXPORT gzclose_r(gzFile file);
    public static extern ZEXTERN int ZEXPORT gzclose_w(gzFile file);
    public static extern IntPtr gzerror(gzFile file, ref int errnum);
    public static extern ZEXTERN void ZEXPORT gzclearerr(gzFile file);
    public static extern ZEXTERN uLong ZEXPORT adler32(uLong adler, ref Bytef buf, uInt len);
    public static extern ZEXTERN uLong ZEXPORT adler32_z(uLong adler, ref Bytef buf, z_size_t len);
    public static extern ZEXTERN uLong ZEXPORT crc32(uLong crc, ref Bytef buf, uInt len);
    public static extern ZEXTERN uLong ZEXPORT crc32_z(uLong crc, ref Bytef buf, z_size_t len);
    public static extern ZEXTERN uLong ZEXPORT crc32_combine_op(uLong crc1, uLong crc2, uLong op);
    public static extern ZEXTERN int ZEXPORT deflateInit_(z_streamp strm, int level, ref byte version, int stream_size);
    public static extern ZEXTERN int ZEXPORT inflateInit_(z_streamp strm, ref byte version, int stream_size);
    public static extern ZEXTERN int ZEXPORT deflateInit2_(z_streamp strm, int level, int method, int windowBits, int memLevel, int strategy, ref byte version, int stream_size);
    public static extern ZEXTERN int ZEXPORT inflateInit2_(z_streamp strm, int windowBits, ref byte version, int stream_size);
    public static extern ZEXTERN int ZEXPORT inflateBackInit_(z_streamp strm, int windowBits, ref unsigned char FAR window, ref byte version, int stream_size);
    public static extern ZEXTERN int ZEXPORT inflateInit2_(z_streamp strm, int windowBits, ref byte version, int stream_size);
    public static extern ZEXTERN int ZEXPORT gzgetc_(gzFile file);
    public static extern ZEXTERN gzFile ZEXPORT gzopen64(ref byte p0, ref byte p1);
    public static extern ZEXTERN z_off64_t ZEXPORT gzseek64(gzFile p0, z_off64_t p1, int p2);
    public static extern ZEXTERN z_off64_t ZEXPORT gztell64(gzFile p0);
    public static extern ZEXTERN z_off64_t ZEXPORT gzoffset64(gzFile p0);
    public static extern ZEXTERN uLong ZEXPORT adler32_combine64(uLong p0, uLong p1, z_off64_t p2);
    public static extern ZEXTERN uLong ZEXPORT crc32_combine64(uLong p0, uLong p1, z_off64_t p2);
    public static extern ZEXTERN uLong ZEXPORT crc32_combine_gen64(z_off64_t p0);
    public static extern ZEXTERN gzFile ZEXPORT gzopen64(ref byte p0, ref byte p1);
    public static extern ZEXTERN z_off_t ZEXPORT gzseek64(gzFile p0, z_off_t p1, int p2);
    public static extern ZEXTERN z_off_t ZEXPORT gztell64(gzFile p0);
    public static extern ZEXTERN z_off_t ZEXPORT gzoffset64(gzFile p0);
    public static extern ZEXTERN uLong ZEXPORT adler32_combine64(uLong p0, uLong p1, z_off_t p2);
    public static extern ZEXTERN uLong ZEXPORT crc32_combine64(uLong p0, uLong p1, z_off_t p2);
    public static extern ZEXTERN uLong ZEXPORT crc32_combine_gen64(z_off_t p0);
    public static extern ZEXTERN gzFile ZEXPORT gzopen(ref byte p0, ref byte p1);
    public static extern ZEXTERN z_off_t ZEXPORT gzseek(gzFile p0, z_off_t p1, int p2);
    public static extern ZEXTERN z_off_t ZEXPORT gztell(gzFile p0);
    public static extern ZEXTERN z_off_t ZEXPORT gzoffset(gzFile p0);
    public static extern ZEXTERN uLong ZEXPORT adler32_combine(uLong p0, uLong p1, z_off_t p2);
    public static extern ZEXTERN uLong ZEXPORT crc32_combine(uLong p0, uLong p1, z_off_t p2);
    public static extern ZEXTERN uLong ZEXPORT crc32_combine_gen(z_off_t p0);
    public static extern ZEXTERN uLong ZEXPORT adler32_combine(uLong p0, uLong p1, z_off_t p2);
    public static extern ZEXTERN uLong ZEXPORT crc32_combine(uLong p0, uLong p1, z_off_t p2);
    public static extern ZEXTERN uLong ZEXPORT crc32_combine_gen(z_off_t p0);
    public static extern IntPtr zError(int p0);
    public static extern ZEXTERN int            ZEXPORT inflateSyncPoint(z_streamp p0);
    public static extern IntPtr get_crc_table();
    public static extern ZEXTERN int            ZEXPORT inflateUndermine(z_streamp p0, int p1);
    public static extern ZEXTERN int            ZEXPORT inflateValidate(z_streamp p0, int p1);
    public static extern ZEXTERN unsigned long  ZEXPORT inflateCodesUsed(z_streamp p0);
    public static extern ZEXTERN int            ZEXPORT inflateResetKeep(z_streamp p0);
    public static extern ZEXTERN int            ZEXPORT deflateResetKeep(z_streamp p0);
    public static extern ZEXTERN gzFile         ZEXPORT gzopen_w(ref wchar_t path, ref byte mode);
    public static extern ZEXTERN int            ZEXPORTVA gzvprintf(gzFile file, ref byte format, va_list va);
}
