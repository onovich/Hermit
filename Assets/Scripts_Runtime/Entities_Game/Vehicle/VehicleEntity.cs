using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hermit {

    public class VehicleEntity : MonoBehaviour {

        public readonly EntityType entityType = EntityType.Vehicle;

        public int entityID;
        public int typeID;
        public string typeName;
        public AllyStatus allyStatus;
        public VehicleType vehicleType;

        public bool isDead;
    }

}