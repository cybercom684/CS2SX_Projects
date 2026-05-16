// Auto-generated from externLibs/freetype/include\dlg\dlg.h
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
    public static extern DLG_API void dlg__do_log(enum dlg_level lvl, ref byte p0, ref byte p1, int p2, ref byte p3, ref byte p4, ref byte p5);
    public static extern IntPtr dlg__strip_root_path(ref byte file, ref byte @base);
    public static extern DLG_API void dlg_set_handler(dlg_handler handler, IntPtr data);
    public static extern DLG_API void dlg_default_output(ref struct dlg_origin, ref byte @string, IntPtr p0);
    public static extern DLG_API dlg_handler dlg_get_handler(IntPtr data);
    public static extern DLG_API void dlg_add_tag(ref byte tag, ref byte func);
    public static extern DLG_API bool dlg_remove_tag(ref byte tag, ref byte func);
    public static extern IntPtr dlg_thread_buffer(ref ulong size);
}
