using UnityEngine;
using VrLifeAPI.Client.Applications.MenuApp.MenuItems;

public class MenuAppContainer : MonoBehaviour
{

    public static MenuAppContainer current;
    private IMenuItem _item = null;
    // Start is called before the first frame update

    private void Awake()
    {
        current = this;
    }

    public bool SetView(IMenuItem item)
    {
        if(item == null)
        {
            return false;
        }
        if(_item != null && _item != item)
        {
            ((IGOReadable)_item).GetGameObject().transform.SetParent(null);
            ((IGOReadable)_item).GetGameObject().SetActive(false);
        }
        ((IGOReadable)item).GetGameObject().transform.SetParent(gameObject.transform);
        item.SetRectTransform(Vector2.zero, Vector2.one, Vector2.zero);
        ((IGOReadable)item).GetGameObject().SetActive(true);
        _item = item;
        return true;
    }

    public void OnOpenMenu()
    {
        if(_item != null)
        {
            ((IGOReadable)_item).GetGameObject().transform.localRotation = Quaternion.identity;
        }
    }

    private void OnDestroy()
    {
        current = null;
    }
}
