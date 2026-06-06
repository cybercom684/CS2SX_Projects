namespace piano.Core.Models
{
    public static class PianoEngine
    {
        // MIDI-Frequenzberechnung: A4 = 69 = 440 Hz
        public static float MidiToFrequency(int midiNote)
        {
            return 440.0f * MathF(2.0f, (midiNote - 69) / 12.0f);
        }

        private static float MathF(float b, float e)
        {
            // CS2SX hat kein System.MathF — eigene Pow-Implementierung
            if (e == 0.0f) return 1.0f;
            bool neg = e < 0.0f;
            if (neg) e = -e;
            // ln(b) * e via Taylor wäre aufwendig; wir nutzen int-Anteil + Rest
            int intE = (int)e;
            float fracE = e - intE;
            float result = 1.0f;
            for (int i = 0; i < intE; i++) result *= b;
            // Näherung für fraktionalen Anteil: e^(fracE * ln(b))
            // ln(2) ≈ 0.693147, also für b=2: result *= e^(fracE * 0.693147)
            // e^x ≈ 1 + x + x²/2 + x³/6 für kleine x
            float x = fracE * 0.693147f;
            float ex = 1.0f + x + (x * x) / 2.0f + (x * x * x) / 6.0f + (x * x * x * x) / 24.0f;
            result *= ex;
            return neg ? (1.0f / result) : result;
        }

        // Generiert eine Oktave (C bis H + Schwarze Tasten), MIDI-Basis z.B. C4 = 60
        public static PianoKey[] GenerateOctave(int octaveNumber)
        {
            int baseNote = 12 + octaveNumber * 12; // C-1 = MIDI 0, C0 = 12, C4 = 60
            string[] names = { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };
            bool[] blacks = { false, true, false, true, false, false, true, false, true, false, true, false };
            var keys = new PianoKey[12];
            for (int i = 0; i < 12; i++)
            {
                keys[i] = new PianoKey
                {
                    MidiKey = baseNote + i,
                    Name = names[i] + octaveNumber,
                    isSecondary = blacks[i]
                };
            }
            return keys;
        }
    }
}