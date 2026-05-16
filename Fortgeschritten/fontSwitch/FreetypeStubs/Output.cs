// Auto-generated from externLibs/freetype/include\dlg\output.h
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
    public static extern DLG_API int dlg_vfprintf(ref FILE stream, ref byte format, va_list list);
    public static extern DLG_API void dlg_generic_output(dlg_generic_output_handler output, IntPtr data, uint features, ref struct dlg_origin origin, ref byte @string, struct dlg_style styles);
    public static extern DLG_API void dlg_generic_outputf(dlg_generic_output_handler output, IntPtr data, ref byte format_string, ref struct dlg_origin origin, ref byte @string, struct dlg_style styles);
    public static extern DLG_API void dlg_generic_output_stream(ref FILE stream, uint features, ref struct dlg_origin origin, ref byte @string, struct dlg_style styles);
    public static extern DLG_API void dlg_generic_outputf_stream(ref FILE stream, ref byte format_string, ref struct dlg_origin origin, ref byte @string, struct dlg_style styles, bool lock_stream);
    public static extern DLG_API void dlg_generic_output_buf(ref byte buf, ref ulong size, uint features, ref struct dlg_origin origin, ref byte @string, struct dlg_style styles);
    public static extern DLG_API void dlg_generic_outputf_buf(ref byte buf, ref ulong size, ref byte format_string, ref struct dlg_origin origin, ref byte @string, struct dlg_style styles);
    public static extern DLG_API bool dlg_is_tty(ref FILE stream);
    public static extern DLG_API void dlg_escape_sequence(struct dlg_style style, byte buf);
    public static extern DLG_API bool dlg_win_init_ansi();
}
