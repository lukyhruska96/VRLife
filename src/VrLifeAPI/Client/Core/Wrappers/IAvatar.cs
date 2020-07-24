using VrLifeAPI.Client.Core.Character;

namespace VrLifeAPI.Client.Core.Wrappers
{
    public interface IAvatar
    {
        ulong GetUserId();
        SkeletonState GetCurrentSkeleton();

        void SetSkeleton(SkeletonState skeleton);

        UnityEngine.GameObject GetGameObject();

        void SetControls(bool enabled);

        UnityEngine.GameObject GetHead();

        ISoundPlayer GetSoundPlayer();

        void Destroy();
    }
}
