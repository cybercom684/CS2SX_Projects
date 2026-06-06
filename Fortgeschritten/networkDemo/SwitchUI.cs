// SwitchUI.cs — Nintendo-System-UI direkt aus C# nutzbar.
// Diese Datei wird NICHT transpiliert (in s_excludedFileNames gelistet).
// Die Implementierung liegt in switchapp.h (static inline C-Funktionen).
//
// Verwendung:
//   string name = SwitchUI.Keyboard("Name eingeben", "Max. 16 Zeichen");
//   string pin  = SwitchUI.KeyboardNum("PIN", "4 Stellen");
//   string pw   = SwitchUI.KeyboardPassword("Passwort");

#pragma warning disable CS0626, CS0649, CS0169, CS8981

public static class SwitchUI
{
    // Freie Texteingabe. Gibt "" zurueck wenn der Nutzer abbricht.
    // title   = Ueberschrift der Tastatur
    // hint    = grauer Hilfstext im Eingabefeld
    // initial = vorausgefuellter Starttext (optional)
    public static extern string Keyboard(string title, string hint, string initial);

    // Nur Zahlen (NumPad-Layout).
    public static extern string KeyboardNum(string title, string hint);

    // Passwort-Eingabe (Zeichen verdeckt).
    public static extern string KeyboardPassword(string title);
}
