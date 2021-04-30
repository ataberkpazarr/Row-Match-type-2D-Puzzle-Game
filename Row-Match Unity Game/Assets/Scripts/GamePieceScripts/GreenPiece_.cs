using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenPiece_ : GamePiece
{
    public override void handleScore()
    {
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddScore(150);
        }

    }
}
