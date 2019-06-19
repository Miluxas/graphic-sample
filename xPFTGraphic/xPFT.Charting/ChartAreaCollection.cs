/***************************************************************************
 *  ChartAreaCollection.cs
 *  Implementation of the Class ChartAreaCollection
 *  Created on:      07-Oct-2014 12:57:32 PM
 *  Original author: Teb Tasvir
 *  
 *  Update 2014/10/15 *****************************************************
 *  Add comment to this code.
 * 
 **************************************************************************/

using System.Collections.ObjectModel;
 
namespace xPFT.Charting
{
	/// <summary>
	/// Represents a collection of ChartArea objects. 
	/// </summary>
    public class ChartAreaCollection : Collection<ChartArea> 
    {

        /// <summary>
        /// Get or Set chart aria with it's name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ChartArea this[string name]
        {

            get
            {
                for (int index = 0; index < Count; index++)
                {
                    if (this[index].Name.ToUpper() == name.ToUpper())
                        return this[index];
                }
                return null;
            }

            set
            {
                for (int index = 0; index < Count; index++)
                {
                    if (this[index].Name.ToUpper() == name.ToUpper())
                    {
                        this[index] = value;
                        break;
                    }
                }
            }
        }

	}//end ChartAreaCollection

}//end namespace xPFT