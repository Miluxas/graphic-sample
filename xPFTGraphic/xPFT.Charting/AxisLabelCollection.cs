/**************************************************************************
 *  AxesLabelCollection.cs
 *  Implementation of the Class AxesLabelCollection
 *  Created on:      15-Oct-2014 09:57:32 PM
 *  Original author: Teb Tasvir
 *
 **************************************************************************/
using xPFT.Charting.Base;
namespace xPFT.Charting
{
    internal class AxisLabelCollection : ChartNamedElementCollection<AxisLabel>
    {
        public void Dispose()
        {
            foreach (AxisLabel iL in Items)
                iL.Dispose();
            Clear();
        }
    }
}
