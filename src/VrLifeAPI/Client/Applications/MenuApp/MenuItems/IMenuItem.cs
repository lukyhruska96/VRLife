using System;
using System.Collections.Generic;
using UnityEngine;

namespace VrLifeAPI.Client.Applications.MenuApp.MenuItems
{
    /// <summary>
    /// Interface wrapperu herního objektu v UI rozhraní (menu)
    /// </summary>
    public interface IMenuItem : IDisposable
    {
        /// <summary>
        /// Getter informace menu objektu
        /// </summary>
        /// <returns>Objekt popisující daný menu objekt.</returns>
        MenuItemInfo GetInfo();

        /// <summary>
        /// Getter seznamu objektů, které mají tento objekt jako parent
        /// </summary>
        /// <returns>Seznam´menu objektů.</returns>
        List<IMenuItem> GetChildren();

        /// <summary>
        /// Nastavení lokace objektu.
        /// </summary>
        /// <param name="anchorMin">Vektor označující procentuálně <0;1> levý dolní roh menu objektu v rodičovském objektu.</param>
        /// <param name="anchorMax">Vektor označující procentuálně <0;1> pravý horní roh menu objektu v rodičovském objektu.</param>
        /// <param name="pivot">Vektor středu rotace (procentuálně <0;1>)</param>
        void SetRectTransform(Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot);

        /// <summary>
        /// Smaže daný element ze své struktury (objekt však stále existuje v kořenové struktuře, jelikož parent má null).
        /// </summary>
        /// <param name="child">Objekt k odstranění.</param>
        /// <returns>Instance daného objektu v případě, že byl smazán, jinak null.</returns>
        IMenuItem RemoveChild(IMenuItem child);

        /// <summary>
        /// Nastavení odsazení v px jednotách.
        /// </summary>
        /// <param name="left">Levé odsazení v px.</param>
        /// <param name="top">Horní odsazení v px.</param>
        /// <param name="right">Pravé odsazení v px.</param>
        /// <param name="bottom">Dolní odsazení v px.</param>
        void SetPadding(float left, float top, float right, float bottom);

        /// <summary>
        /// Nastavení odsazení v px jednotách.
        /// </summary>
        /// <param name="horizontal">Odsazení v levé a pravé části objektu v px.</param>
        /// <param name="vertical">Odsazení v horní a dolní části objektu v px.</param>
        void SetPadding(float horizontal, float vertical);
    }

    /// <summary>
    /// Interface skrývající getter na herní objekt (používá se pouze uvnitř struktur)
    /// </summary>
    public interface IGOReadable
    {
        /// <summary>
        /// Getter na GameObject daného UI elementu.
        /// </summary>
        /// <returns>Instance herního objektu.</returns>
        GameObject GetGameObject();
    }


}
