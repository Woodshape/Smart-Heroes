using UnityEngine;

namespace DefaultNamespace {
    public class Entity : MonoBehaviour {
        public Entity CheckEntityInRange(string entityTag, float range) {
            RaycastHit2D[] hits = Physics2D.BoxCastAll((Vector2) transform.position, new Vector2(range, range), 0f, Vector2.right);
            if (hits.Length == 0) {
                return null;
            }

            foreach (RaycastHit2D hit in hits) {
                if (hit.collider.CompareTag(entityTag)) {
                    Entity entity = hit.collider.GetComponent<Entity>();
                    if (entity != null) {
                        Debug.Log("Entity found: " + entity, entity.gameObject);
                        return entity;
                    }
                }
            }

            return null;
        }
    }
}
