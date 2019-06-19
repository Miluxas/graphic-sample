/*************************************************************************************
 * This program is design and Implement in Teb Tasvir Co.                                                                      
 * first Version creation date is: 2014/09/20                                                                                  
 * This file contain xPFT.ChildAnimation class that is the main class in this DLL        
 * 
 * Update : 2014/09/29
 * 
 * Update : 2014/10/08 **************************************************************
 * Move birdList from this class to each scenarios classes.
 * Set not static to the bird lists.
 * 
 * **********************************************************************************/

using System;
using System.Windows.Forms;
using System.Collections.Generic;
using xPFT.GraphicEngine;
using System.Reflection;

namespace xPFT.ChildAnimation
{
    public enum ScenarioType
    {
        APPLE_AND_BIRD=0,
        BALLON_AND_BIRD=1
    }
    /// <summary>
    /// Used to animate the selected Scenario.
    /// </summary>
    public partial class ChildAnimation :UserControl
    {
        #region Contructors
        /// <summary>
        /// The xPFT.ChildAnimation's class Contructor.
        /// create an object and initialize the component and scene.
        /// </summary>
        public ChildAnimation()
        {
            try
            {
                InitializeComponent();
                InitializeDevice();
                InitializeScene();
                MovingAvrage.Reset();
            }
            catch (Exception ex)
            {
                xPFT.Exceptions.ExceptionHandler.LogError(ex);
            }
        }
        #endregion

        #region Fields
        /// <summary>
        /// DirectX3D9 device.
        /// </summary>
        private IDrawing.IDevice device=GraphicEngine.GraphicEngine.CreateDevice();
        /// <summary>
        /// Scenarios.
        /// </summary>
        private AppleAndBirdScenario appleAndBirdScenario;
        private BallonAndBirdScenario ballonAndBirdScenario;
        /// <summary>
        /// Texture of the Flutter levels.
        /// </summary>
        ScenarioType currentScenario = ScenarioType.APPLE_AND_BIRD;
        IScenario curScenario;
        #endregion

        #region Propereties
        public bool childAnimationMovingAveEnable = true;
        /// <summary>
        /// Child animation moving filter implementation enable flag.
        /// </summary>
        public bool ChildAnimationMovingAveEnable 
        { 
            get
            {
                return childAnimationMovingAveEnable;
            }
            set
            {
                childAnimationMovingAveEnable = value;
            }
        }

        private UInt16 childAnimationMovingAveLength = MovingAvrage.MOVING_AVG_LENGHT;
        /// <summary>
        /// Child animation moving averation filter length.
        /// </summary>
        public UInt16 ChildAnimationMovingAveLength 
        { 
            get
            {
                return childAnimationMovingAveLength;
            }
            set
            {
                childAnimationMovingAveLength =value;
            }
        }

        ScenarioType defaultScenario;
        public ScenarioType DefaultScenario
        {
            set
            {
                defaultScenario = value;
                currentScenario = defaultScenario;
            }
            get
            {
                return defaultScenario;
            }
        }
        #endregion

        #region Methodes

        /// <summary>
        /// Initialize the current scene 
        /// set Device present parametrs
        /// loading texture and set the element start position 
        /// </summary>
        private void InitializeScene()
        {
            try
            {
                //! Create scenarios.
                ballonAndBirdScenario = new BallonAndBirdScenario();
                appleAndBirdScenario = new AppleAndBirdScenario();
                curScenario = appleAndBirdScenario;
                //! Initialize the scenarios.
                appleAndBirdScenario.Initialize(device, Width, Height);
                ballonAndBirdScenario.Initialize(device, Width, Height);
                //! Add bird flutter step texture to the TextureList.
                ballonAndBirdScenario.TextureList.Add(GraphicEngine.GraphicEngine.CreateTexture(device, Properties.Resources.B10));
                ballonAndBirdScenario.TextureList.Add(GraphicEngine.GraphicEngine.CreateTexture(device, Properties.Resources.B11));
                ballonAndBirdScenario.TextureList.Add(GraphicEngine.GraphicEngine.CreateTexture(device, Properties.Resources.B12));
                ballonAndBirdScenario.TextureList.Add(GraphicEngine.GraphicEngine.CreateTexture(device, Properties.Resources.B13));
                ballonAndBirdScenario.TextureList.Add(GraphicEngine.GraphicEngine.CreateTexture(device, Properties.Resources.B14));

                appleAndBirdScenario.TextureList.Add(GraphicEngine.GraphicEngine.CreateTexture(device, Properties.Resources.B10));
                appleAndBirdScenario.TextureList.Add(GraphicEngine.GraphicEngine.CreateTexture(device, Properties.Resources.B11));
                appleAndBirdScenario.TextureList.Add(GraphicEngine.GraphicEngine.CreateTexture(device, Properties.Resources.B12));
                appleAndBirdScenario.TextureList.Add(GraphicEngine.GraphicEngine.CreateTexture(device, Properties.Resources.B13));
                appleAndBirdScenario.TextureList.Add(GraphicEngine.GraphicEngine.CreateTexture(device, Properties.Resources.B14)); 


            }
            catch (Exception ex)
            {
                xPFT.Exceptions.ExceptionHandler.LogError(ex);
            }
        }
        
        /// <summary>
        /// Convert Bitmap image to the Texture.
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>


        /// <summary>
        /// Initialize the control's properties.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // ChildAnimation
            // 
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.MaximumSize = new System.Drawing.Size(1000, 1000);
            this.MinimumSize = new System.Drawing.Size(200, 200);
            this.Name = "ChildAnimation";
            this.Size = new System.Drawing.Size(1000, 1000);
            this.ResumeLayout(false);

        }

        /// <summary>
        /// Reset the Child animation. 
        /// </summary>
        public void Reset()
        {
            MovingAvrage.Reset();
            ballonAndBirdScenario.Reset();
            appleAndBirdScenario.Reset();
        }

        protected override void Dispose(bool disposing)
        {
            ballonAndBirdScenario.Dispose();
            appleAndBirdScenario.Dispose();
            device.Dispose();
            base.Dispose(disposing);
            GC.Collect();
        }

        /// <summary>
        /// Create Device.
        /// </summary>
        private void InitializeDevice()
        {
            try
            {
                device.Initialize(this);
            }
            catch (Exception ex)
            {
                //! Log to the Log error file.
                xPFT.Exceptions.ExceptionHandler.LogError(ex);
                throw new xPFT.Exceptions.ChildAnimation.NotCreateDeviceException("Create device error.\nPlease check the display adapter driver.");
            }
        }

        /// <summary>
        /// change the object style and location according to the new values.
        /// </summary>
        /// <param name="values"></param>
        public void NewSample(float values)
        {
            try
            {
                if(ChildAnimationMovingAveEnable)
                    values = MovingAvrage.Clock(values,ChildAnimationMovingAveLength);
                if (values > 0.9f)
                    values = 1;
                device.BeginLayerDraw(0);
                //! Select the scenario.
                curScenario.NewSample(values);
            }
            catch (Exception ex)
            {
                xPFT.Exceptions.ExceptionHandler.LogError(ex);
            }
            finally
            {
                if (device != null)
                {
                    device.EndLayerDraw(0);
                    device.Present();
                }
            }
        }

        /// <summary>
        /// Change the scenario.
        /// </summary>
        public void ChangeScenario()
        {
            if (curScenario.scenarioType == ScenarioType.BALLON_AND_BIRD)
            {
                curScenario = appleAndBirdScenario;
                //currentScenario = ScenarioType.APPLE_AND_BIRD;
                appleAndBirdScenario.isAppleHitBirds = ballonAndBirdScenario.isBallonHitBirds;
                appleAndBirdScenario.StateOfTheBirdsFlying = ballonAndBirdScenario.StateOfTheBirdsFlying;
                appleAndBirdScenario.isAppleHitBirdAndReturnToBoyHand = ballonAndBirdScenario.isBallonHitBirdsAndReturnOnBough;
            }
            else
            {
                curScenario = ballonAndBirdScenario;
                //currentScenario = ScenarioType.BALLON_AND_BIRD;
                ballonAndBirdScenario.isBallonHitBirds = appleAndBirdScenario.isAppleHitBirds;
                ballonAndBirdScenario.StateOfTheBirdsFlying = appleAndBirdScenario.StateOfTheBirdsFlying;
                ballonAndBirdScenario.isBallonHitBirdsAndReturnOnBough = appleAndBirdScenario.isAppleHitBirdAndReturnToBoyHand;
            }
        }

        #endregion
    }
}
