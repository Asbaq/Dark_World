
# 🌑 **Dark World**  

## 📌 **Introduction**  
**Dark World** is a **2D side-scrolling action game** where players control **Charles**, a lone warrior trying to escape a mysterious and hostile world. The game features **procedurally generated levels, power-ups, traps, and enemies**. Players must survive by dodging traps, defeating enemies, and upgrading their abilities.  

🔗 Video Trailer

https://youtu.be/18bybA02Vwk


![image](https://github.com/Asbaq/Dark_World/assets/62818241/b54c0133-30f9-4c33-a36c-1ab82f94de83)

## 🎮 **Core Features**  
- 🏃 **Side-Scrolling Movement** – Smooth character controls with animations.  
- ⚔️ **Combat System** – Melee and ranged attacks with tap/swipe controls.  
- 🧩 **Procedural Level Generation** – Unique levels in every playthrough.  
- 🩹 **Health & Power-ups** – Health regeneration and upgradeable abilities.  
- ⚠️ **Traps & Hazards** – Spikes, fire, and other deadly obstacles.  
- 🏆 **Achievements & Progression** – Unlockable upgrades and milestones.  
- 💰 **Monetization** – Ads, in-app purchases, and an ad removal option.  
- 🔥 **Firebase Integration** – Cloud saves, analytics, and leaderboards.  

---

## 🏗️ **How It Works**  

The game consists of various systems, each handling different mechanics like movement, combat, and level generation. Below is an overview of key mechanics with their implementations.  

---

### 🎮 **Player Movement**  
Handles **keyboard input** for character movement.  

```csharp
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] Rigidbody2D rb;
    private float moveInput;

    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
    }
}
```

---

### ⚔️ **Combat System**  
Implements **basic attacks and damage handling** for the player.  

```csharp
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] int attackDamage = 10;
    [SerializeField] Transform attackPoint;
    [SerializeField] float attackRange = 0.5f;
    [SerializeField] LayerMask enemyLayers;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
    }

    void Attack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }
    }
}
```

---

### 🧩 **Procedural Level Generation**  
Dynamically generates levels **with platforms, enemies, and obstacles**.  

```csharp
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] GameObject[] levelChunks;
    [SerializeField] Transform player;
    private float spawnPos = 0;
    private float chunkLength = 10;

    void Update()
    {
        if (player.position.x > spawnPos - (chunkLength * 2))
        {
            SpawnChunk();
        }
    }

    void SpawnChunk()
    {
        Instantiate(levelChunks[Random.Range(0, levelChunks.Length)], new Vector3(spawnPos, 0, 0), Quaternion.identity);
        spawnPos += chunkLength;
    }
}
```

---

### 🩹 **Health & Power-ups**  
Handles **player health and regeneration**.  

```csharp
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] int maxHealth = 100;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0) Die();
    }

    void Die()
    {
        Debug.Log("Player Died!");
        // Restart level or show game over screen.
    }
}
```

---

### ⚠️ **Traps & Hazards**  
Spikes and other **environmental hazards** deal damage to the player.  

```csharp
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] int trapDamage = 20;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerHealth>().TakeDamage(trapDamage);
        }
    }
}
```

---

### 🏆 **Achievements & Progression**  
Manages **XP, upgrades, and player progression**.  

```csharp
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] int xp = 0;
    [SerializeField] int level = 1;
    [SerializeField] int xpToNextLevel = 100;

    public void GainXP(int amount)
    {
        xp += amount;
        if (xp >= xpToNextLevel)
        {
            LevelUp();
        }
    }

    void LevelUp()
    {
        level++;
        xp = 0;
        xpToNextLevel += 50;
        Debug.Log("Level Up! New Level: " + level);
    }
}
```

---

### 🔥 **Firebase Integration**  
Handles **leaderboards, cloud saves, and analytics**.  

```csharp
using UnityEngine;
using Firebase;
using Firebase.Database;

public class FirebaseManager : MonoBehaviour
{
    DatabaseReference dbReference;

    void Start()
    {
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void SaveHighScore(int score)
    {
        dbReference.Child("HighScores").Child("Player").SetValueAsync(score);
    }
}
```

---

## 💰 **Monetization**  
The game will generate revenue through:  
- 📺 **Interstitial Ads** – Shown between levels.  
- 🎁 **In-App Purchases** – Power-ups, skins, and XP boosts.  
- 🚫 **Ad Removal** – One-time purchase to remove ads.  

---

## 🎯 **Conclusion**  
**Dark World** is an immersive side-scrolling adventure featuring **dynamic level generation, combat mechanics, enemy AI, and progression systems**. With **Firebase integration, achievements, and monetization strategies**, the game is designed for long-term engagement and replayability.  

🔥 Get ready to escape the **Dark World**! 🌑💀🏹  
