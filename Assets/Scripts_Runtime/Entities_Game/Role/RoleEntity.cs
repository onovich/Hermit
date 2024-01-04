using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hermit {

    public class RoleEntity : MonoBehaviour {

        public readonly EntityType entityType = EntityType.Role;

        public int entityID;
        public int typeID;
        public string typeName;
        public AllyStatus allyStatus;
        public RoleType roleType;

        public bool isDead;

    }

}