using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace VrLifeAPI.Common.Core.Services
{
    public interface IAppDataStorage
    {
        FileStream GetFile(string filePath, FileMode mode);

        string[] ListFiles(string dir = "");

        bool FileExists(string path);

        bool DirectoryExists(string path);

        string[] ListDirs(string dir = "");

        void FromZipStream(MemoryStream stream);

        void FromZipFile(string path);

        void RemoveFile(string path);

        void RemoveDirectory(string path);

        byte[] LoadFileChunk(string path, uint chunkSize, uint chunkId);

        void ClearStorage();
    }
}
