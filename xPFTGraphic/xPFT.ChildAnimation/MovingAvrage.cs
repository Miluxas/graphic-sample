/***********************************************************************************
 * This program is design and Implement in Teb Tasvir Co.                                                                      
 * first Version creation date is: 2014/09/29 
 * Moving avrage implement class.
 * 
 * Update : 2014/09/29
 * **********************************************************************************/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xPFT.ChildAnimation
{
    /// <summary>
    /// Moving Avrage class.
    /// </summary>
    public class MovingAvrage
    {
        #region Constant
        /// <summary>
        /// Lenght of the Moving Avrage List.
        /// </summary>
        public const int MOVING_AVG_LENGHT =3;
        #endregion

        #region Fileds
        /// <summary>
        /// List of the samples.
        /// </summary>
        static List<float> sampleList = new List<float>();
        #endregion

         #region Methods
        /// <summary>
        /// Add new sample to list.
        /// </summary>
        /// <param name="newSample"></param>
        /// <returns>New Moving avrage value.</returns>
        static public float Clock(float newSample, UInt16 Length)
        {
            sampleList.Add(newSample);
            if (sampleList.Count > Length)
                sampleList.RemoveAt(0);
            return sampleList.Average()+Math.Abs(sampleList.Max()-sampleList.Min())/2;
        }

        /// <summary>
        /// Reset the Moving avrage count.
        /// </summary>
        static public void Reset()
        {
            sampleList.Clear();
        }
        #endregion

    }
}
