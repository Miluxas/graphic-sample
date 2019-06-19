/***************************************************************************
 *  ChartNamedElementCollection.cs
 *  Implementation of the Class ChartNamedElementCollection
 *  Created on:      28-Sep-2014 11:44:20 AM
 *  Original author: Teb Tasvir
 *  
 ***************************************************************************/

namespace xPFT.Charting.Base
{
    /// <summary>
    /// Represents the base class for all collections of named
    /// chart elements. Performs name management and enforces the uniqueness of the
    /// names. 
    /// </summary>
    public class ChartNamedElementCollection<T> : ChartElementCollection<T>
        where T : ChartNamedElement
    {

        public ChartNamedElementCollection()
        {

        }

        /// <summary>
        /// Gets or sets the chart element with the specified name.
        /// </summary>
        /// <returns>The ChartNamedElement object of the specified name in the
        /// collection</returns>
        /// <param name="name">The name of the chart element.</param>
        public T this[string name]
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

        /// <summary>
        /// Finds the chart element by the specified name.
        /// </summary>
        /// <returns>The ChartNamedElement object of the specified name in the collection.
        /// </returns>
        /// <param name="name">The name of the chart element.</param>
        public virtual T FindByName(string name)
        {
            for (int index = 0; index < Count; index++)
            {
                if (this[index].Name.ToUpper() == name.ToUpper())
                    return this[index];
            }
            return null;
        }

        /// <summary>
        /// Finds the index of the chart element with the specified name.
        /// </summary>
        /// <returns>An integer value that represents the position of the chart element in
        /// the zero-based index</returns>
        /// <param name="name">The name of the chart element.</param>
        public int IndexOf(string name)
        {
            for (int index = 0; index < Count; index++)
            {
                if (this[index].Name.ToUpper() == name.ToUpper())
                    return index;
            }
            return -1;
        }

        /// <summary>
        /// Determines whether the chart element with the specified name already exists in
        /// the collection.
        /// </summary>
        /// <returns>True if the new chart element name is unique, otherwise false.
        /// </returns>
        /// <param name="name">The new chart element name.</param>
        public virtual bool IsUniqueName(string name)
        {
            for (int index = 0; index < Count; index++)
            {
                if (this[index].Name.ToUpper() == name.ToUpper())
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Finds the unique name for the new element to be added to the collection.
        /// </summary>
        /// <returns>A string value that represents the next unique chart element name.
        /// </returns>
        public virtual string NextUniqueName()
        {
            return "";
        }
    }
}