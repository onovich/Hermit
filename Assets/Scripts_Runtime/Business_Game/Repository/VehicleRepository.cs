using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hermit.Business.Game {

    public class VehicleRepository {

        Dictionary<int, VehicleEntity> all;
        VehicleEntity selectedVehicle;

        VehicleEntity[] temp;

        public VehicleRepository() {
            all = new Dictionary<int, VehicleEntity>();
            temp = new VehicleEntity[1000];
        }

        public void Vehicle_Add(VehicleEntity vehicle) {
            all.Add(vehicle.entityID, vehicle);
        }

        public bool Vehicle_TryGet(int entityID, out VehicleEntity vehicle) {
            return all.TryGetValue(entityID, out vehicle);
        }

        public void SelectedVehicle_Set(VehicleEntity vehicle) {
            selectedVehicle = vehicle;
        }

        public VehicleEntity SelectedVehicle_Get() {
            return selectedVehicle;
        }

        public bool Vehicle_TryGetAlive(int entityID, out VehicleEntity vehicle) {
            bool has = all.TryGetValue(entityID, out vehicle);
            return has && !vehicle.isDead;
        }

        public void Vehicle_Remove(VehicleEntity vehicle) {
            all.Remove(vehicle.entityID);
        }

    }

}