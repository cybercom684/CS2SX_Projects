// Auto-generated from externLibs/freetype/include\freetype\internal\ftobjs.h
// DO NOT EDIT — regeneriert via cs2sx addLib
//
// HINWEIS: Diese Datei dient NUR der IDE-Unterstützung (Roslyn-Typen).
// Der Ordner 'FreetypeStubs/' ist in ProjectReader.s_excludedDirNames
// gelistet und wird NICHT von CS2SX transpiliert.
// Die echten C-Funktionen werden direkt aus externLibs/ mitcompiliert.

#pragma warning disable CS0626, CS0649, CS0169, CS8981, CS1591

namespace Freetype;

public unsafe struct FT_CMapRec
{
    public FT_CharMapRec charmap;
    public FT_CMap_Class clazz;
}

public unsafe struct FT_CMap_ClassRec
{
    public FT_ULong size;
    public FT_CMap_InitFunc init;
    public FT_CMap_DoneFunc done;
    public FT_CMap_CharIndexFunc char_index;
    public FT_CMap_CharNextFunc char_next;
    public FT_CMap_CharVarIndexFunc char_var_index;
    public FT_CMap_CharVarIsDefaultFunc char_var_default;
    public FT_CMap_VariantListFunc variant_list;
    public FT_CMap_CharVariantListFunc charvariant_list;
    public FT_CMap_VariantCharListFunc variantchar_list;
}

public unsafe struct FT_Face_InternalRec
{
    public FT_Matrix transform_matrix;
    public FT_Vector transform_delta;
    public FT_Int transform_flags;
    public FT_ServiceCacheRec services;
    public IntPtr incremental_interface;
    public FT_Char no_stem_darkening;
    public FT_Int32 random_seed;
    public FT_Int refcount;
}

public unsafe struct FT_GlyphSlot_InternalRec
{
    public FT_GlyphLoader loader;
    public FT_UInt flags;
    public FT_Bool glyph_transformed;
    public FT_Matrix glyph_matrix;
    public FT_Vector glyph_delta;
    public IntPtr glyph_hints;
    public FT_Int32 load_flags;
}

public unsafe struct FT_Size_InternalRec
{
    public IntPtr module_data;
    public FT_Render_Mode autohint_mode;
    public FT_Size_Metrics autohint_metrics;
}

public unsafe struct FT_ModuleRec
{
    public IntPtr clazz;
    public FT_Library library;
    public FT_Memory memory;
}

public unsafe struct FT_RendererRec
{
    public FT_ModuleRec root;
    public IntPtr clazz;
    public FT_Glyph_Format glyph_format;
    public FT_Glyph_Class glyph_class;
    public FT_Raster raster;
    public FT_Raster_Render_Func raster_render;
    public FT_Renderer_RenderFunc render;
}

public unsafe struct FT_DriverRec
{
    public FT_ModuleRec root;
    public FT_Driver_Class clazz;
    public FT_ListRec faces_list;
    public FT_GlyphLoader glyph_loader;
}

public unsafe struct FT_LibraryRec
{
    public FT_Memory memory;
    public FT_Int version_major;
    public FT_Int version_minor;
    public FT_Int version_patch;
    public FT_UInt num_modules;
    public FT_Module modules;
    public FT_ListRec renderers;
    public FT_Renderer cur_renderer;
    public FT_Module auto_hinter;
    // skipped array field: FT_DebugHook_Func  debug_hooks[4]
    public FT_LcdFiveTapFilter lcd_weights;
    // skipped array field: FT_Vector                lcd_geometry[3]
    public FT_Int refcount;
}

