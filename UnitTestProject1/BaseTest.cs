﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Modal;
using Controller;
using System.Collections.Generic;
using System;


namespace Tests
{
  
        [TestClass]
        public class BaseTests
        {
            List<CreatureController> CreatureControllers = new List<CreatureController>();
            MapController Map;
          
          


            [TestInitialize()]
            public void Intialize()
            {
           
                Map = new MapController(100, 100, 10);
                CreatureController.Map = Map;
              
                Random d = new Random();
                var temp = Map.EmpetyCells;
               
                for (int i = 0; i < temp / 100; i++)
                {

                    var x = d.Next(0, Map.Width);
                    var y = d.Next(0, Map.Height);
                    var element = Map[x, y];
                    if (element is null)
                    {
                        CreatureControllers.Add(new CreatureController(x, y));
                    }
                    else
                    {
                        i--;
                    }

                }
            }
        
       

            [TestMethod()]
            public void PositionCheckerTest()
            {
                for (int x = 0; x < Map.Width; x++)
                {
                    for (int y = 0; y < Map.Height; y++)
                    {
                        var element = Map[x, y];

                        if (element is null)
                            continue;

                        Assert.AreEqual(x, element.X);
                        Assert.AreEqual(y, element.Y);



                    }
                }


            }

            [TestMethod()]
            public void NullCountCheckerTest()
            {
                var ForCheckCount = 0;
                 for (int y = 0; y < Map.Height; y++)
                 {
                    for (int x = 0; x < Map.Width; x++)
                    {   
                    
                        var element = Map[x, y];

                        if (element == null)
                            ForCheckCount++;
                    }
                 }
                Assert.AreEqual(Map.EmpetyCells, ForCheckCount);
            }

            [TestMethod()]
            public void PoisonCountCheckerTest()
            {
                var ForCheckCount = 0;
                for (int x = 0; x < Map.Width; x++)
                {
                    for (int y = 0; y < Map.Height; y++)
                    {
                        var element = Map[x, y];

                        if (element is Poison)
                            ForCheckCount++;
                    }
                }
                Assert.AreEqual(Map.PoisonOnMap, ForCheckCount);
            }

            [TestMethod()]
            public void FoodCountCheckerTest()
            {
                var ForCheckCount = 0;
                for (int x = 0; x < Map.Width; x++)
                {
                    for (int y = 0; y < Map.Height; y++)
                    {
                        var element = Map[x, y];

                        if (element is Food)
                            ForCheckCount++;
                    }
                }
                Assert.AreEqual(Map.FoodOnMap, ForCheckCount);
            }

            [TestMethod()]
            public void CreatureCountCheckerTest()
            {
                var ForCheckCount = 0;
                for (int x = 0; x < Map.Width; x++)
                {
                    for (int y = 0; y < Map.Height; y++)
                    {
                        var element = Map[x, y];

                        if (element is CreatureBody)
                            ForCheckCount++;
                    }
                }
                Assert.AreEqual(CreatureControllers.Count, ForCheckCount);
            }

       
        }

    
   

    
}