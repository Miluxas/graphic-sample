/***********************************************************************************
 * This program is design and Implement in Teb Tasvir Co.                                                                      
 * first Version creation date is: 2014/09/20 
 * Apple and Bird scenario implement class.
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
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace xPFT.ChildAnimation
{
    class AppleAndBirdScenario:IScenario
    {

        const float SPEED = 0.02f;

        #region Fileds
        /// <summary>
        /// Animation object that shown in this scenario.
        /// </summary>
        private AnimationObject BackGround, Apple, Boy, BoyHand;
        private List<AnimationObject> BirdsList;
        /// <summary>
        /// Textures of the Boy.
        /// </summary>
        private ITexture BoySadTexture, BoyHappyTexture, BoySuccessTexture;
        /// <summary>
        /// Flying tate of the birds.
        /// </summary>
        public float StateOfTheBirdsFlying = 0;
        /// <summary>
        /// Determin is apple hits Birds or not.
        /// </summary>
        public Boolean isAppleHitBirds = false;
        public Boolean isAppleHitBirdAndReturnToBoyHand = false;

        private float PrevValue = 0;

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
                            
                BackGround = new AnimationObject(device, Properties.Resources.AppleBack);

                Apple = new AnimationObject(device,
                    new PointF((Width - Properties.Resources.Apple.Width) / 2, (Height - Properties.Resources.Apple.Height) * 0.871f),
                    new PointF((Width - Properties.Resources.Apple.Width) / 2, (Height - Properties.Resources.Apple.Height) * 0.18f),
                    Properties.Resources.Apple);

                PointF BoyPosition = new PointF((Width - Properties.Resources.Boy.Width) / 2, Height - Properties.Resources.Boy.Height);
                Boy = new AnimationObject(device, BoyPosition);
                BoySadTexture = GraphicEngine.GraphicEngine.CreateTexture(device, Properties.Resources.BoyDep);
                BoyHappyTexture = GraphicEngine.GraphicEngine.CreateTexture(device, Properties.Resources.Boy);
                BoySuccessTexture = GraphicEngine.GraphicEngine.CreateTexture(device, Properties.Resources.Boy2);

                PointF BoyHandPosition = new PointF(60+BoyPosition.X, 52+BoyPosition.Y);
                BoyHand = new AnimationObject(device, BoyHandPosition, Properties.Resources.hand);
                BoyHand.RotateOrigin = new PointF(0.1f, 0.92f);
                BoyHand.RotateAngle = 0.046f;
                BirdsList = new List<AnimationObject>();

                //! Create and add to list birds.
                CreateBirdAndAddToScenario(device,Width, Height, 315, 5, 783, 828);
                CreateBirdAndAddToScenario(device, Width, Height, 670, 95, 190, 880);
                CreateBirdAndAddToScenario(device, Width, Height, 400, 3, 736, 908);
                CreateBirdAndAddToScenario(device, Width, Height, 455, 2, 806, 903);
                CreateBirdAndAddToScenario(device, Width, Height, 612, 95, 869, 920);

                CreateBirdAndAddToScenario(device, Width, Height, 563, 90, 240, 770);
                CreateBirdAndAddToScenario(device, Width, Height, 355, 4, 253, 860);
                CreateBirdAndAddToScenario(device, Width, Height, 511, 90, 882, 731);
                CreateBirdAndAddToScenario(device, Width, Height, 460, 89, 682, 900);
                CreateBirdAndAddToScenario(device, Width, Height, 258, 5, 850, 823);
            }
            catch (Exception ex)
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
                    if (isAppleHitBirds)
                        isAppleHitBirdAndReturnToBoyHand = true;
                }

                if (isAppleHitBirdAndReturnToBoyHand)
                    if (PrevValue < NewValue)
                        NewValue = 0;

                Apple.CurrentPoint.X = NewValue * (Apple.EndPoint.X - Apple.StartPoint.X) + Apple.StartPoint.X;
                Apple.CurrentPoint.Y = NewValue * (Apple.EndPoint.Y - Apple.StartPoint.Y) + Apple.StartPoint.Y;
                Apple.RotateAngle = NewValue * 0.25f;

                if (Apple.CurrentPoint.Y > Apple.StartPoint.Y - 100)
                {
                    float rate = (Apple.StartPoint.Y - Apple.CurrentPoint.Y) / Apple.StartPoint.Y - (float)(0.2);
                    BoyHand.RotateAngle = -(float)(rate * Math.PI / 12);
                }
                if (isAppleHitBirds)
                {
                    if (StateOfTheBirdsFlying <= 1)
                        StateOfTheBirdsFlying += 0.01f;
                    if (StateOfTheBirdsFlying > 0.9f)
                        Boy.texture = BoySuccessTexture;
                    else
                        Boy.texture = BoyHappyTexture;
                }
                else
                    Boy.texture = BoySadTexture;

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
                BoyHand.Draw();
                Apple.Draw();
                Boy.Draw();
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
        /// Detect is apple hit birds or not.
        /// </summary>
        void DetectCollision()
        {
            if (Apple.CurrentPoint.Y != 0 && Apple.CurrentPoint.Y <= Apple.EndPoint.Y+200)
                isAppleHitBirds = true;
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
            isAppleHitBirds = false;
            isAppleHitBirdAndReturnToBoyHand = false;
        }

        /// <summary>
        /// Dispose Scenario elements.
        /// </summary>
        public void Dispose()
        {
            try
            {
                BackGround.texture.Dispose();
                Apple.texture.Dispose();
                Boy.texture.Dispose();
                BoySadTexture.Dispose();
                BoyHappyTexture.Dispose();
                BoySuccessTexture.Dispose();
                BoyHand.texture.Dispose();

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
                return ScenarioType.APPLE_AND_BIRD;
            }
        }


        public void DrawBackGround()
        {
            
        }
    }
}
