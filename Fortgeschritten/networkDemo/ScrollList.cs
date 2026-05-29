using System.Collections.Generic;

// Scrollable list UI component.
// Items are strings; the selected index is highlighted.
// Scroll via Up/Down, confirm via A.
public class ScrollList
{
    private List<RepoEntry> _items     = new List<RepoEntry>();
    private int             _selected  = 0;
    private int             _scrollTop = 0;

    public int VisibleRows  = 10;
    public int X            = 40;
    public int Y            = 140;
    public int RowHeight    = 42;
    public int Width        = 1200;

    public int       SelectedIndex => _selected;
    public RepoEntry SelectedItem  => _items.Count > 0 ? _items[_selected] : null;
    public int       Count         => _items.Count;
    public RepoEntry GetItem(int i) => i >= 0 && i < _items.Count ? _items[i] : null;

    public void SetItems(List<RepoEntry> items)
    {
        _items     = items;
        _selected  = 0;
        _scrollTop = 0;
    }

    public void Update()
    {
        if (_items.Count == 0) return;

        if (Input.IsDown(NpadButton.Down))
        {
            _selected++;
            if (_selected >= _items.Count) _selected = _items.Count - 1;
            if (_selected >= _scrollTop + VisibleRows)
                _scrollTop = _selected - VisibleRows + 1;
        }

        if (Input.IsDown(NpadButton.Up))
        {
            _selected--;
            if (_selected < 0) _selected = 0;
            if (_selected < _scrollTop)
                _scrollTop = _selected;
        }
    }

    public void Draw()
    {
        if (_items.Count == 0)
        {
            Graphics.FillRect(X, Y, Width, RowHeight, Color.RGB(30, 30, 45));
            Graphics.DrawText(X + 12, Y + 12, "(keine Eintraege)", Color.Gray, 1);
            return;
        }

        int end = _scrollTop + VisibleRows;
        if (end > _items.Count) end = _items.Count;

        for (int i = _scrollTop; i < end; i++)
        {
            int ry = Y + (i - _scrollTop) * RowHeight;
            bool sel = i == _selected;

            uint bg  = sel ? Color.RGB(0, 90, 170)  : (i % 2 == 0 ? Color.RGB(22, 22, 34) : Color.RGB(28, 28, 42));
            uint fg  = sel ? Color.White             : Color.RGB(210, 210, 230);
            uint tag = _items[i].Downloaded ? Color.RGB(80, 220, 80) : Color.RGB(120, 120, 140);

            Graphics.FillRect(X, ry, Width, RowHeight - 2, bg);
            if (sel) Graphics.DrawRect(X, ry, Width, RowHeight - 2, Color.RGB(60, 140, 240));

            // Row number
            Graphics.DrawText(X + 8, ry + 12, (i + 1) + ".", Color.Gray, 1);

            // Name
            Graphics.DrawText(X + 42, ry + 12, _items[i].Name, fg, 1);

            // Size holds the full weather string (persistent C buffer) or "(nicht geladen)"
            int tagW = Graphics.MeasureTextWidth(_items[i].Size, 1);
            Graphics.DrawText(X + Width - tagW - 16, ry + 12, _items[i].Size, tag, 1);
        }

        // Scroll indicator
        if (_items.Count > VisibleRows)
        {
            int barH  = VisibleRows * RowHeight;
            int thumbH = barH * VisibleRows / _items.Count;
            int thumbY = Y + barH * _scrollTop / _items.Count;
            Graphics.FillRect(X + Width + 4, Y, 6, barH, Color.RGB(30, 30, 50));
            Graphics.FillRect(X + Width + 4, thumbY, 6, thumbH, Color.RGB(80, 140, 240));
        }
    }
}
