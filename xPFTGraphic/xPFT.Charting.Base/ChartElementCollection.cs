/***************************************************************************
 *  ChartElementCollection.cs
 *  Implementation of the Class ChartElementCollection
 *  Created on:      28-Sep-2014 11:44:20 AM
 *  Original author: Teb Tasvir
 *  
 *  Update 2014/10/15 *****************************************************
 *  Add comment to this code.
 * 
***************************************************************************/

using System.Collections.ObjectModel;


namespace xPFT.Charting.Base
{
    /// <summary>
    /// Represents the base class for all chart element
    /// collections. 
    /// </summary>
    public class ChartElementCollection<T> : Collection<T>
    {

        public ChartElementCollection()
        {
        }

        public delegate void EventHandler(T item);
        public event EventHandler AddNewItem;
        public bool isActionAddNewItemEventHandler=true;
        protected override void InsertItem(int index, T item)
        {
            base.InsertItem(index, item);
            if (AddNewItem != null)
                if(isActionAddNewItemEventHandler)
                AddNewItem(item);
        }
        public void ActionAddNewItemEventHandler()
        {
            if (AddNewItem != null)
                if(Count>0)
                    AddNewItem(this[0]);
        }
    }
}