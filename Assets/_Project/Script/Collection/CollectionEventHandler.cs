using System;

namespace Alphabet.Collection
{
    public class CollectionEventHandler
    {
        // Event
        public static event Action OnCollectionOpen;
        public static event Action OnCollectionClose;

        // Caller
        public static void CollectionOpenEvent() => OnCollectionOpen?.Invoke();
        public static void CollectionCloseEvent() => OnCollectionClose?.Invoke();
    }
}