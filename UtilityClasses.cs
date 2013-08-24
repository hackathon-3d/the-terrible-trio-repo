﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualMove
{
    public class Move
    {
        public Move()
        {
            Boxes = new Collection<Box>();
        }

        public Box FindBox(QRCodeWrapper QRCode)
        {
            foreach(Box oBox in Boxes)
            {
                if (oBox.QRCode == QRCode) // This WILL NOT WORK. Need to find a compare method in QR code library that will work
                {
                    return oBox;
                }
            }
            return null;
        }

        public Collection<Box> Boxes 
        {
            get;
            set;
        }
    }

    public class Box
    {
        public Box()
        {
            Photos = new Collection<PhotoWrapper>();
        }

        public Box(QRCodeWrapper oQRCode)
        {
            Photos = new Collection<PhotoWrapper>();
        }

        public QRCodeWrapper QRCode
        {
            get;
            set;
        }

        public Collection<PhotoWrapper> Photos
        {
            get;
            set;
        }

        public PhotoWrapper Scroll(bool bForward)
        {
            if(bForward == true)
            {
                if (Photos.IndexOf(CurrentPhoto) + 1 < Photos.Count)
                {
                    CurrentPhoto = Photos[Photos.IndexOf(CurrentPhoto) + 1];
                }
                else
                {
                    CurrentPhoto = Photos[0];
                }
            }
            else
            {
                if (Photos.IndexOf(CurrentPhoto) - 1 >= 0)
                {
                    CurrentPhoto = Photos[Photos.IndexOf(CurrentPhoto) - 1];
                }
                else
                {
                    CurrentPhoto = Photos[Photos.Count - 1];
                }
            }
            return CurrentPhoto;
        }

        public PhotoWrapper CurrentPhoto
        {
            get;
            set;
        }
    }

    public class QRCodeWrapper
    {
        public QRCodeWrapper()
        { }
    }

    public class PhotoWrapper
    {
        public PhotoWrapper()
        { }
    }
}