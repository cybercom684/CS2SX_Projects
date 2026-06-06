using SXStore.Core.Controller;
using SXStore.Core.GUI.Components;
using SXStore.Core.Repository;
using System;

namespace SXStore.Core.Screens
{
    public class SidebarScreen : IScreen
    {
        private readonly InputHandler _input;
        private readonly IAppRepository _repository;
        private Sidebar _sidebar;
        private int _focusedIndex = 0;
        private bool _sidebarFocused = true;

        public SidebarScreen(InputHandler input, IAppRepository repository)
        {
            _input = input;
            _repository = repository;
        }

        public void OnInit()
        {
            _sidebar = new Sidebar
            {
                Title = GlobalDefines.AppName,
                X = 0,
                Y = 0,
                Width = 240,
                Height = 720
            };

            _sidebar.AddItem(new SidebarItem("Home", () => Navigator.Replace(new HomeScreen(_input, _repository))));
            _sidebar.AddItem(new SidebarItem("Browse", () => Navigator.Replace(new BrowseScreen(_input, _repository))));
            _sidebar.AddItem(new SidebarItem("Installed", () => Navigator.Replace(new InstalledScreen(_input, _repository))));
            _sidebar.AddItem(new SidebarItem("Settings", () => Navigator.Replace(new SettingsScreen(_input))));

            _sidebar.Update();

            Navigator.Push(new HomeScreen(_input, _repository));
        }

        public void OnUpdate()
        {
            // Auto-refocus sidebar when all content screens are popped
            if (Navigator.Current == null)
                _sidebarFocused = true;

            HandleNavigation();
            HandleTouch();
        }

        public void OnDraw()
        {
            _sidebar.Draw();
        }

        public void OnDestroy() { }

        private void HandleNavigation()
        {
            // ZL re-focuses the sidebar from anywhere in the content
            if (_input.IsDown(NpadButton.ZL))
                _sidebarFocused = true;

            if (!_sidebarFocused) return;

            if (_input.IsDown(NpadButton.Down) || _input.IsDown(NpadButton.StickLDown))
                MoveFocus(1);
            else if (_input.IsDown(NpadButton.Up) || _input.IsDown(NpadButton.StickLUp))
                MoveFocus(-1);

            if (_input.IsDown(NpadButton.A))
            {
                _sidebar.HandleInput(_sidebar.X + 1, GetItemY(_focusedIndex), true);
                _sidebarFocused = false;
            }
        }

        private void HandleTouch()
        {
            if (!_input.Touch.IsTouched) return;
            _sidebar.HandleInput(_input.Touch.X0, _input.Touch.Y0, _input.TouchBegan);
            if (_input.TouchBegan)
                _sidebarFocused = false;
        }

        private void MoveFocus(int delta)
        {
            _focusedIndex = Math.Clamp(_focusedIndex + delta, 0, _sidebar.Items.Count - 1);
            _sidebar.HandleInput(_sidebar.X + 1, GetItemY(_focusedIndex), false);
        }

        private int GetItemY(int index)
        {
            // Matches Sidebar.GetHeaderHeight: logo block (66) + section label (28)
            int headerHeight = string.IsNullOrEmpty(_sidebar.Title) ? 28 : 94;
            return _sidebar.Y + headerHeight + index * 52 + 1;
        }
    }
}
