// Auto-generated from externLibs/freetype/src\truetype\ttobjs.h
// DO NOT EDIT — regeneriert via cs2sx addLib
//
// HINWEIS: Diese Datei dient NUR der IDE-Unterstützung (Roslyn-Typen).
// Der Ordner 'FreetypeStubs/' ist in ProjectReader.s_excludedDirNames
// gelistet und wird NICHT von CS2SX transpiliert.
// Die echten C-Funktionen werden direkt aus externLibs/ mitcompiliert.

#pragma warning disable CS0626, CS0649, CS0169, CS8981, CS1591

namespace Freetype;

public unsafe struct TT_GraphicsState
{
    public FT_UShort rp0;
    public FT_UShort rp1;
    public FT_UShort rp2;
    public FT_UShort gep0;
    public FT_UShort gep1;
    public FT_UShort gep2;
    public FT_UnitVector dualVector;
    public FT_UnitVector projVector;
    public FT_UnitVector freeVector;
    public FT_Long loop;
    public FT_Int round_state;
    // skipped array field: FT_F26Dot6     compensation[4]
    public FT_F26Dot6 minimum_distance;
    public FT_F26Dot6 control_value_cutin;
    public FT_F26Dot6 single_width_cutin;
    public FT_F26Dot6 single_width_value;
    public FT_UShort delta_base;
    public FT_UShort delta_shift;
    public FT_Bool auto_flip;
    public FT_Byte instruct_control;
    public FT_Bool scan_control;
    public FT_Int scan_type;
}

public unsafe struct TT_Size_Metrics
{
    public FT_Long x_ratio;
    public FT_Long y_ratio;
    public FT_Long ratio;
    public FT_Fixed scale;
    public FT_UShort ppem;
    public FT_Bool rotated;
    public FT_Bool stretched;
}

public unsafe struct TT_SizeRec
{
    public FT_SizeRec root;
    public IntPtr metrics;
    public FT_Size_Metrics hinted_metrics;
    public TT_Size_Metrics ttmetrics;
    public IntPtr widthp;
    public FT_ULong strike_index;
    public FT_Long point_size;
    public TT_GraphicsState GS;
    public TT_GlyphZoneRec twilight;
    public TT_ExecContext context;
    public FT_Error bytecode_ready;
    public FT_Error cvt_ready;
}

public unsafe struct TT_DriverRec
{
    public FT_DriverRec root;
    public TT_GlyphZoneRec zone;
    public FT_UInt interpreter_version;
}

