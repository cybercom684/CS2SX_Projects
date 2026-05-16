// Auto-generated from externLibs/freetype/include\freetype\internal\sfnt.h
// DO NOT EDIT — regeneriert via cs2sx addLib
//
// HINWEIS: Diese Datei dient NUR der IDE-Unterstützung (Roslyn-Typen).
// Der Ordner 'FreetypeStubs/' ist in ProjectReader.s_excludedDirNames
// gelistet und wird NICHT von CS2SX transpiliert.
// Die echten C-Funktionen werden direkt aus externLibs/ mitcompiliert.

#pragma warning disable CS0626, CS0649, CS0169, CS8981, CS1591

namespace Freetype;

public unsafe struct SFNT_Interface
{
    public TT_Loader_GotoTableFunc goto_table;
    public TT_Init_Face_Func init_face;
    public TT_Load_Face_Func load_face;
    public TT_Done_Face_Func done_face;
    public FT_Module_Requester get_interface;
    public TT_Load_Any_Func load_any;
    public TT_Load_Table_Func load_head;
    public TT_Load_Metrics_Func load_hhea;
    public TT_Load_Table_Func load_cmap;
    public TT_Load_Table_Func load_maxp;
    public TT_Load_Table_Func load_os2;
    public TT_Load_Table_Func load_post;
    public TT_Load_Table_Func load_name;
    public TT_Free_Table_Func free_name;
    public TT_Load_Table_Func load_kern;
    public TT_Load_Table_Func load_gpos;
    public TT_Load_Table_Func load_gasp;
    public TT_Load_Table_Func load_pclt;
    public TT_Load_Table_Func load_bhed;
    public TT_Load_SBit_Image_Func load_sbit_image;
    public TT_Get_PS_Name_Func get_psname;
    public TT_Free_Table_Func free_psnames;
    public TT_Face_GetKerningFunc get_kerning;
    public TT_Face_GetKerningFunc get_gpos_kerning;
    public TT_Load_Table_Func load_font_dir;
    public TT_Load_Metrics_Func load_hmtx;
    public TT_Load_Table_Func load_eblc;
    public TT_Free_Table_Func free_eblc;
    public TT_Set_SBit_Strike_Func set_sbit_strike;
    public TT_Load_Strike_Metrics_Func load_strike_metrics;
    public TT_Load_Table_Func load_cpal;
    public TT_Load_Table_Func load_colr;
    public TT_Free_Table_Func free_cpal;
    public TT_Free_Table_Func free_colr;
    public TT_Set_Palette_Func set_palette;
    public TT_Get_Colr_Layer_Func get_colr_layer;
    public TT_Get_Color_Glyph_Paint_Func get_colr_glyph_paint;
    public TT_Get_Color_Glyph_ClipBox_Func get_color_glyph_clipbox;
    public TT_Get_Paint_Layers_Func get_paint_layers;
    public TT_Get_Colorline_Stops_Func get_colorline_stops;
    public TT_Get_Paint_Func get_paint;
    public TT_Blend_Colr_Func colr_blend;
    public TT_Get_Metrics_Func get_metrics;
    public TT_Get_Name_Func get_name;
    public TT_Get_Name_ID_Func get_name_id;
    public TT_Load_Table_Func load_svg;
    public TT_Free_Table_Func free_svg;
    public TT_Load_Svg_Doc_Func load_svg_doc;
}

