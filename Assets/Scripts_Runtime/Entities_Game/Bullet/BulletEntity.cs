using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hermit {

    public class BulletEntity : MonoBehaviour {

        public readonly EntityType entityType = EntityType.Bullet;

        public int entityID;
        public int typeID;
        public string typeName;
        public AllyStatus allyStatus;
        public BulletFlyType flyType;

        public bool isDead;

    }

}