/***************************************************************************
 *  ChartElementCollection.cs
 *  Implementation of the Class ChartElementCollection
 *  Created on:      28-Sep-2014 11:44:20 AM
 *  Original author: Teb Tasvir
 *  
 *  Update 2014/10/15 *****************************************************
 *  Add comment to this code.
 * 
 *  Update 2014/11/16 Update#3 *****************************************
 *  Change code according the new layering design.
***************************************************************************/



using System;


namespace xPFT.Charting.Base
{
	// summary:
	// Base class for all the chart elements. The ChartElement
	// class represents the most basic element of the chart element hierarchy. 

	public class ChartElement : Object {

		private Object tag;

		protected ChartElement(){

		}

		/// <summary>
		/// Releases all resources used by this object. 
		/// </summary>
		public virtual void Dispose(){

		}

		/// <summary>
		/// Releases the unmanaged resources used by the object and
		/// optionally releases the managed resources. 
		/// </summary>
		/// <param name="disposing"></param>
		protected virtual void Dispose(bool disposing){

		}

		/// <summary>
		/// An Object that contains data about the control. The
		/// default is null. 
		/// </summary>
		public Object Tag{
            get
            {
                return tag;
            }
            set
            {
                tag = value;
            }
		}

    }//end ChartElement

}//end namespace xPFT