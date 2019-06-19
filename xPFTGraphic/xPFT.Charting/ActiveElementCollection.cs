using System.Collections.ObjectModel;
using System.Drawing;

namespace xPFT.Charting
{
    public class ActiveElementCollection<T>:Collection<T>
        where T:ActiveElement
    {
        /// <summary>
        /// Get or Set active element with it's name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
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
        /// Check the condition then run the mouse down method for each Active element.
        /// </summary>
        /// <param name="point"></param>
        /// <param name="e"></param>
        public void MouseDown(PointF point, System.Windows.Forms.MouseEventArgs e)
        {
            for (int index = 0; index < Count; index++)
            {
                if (this[index].IsReactionEnable)
                    this[index].OnMouseDown(point, e);
            }
        }

        /// <summary>
        /// Check the condition then run the mouse up method for each Active element.
        /// </summary>
        /// <param name="point"></param>
        /// <param name="e"></param>
        public void MouseUp(PointF point, System.Windows.Forms.MouseEventArgs e)
        {
            for (int index = 0; index < Count; index++)
            {
                if (this[index].IsReactionEnable)
                    this[index].OnMouseUp(point, e);
            }
        }

        /// <summary>
        /// Check the condition then run the mouse move method for each Active element.
        /// </summary>
        /// <param name="point"></param>
        /// <param name="e"></param>
        public void MouseMove(PointF point, System.Windows.Forms.MouseEventArgs e)
        {
            for (int index = 0; index < Count; index++)
            {
                if (this[index].IsReactionEnable)
                    this[index].OnMouseMove(point, e);
            }
        }

        /// <summary>
        /// Check the condition then run the mouse click method for each Active element.
        /// </summary>
        /// <param name="point"></param>
        /// <param name="e"></param>
        public void MouseClick(PointF point, System.Windows.Forms.MouseEventArgs e)
        {
            for (int index = 0; index < Count; index++)
            {
                if (this[index].IsReactionEnable)
                    this[index].OnMouseClick(point, e);
            }
        }

        /// <summary>
        /// Check the condition then run the mouse double click method for each Active element.
        /// </summary>
        /// <param name="point"></param>
        /// <param name="e"></param>
        public void MouseDoubleClick(PointF point, System.Windows.Forms.MouseEventArgs e)
        {
            for (int index = 0; index < Count; index++)
            {
                if (this[index].IsReactionEnable)
                    this[index].OnMouseDoubleClick(point, e);
            }
        }

        /// <summary>
        /// Dispose all items of this collection
        /// </summary>
        public void DisposeAllItems()
        {
            for (int index = 0; index < Count; index++)
            {
                this[index].Dispose();
            }
        }

        /// <summary>
        /// Initialize all items of this collection
        /// </summary>
        public void InitializeAllItems(IDrawing.IDevice device)
        {
            for (int index = 0; index < Count; index++)
            {
                this[index].Initialize(device);
            }
        }
    }
}
