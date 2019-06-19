/***************************************************************************
 *  DataPoint.cs
 *  Implementation of the Class DataPoint
 *  Created on:      28-Sep-2014 11:44:20 AM
 *  Original author: Teb Tasvir
 * 
***************************************************************************/

using System.ComponentModel;
using System.Collections.Generic;

namespace xPFT.Charting.Base
{
    /// <summary>
    /// Represents a data point that is stored in the
    /// DataPointCollection class. 
    /// </summary>
    [DefaultProperty("YValues")]
    public class DataPoint : DataPointCustomProperties
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the DataPoint
        /// </summary>
        public DataPoint()
        {
            yValues = new List<float>();
        }

        /// <summary>
        /// Initializes a new instance of the DataPoint class with the specified X and Y-value.
        /// </summary>
        /// <param name="xValue">The X-value of the data point.</param>
        /// <param name="yValue">The Y-value of the data point.</param>
        public DataPoint(float xValue, float yValue)
        {
            YValues = new List<float>();
            XValue = xValue;
            YValues.Add(yValue);
        }
        public DataPoint(float xValue, float yValue,object tag)
        {
            YValues = new List<float>();
            XValue = xValue;
            YValues.Add(yValue);
            this.Tag = tag;
        }
        /// <summary>
        /// Initializes a new instance of the DataPoint class with the specified X-value and an array of Y-values.
        /// </summary>
        /// <param name="xValue">The X-value of the data point.</param>
        /// <param name="yValues">An array of Y-values of the data point.</param>
        public DataPoint(float xValue, float[] yValues)
        {
            YValues = new List<float>();double
            XValue = xValue;
            YValues.AddRange(yValues);
        }

        /// <summary>
        /// Initializes a new instance of the DataPoint class with the specified X-value and Y-values.
        /// </summary>
        /// <param name="xValue">The X-value of the data point.</param>
        /// <param name="yValues">A comma-separated string of Y-values of the data point.
        /// </param>
        public DataPoint(float xValue, string yValues)
        {
            XValue = xValue;
            string[] ys = yValues.Split(',');
            ys.CopyTo(YValues.ToArray(), 0);
        }
        #endregion

        #region Fields
        /// <summary>
        /// X-value of a data point.
        /// </summary>
        private float xValue;
        /// <summary>
        /// A flag that indicates whether a data point is marked as empty.
        /// </summary>
        private bool isEmpty;
        /// <summary>
        /// The Y-value(s) of a data point.
        /// </summary>
        private List<float> yValues;


        #endregion

        #region Properties
        /// <summary>
        /// A flag that indicates whether a data point is marked as empty.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return isEmpty;
            }
            set
            {
                isEmpty = value;
            }
        }

        /// <summary>
        /// X-value of a data point.
        /// </summary>
        public float XValue
        {
            get
            {
                return xValue;
            }
            set
            {
                xValue = value;
            }
        }

        /// <summary>
        /// The Y-value(s) of a data point.
        /// </summary>
        public List<float> YValues
        {
            get
            {
                return yValues;
            }
            set
            {
                yValues = value;
            }
        }

        /// <summary>
        /// Return the y that in this index
        /// </summary>
        /// <param name="index"></param>
        public float this[int index]
        {
            get
            {
                return YValues[index];
            }
            set
            {
                YValues[index] = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the data point.
        /// </summary>
        public override string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }
        #endregion

        #region Methods

        /// <summary>
        /// Returns a new instance that is an exact copy of the data point.
        /// </summary>
        public DataPoint Clone()
        {
            return new DataPoint(XValue, YValues.ToArray());
        }

        /// <summary>
        /// Returns a data point that is specified by its name. This is a helper function.
        /// </summary>
        /// <returns>A value that represents a data point.</returns>
        /// <param name="valueName">Point value names. X, Y, Y2...</param>
        public float GetValueByName(string valueName)
        {
            try
            {
                if (valueName.ToUpper() == "X")
                    return XValue;
                else if (valueName.ToUpper() == "Y")
                    return YValues[0];
                else
                    return YValues[int.Parse(valueName.Substring(1))];
            }
            catch
            {
                return 0;
            }
        }

        #endregion

    }
}