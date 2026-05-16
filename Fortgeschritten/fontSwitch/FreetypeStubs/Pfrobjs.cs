// Auto-generated from externLibs/freetype/src\pfr\pfrobjs.h
// DO NOT EDIT — regeneriert via cs2sx addLib
//
// HINWEIS: Diese Datei dient NUR der IDE-Unterstützung (Roslyn-Typen).
// Der Ordner 'FreetypeStubs/' ist in ProjectReader.s_excludedDirNames
// gelistet und wird NICHT von CS2SX transpiliert.
// Die echten C-Funktionen werden direkt aus externLibs/ mitcompiliert.

#pragma warning disable CS0626, CS0649, CS0169, CS8981, CS1591

namespace Freetype;

public unsafe struct PFR_FaceRec
{
    public FT_FaceRec root;
    public PFR_HeaderRec header;
    public PFR_LogFontRec log_font;
    public PFR_PhyFontRec phy_font;
}

public unsafe struct PFR_SizeRec
{
    public FT_SizeRec root;
}

public unsafe struct PFR_SlotRec
{
    public FT_GlyphSlotRec root;
    public PFR_GlyphRec glyph;
}

