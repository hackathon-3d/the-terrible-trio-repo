using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.Storage;

namespace VisualMove
{
    public static class Move
    {
        public static async Task<Box> FindBox(QRCodeWrapper oQRCode)
        {
            CurrentBox = Boxes.FirstOrDefault(oBox => oBox.QRCode == oQRCode);
            if (CurrentBox == null)
            {
                CurrentBox = new Box(oQRCode);
                await ApplicationData.Current.LocalFolder.CreateFolderAsync(CurrentBox.ImageFolder);
                Boxes.Add(CurrentBox);
            }

            return CurrentBox;
        }

        public static Box CurrentBox = null;

        public static Collection<Box> Boxes = new Collection<Box>();

        public static async void LoadFolders()
        {
            IReadOnlyList<StorageFolder> oFolders = await ApplicationData.Current.LocalFolder.GetFoldersAsync();

            foreach (StorageFolder oFolder in oFolders)
            {
                Boxes.Add(new Box(new QRCodeWrapper(oFolder)));
            }
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
            QRCode = oQRCode;
            Photos = new Collection<PhotoWrapper>();
        }

        public QRCodeWrapper QRCode
        {
            get;
            set;
        }

        public string ImageFolder
        {
            get
            {
                return QRCode.QRCode;
            }
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

        public override bool Equals(object obj)
        {
            Box oBox = obj as Box;
            if (oBox == null)
                return false;

            return oBox.QRCode == QRCode;
        }

        public override int GetHashCode()
        {
            return QRCode.GetHashCode();
        }

        public static bool operator==(Box oBox1, Box oBox2)
        {
            if ((object)oBox1 == null)
            {
                if ((object)oBox2 == null)
                    return true;
                return false;
            }

            return oBox1.Equals(oBox2);
        }

        public static bool operator!=(Box oBox1, Box oBox2)
        {
            return !oBox1.Equals(oBox2);
        }
    }

    public class QRCodeWrapper
    {
        private int m_nCodeHash;

        public QRCodeWrapper(StorageFolder oStorageFolder)
        {
            QRCode = oStorageFolder.Name;
            m_nCodeHash = int.Parse(QRCode);
        }

        public QRCodeWrapper(string sQRCode)
        {
            m_nCodeHash = sQRCode.GetHashCode();
            QRCode = m_nCodeHash.ToString();
        }

        public string QRCode
        {
            get;
            set;
        }

        public override bool Equals(object obj)
        {
            QRCodeWrapper oQRCodeWrapper = obj as QRCodeWrapper;
            if (oQRCodeWrapper == null)
                return false;

            return oQRCodeWrapper.QRCode == QRCode;
        }

        public override int GetHashCode()
        {
            return m_nCodeHash;
        }

        public static bool operator==(QRCodeWrapper oQR1, QRCodeWrapper oQR2)
        {
            if ((object)oQR1 == null)
            {
                if ((object)oQR2 == null)
                    return true;
                return false;
            }
            
            return oQR1.Equals(oQR2);
        }

        public static bool operator!=(QRCodeWrapper oQR1, QRCodeWrapper oQR2)
        {
            return !(oQR1 == oQR2);
        }
    }

    public class PhotoWrapper
    {
        public PhotoWrapper()
        {
            Id = Guid.NewGuid();
        }

        public String FileName
        {
            get
            {
                return String.Format("{0}.jpg", Id.ToString());
            }
        }

        public Guid Id
        {
            get;
            private set;
        }
    }
}