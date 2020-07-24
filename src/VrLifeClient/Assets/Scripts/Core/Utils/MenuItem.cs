using Assets.Scripts.Core.Applications.MenuApp.MenuItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using VrLifeAPI.Client.Applications.MenuApp.MenuItems;

namespace Assets.Scripts.Core.Utils
{
    public static class MenuItem
    {
        public static void SetRectTransform(this IMenuItem item, RectTransform local, Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot)
        {
            local.pivot = pivot;
            local.localScale = new Vector3(1, 1, 1);
            local.anchorMin = anchorMin;
            local.anchorMax = anchorMax;
            local.SetLTRB(0f, 0f, 0f, 0f);
            local.localPosition = new Vector3(local.localPosition.x, local.localPosition.y, 0);
            ((IGOReadable)item).GetGameObject().transform.localRotation = Quaternion.identity;
        }

        public static void InitMenuType(this RectTransform transform, RectTransform parent)
        {
            transform.pivot = new Vector2(0, 0);
            transform.anchorMin = new Vector2(parent.rect.x, parent.rect.y);
            transform.anchorMax = new Vector2(parent.rect.x + parent.rect.width,
                parent.rect.y + parent.rect.height);
        }

        public static void SetLeft(this RectTransform rect, float val)
        {
            rect.offsetMin = new Vector2(val, rect.offsetMin.y);
        }

        public static void SetRight(this RectTransform rect, float val)
        {
            rect.offsetMax = new Vector2(-val, rect.offsetMax.y);
        }

        public static void SetTop(this RectTransform rect, float val)
        {
            rect.offsetMax = new Vector2(rect.offsetMax.x, -val);
        }

        public static void SetBottom(this RectTransform rect, float val)
        {
            rect.offsetMin = new Vector2(rect.offsetMin.x, val);
        }

        public static void SetLTRB(this RectTransform rect, float left, float top, float right, float bottom)
        {
            rect.offsetMin = new Vector2(left, bottom);
            rect.offsetMax = new Vector2(-right, -top);
        }
    }
}
