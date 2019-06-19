using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xPFT.IDrawing;
using System.Windows.Forms;
using System.Drawing;


namespace xPFT.ChildAnimation
{
    internal interface IScenario
    {
        void Initialize(IDevice device, int Width, int Height);
        void NewSample(float NewValue);
        void Reset();
        void Dispose();
        ScenarioType scenarioType { get; }
    }
}
