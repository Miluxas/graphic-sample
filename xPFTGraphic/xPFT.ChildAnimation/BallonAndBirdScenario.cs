/***********************************************************************************
 * This program is design and Implement in Teb Tasvir Co.                                                                      
 * first Version creation date is: 2014/09/20 
 * Ballon and Bird scenario implement class.
 * 
 * Update : 2014/09/29
 * 
 * Update : 2014/10/08 **************************************************************
 * Move birdList from ChildAnimation class to this class
 * Set not static to the bird list.
 * 
 * **********************************************************************************/

using System;
using System.Collections.Generic;
using xPFT.IDrawing;
using System.Windows.Forms;
using System.Drawing;

namespace xPFT.ChildAnimation
{
    class BallonAndBirdScenario:IScenario
    {
        const float SPEED = 0.02f;

        #region Fileds
        /// <summary>
        /// Animation object that shown in this scenario.
        /// </summary>
        private AnimationObject BackGround, Ballon, Bough;
        private List<AnimationObject> BirdsList;
        /// <summary>
        /// Textures of the ballon.
        /// </summary>
        private ITexture txuUperBallon, txuDownerBallon;
        /// <summary>
        /// Flying tate of the birds.
        /// </summary>
        public float StateOfTheBirdsFlying = 0;
        /// <summary>
        /// Determin is ballon hits Birds or not.
        /// </summary>
        public Boolean isBallonHitBirds = false;
        /// <summary>
        /// Previous Height of the ballon.
        /// </summary>
        private float BallonPrevHeight = 0;
        private float PrevValue = 0;

        public Boolean isBallonHitBirdsAndReturnOnBough = false;

        internal List<ITexture> TextureList = new List<ITexture>();
        IDevice device;
        #endregion

        #region Methods
        /// <summary>
        /// Initialize the scenario.
        /// </summary>
        /// <param name="device"></param>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        public void Initialize(IDevice device, int Width, int Height)
        {
            try
            {
                this.device = device;
                BackGround = new AnimationObject(device, Properties.Resources.Background03);
                float BallonPositionX = (Width - Properties.Resources._01b.Width) / 2;
                float BallonPositionY = (Height - Properties.Resources._01b.Height);

                Ballon = new AnimationObject(device,
                    new PointF(BallonPositionX, BallonPositionY * 0.94f),
                    new PointF(BallonPositionX, BallonPositionY * 0.19f), Properties.Resources.B10);

                txuUperBallon = GraphicEngine.GraphicEngine.CreateTexture(device, Properties.Resources._03b);
                txuDownerBallon = GraphicEngine.GraphicEngine.CreateTexture(device, Properties.Resources._01b);

                Bough = new AnimationObject(device, new PointF((Width - Properties.Resources.Bough.Width) / 2, Height - Properties.Resources.Bough.Height), Properties.Resources.Bough);

                BirdsList = new List<AnimationObject>();

                CreateBirdAndAddToScenario(device,Width, Height, 415, 0, 130, 720);
                CreateBirdAndAddToScenario(device, Width, Height, 572, 0, 925, 750);
                CreateBirdAndAddToScenario(device, Width, Height, 555, 80, 755, 735);
                CreateBirdAndAddToScenario(device, Width, Height, 511, 80, 780, 862);
                CreateBirdAndAddToScenario(device, Width, Height, 500, 0, 878, 850);

                CreateBirdAndAddToScenario(device, Width, Height, 336, 80, 50, 670);
                CreateBirdAndAddToScenario(device, Width, Height, 455, 80, 825, 680);
                CreateBirdAndAddToScenario(device, Width, Height, 398, 80, -9, 348);
                CreateBirdAndAddToScenario(device, Width, Height, 300, 0, 75, 455);
                CreateBirdAndAddToScenario(device, Width, Height, 358, 0, 120, 580);
            }
            catch(Exception ex)
            {
                xPFT.Exceptions.ExceptionHandler.LogError(ex);
            }
        }

        /// <summary>
        /// Create bird and add to the scenario.
        /// </summary>
        /// <param name="scenario"></param>
        /// <param name="X1">movement start point X</param>
        /// <param name="Y1">movement start point Y</param>
        /// <param name="X2">movement end point X</param>
        /// <param name="Y2">movement end point Y</param>
        private void CreateBirdAndAddToScenario(IDevice device, int Width, int Height, float X1, float Y1, float X2, float Y2)
        {
            AnimationObject bird = new AnimationObject(device, new PointF(Width * X1 / 1000, Height * Y1 / 1000), new PointF(Width * X2 / 1000, Height * Y2 / 1000), Properties.Resources.B10);
            BirdsList.Add(bird);
        }

        /// <summary>
        /// Update the object position on the scene.
        /// </summary>
        /// <param name="NewValue"></param>
        void UpdatePosition(float NewValue)
        {
            try
            {
                if (NewValue <= 0)
                {
                    NewValue = 0;
                    if (isBallonHitBirds)
                        isBallonHitBirdsAndReturnOnBough = true;
                }


                if (isBallonHitBirdsAndReturnOnBough)
                    if (PrevValue < NewValue)
                        NewValue = 0;

                Ballon.CurrentPoint.X = NewValue * (Ballon.EndPoint.X - Ballon.StartPoint.X) + Ballon.StartPoint.X;
                Ballon.CurrentPoint.Y = NewValue * (Ballon.EndPoint.Y - Ballon.StartPoint.Y) + Ballon.StartPoint.Y;

                if (isBallonHitBirds)
                {
                    if (StateOfTheBirdsFlying <= 1)
                        StateOfTheBirdsFlying += 0.01f;
                }
                if (Ballon.CurrentPoint.Y >= BallonPrevHeight)
                    Ballon.texture = txuDownerBallon;
                else
                    Ballon.texture = txuUperBallon;

                foreach (AnimationObject ao in BirdsList)
                {
                    ao.CurrentPoint.X = ao.StartPoint.X + (ao.EndPoint.X - ao.StartPoint.X) * StateOfTheBirdsFlying;
                    ao.CurrentPoint.Y = ao.StartPoint.Y + (ao.EndPoint.Y - ao.StartPoint.Y) * StateOfTheBirdsFlying;
                    if (StateOfTheBirdsFlying == 0 || StateOfTheBirdsFlying >= 1)
                        ao.texture = TextureList[0];
                    else
                        ao.texture = TextureList[(int)((StateOfTheBirdsFlying * 25) % (TextureList.Count - 1)) + 1];
                    if (ao.StartPoint.X < ao.EndPoint.X)
                        ao.VerticalMirror = true;
                }
                BallonPrevHeight = Ballon.CurrentPoint.Y;
                PrevValue = NewValue;
            }
            catch (Exception ex)
            {
                xPFT.Exceptions.ExceptionHandler.LogError(ex);
            }
        }

        /// <summary>
        /// Update the scene.
        /// </summary>
        void UpdateScene()
        {
            try
            {
                BackGround.Draw();
                Bough.Draw();
                Ballon.Draw();
                foreach (AnimationObject ao in BirdsList)
                {
                    ao.Draw();
                }
            }
            catch (Exception ex)
            {
                xPFT.Exceptions.ExceptionHandler.LogError(ex);
            }
        }

        /// <summary>
        /// Detect is ballon hit birds or not.
        /// </summary>
        void DetectCollision()
        {
            if (Ballon.CurrentPoint.Y !=0 && Ballon.CurrentPoint.Y <= Ballon.EndPoint.Y )
                isBallonHitBirds = true;
        }

        /// <summary>
        /// Update the scenario.
        /// </summary>
        /// <param name="NewValue"></param>
        public void NewSample(float NewValue)
        {
            try
            {
                DetectCollision();
                if (NewValue > 1.4f) NewValue = 1.4f;
                UpdatePosition(NewValue);
                UpdateScene();
            }
            catch (Exception ex)
            {
                xPFT.Exceptions.ExceptionHandler.LogError(ex);
            }
        }

        /// <summary>
        /// Reset the scenario.
        /// </summary>
        public void Reset()
        {
            StateOfTheBirdsFlying = 0;
            isBallonHitBirds = false;
            BallonPrevHeight = 0;
            isBallonHitBirdsAndReturnOnBough = false;
        }

        /// <summary>
        /// Dispose Scenario elements.
        /// </summary>
        public void Dispose()
        {
            try
            {
                BackGround.texture.Dispose();
                Ballon.texture.Dispose();
                txuUperBallon.Dispose();
                txuDownerBallon.Dispose();
                Bough.texture.Dispose();
                for (int i = 0; i < BirdsList.Count; i++)
                {
                    BirdsList[i].texture.Dispose();
                }
                BirdsList.Clear();
            }
            catch { }
        }

        #endregion


        public ScenarioType scenarioType
        {
            get
            {
                return ScenarioType.BALLON_AND_BIRD;
            }
        }


        public void DrawBackGround()
        {
           
        }
    }
}
