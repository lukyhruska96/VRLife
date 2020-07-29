using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace VrLifeAPI.Common.Core.Services
{
    /// <summary>
    /// Interface na uložiště přiřazené dané aplikaci.
    /// 
    /// V případě aplikačního balíčku tato složka obsahuje
    /// rozbalená data z .zip souboru.
    /// </summary>
    public interface IAppDataStorage
    {
        /// <summary>
        /// Získání streamu na daný soubor v relativní cestě.
        /// </summary>
        /// <param name="filePath">Cesta k souboru uvnitř dané složky.</param>
        /// <param name="mode">Režim otevření souboru.</param>
        /// <returns>FileStream na daný souboru.</returns>
        FileStream GetFile(string filePath, FileMode mode);

        /// <summary>
        /// Seznam souborů v dané úrovni.
        /// </summary>
        /// <param name="dir">cesta k prohledání uvnitř přiřazené složky.</param>
        /// <returns>Seznam názvů souborů.</returns>
        string[] ListFiles(string dir = "");

        /// <summary>
        /// Kontrola, zda soubor v dané cestě existuje.
        /// </summary>
        /// <param name="path">Cesta k souboru uvnitř přiřazené složky.</param>
        /// <returns>true - existuje, false - neexistuje</returns>
        bool FileExists(string path);

        /// <summary>
        /// Kontrola, zda složka v dané cestě existuje.
        /// </summary>
        /// <param name="path">Cesta ke složce uvnitř přiřazené složky.</param>
        /// <returns>true - existuje, false - neexistuje</returns>
        bool DirectoryExists(string path);

        /// <summary>
        /// Seznam složek v dané úrovni.
        /// </summary>
        /// <param name="dir">cesta k prohledání uvnitř přiřazené složky.</param>
        /// <returns>Seznam názvů složek.</returns>
        string[] ListDirs(string dir = "");

        /// <summary>
        /// Rozbalení Zip souboru ve formě MemoryStreamu
        /// </summary>
        /// <param name="stream">MemoryStream zip souboru.</param>
        void FromZipStream(MemoryStream stream);

        /// <summary>
        /// Rozbalení Zip souboru ve formě cesty k němu.
        /// </summary>
        /// <param name="path">Cesta k zip souboru k rozbalení.</param>
        void FromZipFile(string path);

        /// <summary>
        /// Smazání souboru.
        /// </summary>
        /// <param name="path">Cesta k souboru uvnitř přiřazené složky.</param>
        void RemoveFile(string path);

        /// <summary>
        /// Rekurzivní smazání složky.
        /// </summary>
        /// <param name="path">Cesta ke složce.</param>
        void RemoveDirectory(string path);

        /// <summary>
        /// Načtení pouze části souboru ve formě byte array.
        /// 
        /// V případě, že vrácené pole je menší, než-li očekáváné,
        /// jedná se o konec souboru.
        /// </summary>
        /// <param name="path">Cesta k souboru.</param>
        /// <param name="chunkSize">Velikost k načtení.</param>
        /// <param name="chunkId">ID části o velikosti chunkSize.</param>
        /// <returns>Načtená data v daném místě.</returns>
        byte[] LoadFileChunk(string path, uint chunkSize, uint chunkId);

        /// <summary>
        /// Smazání obsahu přiřazené složky.
        /// </summary>
        void ClearStorage();
    }
}
