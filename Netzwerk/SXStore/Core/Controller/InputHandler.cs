using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SXStore.Core.Controller
{
    public class InputHandler
    {
        private bool _prevTouched = false;

        public TouchState Touch { get; private set; }
        public bool TouchBegan { get; private set; }
        public bool TouchEnded { get; private set; }

        public bool IsDown(NpadButton btn) => Input.IsDown(btn);
        public bool IsHeld(NpadButton btn) => Input.IsHeld(btn);
        public bool IsUp(NpadButton btn) => Input.IsUp(btn);

        public StickPos StickLeft => Input.GetStickLeft();
        public StickPos StickRight => Input.GetStickRight();

        public void Poll()
        {
            Touch = Input.GetTouch();
            TouchBegan = Touch.IsTouched && !_prevTouched;
            TouchEnded = !Touch.IsTouched && _prevTouched;
            _prevTouched = Touch.IsTouched;
        }
    }
}
