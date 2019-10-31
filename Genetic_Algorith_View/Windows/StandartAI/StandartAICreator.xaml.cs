﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using Controller;
using Modal;

namespace Genetic_Algorith_View.Windows
{
    /// <summary>
    /// Логика взаимодействия для StandartAICreator.xaml
    /// </summary>
    public partial class StandartAICreator : Window
    {
        //For real time testing
        System.Timers.Timer timer;
        //In ms
        const double SpeedOfTesting = 500;

        private int[] BrainArray = new int[64];
        SpecialWorldController WorldController;
        const int WidthMap = 30, HeightMap = 30;

    
        public StandartAICreator()
        {
            InitializeComponent();

            for (int i = 0; i < 64; i++)
            {
                var image = new Grid();
                
             
                LogicBlocksGrid.Children.Add(image);
                image.MouseDown += ImageClick;
                image.Tag = i;
            }
            RandomizeBlocks();
            WorldController = new SpecialWorldController(new CreatureController(BrainArray), new MapController(WidthMap, HeightMap, Guid.NewGuid().GetHashCode()));
            
            WorldController.GetType().GetProperty("MinFood").SetValue(WorldController, 20, null);
            WorldController.GetType().GetProperty("MinPoison").SetValue(WorldController, 20, null);

            FullReDraw();
            WorldController.RestartEvent += (o, e) => FullReDraw();
            App.CurrentMain = this;
            LogicBlocksGrid.Columns = 8;
            LogicBlocksGrid.Rows = 8;

            


            //For real time testing
            timer = new System.Timers.Timer(SpeedOfTesting);
            timer.Elapsed += RealTimeTesting;

            WorldController.SpecialInfoChanged += (o, a) => new Thread(() => ChangeInfos()).Start(); 

            for (int i = 0; i < WidthMap*HeightMap; i++)
            {
                var rect = new Rectangle();              
                VisualRealTimeTestingGrid.Children.Add(rect);

            }
            VisualRealTimeTestingGrid.Columns = WidthMap;
            VisualRealTimeTestingGrid.Rows = HeightMap;

           

        }

        private void RealTimeTesting(object o, object f)
        {
           
                WorldController.WorldLive(ReDrawMap);    
        }

        private void ChangeInfos()
        {
            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                        (ThreadStart)delegate ()
                        {
                           EatsAllFoodLabel.Content= WorldController.EatsAllAvinableFood;
                            FindInfinityLoopsLabel.Content = WorldController.HaveLoops;
                        });
        }

        private void ReDrawMap(int x,int y)
        {
            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                        (ThreadStart)delegate ()
                        {
                            var rect = VisualRealTimeTestingGrid.Children[x + y * WidthMap] as Rectangle;

                            WorldObject element = WorldController.CurrentMap[x, y];



                            if (element is Wall)
                            {
                                rect.Fill = new SolidColorBrush(Color.FromRgb(128, 128, 128));
                            }
                            else if (element is CreatureBody)
                            {
                                rect.Fill = new SolidColorBrush(Color.FromRgb(0, 0, 255));
                            }
                            else if (element is Food)
                            {
                                rect.Fill = new SolidColorBrush(Color.FromRgb(0, 255, 0));
                            }
                            else if (element is Poison)
                            {
                                rect.Fill = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                            }
                            else if (element is null)
                            {
                                rect.Fill = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                            }
                            else
                            {
                                //This was all types I dnk what to do
                                throw new Exception();
                            }
                        });
        }

        private void ImageClick(object sender,MouseEventArgs e)
        {
            var choosingwindow = new ChooseLogicBlock();
            var it = sender as Grid;
            choosingwindow.ChoosedItemCode += (s, i) => { SetCode((int)it.Tag, i); };
            choosingwindow.ShowDialog();


            choosingwindow.Close();
        }

        private void RandomizeBlocks()
        {

           

            Random rnd = new Random();
            
            for (int i = 0; i < 64; i++)
            {
                
              
                var code = rnd.Next(0, 64);
                var element = LogicBlocksGrid.Children[i] as Grid;

                //Do not use SetCOde for optimization
                #region SetCodeCopy
                Brush brush = new ImageBrush();


              
                element.Children.Clear();
                if (code < 8)
                {
                    brush = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/StandartAI/Rotate" + code + ".png")));
                }
                else if (code < 16)
                {
                    brush = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/StandartAI/See" + (code - 8) + ".png")));

                }
                else if (code < 24)
                {
                    brush = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/StandartAI/Move" + (code - 16) + ".png")));

                }
                else if (code < 32)
                {
                    brush = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/StandartAI/Catch" + (code - 24) + ".png")));
                }
                else
                {
                    brush = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                    element.Children.Add(new Label()
                    {
                        Content = code,
                        HorizontalContentAlignment = HorizontalAlignment.Center,
                        VerticalContentAlignment = VerticalAlignment.Center,
                        FontSize = FontSize * 1.5
                    });
                }



                BrainArray[i] = code;
                element.Background = brush;
                #endregion

            }
        }

        private void SetCode(int index,int code)
        {
            /*
         * 0-7 Rotate
         * 8-15 See
         * 16-23 Move
         * 23-31 Catch
         */

            Brush brush = new ImageBrush();

           
            var element = LogicBlocksGrid.Children[index] as Grid;
            element.Children.Clear();
            if (code < 8)
            {
                brush = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/StandartAI/Rotate" + code + ".png")));
            }
            else if (code < 16)
            {
                brush = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/StandartAI/See" + (code - 8) + ".png")));

            }
            else if (code < 24)
            {
                brush = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/StandartAI/Move" + (code - 16) + ".png")));

            }
            else if (code < 32)
            {
                brush = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/StandartAI/Catch" + (code - 24) + ".png")));
            }
            else
            {
                brush = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                element.Children.Add(new Label()
                {
                    Content = code,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    FontSize = FontSize * 1.5
                });
            }



            BrainArray[index] = code;
            element.Background = brush;
            WorldController.ChangeSubject(BrainArray);
          

        
           
            
        }

        private void FullReDraw()
        {
            for (int x = 0; x < WidthMap; x++)
            {
                for (int y = 0; y < HeightMap; y++)
                {
                    ReDrawMap(x, y);
                }
            }

        }

        private void CheckBoxRect_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var rect = sender as Rectangle;
            if (timer.Enabled)
            {
                rect.Fill = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                CheckBoxStateLabel.Content = "OFF";

            }
            else
            {
                rect.Fill = new SolidColorBrush(Color.FromRgb(0, 255, 0));
                CheckBoxStateLabel.Content = "ON";
            }
            timer.Enabled = !timer.Enabled;
        }
    }
}
