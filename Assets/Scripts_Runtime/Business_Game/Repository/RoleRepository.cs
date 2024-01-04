using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hermit.Business.Game {

    public class RoleRepository {

        Dictionary<int, RoleEntity> all;
        Dictionary<int, RoleEntity> monsters;
        RoleEntity selectedRole;

        RoleEntity[] temp;

        public RoleRepository() {
            all = new Dictionary<int, RoleEntity>();
            monsters = new Dictionary<int, RoleEntity>(1000);
            temp = new RoleEntity[1000];
        }

        public void Role_Add(RoleEntity role) {
            all.Add(role.entityID, role);
            if (role.roleType == RoleType.Monster) {
                monsters.Add(role.entityID, role);
            }
        }

        public bool Role_TryGet(int entityID, out RoleEntity role) {
            return all.TryGetValue(entityID, out role);
        }

        public void SelectedRole_Set(RoleEntity role) {
            selectedRole = role;
        }

        public RoleEntity SelectedRole_Get() {
            return selectedRole;
        }

        public IEnumerable<RoleEntity> Monsters_Get() {
            return monsters.Values;
        }

        public bool Role_TryGetAlive(int entityID, out RoleEntity role) {
            bool has = all.TryGetValue(entityID, out role);
            return has && !role.isDead;
        }

        public void Role_Remove(RoleEntity role) {
            all.Remove(role.entityID);
            if (role.roleType == RoleType.Monster) {
                monsters.Remove(role.entityID);
            }
        }

    }

}