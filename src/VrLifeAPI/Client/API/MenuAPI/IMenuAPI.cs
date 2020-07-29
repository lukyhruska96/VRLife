using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VrLifeAPI.Client.Applications.MenuApp.MenuItems;

namespace VrLifeAPI.Client.API.MenuAPI
{
    /// <summary>
    /// API pro komunikaci s Menu rozhraním
    /// </summary>
    public interface IMenuAPI
    {
        /// <summary>
        /// Spustí Coroutine běžným způsobem.
        /// Užitečné v případě práce s Unity strukturou,
        /// které jsou výhradně povoleny pouze v hlavním vlákně.
        /// </summary>
        /// <param name="coroutine">Coroutine ke spuštění.</param>
        /// <returns>Identifikační číslo dané coroutine pro možné zastavení.</returns>
        ulong StartCoroutine(IEnumerator coroutine);

        /// <summary>
        /// Getter běžné Unity yield return podmínky WaitForSeconds.
        /// </summary>
        /// <param name="sec">Počet sekund.</param>
        /// <returns>Unity Coroutine podmínku.</returns>
        YieldInstruction WaitForSeconds(float sec);

        /// <summary>
        /// Getter běžné Unity yield return podmínky WaitUntil.
        /// </summary>
        /// <param name="predicate">Podmínka ke splnění.</param>
        /// <returns>Unity Coroutine podmínku.</returns>
        CustomYieldInstruction WaitUntil(Func<bool> predicate);

        /// <summary>
        /// Zastavení běžící Coroutine podle ID získaného při její
        /// spuštění pomocí IMenuAPI.
        /// </summary>
        /// <param name="id">Identifikační číslo Coroutine.</param>
        void StopCoroutine(ulong id);

        /// <summary>
        /// Konstruktor wrapperu pro menu tlačítko.
        /// </summary>
        /// <param name="name">Název objektu, který musí být unikátní v dané hierarchické vrstvě.</param>
        /// <returns>Instance objektu.</returns>
        IMenuItemButton CreateButton(string name);

        /// <summary>
        /// Konstruktor wrapperu pro zaškrtávací pole s popiskem.
        /// </summary>
        /// <param name="name">Název objektu, který musí být unikátní v dané hierarchické vrstvě.</param>
        /// <returns>Instance objektu.</returns>
        IMenuItemCheckbox CreateCheckBox(string name);

        /// <summary>
        /// Konstruktor wrapperu pro tabulku dalších objektů.
        /// </summary>
        /// <param name="name">Název objektu, který musí být unikátní v dané hierarchické vrstvě.</param>
        /// <param name="width">Počet ekvivalentně širokých sloupců.</param>
        /// <param name="height">Počet ekvivalentně vysokých řádků.</param>
        /// <returns>Instance objektu.</returns>
        IMenuItemGrid CreateGrid(string name, int width, int height);

        /// <summary>
        /// Konstruktor wrapperu pro obrázek.
        /// </summary>
        /// <param name="name">Název objektu, který musí být unikátní v dané hierarchické vrstvě.</param>
        /// <returns>Instance objektu.</returns>
        IMenuItemImage CreateImage(string name);

        /// <summary>
        /// Konstruktor wrapperu pro vstupní pole.
        /// </summary>
        /// <param name="name">Název objektu, který musí být unikátní v dané hierarchické vrstvě.</param>
        /// <returns>Instance objektu.</returns>
        IMenuItemInput CreateInput(string name);

        /// <summary>
        /// Konstruktor wrapperu pro obrázek.
        /// </summary>
        /// <param name="name">Název objektu, který musí být unikátní v dané hierarchické vrstvě.</param>
        /// <param name="layout">Vertikální zarovnání objektů ve struktuře. Volba horizontálního zarovnání je ignorována.</param>
        /// <returns>Instance objektu.</returns>
        IMenuItemScrollable CreateScrollable(string name, TextAnchor layout);

        /// <summary>
        /// Konstruktor wrapperu pro text.
        /// </summary>
        /// <param name="name">Název objektu, který musí být unikátní v dané hierarchické vrstvě.</param>
        /// <returns>Instance objektu.</returns>
        IMenuItemText CreateText(string name);
    }
}
