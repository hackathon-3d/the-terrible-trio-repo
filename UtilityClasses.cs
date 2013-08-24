using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.Storage;

namespace VisualMove
{
    public static class MoveList
    {
        static MoveList()
        {
            MoveListCollection = new Collection<Move>();
        }

        public static Collection<Move> MoveListCollection
        {
            get;
            set;
        }

        public static Move CurrentMove
        {
            get;
            set;
        }

        public static async Task<Move> FindMove(string sName)
        {
            CurrentMove = MoveListCollection.FirstOrDefault(oMove => oMove.Name == sName);
            if (CurrentMove == null)
            {
                StorageFolder oMoveFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync(sName);
                CurrentMove = new Move(oMoveFolder);
                MoveListCollection.Add(CurrentMove);
            }

            return CurrentMove;
        }

        public static async void LoadFolders()
        {
            MoveListCollection.Clear();
            IReadOnlyList<StorageFolder> oFolders = await ApplicationData.Current.LocalFolder.GetFoldersAsync();

            foreach (StorageFolder oFolder in oFolders)
            {
                Move oMove = new Move(oFolder);
                MoveListCollection.Add(oMove);
                oMove.LoadFolders();
            }

            //TODO:  Remove this:
            CurrentMove = await MoveList.FindMove("TempMove");
        }
    }

    public class Move
    {
        public Move(StorageFolder oFolder)
        {
            MoveFolder = oFolder;
        }

        public async Task<Box> FindBox(QRCodeWrapper oQRCode)
        {
            CurrentBox = Boxes.FirstOrDefault(oBox => oBox.QRCode == oQRCode);
            if (CurrentBox == null)
            {
                CurrentBox = new Box(this, oQRCode);
                await MoveFolder.CreateFolderAsync(CurrentBox.ImageFolder);
                Boxes.Add(CurrentBox);
            }

            return CurrentBox;
        }

        public string Name
        {
            get
            {
                return MoveFolder.Name;
            }
        }

        public StorageFolder MoveFolder
        {
            get;
            private set;
        }

        public Box CurrentBox = null;

        public Collection<Box> Boxes = new Collection<Box>();

        public async void LoadFolders()
        {
            Boxes.Clear();
            IReadOnlyList<StorageFolder> oFolders = await MoveFolder.GetFoldersAsync();

            foreach (StorageFolder oFolder in oFolders)
            {
                //If a QR code has no images associated, we kill the folder
                if ((await oFolder.GetFilesAsync()).Count == 0)
                {
                    await oFolder.DeleteAsync();
                }

                Boxes.Add(new Box(this, new QRCodeWrapper(oFolder)));
            }
        }
    }

    public class Box
    {
        public Box(Move oMove, QRCodeWrapper oQRCode)
        {
            AssociatedMove = oMove;
            QRCode = oQRCode;
        }

        public Move AssociatedMove
        {
            get;
            private set;
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
            if ((object)oBox1 == null)
            {
                if ((object)oBox2 == null)
                    return false;
                return true;
            }

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
}