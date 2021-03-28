using System.Collections;
using System.Collections.Generic;
using Packages.Rider.Editor;
using UnityEngine;
public class Block : MonoBehaviour
{
    [SerializeField] AudioClip breakSound;
    [SerializeField] GameObject blockSparklesVFX;
    [SerializeField] Sprite[] hitSprites;

    Level level;
    LevelMidCollider levelMid;
    GameSession gameSession;
    ContactPoint2D[] contacts;
    float brickBottomYValue;
    PolygonCollider2D polyCollider;

    [SerializeField] int totalHits;
    [SerializeField] int points;
    private void Start()
    {
        polyCollider = gameObject.GetComponent<PolygonCollider2D>();

        CountBreakableBlocks();
        gameSession = FindObjectOfType<GameSession>();
        levelMid = FindObjectOfType<LevelMidCollider>();
    }

    private void Update()
    {
        if (AssignYValue() <= levelMid.gameObject.transform.position.y)
        {
            //Debug.Log("Rebound CALLED");
            ReboundCollider();
        }    
    }

    private float AssignYValue()
    {
        return brickBottomYValue = polyCollider.bounds.center.y - polyCollider.bounds.extents.y;
    }
    private void CountBreakableBlocks()
    {
        level = FindObjectOfType<Level>();
        if (tag == "Breakable")
        {
            level.CountBlocks();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision detected from object :" + collision.gameObject);
        if (collision.gameObject.tag == "Ball")
        {
            HandleHit();
        }
       return;
    }
    private void HandleHit()
    {
        totalHits--;
        if (totalHits <= 0)
        {
            gameSession.AddToScore(points);
            DestroyBlock();
        }
        else
        {
            ShowNextHitSprite();
        }
    }
    private void ShowNextHitSprite()
    {
        int spriteIndex = totalHits;
        if (hitSprites[spriteIndex] != null)
        {
            GetComponent<SpriteRenderer>().sprite = hitSprites[spriteIndex];
        }
        else
        {
            Debug.LogError("Block sprite is missing from array." + gameObject.name);
        }
    }
    private void DestroyBlock()
    {
        //PlayBlockDestorySFX();
        Debug.Log("Block Destroyed Called");
        TriggerVFX();
        Destroy(gameObject);

    }
    private void PlayBlockDestorySFX()
    {
       AudioSource.PlayClipAtPoint(breakSound, Camera.main.transform.position);
    }

    private void TriggerVFX()
    {
       GameObject sparkles = Instantiate(blockSparklesVFX, transform.position, transform.rotation);
       Destroy(sparkles, .5f);
    }

    private void ReboundCollider()
    {
        var polyArr = polyCollider.points;
        polyArr[1].y = transform.InverseTransformPoint(levelMid.gameObject.transform.position).y;
        polyArr[2].y = transform.InverseTransformPoint(levelMid.gameObject.transform.position).y;
        polyCollider.points = polyArr;

        if (polyArr[1].y > polyArr[3].y)
            polyCollider.enabled = false;
    }
}
