﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;        //Backgroundworker
using System.Drawing;
using Tobii.Gaze.Core;

namespace LookAndPlayForm
{
    public class MouseController
    {
        public Rectangle monitorBounds = Screen.PrimaryScreen.Bounds;

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(Int32 dwFlags, Int32 dx, Int32 dy, Int32 cButtons, Int32 dwExtraInfo);

        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        public MouseController()
        {
        }

        /// <summary>
        /// Ubica el cursor en la posicion especificada por posx y posy, donde estos valores son valores
        /// absolutos en la pantalla
        /// </summary>
        /// <param name="posx"></param>
        /// <param name="posy"></param>
        /// <returns></returns>
        public bool locateCursor(PointD gazeData)
        {
            if (Double.IsNaN(gazeData.X) || Double.IsNaN(gazeData.Y))
                return false;
            else
            {
                Cursor.Position = posicionMouseFromGazeNormalized(gazeData);
                return true;
            }
        }

        public void click()
        {
            //Call the imported function with the cursor's current position
            int X = Cursor.Position.X;
            int Y = Cursor.Position.Y;
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, X, Y, 0, 0);
        }

        private Point posicionMouseFromGazeNormalized(PointD gazeDataFiltered)
        {
            int posX = Convert.ToInt32(gazeDataFiltered.X * monitorBounds.Size.Width) + monitorBounds.X;
            int posY = Convert.ToInt32(gazeDataFiltered.Y * monitorBounds.Size.Height) + monitorBounds.Y;
            Point posicionMousePx = new Point(posX, posY);
            return posicionMousePx;
        }
        








        private Point posicionMouseFromGazePixel(PointD gazeDataFiltered)
        {
            int posX = Convert.ToInt32(gazeDataFiltered.X * 1) + 0;
            int posY = Convert.ToInt32(gazeDataFiltered.Y * 1) + 0;
            Point posicionMousePx = new Point(posX, posY);
            return posicionMousePx;
        }        
        
        /// <summary>
        /// Se verifica 3 casos:
        ///     1. si es mayor a 1 se iguala a 1
        ///     2. si es menor que 0 se iguala a 0
        ///     3. sino (ambas anteriores) se retorna igual (covertido de double a float)
        /// </summary>
        /// <param name="unvalidF"></param>
        /// <returns></returns>
        private float validateCoordinateF(double unvalidF)
        {
            float validF;

            //1.
            if (unvalidF > 1)
                validF = 1;
            else
            {
                //2.
                if (unvalidF < 0)
                    validF = 0;
                //3.
                else
                    validF = Convert.ToSingle(unvalidF);
            }

            return validF;
        }

        
    }
}