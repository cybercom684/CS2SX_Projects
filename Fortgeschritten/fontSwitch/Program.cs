public class FontSwitchApp : SwitchApp
{
    private ulong _library;
    private ulong _face;
    private bool  _ftReady;
    private int   _errCode;   // letzter Fehlercode für Diagnose
    private int   _errStep;   // welcher Schritt fehlgeschlagen ist (1/2/3)
    private string _text = "Hello Switch!";

    public override void OnInit()
    {
        Graphics.Init(1280, 720);

        // Schritt 1: Init
        _errStep = 1;
        _errCode = Freetype.FT_Init_FreeType(ref _library);
        if (_errCode != 0) return;

        // Schritt 2: Font laden — direkt neben der NRO
        _errStep = 2;
        _errCode = Freetype.FT_New_Face(_library, "font.ttf", 0, ref _face);
        if (_errCode != 0)
        {
            Freetype.FT_Done_FreeType(_library);
            return;
        }

        // Schritt 3: Größe setzen
        _errStep = 3;
        _errCode = Freetype.FT_Set_Char_Size(_face, 0, 32 * 64, 96, 96);
        if (_errCode != 0)
        {
            Freetype.FT_Done_Face(_face);
            Freetype.FT_Done_FreeType(_library);
            return;
        }

        _errStep = 0;
        _ftReady = true;
    }

    public override void OnFrame()
    {
        Graphics.FillScreen(Color.Black);

        if (!_ftReady)
        {
            // Fehlerdiagnose ohne interpolierte Strings
            Graphics.DrawText(20, 40,  "Freetype nicht bereit", Color.Red, 2);
            Graphics.DrawText(20, 100, "Schritt:", Color.White, 1);
            Graphics.DrawText(200, 100, Int_ToString(_errStep), Color.Yellow, 1);
            Graphics.DrawText(20, 120, "Fehlercode:", Color.White, 1);
            Graphics.DrawText(200, 120, Int_ToString(_errCode), Color.Yellow, 1);
            Graphics.DrawText(20, 160, "Schritt 1 = FT_Init_FreeType", Color.Gray, 1);
            Graphics.DrawText(20, 180, "Schritt 2 = FT_New_Face (font.ttf)", Color.Gray, 1);
            Graphics.DrawText(20, 200, "Schritt 3 = FT_Set_Char_Size", Color.Gray, 1);
            Graphics.DrawText(20, 240, "font.ttf muss neben der NRO liegen", Color.Cyan, 1);
            return;
        }

        DrawFreetypeText(_text,              60, 200, Color.White);
        DrawFreetypeText("CS2SX + Freetype", 60, 300, Color.Cyan);
        DrawFreetypeText("Nintendo Switch",  60, 400, Color.Yellow);

        if (Input.IsDown(NpadButton.A))
            _text = _text == "Hello Switch!" ? "Freetype funktioniert!" : "Hello Switch!";
    }

    private void DrawFreetypeText(string text, int x, int y, uint color)
    {
        if (!_ftReady || text == null) return;
        int penX = x;
        int len  = text.Length;
        for (int i = 0; i < len; i++)
        {
            uint glyphIndex = Freetype.FT_Get_Char_Index(_face, (uint)text[i]);
            if (glyphIndex == 0) { penX += 10; continue; }
            int err = Freetype.FT_Load_Glyph(_face, glyphIndex, 0);
            if (err != 0) { penX += 10; continue; }
            ulong slotPtr = Freetype.FT_Face_GetGlyphSlot(_face);
            if (slotPtr == 0) { penX += 10; continue; }
            err = Freetype.FT_Render_Glyph(slotPtr, 0);
            if (err != 0) { penX += 10; continue; }
            int bmpWidth = Freetype.FT_GlyphSlot_GetBitmapWidth(slotPtr);
            int bmpRows  = Freetype.FT_GlyphSlot_GetBitmapRows(slotPtr);
            int bmpLeft  = Freetype.FT_GlyphSlot_GetBitmapLeft(slotPtr);
            int bmpTop   = Freetype.FT_GlyphSlot_GetBitmapTop(slotPtr);
            int advX     = Freetype.FT_GlyphSlot_GetAdvanceX(slotPtr) >> 6;
            Freetype.DrawGlyphToFramebuffer(slotPtr, bmpWidth, bmpRows,
                penX + bmpLeft, y - bmpTop, color);
            penX += advX;
        }
    }

    public override void OnExit()
    {
        if (_ftReady)
        {
            Freetype.FT_Done_Face(_face);
            Freetype.FT_Done_FreeType(_library);
        }
    }
}