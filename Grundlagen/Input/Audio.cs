// CS2SX Stub — wird nicht transpiliert
public static class Audio
{
    // ── Core ──────────────────────────────────────────────────────────────────

    /// Initialisiert das Audio-System. sampleRate wird ignoriert (immer 48000 Hz).
    public static bool Init(int sampleRate = 48000) => false;

    /// Muss einmal pro Frame in OnFrame() aufgerufen werden — hält den Hardware-Buffer gefüllt.
    public static void Update() { }

    /// Setzt die Master-Lautstärke (0.0 = stumm, 1.0 = voll).
    public static void SetVolume(float volume) { }

    /// Stoppt alle laufenden Töne und Samples sofort.
    public static void Stop() { }

    /// Gibt das Audio-System frei (am App-Ende aufrufen).
    public static void Exit() { }

    // ── Sinuston-Synthesizer ──────────────────────────────────────────────────

    /// Spielt einen Sinuston mit Piano-Timbre (Grundton + Harmonische).
    /// freqHz    : Frequenz in Hz (z. B. 440.0 = A4)
    /// amplitude : Lautstärke 0.0–1.0
    /// duration_ms: Dauer in Millisekunden (exponentielles Decay)
    public static void PlayTone(float freqHz, float amplitude, int duration_ms) { }

    // ── WAV-Datei Wiedergabe ──────────────────────────────────────────────────

    /// Lädt eine WAV-Datei (16-bit PCM, mono oder stereo, beliebige Samplerate).
    /// path: z. B. "romfs:/sfx/jump.wav" oder "/switch/myapp/effect.wav"
    /// Gibt ein Sound-Handle zurück (0–15) oder -1 bei Fehler.
    /// Hinweis: romfs muss zuvor mit romfsInit() gemountet worden sein.
    public static int LoadWav(string path) => -1;

    /// Gibt einen geladenen Sound frei und stoppt alle seine Voices.
    public static void UnloadSound(int handle) { }

    /// Spielt einen geladenen Sound ab.
    /// handle    : Sound-Handle von LoadWav()
    /// volume    : Lautstärke 0.0–1.0
    /// loop      : true = Endlosschleife bis StopInstance/StopSound
    /// pitch     : 1.0 = Originalgeschwindigkeit, 2.0 = Oktave höher, 0.5 = Oktave tiefer
    /// pan       : -1.0 = links, 0.0 = Mitte, 1.0 = rechts (Equal-Power-Panning)
    /// Gibt eine Voice-Instanz-ID zurück (0–7) oder -1 bei Fehler.
    public static int PlaySound(int handle, float volume, bool loop, float pitch, float pan) => -1;

    /// Stoppt eine bestimmte Wiedergabe-Instanz (ID von PlaySound).
    public static void StopInstance(int instanceId) { }

    /// Stoppt alle Voices eines bestimmten Sounds.
    public static void StopSound(int handle) { }

    /// Stoppt alle laufenden Sample-Voices.
    public static void StopAllSounds() { }

    /// Gibt true zurück, wenn die Wiedergabe-Instanz noch aktiv ist.
    public static bool IsPlaying(int instanceId) => false;

    // ── Effekte ───────────────────────────────────────────────────────────────

    /// Tiefpassfilter auf den Gesamtmix.
    /// cutoffHz: Grenzfrequenz in Hz (20–20000). 0 = deaktivieren.
    /// Beispiel: Audio.SetLowPass(800) → dumpfer, unterwasser-artiger Klang.
    public static void SetLowPass(float cutoffHz) { }

    /// Echo-Effekt auf den Gesamtmix.
    /// delayMs: Verzögerung in ms (50–2000).
    /// decay  : Abklingfaktor pro Wiederholung (0.0 = kein Echo, 0.9 = langer Nachhall).
    /// Beispiel: Audio.SetEcho(300, 0.5) → 300 ms Echo bei halber Lautstärke.
    public static void SetEcho(int delayMs, float decay) { }

    /// Deaktiviert alle Effekte (Tiefpass + Echo).
    public static void ClearEffects() { }
}
