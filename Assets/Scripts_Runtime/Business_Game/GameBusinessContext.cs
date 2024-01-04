using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hermit.Business.Game {

    public class GameBusinessContext {

        // Internal
        public GameEntity gameEntity;
        public InputEntity inputEntity;

        public PlayerEntity playerEntity;
        RoleRepository roleRepository;
        VehicleRepository vehicleRepository;
        BulletRepository bulletRepository;

        // Extra
        public UIAppContext uiAppContext;
        public AssetsInfraContext assetsInfraContext;
        public TemplateInfraContext templateInfraContext;
        public CameraCoreContext cameraCoreContext;

        public GameBusinessContext() {
            gameEntity = new GameEntity();
            inputEntity = new InputEntity();

            playerEntity = new PlayerEntity();
            roleRepository = new RoleRepository();
            vehicleRepository = new VehicleRepository();
            bulletRepository = new BulletRepository();
        }

        public void Player_Set(PlayerEntity playerEntity) {
            this.playerEntity = playerEntity;
        }

        public void Player_TearDown() {
            playerEntity = null;
        }

        public void Role_Add(RoleEntity roleEntity) {
            roleRepository.Role_Add(roleEntity);
        }

        public void Role_Remove(RoleEntity roleEntity) {
            roleRepository.Role_Remove(roleEntity);
        }

        public bool Role_TryGet(int entityID, out RoleEntity roleEntity) {
            return roleRepository.Role_TryGet(entityID, out roleEntity);
        }

        public void Vehicle_Add(VehicleEntity vehicleEntity) {
            vehicleRepository.Vehicle_Add(vehicleEntity);
        }

        public void Vehicle_Remove(VehicleEntity vehicleEntity) {
            vehicleRepository.Vehicle_Remove(vehicleEntity);
        }

        public bool Vehicle_TryGet(int entityID, out VehicleEntity vehicleEntity) {
            return vehicleRepository.Vehicle_TryGet(entityID, out vehicleEntity);
        }

        public void Bullet_Add(BulletEntity bulletEntity) {
            bulletRepository.Bullet_Add(bulletEntity);
        }

        public void Bullet_Remove(BulletEntity bulletEntity) {
            bulletRepository.Bullet_Remove(bulletEntity);
        }

        public bool Bullet_TryGet(int entityID, out BulletEntity bulletEntity) {
            return bulletRepository.Bullet_TryGet(entityID, out bulletEntity);
        }

    }

}