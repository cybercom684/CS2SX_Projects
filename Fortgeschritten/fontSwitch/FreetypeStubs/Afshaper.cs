// Auto-generated from externLibs/freetype/src\autofit\afshaper.h
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
    public static extern FT_Error af_shaper_get_coverage(AF_FaceGlobals globals, AF_StyleClass style_class, ref FT_UShort gstyles, FT_Bool default_script);
    public static extern IntPtr af_shaper_buf_create(AF_FaceGlobals globals);
    public static extern void af_shaper_buf_destroy(AF_FaceGlobals globals, IntPtr buf);
    public static extern IntPtr af_shaper_get_cluster(ref byte p, AF_StyleMetrics metrics, IntPtr buf_, ref uint count);
    public static extern FT_ULong af_shaper_get_elem(AF_StyleMetrics metrics, IntPtr buf_, uint idx, ref FT_Long x_advance, ref FT_Long y_offset);
}
