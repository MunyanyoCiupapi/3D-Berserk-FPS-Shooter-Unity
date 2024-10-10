using UnityEngine;

public interface IDamageable
{
    void Damage(int damage);
}
public class Health : MonoBehaviour, IDamageable
{
    public int hp;

    public void Damage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
