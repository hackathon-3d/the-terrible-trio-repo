using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualMove
{
    public static class Move
    {
        public static Box FindBox(QRCodeWrapper oQRCode)
        {
            foreach(Box oBox in Boxes)
            {
                if (oBox.QRCode == oQRCode) // This WILL NOT WORK. Need to find a compare method in QR code library that will work
                {
                    return oBox;
                }
            }
            return null;
        }

        public static Collection<Box> Boxes = new Collection<Box>();
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

        // May not be used
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

        // May not be used
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