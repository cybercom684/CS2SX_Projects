// StrHelper.cs — Hilfsmethoden fuer Konvertierungen in CS2SX.
// Diese Datei wird NICHT transpiliert (in s_excludedFileNames).
// Jede Methode ist ein Transpiler-Handler der direkte C-Ausdrücke erzeugt.

#pragma warning disable CS0626, CS0649, CS0169, CS8981

public static class Str
{
    // byte[]-Buffer nach C-String-Konvention zu string.
    // Generiert: (const char*)buffer
    // Verwendung nach Output-Buffer-Funktionen:
    //   byte[] buf = new byte[256];
    //   SomeLib.GetText(ref buf[0], 256);
    //   string result = Str.From(buf);
    public static string From(byte[] buffer) => "";

    // Gleich wie From, aber mit Offset-Einstieg.
    // Generiert: (const char*)(buffer + offset)
    public static string From(byte[] buffer, int offset) => "";

    // Prueft ob ein byte[]-Buffer leer (nur Null-Byte) ist.
    // Generiert: (buffer[0] == 0)
    public static bool IsEmpty(byte[] buffer) => true;
}
