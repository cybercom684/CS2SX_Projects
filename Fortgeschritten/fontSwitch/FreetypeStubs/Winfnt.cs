// Auto-generated from externLibs/freetype/src\winfonts\winfnt.h
// DO NOT EDIT — regeneriert via cs2sx addLib
//
// HINWEIS: Diese Datei dient NUR der IDE-Unterstützung (Roslyn-Typen).
// Der Ordner 'FreetypeStubs/' ist in ProjectReader.s_excludedDirNames
// gelistet und wird NICHT von CS2SX transpiliert.
// Die echten C-Funktionen werden direkt aus externLibs/ mitcompiliert.

#pragma warning disable CS0626, CS0649, CS0169, CS8981, CS1591

namespace Freetype;

public unsafe struct WinMZ_HeaderRec
{
    public FT_UShort magic;
    public FT_UShort lfanew;
}

public unsafe struct WinNE_HeaderRec
{
    public FT_UShort magic;
    public FT_UShort resource_tab_offset;
    public FT_UShort rname_tab_offset;
}

public unsafe struct WinPE32_HeaderRec
{
    public FT_ULong magic;
    public FT_UShort machine;
    public FT_UShort number_of_sections;
    public FT_UShort size_of_optional_header;
    public FT_UShort magic32;
    public FT_ULong rsrc_virtual_address;
    public FT_ULong rsrc_size;
}

public unsafe struct WinPE32_SectionRec
{
    // skipped array field: FT_Byte   name[8]
    public FT_ULong virtual_address;
    public FT_ULong size_of_raw_data;
    public FT_ULong pointer_to_raw_data;
}

public unsafe struct WinPE_RsrcDirRec
{
    public FT_ULong characteristics;
    public FT_ULong time_date_stamp;
    public FT_UShort major_version;
    public FT_UShort minor_version;
    public FT_UShort number_of_named_entries;
    public FT_UShort number_of_id_entries;
}

public unsafe struct WinPE_RsrcDirEntryRec
{
    public FT_ULong name;
    public FT_ULong offset;
}

public unsafe struct WinPE_RsrcDataEntryRec
{
    public FT_ULong offset_to_data;
    public FT_ULong size;
    public FT_ULong code_page;
    public FT_ULong reserved;
}

public unsafe struct WinNameInfoRec
{
    public FT_UShort offset;
    public FT_UShort length;
    public FT_UShort flags;
    public FT_UShort id;
    public FT_UShort handle;
    public FT_UShort usage;
}

public unsafe struct WinResourceInfoRec
{
    public FT_UShort type_id;
    public FT_UShort count;
}

