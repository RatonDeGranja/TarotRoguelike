using System.Collections.ObjectModel;
using UnityEngine;

public class HandCollection : Collection<Card>
{
    // Sobrescribimos el método que se llama cuando alguien hace mano.Clear()
    protected override void ClearItems()
    {
        DeckManager.Instance.DiscardHand();
    }

}