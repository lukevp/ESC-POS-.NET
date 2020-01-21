using ESCPOS_NET.Events;
using System;
using System.IO;

namespace ESCPOS_NET
{
    public class FilePrinter : BasePrinter
    {
        #region Private Members

        private readonly string _path;
        private FileStream _file;

        #endregion

        #region Constructor

        // TODO: default values to their default values in ctor.
        public FilePrinter(string filePath) : base()
        {
            this._path = filePath;
            this.InitPrinter();
        }

        #endregion

        protected override void InitPrinter()
        {
            try
            {
                this._file = File.Open(this._path, FileMode.Open);
                this._writer = new BinaryWriter(_file);
                this._reader = new BinaryReader(_file);
            }
            catch (Exception ex)
            {
                OnStatusChanged(this, new StatusUpdateEventArgs()
                {
                    Message = ex.Message,
                    UpdateType = StatusEventType.Error
                });
            }
        }

        public override void Write(byte[] bytes)
        {
            if (!this._file.CanWrite || !this._writer.BaseStream.CanWrite)
            {
                this.InitPrinter();
            }

            base.Write(bytes);

            this._writer.Close();
            this._file.Close();
        }

        #region Dispose

        private bool _disposed = false;

        protected override void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                this._file?.Close();
                this._file?.Dispose();
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}