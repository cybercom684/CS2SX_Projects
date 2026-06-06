using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SXStore.Core.Controller
{
    public interface IScreen
    {
        void OnInit();
        void OnUpdate();
        void OnDraw();
        void OnDestroy();
    }
}
