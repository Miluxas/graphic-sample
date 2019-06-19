/***************************************************************************
 *  DataPointCollection.cs
 *  Implementation of the Class DataPointCollection
 *  Created on:      28-Sep-2014 11:44:20 AM
 *  Original author: Teb Tasvir
 *  
 *  Update 2014/10/15 *****************************************************
 *  Add comment to this code.
 * 
***************************************************************************/

namespace xPFT.Charting.Base
{
    public class DataPointCollection : ChartElementCollection<DataPoint>
    {
        #region Constructor
        public DataPointCollection()
        {
        }
        #endregion

        #region Properties

        float width = 100;
        /// <summary>
        /// Get or Set the length of the collection show region.
        /// </summary>
        public float Width
        {
            get
            {
                return width;
            }
            set
            {
                width = value;
            }
        }

        float heightTop=1, heightBottom=-1;
        public float HeightTop
        {
            set
            {
                heightTop = value;
            }
        }

        public float HeightBottom
        {
            set
            {
                heightBottom = value;
            }
        }

        bool autoShift = true;
        /// <summary>
        /// Get or Set is points shifting automatic or not.
        /// </summary>
        public bool AutoShift
        {
            get
            {
                return autoShift;
            }
            set
            {
                autoShift = value;
            }
        }

        #endregion

        #region Methods

        public new void Add(DataPoint newPoint)
        {
            AddNewDataPoint(newPoint);
        }

        /// <summary>
        /// Adds a DataPoint object to  the end of the collection, with the specified X- value and Y-value.
        /// </summary>
        /// <returns>An integer that represents the zero-based index where the item was inserted into the data point collection.</returns>
        /// <param name="xValue">The X-value of the data point.</param>
        /// <param name="yValue">The Y-value of the data point.</param>
        public int AddXY(float xValue, float yValue)
        {
            DataPoint tmpDataPoint = new DataPoint(xValue, yValue);
            AddNewDataPoint(tmpDataPoint);
            return base.IndexOf(tmpDataPoint);
        }

        /// <summary>
        /// Adds a DataPoint object to  the end of the collection, with the specified X- value and Y-value.
        /// Added on 2015-04-08
        /// </summary>
        /// <returns>An integer that represents the zero-based index where the item was inserted into the data point collection.</returns>
        /// <param name="xValue">The X-value of the data point.</param>
        /// <param name="yValue">The Y-value of the data point.</param>
        public void AddPointFArray(System.Drawing.PointF[] points)
        {
            isActionAddNewItemEventHandler = false;
            foreach (System.Drawing.PointF point in points)
            {
                DataPoint tmpDataPoint = new DataPoint(point.X,point.Y);
                AddNewDataPoint(tmpDataPoint);
            }
            isActionAddNewItemEventHandler = true;
            ActionAddNewItemEventHandler();
        }
        public void AddDataPointArray(DataPoint[] points)
        {
            isActionAddNewItemEventHandler = false;
            foreach (DataPoint point in points)
            {
                AddNewDataPoint(point);
            }
            isActionAddNewItemEventHandler = true;
            ActionAddNewItemEventHandler();
        }
        /// <summary>
        /// Adds a DataPoint object to the end of the collection, with the specified X-value and Y-value(s).
        /// </summary>
        /// <returns>An integer value that represents the zero-based index where the item was inserted into the collection.</returns>
        /// <param name="xValue">The X value of the data point.</param>
        /// <param name="yValue">One or more comma-separated values that represent the Y-value(s) of the data point.</param>
        public int AddXY(float xValue, params float[] yValue)
        {
            DataPoint tmpDataPoint = new DataPoint(xValue, yValue);
            AddNewDataPoint(tmpDataPoint);
            return base.IndexOf(tmpDataPoint);
        }

        /// <summary>
        /// Adds a DataPoint object to the end of the collection, with the specified Y-value.
        /// </summary>
        /// <returns>An integer that represents the zero-based index where the item was inserted into the data point collection.</returns>
        /// <param name="yValue">The Y-value of the data point.</param>
        public int AddY(float yValue)
        {
            DataPoint tmpDataPoint = new DataPoint(FindMaxByValue("X").XValue + 1, yValue);
            AddNewDataPoint(tmpDataPoint);
            return base.IndexOf(tmpDataPoint);
        }

        /// <summary>
        /// Add new data point to the collection and control collection.
        /// </summary>
        /// <param name="tmpDataPoint"></param>
        private void AddNewDataPoint(DataPoint tmpDataPoint)
        {
            base.Add(tmpDataPoint);
            if (autoShift)
                while (this.Items[Count - 1].XValue - this.Items[0].XValue > Width)
                    Items.RemoveAt(0);
        }

        /// <summary>
        /// Adds a DataPoint object to the end of the collection, with the specified Y-value(s).
        /// </summary>
        /// <returns>An integer that represents the location in zero-based index where the item was inserted into the collection.</returns>
        /// <param name="yValue">A comma-separated list of Y-value(s) of the DataPoint object added to the collection.</param>
        public int AddY(params float[] yValue)
        {
            DataPoint tmpDataPoint = new DataPoint(FindMaxByValue("X").XValue + 1, yValue);
            AddNewDataPoint(tmpDataPoint);
            return base.IndexOf(tmpDataPoint);
        }

        /// <summary>
        /// Removes all elements from the System.Collections.ObjectModel.Collection<T>.
        /// </summary>
        protected override void ClearItems()
        {
            base.Clear();
        }

        /// <summary>
        /// Finds the first data point that is equal to the specified value.
        /// </summary>
        /// <returns>The DataPoint that matches the specified value, or null if there is no
        /// match.</returns>
        /// <param name="valueToFind">The data point value to find.</param>
        public DataPoint FindByValue(float valueToFind)
        {
            foreach (DataPoint dp in this)
                if (dp.XValue == valueToFind)
                    return dp;
            return null;
        }

        /// <summary>
        /// Finds the first data point that is equal to the specified value.
        /// </summary>
        /// <returns>The DataPoint that matches the specified value, or null if there is no match.</returns>
        /// <param name="valueToFind">The data point value to find.</param>
        /// <param name="useValue">The data point value to use (X, Y1, Y2...).</param>
        public DataPoint FindByValue(float valueToFind, string useValue)
        {
            foreach (DataPoint dp in this)
                if (dp.GetValueByName(useValue) == valueToFind)
                    return dp;
            return null;
        }

        /// <summary>
        /// Finds the first data point that is equal to the specified value,
        /// starting from the specified index location.
        /// </summary>
        /// <returns> The DataPoint that matches the specified value, or null if there is no match.</returns>
        /// <param name="valueToFind">The data point value to find.</param>
        /// <param name="useValue">The data point value to use (X, Y1, Y2...).</param>
        /// <param name="startIndex">The index location of the data point to start searching from.</param>
        public DataPoint FindByValue(float valueToFind, string useValue, int startIndex)
        {
            for (int index = startIndex; index < this.Count; index++)
                if (this[index].GetValueByName(useValue) == valueToFind)
                    return this[index];
            return null;
        }

        /// <summary>
        /// Finds the data point with the maximum value.
        /// </summary>
        /// <returns>The DataPoint object with the maximum value.</returns>
        public DataPoint FindMaxByValue()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Finds the data point value with the maximum value. 
        /// </summary>
        /// <returns> The DataPoint object with the maximum value.</returns>
        /// <param name="useValue">The data point value to use (X, Y1, Y2...).</param>
        public DataPoint FindMaxByValue(string useValue)
        {
            int maxIndex = 0;
            float maxValue = this[0].YValues[0];
            if (useValue == "Y1")
            {
                for (int i = 0; i < Count; i++)
                {
                    if (this[i].YValues[0] > maxValue)
                    {
                        maxValue = this[i].YValues[0];
                        maxIndex = i;
                    }
                }
                return this[maxIndex];
            }

            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Finds the data point with the maximum value, starting from the specified index location.
        /// </summary>
        /// <returns>The DataPoint object with the maximum value.</returns>
        /// <param name="useValue">The data point value to use (X, Y1, Y2...).</param>
        /// <param name="startIndex">The index location of the data point to start searching from.</param>
        public DataPoint FindMaxByValue(string useValue, int startIndex)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Get the last data point.
        /// </summary>
        /// <returns></returns>
        public DataPoint GetLast()
        {
            return this[Count - 1];
        }

        /// <summary>
        /// Finds the data point with the minimum value. 
        /// </summary>
        /// <returns>The DataPoint object with the minimum value.</returns>
        public DataPoint FindMinByValue()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Finds the data point with the minimum value.
        /// </summary>
        /// <returns>A DataPoint object with the minimum value.</returns>
        /// <param name="useValue">The data point value to use (X, Y1, Y2...)</param>
        public DataPoint FindMinByValue(string useValue)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Finds the data point with the minimum value, starting from the specified index location.
        /// </summary>
        /// <returns>The DataPoint object with the minimum value.</returns>
        /// <param name="useValue">The data point value to use (X, Y1, Y2...).</param>
        /// <param name="startIndex">The index location of the data point to start searching from. </param>
        public DataPoint FindMinByValue(string useValue, int startIndex)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Inserts a data point with the specified X value and one or more specified Y values. 
        /// </summary>
        /// <param name="index">The index at which the data point will be inserted.</param>
        /// <param name="xValue">The X value of the data point. </param>
        /// <param name="yValue">A comma-separated list of Y values of the data point.</param>
        public void InsertXY(int index, float xValue, params float[] yValue)
        {
            this.Insert(index, new DataPoint(xValue, yValue));
        }

        /// <summary>
        /// Inserts a data point with one or more specified Y values.  
        /// </summary>
        /// <param name="index">The index at which the data point will be inserted.</param>
        /// <param name="yValue"> A comma-separated list of Y values of the data point.</param>
        public void InsertY(int index, params object[] yValue)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}