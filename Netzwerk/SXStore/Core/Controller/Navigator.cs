using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SXStore.Core.Controller
{
    // Navigator.cs — mit Shell-Support
    public static class Navigator
    {
        private static IScreen _shell;
        private static readonly Stack<IScreen> _stack = new Stack<IScreen>();

        public static IScreen Current => _stack.Count > 0 ? _stack.Peek() : null;

        public static void SetShell(IScreen shell)
        {
            _shell = shell;
            _shell.OnInit();
        }

        public static void Push(IScreen screen)
        {
            screen.OnInit();
            _stack.Push(screen);
        }

        public static void Pop()
        {
            if (_stack.Count == 0) return;
            _stack.Pop().OnDestroy();
        }

        public static void Replace(IScreen screen)
        {
            Pop();
            Push(screen);
        }

        public static void Update()
        {
            _shell?.OnUpdate();
            Current?.OnUpdate();
        }

        public static void Draw()
        {
            Graphics.FillScreen(0xFF13131F);
            Current?.OnDraw();
            _shell?.OnDraw();
        }
    }
}
