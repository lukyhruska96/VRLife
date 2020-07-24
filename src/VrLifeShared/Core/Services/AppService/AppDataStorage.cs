using Ionic.Zip;
using System;
using System.IO;
using System.Linq;
using VrLifeAPI.Common.Core.Services;

namespace VrLifeShared.Core.Services.AppService
{
    public class AppDataStorage : IAppDataStorage
    {
        private string _path;
        public AppDataStorage(string path)
        {
            _path = path;
        }

        public FileStream GetFile(string filePath, FileMode mode)
        {
            string path = Path.Combine(_path, filePath);
            return File.Open(path, mode);
        }

        public string[] ListFiles(string dir = "")
        {
            string path = Path.Combine(_path, dir);
            return Directory.GetFiles(path).Select(x => Path.GetFileName(x)).ToArray();
        }

        public bool FileExists(string path)
        {
            return File.Exists(Path.Combine(_path, path));
        }

        public bool DirectoryExists(string path)
        {
            return Directory.Exists(Path.Combine(_path, path));
        }

        public string[] ListDirs(string dir = "")
        {
            string path = Path.Combine(_path, dir);
            return Directory.GetDirectories(path).Select(x => Path.GetFileName(x)).ToArray();
        }

        public void FromZipStream(MemoryStream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
            using (ZipFile zip = ZipFile.Read(stream))
            {
                zip.ExtractAll(_path, ExtractExistingFileAction.OverwriteSilently);
            }
        }

        public void FromZipFile(string path)
        {
            using (ZipFile zip = ZipFile.Read(path))
            {
                zip.ExtractAll(_path, ExtractExistingFileAction.OverwriteSilently);
            }
        }

        public void RemoveFile(string path)
        {
            string absolutePath = Path.Combine(_path, path);
            if(File.Exists(absolutePath))
            {
                File.Delete(absolutePath);
            }
        }

        public void RemoveDirectory(string path)
        {
            string absolutePath = Path.Combine(_path, path);
            if(Directory.Exists(absolutePath))
            {
                Directory.Delete(absolutePath, true);
            }
        }

        public byte[] LoadFileChunk(string path, uint chunkSize, uint chunkId)
        {
            if(!FileExists(path))
            {
                return null;
            }
            byte[] data = new byte[chunkSize];
            int readData;
            string absolutePath = Path.Combine(_path, path);
            using (BinaryReader br = new BinaryReader(new FileStream(absolutePath, FileMode.Open)))
            {
                br.BaseStream.Seek(chunkId * chunkSize, SeekOrigin.Begin);
                readData = br.Read(data, 0, data.Length);
            }
            if (readData != data.Length)
            {
                byte[] smallerData = new byte[readData];
                Array.Copy(data, smallerData, readData);
                return smallerData;
            }
            return data;
        }

        public void ClearStorage()
        {
            DirectoryInfo directory = new DirectoryInfo(_path);
            foreach (FileInfo file in directory.GetFiles())
            {
                file.Delete();
            }

            foreach (DirectoryInfo dir in directory.GetDirectories())
            {
                dir.Delete(true);
            }
        }
    }
}
