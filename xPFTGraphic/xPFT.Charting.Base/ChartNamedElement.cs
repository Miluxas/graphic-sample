/***************************************************************************
 *  ChartNamedElement.cs
 *  Implementation of the Class ChartNamedElement
 *  Created on:      28-Sep-2014 11:44:20 AM
 *  Original author: Teb Tasvir
 *  
 *  Update 2014/10/15 *****************************************************
 *  Add comment to this code.
 * 
 ***************************************************************************/


using xPFT;
namespace xPFT.Charting.Base
{
	/// <summary>
	/// Represents the base class for most chart elements. Chart
	/// elements such as series, chart areas, and legends must have unique names, and
	/// reuse the unique name generation and validation logic provided by the
	/// ChartNamedElementCollection<T> class. 
	/// </summary>
	public class ChartNamedElement : ChartElement {

		protected string name = "";


		/// <summary>
		/// Initializes a new instance of the ChartNamedElement class.
		/// </summary>
		protected ChartNamedElement()
        {
		}

		/// <summary>
		/// Initializes a new instance of the ChartNamedElement class with the specified
		/// chart element name.
		/// </summary>
		/// <param name="elementName">The name of the new chart element.</param>
		protected ChartNamedElement(string elementName){
            Name = elementName;
		}

		/// <summary>
		/// Gets or sets the name of the chart element.
		/// </summary>
		public virtual string Name{
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
		}

	}//end ChartNamedElement

}//end namespace xPFT