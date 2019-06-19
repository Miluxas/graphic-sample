/***********************************************************************************
 * This program is design and Implement in Teb Tasvir Co.                                                                      
 * first Version creation date is: 2014/09/20 
 * The base class in child animation control design is this class.
 * contain a Animation object properties and methods and fields.
 * 
 * Update : 2014/09/29
 * **********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xPFT.IDrawing;
using System.Drawing;
namespace xPFT.ChildAnimation
{
    class AnimationObject
    {
        #region Constructors

        public AnimationObject(IDevice device, PointF start_Point, PointF end_Point, System.Drawing.Bitmap image)
        {
            startPoint = start_Point;
            endPoint = end_Point;
            imageSize = new Size(image.Size.Width, image.Size.Height);
            texture = GraphicEngine.GraphicEngine.CreateTexture(device, image);
        }

        public AnimationObject(IDevice device, PointF current_Point, System.Drawing.Bitmap image)
        {
            CurrentPoint = current_Point;
            imageSize = new Size(image.Size.Width, image.Size.Height);
            texture = GraphicEngine.GraphicEngine.CreateTexture(device, image);
        }

        public AnimationObject(IDevice device, PointF current_Point)
        {
            CurrentPoint = current_Point;
        }

        public AnimationObject(IDevice device, System.Drawing.Bitmap image)
        {
            imageSize = new Size(image.Size.Width, image.Size.Height);
            texture = GraphicEngine.GraphicEngine.CreateTexture(device, image);
            
        }

        #endregion

        #region Fields
        /// <summary>
        /// current point of element.
        /// </summary>
        public PointF CurrentPoint;
        /// <summary>
        /// Texture of the element.
        /// </summary>
        public ITexture texture;
        /// <summary>
        /// Size of the object image.
        /// </summary>
        private Size imageSize;
        /// <summary>
        /// Determine is object image mirrored verticaly or not.
        /// </summary>
        public Boolean VerticalMirror = false;

        #endregion

        #region Properties
        PointF rotateOrigin = new PointF(0.5f, 0.5f);
        /// <summary>
        /// Set the rotate origin.
        /// </summary>
        public PointF RotateOrigin
        {
            set
            {
                rotateOrigin = value;
            }
        }

        protected PointF endPoint;
        /// <summary>
        /// Get the end point of the object movement.
        /// </summary>
        public PointF EndPoint
        {
            get
            {
                return endPoint;
            }
        }

        protected PointF startPoint;
        /// <summary>
        /// Get the start point of the object movement.
        /// </summary>
        public PointF StartPoint
        {
            get
            {
                return startPoint;
            }
        }

        private float rotateAngle;
        /// <summary>
        /// Get or set the Rotete origin of the z index object rotate.
        /// </summary>
        public float RotateAngle
        {
            set
            {
                rotateAngle = value;
            }
            get
            {
                return rotateAngle;
            }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Set element style and position as for value.
        /// </summary>
        public void Draw()
        {
            try
            {
                texture.Draw(0,CurrentPoint, rotateAngle, rotateOrigin,VerticalMirror);
            }
            catch (Exception ex)
            {
                xPFT.Exceptions.ExceptionHandler.LogError(ex);
            }
        }

        /// <summary>
        /// Dispose Object.
        /// </summary>
        public void Dispose()
        {
            if (texture != null)
                texture.Dispose();
        }

        #endregion

    }
}
