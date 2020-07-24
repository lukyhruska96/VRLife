using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VrLifeAPI.Client.Applications.MenuApp.MenuItems;

namespace VrLifeAPI.Client.API.MenuAPI
{
    public interface IMenuAPI
    {
        ulong StartCoroutine(IEnumerator coroutine);
        YieldInstruction WaitForSeconds(float sec);
        CustomYieldInstruction WaitUntil(Func<bool> predicate);
        void StopCoroutine(ulong id);
        IMenuItemButton CreateButton(string name);
        IMenuItemCheckbox CreateCheckBox(string name);
        IMenuItemGrid CreateGrid(string name, int width, int height);
        IMenuItemImage CreateImage(string name);
        IMenuItemInput CreateInput(string name);
        IMenuItemScrollable CreateScrollable(string name, TextAnchor layout);
        IMenuItemText CreateText(string name);
    }
}
